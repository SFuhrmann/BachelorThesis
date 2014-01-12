// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\world_projection_queue.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Donnerstag, 9. Januar 2014 15:18
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Holds a Class that saves a number of World Projections
//                    :  and their average values.
//                    :  Only "weak" properties will be stored (those, which are 
//                    :  affected by anything else than the action of the agent.
// ============================================================

//creates a new WorldProjection
function createWorldProjectionQueue()
{
	%queue = new ScriptObject( WorldProjectionQueue );
	%queue.projections = "";
	%queue.count = 0;
	return %queue;
}

///adds a new Projection and saves the new averages for all weak properties
function WorldProjectionQueue::addProjection(%this, %proj)
{
	//add new WorldProjection
	%this.projections = addWord(%this.projections, %proj);
	//increment count
	%this.count++;
	
	for (%i = 0; %i < $weakIndices.count; %i++)
	{
		//get Index of next weak property
		%j = getWord($weakIndices, %i);
		
		//formula for new average: newAverage = current + (new - current) / newCount;
		%this.averages[%j] += (getWord(%proj.props, %j) - %this.averages[%j]) / %this.count;
	}
}

///removes a new Projection and saves the new averages for all weak properties
function WorldProjectionQueue::removeLastProjection(%this)
{
	//get last projection of the queue (FIFO)
	%proj = getWord(%this.projections, %this.projections.count - 1);
	//remove the last projection
	%this.projections = removeWord(%this.projections, %this.projections.count - 1);
	
	for (%i = 0; %i < $weakIndices.count; %i++)
	{
		//get Index of next weak property
		%j = getWord($weakIndices, %i);
		
		//formula for new average while removing: newAverage = (oldCount * current - removing) / newCount;
		//while newCount = oldCount - 1
		%this.averages[%j] = (%this.count * %this.averages[%j] - getWord(%proj.props, %j)) / (%this.count - 1);
	}
	
	//decrement count
	%this.count--;
}