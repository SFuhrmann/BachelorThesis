// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Elements\gravitpoint.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Montag, 30. Dezember 2013 10:27
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Handles all functions of the gravitation Point the enemy can use
//                    :  to attract the Player to a specific point.
//                    :  First it shoots out like a slow Projectile, second it morphs into the Gravitational Point.
// ============================================================

function shootGravitPoint(%agent)
{
	//convert local position of top-middle point of the Character to World Coordinates
	%position = %agent.getWorldPoint(0, getWord(%agent.Size, 1) / 2);
	
	//get differences on X and Y axes between Agent and Character
	%dX = getWord($character.Position, 0) - getWord(%agent.Position, 0);
	%dY = getWord($character.Position, 1) - getWord(%agent.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	
	%grav = new ShapeVector( GravitPoint );
	
	%grav.setLineColor( 1, 0.5, 1);

	//create Graphics
 	%grav.setIsCircle( true );
	%grav.setCircleRadius( 1 );
	%grav.setFillMode(true);
	%grav.setFillColor("1 0.5 1");
	
	%grav.Position = %position;
	
	//set Scene related Properties
	%grav.SceneGroup = 10;
	%grav.SceneLayer = 10;
	
	//physics
	%grav.setFixedAngle(true);
	%grav.setBodyType( dynamic );
	%grav.setCollisionGroups( 4 );
	%grav.setCollisionCallback( true );
	%grav.setLinearVelocityPolar(%direction, 10);
	
	//add to Scene
	Level.add(%grav);
	$gravitPointProjectile = %grav;
	
	%agent.GravitPoint = %grav;
}

function GravitPoint::onCollision(%this, %obj, %details)
{
	%this.fire();
}

function GravitPoint::fire(%this)
{
	%grav = new Sprite( FiredGravitPoint );
	%grav.Position = %this.Position;
	%grav.Duration = $enemy.gravitPointDuration / 16;
	%grav.setImage("Game:attraction");
	%grav.Size = $attractionPointSize SPC $attractionPointSize;
	%grav.SceneGroup = 23;
	%grav.SceneLayer = 15;
	%grav.setBodyType( static );
	Level.add(%grav);
	Level.add(showGlare(%grav.Position, %grav.Size, 250));
	
	%grav.update();
	
	schedule(1, 0, deleteObj, %this);
}

function FiredGravitPoint::update(%this)
{
	if (%this.duration < 1)
	{
		schedule(1, 0, deleteObj, %this);
		return;
	}
	
	if (%this.duration % 10 == 0)
	{
		createAttractionEffect(%this, 40);
		alxPlay("Game:attractionsound");
	}
		
	%this.duration--;
	
	if (VectorDist($character.Position, %this.Position) < $attractionPointSize / 2)
	{
		//get differences on X and Y axes between GravitPoint and Character
		%dX = getWord($character.Position, 0) - getWord(%this.Position, 0);
		%dY = getWord($character.Position, 1) - getWord(%this.Position, 1);
		
		//normalize Directions
		%vec = VectorNormalize(%dX SPC %dY);
		%dX = - getWord(%vec, 0);
		%dY = - getWord(%vec, 1);
		
		//add to characterSpeed
		$character.setLinearVelocity( getWord($character.getLinearVelocity(), 0) + %dX * $gravitPointInfluence SPC getWord($character.getLinearVelocity(), 1) + %dY * $gravitPointInfluence );
	}
	
	$firedGravitPointUpdateSchedule = %this.schedule(16, update);
}