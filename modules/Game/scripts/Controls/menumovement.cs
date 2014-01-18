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
	menucontrols.bind(joystick, xaxis, menuJoystickLeftX);
	menucontrols.bind(joystick, yaxis, menuJoystickLeftY);
	menucontrols.bindCmd(joystick, button0, "menuConfirmJoystick();", "");
	menucontrols.bindCmd(joystick, button1, "mainMenuClick(UpgradeBackFont);", "");
	
	//Push the ActionMap on top of the stack
	menucontrols.push();
}

function MenuMovement::onTouchDown(%this, %touchID, %position)
{
	%objs = MainMenu.pickPoint(%position);
	for (%i = 0; %i < %objs.count; %i++)
	{
		
		%obj = getWord(%objs, %i);
		mainMenuClick(%obj);
		
	}
}

function mainMenuClick(%obj)
{
	if (%obj.SceneGroup == 30)
	{
		alxPlay("Game:MenuSelect");
		schedule(1, 0, createGame);
	}
	if (%obj.SceneGroup == 29)
	{
		destroyMainMenuItems();
		alxPlay("Game:MenuSelect");
		schedule(1, 0, createUpgradeMenu);
	}
	if (%obj.SceneGroup == 28)
	{
		destroyUpgradeMenuItems();
		alxPlay("Game:MenuBack");
		schedule(1, 0, createMenu);
	}
	if (%obj.SceneGroup < 9 && !(%obj.SceneGroup $= ""))
	{
		increaseUpgradeLevel(%obj.SceneGroup, %obj);
	}
}

function chooseSceneObject(%id)
{
	%sceneObjects = MainMenu.getSceneObjectList();
	for (%i = 0; %i < %sceneObjects.count; %i++)
	{
		%obj = getWord(%sceneObjects, %i);
		if (%obj.SceneGroup == %id)
		{
			return %obj;
		}
	}
	return false;
}

function menuJoystickLeftX(%value)
{
	%active = $mainMenuActiveItem.SceneGroup;
	if (%value > 0.5)
	{
		if (!$menuJoystickRight)
		{
			if (%active < 9)
			{
				if (!(%active % 3 == 2))
				{
					%obj = chooseSceneObject(%active + 1);
					mainMenuSetActiveItem(%obj);
				}
				else if(%active == 8)
				{
					mainMenuSetActiveItem(UpgradeBackFont);
				}
			}
			$menuJoystickRight = true;
		}
	}
	else if (%value < -0.5)
	{
		if (!$menuJoystickLeft)
		{
			if (%active < 9)
			{
				if (!(%active % 3 == 0))
				{
					%obj = chooseSceneObject(%active - 1);
					mainMenuSetActiveItem(%obj);
				}
			}
			if (%active == 28)
			{
				mainMenuSetActiveItem(chooseSceneObject(8));
			}
			$menuJoystickLeft = true;
		}
	}
	else
	{
		if ($menuJoystickLeft)
		{
			$menuJoystickLeft = false;
		}
		else if ($menuJoystickRight)
		{
			$menuJoystickRight = false;
		}
	}
}
function menuJoystickLeftY(%value)
{
	%active = $mainMenuActiveItem.SceneGroup;
	if (%value < -0.5)
	{
		if (!$menuJoystickUp)
		{
			if (%active < 9)
			{
				if (%active > 2)
				{
					%obj = chooseSceneObject(%active - 3);
					mainMenuSetActiveItem(%obj);
				}
			}
			if (%active == 28)
			{
				mainMenuSetActiveItem(chooseSceneObject(8));
			}
			if (%active == 29)
			{
				mainMenuSetActiveItem(StartFont);
			}
			$menuJoystickUp = true;
		}
	}
	else if (%value > 0.5)
	{
		if (!$menuJoystickDown)
		{
			if (%active < 9)
			{
				if (%active < 6)
				{
					%obj = chooseSceneObject(%active + 3);
					mainMenuSetActiveItem(%obj);
				}
				if (%active > 5)
				{
					mainMenuSetActiveItem(UpgradeBackFont);
				}
			}
			if (%active == 30)
			{
				mainMenuSetActiveItem(UpgradeFont);
			}
			$menuJoystickDown = true;
		}
	}
	else
	{
		if ($menuJoystickUp)
		{
			$menuJoystickUp = false;
		}
		else if ($menuJoystickDown)
		{
			$menuJoystickDown = false;
		}
	}
}