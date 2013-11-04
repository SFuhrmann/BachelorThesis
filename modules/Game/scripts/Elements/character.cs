// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\Elements\character.cs
// Copyright          :  
// Author             :  -
// Created on         :  Samstag, 2. November 2013 10:04
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Classes:
//Character -> Sprite
/*
Fields:
maxSpeed - maximum possible speed
acceleration - acceleration in 1/100 meters/ms^2
shootingFequency - delay till new shoot in ms
projectileSpeed - base speed of projectiles


*/

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Declaration              //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

function createCharacter()
{
	//Sprite Properties
	$character = new Sprite( Character );
	$character.setBodyType( dynamic );
	$character.Position = "0 0";
	$character.Size = "6 6";
	$character.SceneLayer = 10;
	$character.SceneGroup = 2;
	$character.Image = "Game:Character";
	$character.createCircleCollisionShape(2);
	$character.setCollisionGroups( None );
	$character.setCollisionCallback(true);
	$character.setFixedAngle(true);
	$character.setLinearDamping(0.5);
	
	//States Properties:
	//Movement
	$character.maxSpeed = 15;
	$character.acceleration = 2;
	
	//Shooting
	$character.shootingFrequency = 350;
	$character.projectileSpeed = 15;
	
	//add to Scene
	Level.add( $character );
	Window.mount( $character );
	
	//update all states and call all common functions
	$character.Update();
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Update                   //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///update all states and call all reocurring functions
function Character::Update(%this)
{
	//align Character to Mouse
	%this.alignToMouse();
	
	//update once every frame
	%this.updateSchedule = %this.schedule(16, Update);
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Movement                 //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///Walk Left
function Character::walkleft(%this)
{
	//add acceleration to velocity
	%this.setLinearVelocityX(%this.getLinearVelocityX() - %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkLschedule = %this.schedule(100, walkleft);
}

///Walk Right
function Character::walkright(%this)
{
	//add acceleration to velocity
	%this.setLinearVelocityX(%this.getLinearVelocityX() + %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkRschedule = %this.schedule(100, walkright);
}

///Walk Up
function Character::walkup(%this)
{
	//add acceleration to velocity
	%this.setLinearVelocityY(%this.getLinearVelocityY() + %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkUschedule = %this.schedule(100, walkup);
}

///Walk Down
function Character::walkdown(%this)
{
	//add acceleration to velocity
	%this.setLinearVelocityY(%this.getLinearVelocityY() - %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkDschedule = %this.schedule(100, walkdown);
}

///Stop Acceleration Left
function Character::stopwalkleft(%this)
{
	//cancel schedule
	cancel(%this.walkLschedule);
}

///Stop Acceleration Right
function Character::stopwalkright(%this)
{
	//cancel schedule
	cancel(%this.walkRschedule);
}

///Stop Acceleration Up
function Character::stopwalkup(%this)
{
	//cancel schedule
	cancel(%this.walkUschedule);
}

///Stop Acceleration Down
function Character::stopwalkdown(%this)
{
	//cancel schedule
	cancel(%this.walkDschedule);
}

///clamp velocity to maxSpeed
function Character::clampspeed(%this)
{
	//calculate length of velocity vector
	if(getWord(%this.getLinearVelocityPolar(), 1) > %this.maxSpeed)
	{
		//set to maxspeed if it exceeds it
		%this.setLinearVelocityPolar(getWord(%this.getLinearVelocityPolar(), 0), %this.maxSpeed);	
	}
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Shooting                 //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///align the Character to the Mouse
function Character::alignToMouse(%this)
{
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(Window.getMousePosition(), 0) - getWord(%this.Position, 0);
	%dY = getWord(Window.getMousePosition(), 1) - getWord(%this.Position, 1);
	
	if (mPow(%dX + %dY, 2) < 1)
		return;
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
	
	//align character to angle
	%this.Angle = %targetAngle;
}

///shoot a projectile
function Character::shoot(%this)
{
	//make sure the attack is not on cooldown
	if (%this.attackOnCoolDown)
	{
		//re-schedule this function, if player keeps pressing the mousebutton
		%this.shootSchedule = %this.schedule(%this.shootingFrequency, shoot);
		return;
	}
	//convert local position of top-middle point of the Character to World Coordinates
	%position = %this.getWorldPoint(0, getWord(%this.Size, 1) / 2);
	
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(Window.getMousePosition(), 0) - getWord(%this.Position, 0);
	%dY = getWord(Window.getMousePosition(), 1) - getWord(%this.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	//copy projectile speed to the speed of this projectile
	%speed = %this.projectileSpeed;
	
	//get Linear Velocity along the axes and multiply it with a constant
	%veloX = %this.getLinearVelocityX() * $speedInfluenceOnProjectiles;
	%veloY = %this.getLinearVelocityY() * $speedInfluenceOnProjectiles;
	
	//create the Projectile
	createProjectile(%position, %direction, %speed, %veloX, %veloY, %this);
	
	//re-schedule this
	%this.shootSchedule = %this.schedule(%this.shootingFrequency, shoot);
	
	//turn on cooldown
	%this.attackOnCooldown = true;
	%this.coolDownSchedule = %this.schedule(%this.shootingFrequency - 1, turnOffCooldown);
}

///reset cooldown
function Character::turnOffCooldown(%this)
{
	%this.attackOnCooldown = false;
}