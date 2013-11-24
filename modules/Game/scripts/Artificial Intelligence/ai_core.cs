// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\ai_core.cs
// Copyright          :  
// Author             :  -
// Created on         :  Sonntag, 17. November 2013 15:56
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  This class is the core behaviour of the AI.
//                    :  It will handle the decision making and action execution.
//                    :  
// ============================================================

//Classes
//decision maker
//action execution

//create Behavior Template
if (!isObject(GOAPBehavior))
{
  %template = new BehaviorTemplate(GOAPBehavior);

  %template.friendlyName = "GOAP";
  %template.behaviorType = "AI Core";
  %template.description  = "Handles decision making and action execution.";

  %template.addBehaviorField(updateRate, "milliseconds between AI updates", int, 250);
}

function GOAPBehavior::onBehaviorAdd(%this)
{
	%this.actionQueue = new ScriptObject( ActionStack );
	%this.actionQueue.initialize();
	
	%firstAction = new ScriptObject(ShootCharacterAction);
	%firstAction.initialize();
	%this.actionQueue.push(%firstAction);
	
	%this.executeNextBehavior();
	
	%this.updateSchedule = %this.schedule(%this.updateRate, update);
}

function GOAPBehavior::update(%this)
{
	/*
	%actionList = goap_plan_actions(%this.owner);
	if (!%this.actionQueue.isSimiliar(%actionList))
	{
		%this.actionQueue.deleteAll();
		%this.actionQueue.pushList(%actionList);
		
		%this.executeNextBehavior();
	}*/
	
	%this.currentBehavior.update();
	
	%this.updateSchedule = %this.schedule(%this.updateRate, update);
}

function GOAPBehavior::executeNextBehavior(%this)
{
	%action = %this.actionQueue.pop();
	
	%this.owner.removeBehavior(%this.currentBehavior);
	
	%this.currentAction = %action;
	
	%this.currentBehavior = %action.id.createInstance();
	%this.owner.addBehavior(%this.currentBehavior);
}