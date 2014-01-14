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

function goap_plan_actions(%agent)
{
	%node = new ScriptObject( GOAP_Node );
	%node.worldprojection = createCurrentWorldProjection();
	%node.startNode = true;
	%node.value = getNodeValue(%node);
	
	%bestnode = %node.depth_limited_search_heuristic(0, %node.value);
	
	%actionlist = %bestnode.getActionList();
	return %actionlist;
}

function getNodeValue(%node)
{
	%value = (%node.worldprojection.convertToGoalSurvive() + %node.worldprojection.convertToGoalKill()) / 2;
	return %value;
}

function GOAP_Node::depth_limited_search_heuristic(%this, %depth, %bestvalue)
{
	if (%depth > $AIDLSDEPTH)
	{
		if (%this.value > %bestvalue)
		{
			return %this;	
		}
		else
		{
			return "$$";
		}
	}
	else
	{
		%nodes = %this.expand();
		%bestnode = getWord(%nodes, 0);
		%bestvalue2 = %bestvalue;
		
		for (%i = 0; %i < %nodes.count; %i++)
		{
			%node = getWord(%nodes, %i);
			
			if (%node.value > %bestvalue2)
			{
				%bestnode = %node.depth_limited_search_heuristic(%depth + 1, %bestvalue2);
				%bestvalue2 = %bestnode.value;
			}
		}
		return %bestnode;
	}
}

function GOAP_Node::expand(%this)
{
	%actions = $enemy.getAvailableActions(%this.worldprojection);
	%nodes = "";
	for (%i = 0; %i < %actions.count; %i++)
	{
		%action = getWord(%actions, %i);
		
		%node = new ScriptObject(GOAP_Node);
		%node.worldprojection = %this.worldprojection.createNewWorldProjection(%action);
		%node.value = getNodeValue(%node);
		%node.action = %action;
		//echo(%action.id SPC %node.value);
		%node.parent = %this;
		%nodes = addWord(%nodes, %node);
		//echo(%action.id SPC %node.value);
	}
	return %nodes;
}

function GOAP_Node::getActionList(%this)
{
	%node = %this;
	%result = "";
	while (!%node.startNode)
	{
		%result = addWordInFront(%result, %node.action);
		%node = %node.parent;
	}
	return %result;
}

