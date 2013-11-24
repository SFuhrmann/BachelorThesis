// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\shoot_character.cs
// Copyright          :  
// Author             :  -
// Created on         :  Dienstag, 19. November 2013 13:36
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Lets Agent shoot the character directly.
//                    :  Without any approximation if character could be hit.
//                    :  
// ============================================================

//Classes
//ShootCharacterBehavior
//ShootCharacterAction

if (!isObject(ShootCharacterBehavior))
{
  %template = new BehaviorTemplate(ShootCharacterBehavior);

  %template.friendlyName = "ShootCharacter";
  %template.behaviorType = "AI ShootCharacter";
  %template.description  = "Lets Agent Shoot Character.";

  %template.addBehaviorField(duration, "duration of the shooting in GOAP-Updates (default 250ms)", int, -1);
}

function ShootCharacterAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = ShootCharacterBehavior;
}
function ShootCharacterAction::initialize(%this)
{
	%this.duration = -1;
	%this.id = ShootCharacterBehavior;
}

function ShootCharacterBehavior::onBehaviorAdd(%this)
{
	
}

function ShootCharacterBehavior::update(%this)
{
	//test if the duration of the behavior is exceeded
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else if (%this.duration > 0)
		%this.duration--;
	
	if (isEventPending(%this.shotSchedule) || %this.attackOnCooldown)
		return;
		
	%this.shoot();
}

function ShootCharacterBehavior::shoot(%this)
{
	//convert local position of top-middle point of the Character to World Coordinates
	%position = %this.owner.getWorldPoint(0, getWord(%this.owner.Size, 1) / 2);
	
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord($character.Position, 0) - getWord(%this.owner.Position, 0);
	%dY = getWord($character.Position, 1) - getWord(%this.owner.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	//copy projectile speed to the speed of this projectile
	%speed = %this.owner.projectileSpeed;
	
	//get Linear Velocity along the axes and multiply it with a constant
	%veloX = %this.owner.getLinearVelocityX() * $speedInfluenceOnProjectiles;
	%veloY = %this.owner.getLinearVelocityY() * $speedInfluenceOnProjectiles;
	
	//create the Projectile
	createProjectile(%position, %direction, %speed, %veloX, %veloY, %this.owner);
	
	//re-schedule this
	%this.shootSchedule = %this.owner.schedule(%this.owner.shootingFrequency, shoot);
	
	//turn on cooldown
	%this.attackOnCooldown = true;
	%this.coolDownSchedule = %this.schedule(%this.owner.shootingFrequency - 1, turnOffCooldown);
}

///reset cooldown
function ShootCharacterBehavior::turnOffCooldown(%this)
{
	%this.attackOnCooldown = false;
}

function ShootCharacterBehavior::onBehaviorRemove(%this)
{
	cancel(%this.shootSchedule);
	schedule(31, 0, deleteObj, %this);
}