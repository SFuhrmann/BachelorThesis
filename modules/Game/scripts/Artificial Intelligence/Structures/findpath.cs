// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\findpath.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Freitag, 27. Dezember 2013 20:30
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

function findPath(%startXY, %goalXY)
{
	%steps = 4;
	//initialize algorithm with start Node
	%start = new ScriptObject( Node );
	%start.X = getWord(%startXY, 0);
	%start.Y = getWord(%startXY, 1);
	%start.Parent = 1;
	%start.G = 0;
	%start.H = getManhattanHeuristic(%startXY, %goalXY);
	
	//initialize openList
	%openList = new ScriptObject( FHeap );
	%openList.insert(%start);
	
	%count = 0;
	
	//start actual algorithm
	while( true )
	{
		if (%openList.count == 0 || %count > 50)
		{
			return %goalXY;
		}
		
		//find Node with Lowest F-Value (F = G + H)
		%currentNode = %openList.removeMin();
		%closedList[%currentNode.X, %currentNode.Y] = true;
		//check if there is a node left in openList
		//check if the target Square is reached
		if (VectorDist(%currentNode.X  SPC %currentNode.Y, %goalXY) <= %steps)
		{
			//if so, return the calculated path
			return getVisibleQueue(calculatePath(%currentNode, getWord(%startXY, 0), getWord(%startXY, 1)), %steps);
		}
		else
		{
			//move current Node to the closedList
			%openList = expandNode(%currentNode, %openList, %closedList, %goalXY, %steps);
		}
		%count++;
	}
}

function getVisibleQueue(%list, %steps)
{
	%lastVisible = getWord(%list, 0) SPC getWord(%list, 1);
	for (%i = 0; %i < (%list.count - 2) / 2; %i++)
	{
		%isInSight = characterIsInLineOfSight(getWord(%list, %i * 2 + 2) SPC getWord(%list, %i * 2 + 3), getWord(%lastVisible, 0) SPC getWord(%lastVisible, 1), %steps);
		
		if (%isInSight)
		{
			%list = removeWord(%list, %i * 2);
			%list = removeWord(%list, %i * 2);
			%i--;
		}
		else
		{
			%lastVisible = getWord(%list, %i * 2) SPC getWord(%list, %i * 2 + 1);
		}
	}
	return %list;
}

function calculatePath(%currentNode, %sX, %sY)
{
	//%last = %list.count - 1;
	if (%currentNode.X == %sX && %currentNode.Y == %sY)
	{
		return "";
	}
	else
	{
		%nextWord = calculatePath(%currentNode.Parent, %sX, %sY);
		if (%nextWord.count != 0)
		{
			return %nextWord SPC %currentNode.X SPC %currentNode.Y;
		}
		else
		{
			return %currentNode.X SPC %currentNode.Y;
		}
	}
}



function isAcceptableNode(%X, %Y, %closedList)
{
	%result = true;
	%objs = Level.pickPoint(%X SPC %Y, 5, -1, collision);
	
	if(isObject(%objs))
	{
		return false;
	}
	if (%closedList[%X, %Y] == true)
	{
		return false;
	}
	return true;
}

function getManhattanHeuristic(%start, %goal)
{
	return mAbs(getWord(%start, 0) - getWord(%goal, 0)) + mAbs(getWord(%start, 1) - getWord(%goal, 1));
}

function expandNode(%currentNode, %openList, %closedList, %goal, %steps)
{
	for (%loopX = -%steps; %loopX <= %steps; %loopX += %steps)
	{
		for (%loopY = -%steps; %loopY <= %steps; %loopY += %steps)
		{
			if ((%loopX == 0 || %loopY == 0) && !(%loopX == 0 && %loopY == 0))
			{
				%acceptable = isAcceptableNode(%currentNode.X + %loopX, %currentNode.Y + %loopY, %closedList);
				if (%acceptable == true)
				{
					//find out if the node already is in the openlist
					if (!%openList.hasNode(%currentNode.X + %loopX, %currentNode.Y + %loopY))
					{
						//if not add the node to the openList
						%newNode = new ScriptObject( Node );
						%newNode.X = %currentNode.X + %loopX;
						%newNode.Y = %currentNode.Y + %loopY;
						%newNode.Parent = %currentNode;
						%newNode.G = %currentNode.G + %steps;
						%newNode.H = getManhattanHeuristic(%newNode.X SPC %newNode.Y, %goal);
						%openList.insert(%newNode);
					}
					else
					{
						//if so check if the G value is smaller than the new one
						if (%currentNode.G + %steps < %node.G)
						{
							//change G Value and Parent of the node
							%openList.setNode(%currentNode.X + %loopX, %currentNode.Y + %loopY, %currentNode.G + %steps, %currentNode);
						}
					}
				}
			}
		}
	}
	return %openList;
}