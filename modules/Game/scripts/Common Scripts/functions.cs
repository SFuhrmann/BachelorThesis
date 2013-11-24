// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Common Scripts\functions.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Montag, 4. November 2013 16:56
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Help Functions:
//MultiplyString - multiplies all elements of %string with %a
//deleteObj - delete %obj. Can be used for self destruction via scheduling

///multiplies all elements of %string with %a
function multiplyString(%string, %a)
{
	for (%i = 0; %i < %string.count; %i++)
	{
		%string = setWord(%string, %i, getWord(%string, %i) * %a);
	}
	return %string;
}

///delete %obj. Can be used for self destruction via scheduling
function deleteObj(%obj)
{
	if (!isObject(%obj))
		return;
		
	%obj.delete();
}

///calculate the time needed for a distance
///returns time in ms
function calculateArrivalTime(%dist, %velo)
{
	return %dist / %velo * 1000;
}

///get Minimum
function mMin(%a, %b)
{
	if (%a < %b)
		return %a;
	else
		return %b;
}

///get Maximum
function mMax(%a, %b)
{
	if (%a > %b)
		return %a;
	else
		return %b;
}