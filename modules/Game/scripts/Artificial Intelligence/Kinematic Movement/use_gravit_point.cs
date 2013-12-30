// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 28. Dezember 2013 20:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent use the gravit Point.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(UseGravitPointBehavior))
{
  %template = new BehaviorTemplate(UseGravitPointBehavior);

  %template.friendlyName = "UseGravitPoint";
  %template.behaviorType = "AI UseGravitPoint";
  %template.description  = "Lets Agent Use a gravitPoint.";
}

function UseGravitPointAction::initialize(%this)
{
	%this.id = UseGravitPointBehavior;
}

function UseGravitPointBehavior::onBehaviorAdd(%this)
{
	
}

function UseGravitPointBehavior::update(%this)
{
	if (%this.done)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.owner.fireGravitPoint();
		%this.done = true;
	}
}