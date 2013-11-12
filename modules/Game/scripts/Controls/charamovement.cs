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
	
	charcontrols.bindCmd(keyboard, "a", "$character.walkleft();", "$character.stopwalkleft();");
	charcontrols.bindCmd(keyboard, "d", "$character.walkright();", "$character.stopwalkright();");
	charcontrols.bindCmd(keyboard, "w", "$character.walkup();", "$character.stopwalkup();");
	charcontrols.bindCmd(keyboard, "s", "$character.walkdown();", "$character.stopwalkdown();");
	
	//Push the ActionMap on top of the stack
	charcontrols.push();
}

function CharaMovement::onTouchDown(%this, %touchID, %position)
{
	$character.shoot();
}

function CharaMovement::onTouchUp(%this, %touchID, %position)
{
	cancel($character.shootSchedule);
}

function CharaMovement::onRightMouseDown(%this, %id, %pos)
{
	$character.leap(%pos);
}