// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\search_for_packages.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Sonntag, 29. Dezember 2013 13:17
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  This file provides a function, that will search the areas around
//                    :  the Agent for packages (hp or mp or both) and will return the area
//                    :  with the most packages.
//					  :  A second function will find the nearest package inside the area of the agent.
// ============================================================

function findNearestPackage(%pos)
{
	%i = 1;
	%radius = 10;
	%count = 0;
	while(true)
	{
		%packages = Level.pickCircle(%pos, %i * %radius);
		for (%j = 0; %j < %packages.count; %j++)
		{
			%package = getWord(%packages, %j);
			if (%package.SceneGroup == 5)
			{
				return %package.Position;
			}
		}
		%i++;
	}
}