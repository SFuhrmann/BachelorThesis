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

///add a word to a string
function addWord(%string, %word)
{
	if (%string $= "")
		return %word;
	else
		return %string SPC %word;
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

///add an amount to the score and Update the Font
function addScore(%amount)
{
	if (!$gameOver)
	{
		$currentScore += %amount * $character.creditsMultiplier;
		Score.update();
	}
}

///Returns %val rounded to the %i'th number
///Use negative values for floating point roundings
function getRounded(%val, %i)
{
	if (%i < 0)
	{
		%factor = mPow(10, - %i);
		return mRound(%val * %factor) / %factor;
	}
	else
	{
		%factor = mPow(10, %i);
		return mRound(%val / %factor) * %facotr;
	}
}

///tests if %pos is in sight of %obj
function isInSight(%pos, %obj)
{
	if (getWord(%pos, 0) - getWord(%obj.Position, 0) <= 40 && getWord(%pos, 1) - getWord(%obj.Position, 1) <= 22.5)
		return true;
	else
		return false;
}

///tests if %pos is in sight of the Character
function isOnScreen(%pos)
{
	if (mAbs(getWord(%pos, 0) - getWord($character.Position, 0)) <= 40 && mAbs(getWord(%pos, 1) - getWord($character.Position, 1)) <= 22.5)
		return true;
	else
		return false;
}

///rounds every element of a vector to the next Integer Value
function roundVector(%vec)
{
	return mRound(getWord(%vec, 0)) SPC mRound(getWord(%vec, 1));
}