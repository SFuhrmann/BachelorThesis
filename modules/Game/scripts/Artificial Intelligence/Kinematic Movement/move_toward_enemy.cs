// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Kinematic Movement\stand_still.cs
// Copyright          :  
// Author             :  -
// Created on         :  Freitag, 27. Dezember 2013 22:36
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Behavior that lets the Agent follow the enemy.
// ============================================================

//Classes
//StandStillBehavior
//StandStillAction

if (!isObject(FollowEnemyBehavior))
{
  %template = new BehaviorTemplate(FollowEnemyBehavior);

  %template.friendlyName = "FollowEnemy";
  %template.behaviorType = "AI FollowEnemy";
  %template.description  = "Lets Agent Follow the Enemy.";

  %template.addBehaviorField(duration, "duration of the following in GOAP-Updates (default 250ms)", int, -1);
}

function FollowEnemyAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = FollowEnemyBehavior;
}

function FollowEnemyBehavior::onBehaviorAdd(%this)
{
	%this.duration = %this.owner.AIBehavior.currentAction.duration;
}

function FollowEnemyBehavior::update(%this)
{
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.duration--;
		%this.owner.moveTowards($character.Position);
	}
}