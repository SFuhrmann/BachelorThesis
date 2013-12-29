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

if (!isObject(MoveAroundEnemyCCWBehavior))
{
  %template = new BehaviorTemplate(MoveAroundEnemyCCWBehavior);

  %template.friendlyName = "MoveAroundEnemyCCW";
  %template.behaviorType = "AI MoveAroundEnemyCCW";
  %template.description  = "Lets Agent Move Around Enemy counter-clockwise.";

  %template.addBehaviorField(duration, "duration of the circling in GOAP-Updates (default 250ms)", int, -1);
}

function MoveAroundEnemyCCWAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = MoveAroundEnemyCCWBehavior;
}

function MoveAroundEnemyCCWBehavior::onBehaviorAdd(%this)
{
	%this.duration = %this.owner.AIBehavior.currentAction.duration;
}

function MoveAroundEnemyCCWBehavior::update(%this)
{
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.duration--;
		%this.owner.moveAroundCCW($character.Position);
	}
}