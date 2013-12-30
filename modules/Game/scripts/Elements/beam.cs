// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Elements\beam.cs
// Copyright          :  
// Author             :  -
// Created on         :  Montag, 18. November 2013 21:34
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

///creates a beam at %pos ("x y") with the %dir
function createBeam(%pos, %dir)
{
	%beam = new ShapeVector( Beam );
	%beam.setBodyType( dynamic );
	%beam.Position = %pos;
	%beam.setIsCircle( true );
	%beam.setCircleRadius( 1 );
	%beam.Size = "0.5 0.5";
	%beam.SceneGroup = 3;
	%beam.setSceneLayer(8);
	%beam.setCollisionGroups("0 1 4");
	%beam.setFillMode( true );
	%beam.setFillColor( "0.5 0.5 1" );
	%beam.setLineColor( "0.5 0.5 1" );
	%beam.createCircleCollisionShape( 0.25 );
	%beam.setLinearVelocityPolar(%dir, $character.beamSpeed);
	%beam.setCollisionCallback( true );
	
	%beam.damage = 2;
	
	Level.add(%beam);
	
	$beamUpdateSchedule = %beam.schedule(31, update);
	
	alxPlay("Game:beamshoot");
}

function Beam::update(%this)
{
	
	%size = getWord(%this.Size, 0) + $character.beamGrowth / 6;
	
	%this.Size = %size SPC %size;
	%this.damage += $character.beamGrowth;
	%this.deleteCollisionShape(0);
	%this.createCircleCollisionShape( %size / 2 );
	
	$beamUpdateSchedule = %this.schedule(31, update);
}

function Beam::onCollision(%this, %obj, %details)
{
	if (%obj.SceneGroup == 1)
	{
		%obj.addHP(-%this.damage);
		addScore(mFloor(%this.damage) * 10);
	}
	
	alxPlay("Game:beam");
	Level.add(showGlare(%this.Position, %this.Size, 200));
	schedule(1, 0, deleteObj, %this);
}