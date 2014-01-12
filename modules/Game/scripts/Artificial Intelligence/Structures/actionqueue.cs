// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\actionqueue.cs
// Copyright          :  
// Author             :  -
// Created on         :  Sonntag, 17. November 2013 20:14
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Classes 
//ActionStack

function ActionStack::initialize(%this)
{
	%this.stack = "";
	%this.length = 0;
	%this.lastIndex = 0;
}

///push an action onto the stack (FIFO)
function ActionStack::push(%this, %action)
{
	if (%this.stack $= "")
	{
		%this.stack = %action;
		%this.length = 1;
		%this.lastIndex = 0;
	}
	else
	{
		%this.stack = %this.stack SPC %action;
		%this.length++;
		%this.lastIndex++;
	}
}

///push an action into the bottom of the stack (LIFO)
function ActionStack::enqueue(%this, %action)
{
	if (%this.stack $= "")
	{
		%this.stack = %action;
		%this.length = 1;
		%this.lastIndex = 0;
	}
	else
	{
		%this.stack = %action SPC %this.stack;
		%this.length++;
		%this.lastIndex++;
	}
}

///peek the next action (FIFO)
function ActionStack::peek(%this)
{
	if (%this.length > 0)
		return getWord(%this.stack, %this.lastIndex);
		
	return false;
}
///peek the next action (LIFO)
function ActionStack::peekLast(%this)
{
	if (%this.length > 0)
		return getWord(%this.stack, 0);
		
	return false;
}

///pop the next action (FIFO)
function ActionStack::pop(%this)
{
	%val = false;
	if (%this.length > 1)
	{
		%val = getWord(%this.stack, %this.lastIndex);
		%this.length--;
		%this.lastIndex--;
	}
	else if (%this.length == 1)
	{
		%val = getWord(%this.stack, %this.lastIndex);
		%this.length = 0;
		%this.lastIndex = 0;
	}
	return %val;
}

///pop the last action (LIFO)
function ActionStack::dequeue(%this)
{
	%val = false;
	if (%this.length > 1)
	{
		%val = getWord(%this.stack, 0);
		%this.length--;
		%this.lastIndex--;
	}
	else if (%this.length == 1)
	{
		%val = getWord(%this.stack, 0);
		%this.length = 0;
		%this.lastIndex = 0;
	}
	return %val;
}

///delete the whole stack
function ActionStack::deleteAll(%this)
{
	%this.initialize();
}

///pushes a whole list of actions
function ActionStack::pushList(%this, %actions)
{
	%this.push(getWord(%actions, %i));
	for (%i = 1; %i < %actions.count; %i++)
	{
		%this.push(getWord(%actions, %i));
	}
}

///asserts if an action list is similiar enough to the queue
///the first 5 actions must have similiar action ids for the
///function to assert true
function ActionStack::isSimiliar(%this, %actions)
{
	%necessaryGrade = 5;
	
	
	if (%actions.count < %necessaryGrade && %actions.count != %this.length)
		return false;
	
	%loopsCount = mMin(mMin(%actions.count, %this.length), %necessaryGrade); //-> lowest of actions.count, queue.length and necessaryGrade
	for (%i = 0; %i < %loopsCount; %i++)
	{
		%action1 = getWord(%actions, %i);
		%action2 = getWord(%this.stack, %this.lastIndex - %i);
		if (%action1.id != %action2.id)
			return false;
	}
	return true;
}