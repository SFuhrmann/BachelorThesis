// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 28. Dezember 2013 20:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent move around the enemy.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(GetPowerupBehavior))
{
  %template = new BehaviorTemplate(GetPowerupBehavior);

  %template.friendlyName = "GetPowerup";
  %template.behaviorType = "AI SGetPowerup";
  %template.description  = "Lets Agent get a Powerup.";
}

function GetPowerupAction::initialize(%this)
{
	%this.id = GetPowerupBehavior;
}

function GetPowerupBehavior::onBehaviorAdd(%this)
{
	
}

function GetPowerupBehavior::update(%this)
{
	if (%this.done || !isObject($powerup))
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.owner.moveTowards($powerup.Position);
	}
}

function GetPowerupAction::onCollision(%this, %obj, %details)
{
	if (%obj.SceneGroup == 25)
		%this.done = true;
}

function GetPowerupAction::applyChanges(%this, %wp)
{
	%wp.ownPosition = VectorAdd(%wp.ownPosition, VectorScale(VectorNormalize(VectorSub(%wp.ownPosition, $powerUp.Position)), 4));
}