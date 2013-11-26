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
	
	charcontrols.bindCmd(keyboard, "a", "if (!$gameOver) $character.walkleft();", "if (!$gameOver) $character.stopwalkleft();");
	charcontrols.bindCmd(keyboard, "d", "if (!$gameOver) $character.walkright();", "if (!$gameOver) $character.stopwalkright();");
	charcontrols.bindCmd(keyboard, "w", "if (!$gameOver) $character.walkup();", "if (!$gameOver) $character.stopwalkup();");
	charcontrols.bindCmd(keyboard, "s", "if (!$gameOver) $character.walkdown();", "if (!$gameOver) $character.stopwalkdown();");
	charcontrols.bindCmd(keyboard, "q", "", "if (!$gameOver) $character.stun();");
	charcontrols.bindCmd(keyboard, "e", "", "if (!$gameOver) $character.beam();");
	
	//Push the ActionMap on top of the stack
	charcontrols.push();
}

function CharaMovement::onTouchDown(%this, %touchID, %position)
{
	if ($gameOver)
	{
		schedule(1, 0, createGame);
		return;
	}
	$character.shoot();
}

function CharaMovement::onTouchUp(%this, %touchID, %position)
{
	if ($gameOver)
		return;
		
	cancel($character.shootSchedule);
}

function CharaMovement::onRightMouseDown(%this, %id, %pos)
{
	if ($gameOver)
		return;
	
	$character.leap(%pos);
}