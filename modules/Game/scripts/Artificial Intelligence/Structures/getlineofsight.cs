// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\getlineofsight.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Freitag, 27. Dezember 2013 20:38
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================
function CharacterIsInLineOfSight(%origin, %destination, %steps)
{
	%vector = VectorDist(%destination, %origin);
	%vectorLength = VectorLen(%vector);
	
	%vecPart = %steps;
	
	%vecX = getWord(%vector, 0);
	%vecY = getWord(%vector, 1);
	
	%unitVector = VectorNormalize(%vector);
	
	for (%i = 0; endNotReached(%vector, %vectorLength, %i, %vecPart); %i++)
	{
		%X = getCurrentPointX(%origin, %unitVector, %i, %vecPart);
		%Y = getCurrentPointY(%origin, %unitVector, %i, %vecPart);
		
		
		%objs = Level.pickPoint(%X, %Y);
		for (%j = 0; %j < %objs.count; %j++)
		{
			%obj = getWord(%objs, %j);
			if (%obj.SceneGroup == 5)
			{
				return false;
			}
		}
	}
	return true;
}

function endNotReached(%vector, %vectorLength, %i, %vecPart)
{
	return %i * %vecPart <= %vectorLength;
}

function getCurrentPointX(%origin, %unitVector, %i, %vecPart)
{
	return getWord(%origin, 0) + getWord(%unitVector, 0) * %i * %vecPart;
}

function getCurrentPointY(%origin, %unitVector, %i, %vecPart)
{
	return getWord(%origin, 1) + getWord(%unitVector, 1) * %i * %vecPart;
}