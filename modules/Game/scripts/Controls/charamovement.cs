// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\Controls\charamovement.cs
// Copyright          :  Fuhrmann
// Author             :  Fuhrmann
// Created on         :  Samstag, 2. November 2013 10:01
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Classes
//CharaMovement -> ActionMap

function CharaMovement::Init_controls( %this )
{
	//Create our new ActionMap
	new ActionMap( charcontrols );

	//Press "a" to execute "PlayerShip::turnleft();"
	// Release "a" to execute "PlayerShip::stopturn();"
	
	charcontrols.bindCmd(keyboard, "a", "$character.pressA();", "$character.upA();");
	charcontrols.bindCmd(keyboard, "d", "$character.pressD();", "$character.upD();");
	charcontrols.bindCmd(keyboard, "w", "$character.pressW();", "$character.upW();");
	charcontrols.bindCmd(keyboard, "s", "$character.pressS();", "$character.upS();");
	charcontrols.bindCmd(keyboard, "q", "$character.pressQ();", "$character.upQ();");
	charcontrols.bindCmd(keyboard, "e", "$character.pressE();", "$character.upE();");
	charcontrols.bindCmd(keyboard, "escape", "openInGameMenu();", "");
	
	//Push the ActionMap on top of the stack
	charcontrols.push();
}

function CharaMovement::onTouchDown(%this, %touchID, %position)
{
	if ($gameOver)
	{
		schedule(1, 0, createMenu);
		return;
	}
	if ($nextStage)
	{
		return;
	}
	$character.shoot();
}

function CharaMovement::onTouchUp(%this, %touchID, %position)
{
	if ($gameOver || $nextStage)
		return;
		
	cancel($character.shootSchedule);
}

function CharaMovement::onRightMouseDown(%this, %id, %pos)
{
	if ($gameOver || $nextStage)
		return;
	
	$character.leap(%pos);
}