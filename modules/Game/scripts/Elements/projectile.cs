// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\Elements\projectile.cs
// Copyright          :  
// Author             :  -
// Created on         :  Samstag, 2. November 2013 22:08
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//classes:
//Projectile -> Sprite
/*
Fields:
owner - the object that shot the projectile

*/

///create a new Projectile at %position, flying at a direction of %direction degrees at %speed
///you may add %addVeloX and %addVeloY to the speed (speed from shooting chara e.g.)
///also you will have to provide the %owner of the projectile
function createProjectile(%position, %direction, %speed, %addVeloX, %addVeloY, %owner)
{
	%shot = new Sprite(Projectile);
	%shot.setBodyType( dynamic );
	%shot.Position = %position;
	%shot.Size = "1 1";
	%shot.setLinearVelocityPolar(%direction, %speed);
	%shot.SceneLayer = 11;
	%shot.SceneGroup = 3;
	%shot.Image = "Game:Projectile";
	%shot.createCircleCollisionShape(0.5);
	%shot.setCollisionCallback(true);
	%shot.setFixedAngle(true);
	%shot.setLinearDamping(0);
	%shot.owner = %owner;
	
	//adjust color Value and Collision Groups
	if (%owner.SceneGroup == $character.SceneGroup)
	{
		%shot.setBlendColor(0.5, 0.5, 1);
		%shot.setCollisionGroups(1);
	}
	else
	{
		%shot.setBlendColor(1, 0.5, 0.5);
		%shot.setCollisionGroups(2);
	}
	
	//add the current speed of the character
	%newVeloX = %shot.getLinearVelocityX() + %addVeloX;
	%newVeloY = %shot.getLinearVelocityY() + %addVeloY;
	
	%shot.setLinearVelocity(%newVeloX SPC %newVeloY);
	
	Level.add(%shot);
}

function Projectile::onCollision(%this, %obj, %details)
{
	
}