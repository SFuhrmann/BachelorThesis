// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Sonntag, 17. November 2013 22:36
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent stand still for a given time.
//                    :  If no time is given, the Agent stands still until
//                    :  the current action is changed.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(StandStillBehavior))
{
  %template = new BehaviorTemplate(StandStillBehavior);

  %template.friendlyName = "StandStill";
  %template.behaviorType = "AI StandStill";
  %template.description  = "Lets Agent Stand Still.";

  %template.addBehaviorField(duration, "duration of the stillstanding in GOAP-Updates (default 250ms)", int, -1);
}

function StandStillAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = StandStillBehavior;
}

function StandStillBehavior::onBehaviorAdd(%this)
{
	%this.duration = %this.owner.AIBehavior.currentAction.duration;
}

function StandStillBehavior::update(%this)
{
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else if (%this.duration > 0)
	{
		%this.duration--;
	}
}

function StandStillAction::applyChanges(%this, %wp)
{
	
}