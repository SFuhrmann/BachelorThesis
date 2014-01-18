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
	%innerMine.setBodyType( dynamic );
	//add to Scene
	Level.add(%innerMine);
	%mine.inner = %innerMine;
	
	
	%outerMine = new ShapeVector( OuterMine );
	
	//determine whether it contains HP or MP
	%outerMine.setLineColor( 1, 0.5, 0.5);

	//create Graphics
 	%outerMine.setIsCircle( true );
	%outerMine.setCircleRadius( 0.5 );
	%outerMine.Size = 15;
	
	%outerMine.Position = %this.Position;
	
	//set Scene related Properties
	%outerMine.SceneGroup = 10;
	%outerMine.SceneLayer = 10;
	
	//physics
	%outerMine.radius = 0.5;
	%outerMine.createCircleCollisionShape( %outerMine.radius * 15 );
	%outerMine.setCollisionGroups( 2 );
	%outerMine.setCollisionCallback(true);
	%outerMine.setCollisionShapeIsSensor(0, true);
	%outerMine.setFixedAngle(true);
	%outerMine.setBodyType( dynamic );
	//add to Scene
	Level.add(%outerMine);
	
	%outerMine.Mine = %mine;
	
	$mine = %outerMine;
	
	$mine.lifeTime = 300;
	
	$mine.update();
}

function OuterMine::Update(%this)
{
	if (%this.lifeTime == 0)
	{
		schedule(1, 0, deleteObj, %this);
		schedule(1, 0, deleteObj, %this.mine.inner);
		return;
	}
	%this.mine.inner.Position = %this.Position;
	
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord($character.Position, 0) - getWord(%this.Position, 0);
	%dY = getWord($character.Position, 1) - getWord(%this.Position, 1);
	//calculate the direction from that
	%angle = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	%this.setLinearVelocityPolar(%angle, 2);
	%this.mine.inner.setLinearVelocityPolar(%angle, 2);
	%this.lifeTime--;
	$mineUpdateSchedule = %this.schedule(100, update);
}

function OuterMine::onCollision(%this, %obj, %details)
{
	Level.add(showGlare(%this.Position, 5, 250));
	
	schedule(1, 0, deleteObj, %this);
	schedule(1, 0, deleteObj, %this.mine.inner);
	
	%obj.addHP(-$enemy.mineDamage);
	//get X and Y difference between goal and agent
	%dX = getWord(%this.position, 0) - getWord(%obj.Position, 0);
	%dY = getWord(%this.position, 1) - getWord(%obj.Position, 1);
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
	
	%obj.setLinearVelocityPolar(%targetAngle, 30);
	%obj.noMoving(250);
	
	alxPlay("Game:Beam");
}