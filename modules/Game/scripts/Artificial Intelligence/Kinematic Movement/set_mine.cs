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

if (!isObject(SetMineBehavior))
{
  %template = new BehaviorTemplate(SetMineBehavior);

  %template.friendlyName = "SetMine";
  %template.behaviorType = "AI SetMine";
  %template.description  = "Lets Agent set a mine.";
}

function SetMineAction::initialize(%this)
{
	%this.id = SetMineBehavior;
}

function SetMineBehavior::onBehaviorAdd(%this)
{
	
}

function SetMineBehavior::update(%this)
{
	if (%this.done)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%this.owner.createMine();
		%this.done = true;
	}
}

function SetMineAction::applyChanges(%this, %wp)
{
	%wp.mineExists = 1;
	%wp.minePosition = %wp.ownPosition;
	%wp.mineCooldown = 1;
	%wp.ownMP--;
}