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
//MultiplyString

///multiplies all elements of %string with %a
function multiplyString(%string, %a)
{
	for (%i = 0; %i < %string.count; %i++)
	{
		%string = setWord(%string, %i, getWord(%string, %i) * %a);
	}
	return %string;
}

function deleteObj(%obj)
{
	%obj.delete();
}