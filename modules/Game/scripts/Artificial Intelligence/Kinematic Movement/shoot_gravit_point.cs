// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 28. Dezember 2013 20:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent shoot a gravit Point.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(ShootGravitPointBehavior))
{
  %template = new BehaviorTemplate(ShootGravitPointBehavior);

  %template.friendlyName = "ShootGravitPoint";
  %template.behaviorType = "AI ShootGravitPoint";
  %template.description  = "Lets Agent shoot a gravitPoint.";
}

function ShootGravitPointAction::initialize(%this)
{
	%this.id = ShootGravitPointBehavior;
}

function ShootGravitPointBehavior::onBehaviorAdd(%this)
{
	
}

function ShootGravitPointBehavior::update(%this)
{
	if (%this.done)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.owner.createGravitPoint();
		%this.done = true;
	}
}

function ShootGravitPointAction::getChanges(%this)
{
	return "0 0 0 1 0 0 -1 0 0 0 0 0";
}