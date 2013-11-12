// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Common Scripts\effects.cs
// Copyright          :  
// Author             :  -
// Created on         :  Sonntag, 10. November 2013 18:33
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

///create Corona Effect
function showGlare(%pos, %size, %time)
{
	%glare = new Sprite( GlareEffect );
	%glare.maxSize = %size * 10;
	%glare.Size = "0 0";
	%glare.Position = %pos;
	%glare.Image = "Game:Corona";
	%glare.setBlendAlpha(0);
	%glare.SceneGroup = 31;
	%glare.SceneLayer = 0;
	%glare.time = %time;
	%glare.updateSchedule = %glare.schedule(%time / 20, update, 0);
	return %glare;
}

function GlareEffect::update(%this, %i)
{
	if (%i > 19)
		schedule(1, 0, deleteObj, %this);
		
	%currentSize = 0;
	if (%i < 10)
	{
		%currentSize = %this.maxSize / 10 * %i;
		%this.setBlendAlpha(0.1 * %i);
	}
	else
	{
		%currentSize = %this.maxSize - (%glare.maxSize / 10 * (%i - 10));
		%this.setBlendAlpha(1 - (0.1 * (%i - 10)));
	}
	%this.Size = %currentSize SPC %currentSize;
	
	%this.updateSchedule = %this.schedule(%this.time / 20, update, %i + 1);
}