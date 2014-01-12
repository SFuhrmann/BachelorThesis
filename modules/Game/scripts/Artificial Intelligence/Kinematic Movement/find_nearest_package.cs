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

if (!isObject(FindNearestPackageBehavior))
{
  %template = new BehaviorTemplate(FindNearestPackageBehavior);

  %template.friendlyName = "FindNearestPackage";
  %template.behaviorType = "AI FindNearestPackage";
  %template.description  = "Lets Agent find the nearest MP or HP Packages.";

  %template.addBehaviorField(amount, "amount of packages, that the agent must find", int, 5);
}

function FindNearestPackageAction::initialize(%this, %amount)
{
	%this.amount = %amount;
	if (%amount $= "")
	{
		%this.amount = 5;
	}
	%this.id = FindNearestPackageBehavior;
}

function FindNearestPackageBehavior::onBehaviorAdd(%this)
{
	%this.amount = %this.owner.AIBehavior.currentAction.amount;
}

function FindNearestPackageBehavior::update(%this)
{
	if (%this.amount == 0)
	{
		%this.owner.AIBehavior.executeNextBehavior();
		return;
	}
	else 
	{
		%target = findNearestPackage(%this.owner.Position);//findBestPackageArea(%this.owner.Position);
		
		%this.owner.moveTowards(%target);
	}
}

//subtract 1 from amount for every package found
function FindNearestPackageBehavior::onCollision(%this, %obj, %details)
{
	if (%obj.SceneGroup == 5)
		%this.amount--;
}

function FindNearestPackageAction::getChanges(%this)
{
	return "0 0.5 -1 0 0 0 0.02 0 2 0 0 0";
}