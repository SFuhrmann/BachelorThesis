// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 28. Dezember 2013 20:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent follow the enemy.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(MoveAroundEnemyCWBehavior))
{
  %template = new BehaviorTemplate(MoveAroundEnemyCWBehavior);

  %template.friendlyName = "MoveAroundEnemyCW";
  %template.behaviorType = "AI MoveAroundEnemyCW";
  %template.description  = "Lets Agent Move Around Enemy clockwise.";

  %template.addBehaviorField(duration, "duration of the circling in GOAP-Updates (default 250ms)", int, -1);
}

function MoveAroundEnemyCWAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = MoveAroundEnemyCWBehavior;
}

function MoveAroundEnemyCWBehavior::onBehaviorAdd(%this)
{
	%this.duration = %this.owner.AIBehavior.currentAction.duration;
}

function MoveAroundEnemyCWBehavior::update(%this)
{
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.duration--;
		%this.owner.moveAroundCW($character.Position);
	}
}

function MoveAroundEnemyCWAction::getChanges(%this)
{
	return "0 0 0 0 0 0 0 0 0 0 0 0";
}