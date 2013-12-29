// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Elements\mine.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Sonntag, 29. Dezember 2013 17:19
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  handles all functions of the Enemy's Mine
//                    :  
//                    :  
// ============================================================

function createMine(%this)
{
	%mine = new ScriptObject( Mine );
	%innerMine = new ShapeVector( InnerMine );
	
	//determine whether it contains HP or MP
	%innerMine.setLineColor( 1, 0.5, 0.5);

	//create Graphics
 	%innerMine.setIsCircle( true );
	%innerMine.setCircleRadius( 0.5 );
	%innerMine.setFillMode(true);
	%innerMine.setFillColor("1 0.5 0.5");
	
	%innerMine.Position = %this.Position;
	
	//set Scene related Properties
	%innerMine.SceneGroup = 10;
	%innerMine.SceneLayer = 10;
	
	//physics
	%innerMine.setFixedAngle(true);
	%innerMine.setBodyType( static );
	//add to Scene
	Level.add(%innerMine);
	%mine.inner = %innerMine;
	
	
	%outerMine = new ShapeVector( OuterMine );
	
	//determine whether it contains HP or MP
	%outerMine.setLineColor( 1, 0.5, 0.5);

	//create Graphics
 	%outerMine.setIsCircle( true );
	%outerMine.setCircleRadius( 0.5 );
	%outerMine.Size = %this.mineRadius;
	
	%outerMine.Position = %this.Position;
	
	//set Scene related Properties
	%outerMine.SceneGroup = 10;
	%outerMine.SceneLayer = 10;
	
	//physics
	%outerMine.radius = 0.5;
	%outerMine.createCircleCollisionShape( %outerMine.radius );
	%outerMine.setCollisionGroups( 2 );
	%outerMine.setCollisionCallback(true);
	%outerMine.setCollisionShapeIsSensor(0, true);
	%outerMine.setFixedAngle(true);
	%outerMine.setBodyType( static );
	//add to Scene
	Level.add(%outerMine);
	
	%outerMine.Mine = %mine;
}

function OuterMine::onCollision(%this, %obj, %details)
{
	if (%obj.SceneGroup == 2)
	{
		Level.add(showGlare(%this.Position, 5, 250));
		
		schedule(1, 0, deleteObj, %this);
		schedule(1, 0, deleteObj, %this.mine.inner);
		
		%obj.addHP(-$mineDamage);
		//get X and Y difference between goal and agent
		%dX = getWord(%this.position, 0) - getWord(%obj.Position, 0);
		%dY = getWord(%this.position, 1) - getWord(%obj.Position, 1);
		
		//get angle
		%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
		
		%obj.setLinearVelocityPolar(%targetAngle, 30);
		%obj.noMoving(250);
		
		alxPlay("Game:Beam");
	}
}