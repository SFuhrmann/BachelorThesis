// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\goap.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Donnerstag, 9. Januar 2014 20:04
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  This class holds the core algorithm for the
//                    :  goal oriented action planning.
//                    :  It returns a list (string) of actions, that can be cued inside the ai_core's action queue.
// ============================================================

function createGOAPModule()
{
	$goapModule = new ScriptObject(GOAPModule);
	$goapModule.nextAction = "";
	
	$goapModule.planAction();
}

////////iterative!!!
function GOAPModule::planAction(%this)
{
	//initialize storage structures
	%this.worldmodels = "";
	%this.actions = "";
	
	//initialize data
	%this.worldmodels = setWord(%this.worldmodels, 0, createCurrentWorldProjection());
	%this.currentDepth = 0;
	
	//initialize best Action
	%this.bestAction = "";
	%this.bestActionList = "";
	%this.bestValue = 0;
	
	%this.schedule(5, nextIteration);
}

function GOAPModule::nextIteration(%this)
{
	//calculate value of first worldprojection
	%currentWP = getWord(%this.worldmodels, %this.currentDepth);
	%this.currentValue = getWPValue(%currentWP);
	
	//check if maximum depth is reached
	if (%this.currentDepth >= $AIDLSDEPTH)
	{
		//check if current value is the best value so far
		if (%this.currentValue > %bestValue)
		{
			//if so store new value and accordingly the next action
			%this.bestValue = %currentValue;
			%this.bestAction = getWord(%this.actions, 0);
			%this.bestActionList = %this.actions;
		}
		%this.currentDepth -= 1;
	}
	else
	{
		//get next Action
		%nextAction = %currentWP.nextAction();
		
		if (isObject(%nextAction))
		{
			//create new World Projection
			%this.worldmodels = setWord(%this.worldmodels, %this.currentDepth + 1, %currentWP.createNewWorldProjection(%nextAction));
			
			%this.actions = setWord(%this.actions, %this.currentDepth, %nextAction);
			
			//process it in the next iteration
			%this.currentDepth += 1;
		}
		else
		{	
			%this.currentDepth -= 1;
		}
	}
	
	//test if a new iteration should be performed or not
	if (%this.currentDepth >= 0)
		%this.schedule(5, nextIteration);
	else
		%this.schedule(5, endIterations);
}

function GOAPModule::endIterations(%this)
{
	%this.nextAction = %this.bestAction;
	%this.actionList = %this.bestActionList;
	
	%this.schedule(5, planAction);
}

function getWPValue(%wp)
{
	%survive = %wp.convertToGoalSurvive();
	%kill = %wp.convertToGoalKill();
	%value = (mPow(%survive, 2) + mSqrt(%kill) / 2) / 2;
	return %value;
}