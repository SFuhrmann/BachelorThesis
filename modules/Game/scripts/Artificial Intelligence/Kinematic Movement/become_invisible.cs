// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\become_invisible.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Sonntag, 29. Dezember 2013 21:18
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 28. Dezember 2013 20:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent become invisible.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(BecomeInvisibleBehavior))
{
  %template = new BehaviorTemplate(BecomeInvisibleBehavior);

  %template.friendlyName = "SetMine";
  %template.behaviorType = "AI SetMine";
  %template.description  = "Lets Agent set a mine.";
}

function BecomeInvisibleAction::initialize(%this)
{
	%this.id = BecomeInvisibleBehavior;
}

function BecomeInvisibleBehavior::onBehaviorAdd(%this)
{
	
}

function BecomeInvisibleBehavior::update(%this)
{
	if (%this.done)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.owner.becomeInvisible();
		%this.done = true;
	}
}

function BecomeInvisibleAction::getChanges(%this)
{
	return "0 0 0 1 -1 0 -1 0 0 0 0 0";
}