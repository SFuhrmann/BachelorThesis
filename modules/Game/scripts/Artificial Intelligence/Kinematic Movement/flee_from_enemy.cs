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

if (!isObject(FleeFromEnemyBehavior))
{
  %template = new BehaviorTemplate(FleeFromEnemyBehavior);

  %template.friendlyName = "FleeFromEnemy";
  %template.behaviorType = "AI FleeFromEnemy";
  %template.description  = "Lets Agent Flee From the Enemy.";

  %template.addBehaviorField(duration, "duration of the fleeing in GOAP-Updates (default 250ms)", int, 5);
}

function FleeFromEnemyAction::initialize(%this, %duration)
{
	%this.duration = %duration;
	%this.id = FleeFromEnemyBehavior;
}

function FleeFromEnemyBehavior::onBehaviorAdd(%this)
{
	%this.duration = %this.owner.AIBehavior.currentAction.duration;
}

function FleeFromEnemyBehavior::update(%this)
{
	if (%this.duration == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.duration--;
		%this.owner.moveAwayFrom($character.Position);
	}
}

function FleeFromEnemyAction::applyChanges(%this, %wp)
{
	%wp.ownPosition = VectorAdd(%wp.ownPosition, VectorScale(VectorNormalize(VectorSub(%wp.ownPosition, %wp.enemyPosition)), 4));
}