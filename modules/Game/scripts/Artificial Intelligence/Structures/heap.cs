// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\heap.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Freitag, 27. Dezember 2013 20:31
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

/////asdasdasd
/////
///TODO:
///Fix this function! It will crash as soon as there is an element inside the heap when calling "insert"
/////


//inserts a node and sorts the heap by the f values of the nodes
function FHeap::insert(%this, %node)
{
	if (%this.hash[%node.X, %node.Y] == true)
		return;
	%this.hash[%node.X, %node.Y] = true;
	if (%this.list $= "")
	{
		%this.list = %node;
		%this.count++;
		return;
	}

	%newList = "";
	
	%notInserted = true;
	
	%nodeF = %node.G + %node.H;
	
	%i = 0;
	
	while (%notInserted)
	{
		//if end of heap is reached
		if (%this.list $= "")
		{
			%this.list = %newList SPC %node;
			%this.indices[%node.X, %node.Y] = %i;
			break;
		}
		
		//declare the current Node
		%currentNode = getWord(%this.list, 0);
		
		if (%currentNode.G + %currentNode.H <= %nodeF)
		{
			if (%newList $= "")
				%newList = %currentNode;
			else
				%newList = %newList SPC %currentNode;
				
			%this.list = removeWord(%this.list, 0);
		}
		else
		{
			if (%newList $= "")
				%newList = %node;
			else
				%newList = %newList SPC %node;
			
			%this.indices[%node.X, %node.Y] = %i;
				
			%this.increaseIndices();
				
			%this.list = %newList SPC %this.list;
			
			%notInserted = false;
		}
		%i++;
	}
	%this.count++;
}

function FHeap::increaseIndices(%this)
{
	for (%i = 0; %i < %this.list.count; %i++)
	{
		%node = getWord(%this.list, %i);
		%this.indices[%node.X, %node.Y]++;
	}
}

function FHeap::hasNode(%this, %X, %Y)
{
	if (%this.hash[%X, %Y] == true)
		return true;
	else
		return false;
}

function FHeap::setNode(%this, %X, %Y, %G, %Parent)
{
	%i = %this.indices[%X, %Y];
	%newNode = getWord(%this.list, %i);
	%newNode.G = %G;
	%newNode.Parent = %Parent;
	setWord(%this.list, %i, %newNode);
}

function FHeap::member(%this, %node)
{
	if (%this.hash[%node.X, %node.Y] == true)
		return true;
	else
		return false;
}

function FHeap::peekMin(%this)
{
	return getWord(%this.list, 0);
}

function FHeap::removeMin(%this)
{
	%result = getWord(%this.list, 0);
	%this.list = removeWord(%this.list, 0);
	%this.hash[%result.X, %result.Y] = false;
	%this.count--;
	return %result;
}