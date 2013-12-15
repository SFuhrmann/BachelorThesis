// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Controls\menumovement.cs
// Copyright          :  
// Author             :  -
// Created on         :  Dienstag, 3. Dezember 2013 13:24
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

function MenuMovement::Init_controls( %this )
{
	//Create our new ActionMap
	new ActionMap( menucontrols );

	//Press "a" to execute "PlayerShip::turnleft();"
	// Release "a" to execute "PlayerShip::stopturn();"
	
	//Push the ActionMap on top of the stack
	menucontrols.push();
}

function MenuMovement::onTouchDown(%this, %touchID, %position)
{
	%objs = MainMenu.pickPoint(%position);
	for (%i = 0; %i < %objs; %i++)
	{
		
		%obj = getWord(%objs, %i);
		
		if (%obj.SceneGroup == 30)
			schedule(1, 0, createGame);
		
		if (%obj.SceneGroup == 29)
		{
			destroyMainMenuItems();
			schedule(1, 0, createUpgradeMenu);
		}
		if (%obj.SceneGroup == 28)
		{
			destroyUpgradeMenuItems();
			schedule(1, 0, createMenu);
		}
		if (%obj.SceneGroup < 9 && !(%obj.SceneGroup $= ""))
		{
			increaseUpgradeLevel(%obj.SceneGroup, %obj);
		}
	}
}