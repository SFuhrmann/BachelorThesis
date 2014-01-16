// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\Elements\character.cs
// Copyright          :  
// Author             :  -
// Created on         :  Samstag, 2. November 2013 10:04
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Classes:
//Character -> Sprite
/*
Fields:
maxSpeed - maximum possible speed
acceleration - acceleration in 1/100 meters/ms^2
shootingFequency - delay till new shoot in ms
projectileSpeed - base speed of projectiles


*/

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Declaration              //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

function createCharacter(%pos)
{
	//Sprite Properties
	$character = new Sprite( Character );
	$character.setBodyType( dynamic );
	$character.Position = %pos;
	$character.Size = "6 6";
	$character.SceneLayer = 10;
	$character.SceneGroup = 2;
	$character.Image = "Game:Character";
	$character.radius = 2;
	$character.createCircleCollisionShape($character.radius);
	$character.setBlendColor(" 0.5 0.5 1" );
	$character.setCollisionGroups( None );
	$character.setCollisionCallback(true);
	$character.setFixedAngle(true);
	$character.saveLinearDamping = 1.5;
	$character.setLinearDamping($character.saveLinearDamping);
	
	//States Properties:
	//Movement
	$character.maxSpeed = 20;
	$character.saveMaxSpeed = $character.maxSpeed;
	$character.acceleration = 3;
	
	//Shooting
	$character.shootingFrequency = 350;
	$character.projectileSpeed = 25;
	$character.projectileDamage = 2;
	
	//Values
	$character.maxHP = 100;
	$character.maxMP = 3;
	$character.HP = $character.maxHP;
	$character.MP = $character.maxMP;
	$character.MPCostFactor = 1;
	$character.creditsMultiplier = 1.0;
	
	//Cooldowns
	$character.cooldownTime = 10000;
	$character.leapCooldownTime = 10000;
	
	//Stun
	$character.stunLength = 5000;
	$character.stunRadius = 10;
	
	//Leap
	$character.leapCosts = 1;
	
	//Beam
	$character.beamGrowth = 0.5;
	$character.beamSpeed = 30;
	
	//Upgrades
	//create list with available upgrades
	$character.availableItems = "";
	if (!$saveGame.HP == 0)
		$character.availableItems = addWord($character.availableItems, "LP");
	if (!$saveGame.MP == 0)
		$character.availableItems = addWord($character.availableItems, "AP");
	if (!$saveGame.shotSpeed == 0)
		$character.availableItems = addWord($character.availableItems, "shotSpeed");
	if (!$saveGame.stunLength == 0)
		$character.availableItems = addWord($character.availableItems, "stunLength");
	if (!$saveGame.speed == 0)
		$character.availableItems = addWord($character.availableItems, "speed");
	if (!$saveGame.leapCosts == 0)
		$character.availableItems = addWord($character.availableItems, "leapCosts");
	if (!$saveGame.damage == 0)
		$character.availableItems = addWord($character.availableItems, "damage");
	if (!$saveGame.beamGrowth == 0)
		$character.availableItems = addWord($character.availableItems, "beamGrowth");
	if (!$saveGame.credits == 0)
		$character.availableItems = addWord($character.availableItems, "credits");
	
	//initialize all UpgradeLevels:
	$character.HPUpgrades = 0;
	$character.MPUpgrades = 0;
	$character.shotSpeedUpgrades = 0;
	$character.stunLengthUpgrades = 0;
	$character.leapCostsUpgrades = 0;
	$character.beamGrowthUpgrades = 0;
	$character.speedUpgrades = 0;
	$character.damageUpgrades = 0;
	$character.creditsUpgrades = 0;
	
	//add to Scene
	Level.add( $character );
	Window.mount( $character );
	
	//update all states and call all common functions
	$character.Update();
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Update                   //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///update all states and call all reocurring functions
function Character::Update(%this)
{
	//align Character to Mouse
	if (!$gameMenu)
		%this.alignToMouse();
	
	//update once every frame
	%this.updateSchedule = %this.schedule(16, Update);
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Movement                 //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

function Character::pressW(%this)
{
	if (!$gameOver && !$nextStage && !$gameMenu) 
		$character.walkup();
	if ($nextStage)
		%this.addItem(WIcon.id);
}
function Character::pressA(%this)
{
	if (!$gameOver && !$nextStage && !$gameMenu) 
	$character.walkleft();
}
function Character::pressS(%this)
{
	if (!$gameOver && !$nextStage && !$gameMenu) 
		$character.walkdown();
}
function Character::pressD(%this)
{
	if (!$gameOver && !$nextStage && !$gameMenu) 
		$character.walkright();
}
function Character::pressQ(%this)
{
	if ($nextStage)
		$character.addItem(QIcon.id); 
}

function Character::pressE(%this)
{
	if ($nextStage) 
		$character.addItem(EIcon.id); 
}

//----------------------------------
function Character::upA(%this)
{
	if (!$gameOver && !$nextStage) 
		$character.stopwalkleft();
}
function Character::upS(%this)
{
	if (!$gameOver && !$nextStage) 
		$character.stopwalkdown();
}
function Character::upD(%this)
{
	if (!$gameOver && !$nextStage) 
		$character.stopwalkright();
}
function Character::upQ(%this)
{
	if (!$gameOver && !$nextStage && !$blockQWE && !$gameMenu) 
		$character.stun();
	if($blockQWE)
		$blockQWE = false;
}
function Character::upW(%this)
{
	if (!$gameOver && !$nextStage && !$blockQWE && !$gameMenu) 
		$character.stopwalkup();
	if($blockQWE)
		$blockQWE = false;
}
function Character::upE(%this)
{
	if (!$gameOver && !$nextStage && !$blockQWE && !$gameMenu) 
		$character.beam();
	if($blockQWE)
		$blockQWE = false;
}
//--------------------------------------
///Walk Left
function Character::walkleft(%this)
{
	if (%this.leaping)
		return;
	
	//add acceleration to velocity
	%this.setLinearVelocityX(%this.getLinearVelocityX() - %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkLschedule = %this.schedule(100, walkleft);
}

///Walk Right
function Character::walkright(%this)
{
	if (%this.leaping)
		return;
	
	//add acceleration to velocity
	%this.setLinearVelocityX(%this.getLinearVelocityX() + %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkRschedule = %this.schedule(100, walkright);
}

///Walk Up
function Character::walkup(%this)
{
	if (%this.leaping)
		return;
	
	//add acceleration to velocity
	%this.setLinearVelocityY(%this.getLinearVelocityY() + %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkUschedule = %this.schedule(100, walkup);
}

///Walk Down
function Character::walkdown(%this)
{
	if (%this.leaping)
		return;
	
	//add acceleration to velocity
	%this.setLinearVelocityY(%this.getLinearVelocityY() - %this.acceleration);
	
	//clamp speed to maximum speed
	%this.clampspeed();
	
	//re-schedule this function
	%this.walkDschedule = %this.schedule(100, walkdown);
}

///Stop Acceleration Left
function Character::stopwalkleft(%this)
{
	//cancel schedule
	cancel(%this.walkLschedule);
	%this.saveWalkL = false;
}

///Stop Acceleration Right
function Character::stopwalkright(%this)
{
	//cancel schedule
	cancel(%this.walkRschedule);
	%this.saveWalkR = false;
}

///Stop Acceleration Up
function Character::stopwalkup(%this)
{
	//cancel schedule
	cancel(%this.walkUschedule);
	%this.saveWalkU = false;
}

///Stop Acceleration Down
function Character::stopwalkdown(%this)
{
	//cancel schedule
	cancel(%this.walkDschedule);
	%this.saveWalkD = false;
}

///clamp velocity to maxSpeed
function Character::clampspeed(%this)
{
	//calculate length of velocity vector
	if(getWord(%this.getLinearVelocityPolar(), 1) > %this.maxSpeed)
	{
		//set to maxspeed if it exceeds it
		%this.setLinearVelocityPolar(getWord(%this.getLinearVelocityPolar(), 0), %this.maxSpeed);	
	}
}

function Character::stopMoving(%this)
{
	cancel(%this.walkDSchedule);
	cancel(%this.walkLSchedule);
	cancel(%this.walkRSchedule);
	cancel(%this.walkUSchedule);
	cancel(%this.shootSchedule);
	%this.saveWalkU = false;
	%this.saveWalkD = false;
	%this.saveWalkR = false;
	%this.saveWalkL = false;
}

///stops Character from taking Move Orders for %time ms
function Character::noMoving(%this, %time)
{
	%this.leaping = true;
	%this.turnOffNoMovingSchedule = %this.schedule(%time, turnOffNoMoving);
}
function Character::turnOffNoMoving(%this)
{
	%this.leaping = false;
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Shooting                 //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///align the Character to the Mouse
function Character::alignToMouse(%this)
{
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(Window.getMousePosition(), 0) - getWord(%this.Position, 0);
	%dY = getWord(Window.getMousePosition(), 1) - getWord(%this.Position, 1);
	
	if (mPow(%dX + %dY, 2) < 1)
		return;
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
	
	//align character to angle
	%this.Angle = %targetAngle;
}

///shoot a projectile
function Character::shoot(%this)
{
	if (%this.leaping)
		return;
		
	//make sure the attack is not on cooldown
	if (%this.attackOnCoolDown)
	{
		//re-schedule this function, if player keeps pressing the mousebutton
		%this.shootSchedule = %this.schedule(%this.shootingFrequency, shoot);
		return;
	}
	//convert local position of top-middle point of the Character to World Coordinates
	%position = %this.getWorldPoint(0, getWord(%this.Size, 1) / 2);
	
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(Window.getMousePosition(), 0) - getWord(%this.Position, 0);
	%dY = getWord(Window.getMousePosition(), 1) - getWord(%this.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	//copy projectile speed to the speed of this projectile
	%speed = %this.projectileSpeed;
	
	//get Linear Velocity along the axes and multiply it with a constant
	%veloX = %this.getLinearVelocityX() * $speedInfluenceOnProjectiles;
	%veloY = %this.getLinearVelocityY() * $speedInfluenceOnProjectiles;
	
	//create the Projectile
	createProjectile(%position, %direction, %speed, %veloX, %veloY, %this);
	
	//re-schedule this
	%this.shootSchedule = %this.schedule(%this.shootingFrequency, shoot);
	
	//turn on cooldown
	%this.attackOnCooldown = true;
	%this.coolDownSchedule = %this.schedule(%this.shootingFrequency - 1, turnOffCooldown);
}

///reset cooldown
function Character::turnOffCooldown(%this)
{
	%this.attackOnCooldown = false;
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Leaping                  //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///leap to mouse position
function Character::leap(%this, %pos)
{
	if (%this.leaping ||(%this.MP < %this.leapCosts) || %this.leapingCooldown)
		return;
	%this.addMP(-%this.leapCosts * %this.MPCostFactor);
	%this.leaping = true;
	%this.leapingCooldown = true;
	
	//cancel all walk schedules and save if they were pending
	if (isEventPending(%this.walkRschedule))
	{
		cancel(%this.walkRschedule);
		%this.saveWalkR = true;
	}
	if (isEventPending(%this.walkLschedule))
	{
		cancel(%this.walkLschedule);
		%this.saveWalkL = true;
	}
	if (isEventPending(%this.walkUschedule))
	{
		cancel(%this.walkUschedule);
		%this.saveWalkU = true;
	}
	if (isEventPending(%this.walkDschedule))
	{
		cancel(%this.walkDschedule);
		%this.saveWalkD = true;
	}
	//set max Speed and Linear Damping to 0
	%this.maxSpeed = 0;
	%this.setLinearDamping(0);
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(%pos, 0) - getWord(%this.Position, 0);
	%dY = getWord(%pos, 1) - getWord(%this.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	//set velocity to 20 in this direction
	%leapVelocity = 100;
	%this.setLinearVelocityPolar(%direction, %leapVelocity);
	
	//calculate distance and time until character will arrive at the destination
	%dist = VectorDist(%this.Position, %pos);
	%time = calculateArrivalTime(%dist, %leapVelocity);
	
	%maxTime = 300;
	if (%time > %maxTime)
		%time = %maxTime;
	//schedule the stop leap function with the calculated time
	%this.leapingSchedule = %this.schedule(%time, stopLeap);
	
	LeapIcon.setImageFrame(0);
	LeapIcon.cooldownSchedule = LeapIcon.schedule(%this.leapCooldownTime / 20, updateCooldown);
}

/// stop leaping
function Character::stopLeap(%this)
{
	%this.setLinearVelocity("0 0");
	%this.maxSpeed = %this.saveMaxSpeed;
	%this.setLinearDamping(%this.saveLinearDamping);
	%this.leaping = false;
	
	//re-call walk schedules
	if (%this.saveWalkR)
	{
		%this.walkright();
		%this.saveWalkR = false;
	}
	if (%this.saveWalkL)
	{
		%this.walkleft();
		%this.saveWalkL = false;
	}
	if (%this.saveWalkU)
	{
		%this.walkup();
		%this.saveWalkU = false;
	}	
	if (%this.saveWalkD)
	{
		%this.walkdown();
		%this.saveWalkD = false;
	}
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Stunning                 //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///stun after a short amount of time
function Character::stun(%this)
{
	if (%this.stunning ||(%this.MP < 1) || %this.stunningCooldown)
		return;
	%this.addMP(-1 * %this.MPCostFactor);
	%this.stunning = true;
	%this.stunningCooldown = true;
	
	createStunRingAnimation(%this.Position, %this);
	
	%this.callStunSchedule = %this.schedule($callStunTime, stunImpact);
	
	StunIcon.setImageFrame(0);
	StunIcon.cooldownSchedule = StunIcon.schedule(%this.cooldownTime / 20, updateCooldown);
}

///impact of the Stun
function Character::stunImpact(%this)
{
	%this.stunning = false;
	alxPlay("Game:stun");
	if (VectorDist(%this.Position, $enemy.Position) <= $character.stunRadius)
	{
		$enemy.stunned();
	}
}

//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//                 Beam                  //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//


function Character::beam(%this)
{
	if (%this.beamCooldown || %this.MP < 1)
		return;
	
	%this.beamCooldown = true;
	%this.addMP(-1 * %this.MPCostFactor);
	
	//convert local position of top-middle point of the Character to World Coordinates
	%position = %this.getWorldPoint(0, getWord(%this.Size, 1) / 2);
	
	//get differences on X and Y axes between Character and MousePointer
	%dX = getWord(Window.getMousePosition(), 0) - getWord(%this.Position, 0);
	%dY = getWord(Window.getMousePosition(), 1) - getWord(%this.Position, 1);
	//calculate the direction from that
	%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	//create a beam in the direction
	createBeam(%position, %direction);
	
	//call cooldown
	BeamIcon.setImageFrame(0);
	BeamIcon.cooldownSchedule = BeamIcon.schedule(%this.cooldownTime / 20, updateCooldown);
}


//#######################################//
//---------------------------------------//
//                                       //
//                                       //
//              Healing                  //
//                                       //
//                                       //
//---------------------------------------//
//#######################################//

///check HP in a certain Interval
function Character::checkLoseHPAmount(%this)
{
	if (%this.saveLoseHPAmount - %this.HP > 60)
	{
		$geneticModule.createNextGenerationKill(0.9);
	}
	
	%this.saveLoseHPAmount = %this.HP;
	$characterLoseALotOfHPKillSchedule = %this.schedule(10000, checkLoseHPAmount);
}

///heal HP by %amount
function Character::addHP(%this, %amount)
{
	%this.HP += %amount;
	//truncate
	if (%this.HP > %this.maxHP)
		%this.HP = %this.maxHP;
		
	//if character HP is zero
	if (%this.HP < 1)
	{
		%this.HP = 0;
		%this.die();
	}
	
	HPMeterFill.update();
	//schedule next generation survive function of genetic module
	if (%amount < 0 && isEventPending($characterSameHPSurviveSchedule))
	{
		cancel($characterSameHPSurviveSchedule);
		$characterSameHPSurviveSchedule = $geneticModule.schedule(20000, createNextGenerationKill, 0.1);
	}
}

///heal MP by %amount
function Character::addMP(%this, %amount)
{
	%this.MP += %amount;
	//truncate
	if (%this.MP > %this.maxMP)
		%this.MP = %this.maxMP;
	MPMeter.fill[0].update(0, $character.MP);
}

function Character::die(%this)
{	
	saveGame();
	
	schedule(1, 0, deleteObj, %this);
	
	%gameover = new Sprite( GameOverScreen );
	%gameover.Position = "0 0";
	%gameover.Size = "80 45";
	%gameover.SceneGroup = 20;
	%gameover.SceneLayer = 0;
	%gameover.Image = "Game:GameOver";
	$gameOver = true;
	Interface.add(%gameover);
}

///flash Character Sprite
function Character::flash(%this)
{
	if (%this.flashing)
		return;
		
	%this.flashing = true;
	//set Color to white
	%this.saveColor = %this.getBlendColor();
	%this.setBlendColor( "1 1 1" );
	
	//schedule flash update
	%this.flashSchedule = %this.schedule(16 * $flashTime, updateFlash);
}

///update Flash
function Character::updateFlash(%this)
{
	%this.setBlendColor(%this.saveColor);
	%this.flashing = false;
}

function Character::changeMaxHP(%this, %amount)
{
	%this.maxHP += %amount;
	destroyHPBar();
	createHPBarOutline();
	createHPBarFill();
}

function Character::changeMaxMP(%this, %amount)
{
	%this.maxMP += %amount;
	destroyMPBars();
	createMPBarOutlines();
	createMPBarFill();
}

function Character::addItem(%this, %i)
{
	%item = getWord(%this.availableItems, %i);
	
	switch$(%item)
	{
		case "LP":
			%this.changeMaxHP(20);
			%this.HPUpgrades++;
			if (%this.HPUpgrades >= $saveGame.HP)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "AP":
			%this.changeMaxMP(1);
			%this.MPUpgrades++;
			if (%this.MPUpgrades >= $saveGame.MP)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "shotSpeed":
			%this.shotSpeed += 5;
			%this.shotSpeedUpgrades++;
			if (%this.shotSpeedUpgrades >= $saveGame.shotSpeed)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "stunLength":
			%this.stunLength += 1000;
			%this.stunLengthUpgrades++;
			if (%this.stunLengthUpgrades >= $saveGame.stunLength)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "speed":
			%this.maxSpeed += 4;
			%this.speedUpgrades++;
			%this.acceleration = %this.maxSpeed / 5;
			if (%this.speedUpgrades >= $saveGame.speed)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "leapCosts":
			%this.leapCosts *= 0.9;
			%this.leapCostsUpgrades++;
			if (%this.leapCostsUpgrades >= $saveGame.leapCosts)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "damage":
			%this.projectileDamage += 0.5;
			%this.damageUpgrades++;
			if (%this.damageUpgrades >= $saveGame.damage)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "beamGrowth":
			%this.beamGrowth += 0.1;
			%this.beamGrowthUpgrades++;
			if (%this.beamGrowthUpgrades >= $saveGame.beamGrowth)
				%this.availableItems = removeWord(%this.availableItems, %i);
		case "credits":
			%this.creditsMultiplier += 0.1;
			%this.creditsUpgrades++;
			if (%this.creditsUpgrades >= $saveGame.credits)
				%this.availableItems = removeWord(%this.availableItems, %i);
	}
	destroyNextStage();
	$nextStage = false;
	
	$blockQWE = true;
}

function Character::getCurrentUpgradeLevel(%this, %i)
{
	switch(%i)
	{
		case 0:
			return %this.HPUpgrades;
		case 1:
			return %this.MPUpgrades;
		case 2:
			return %this.ShotSpeedUpgrades;
		case 3:
			return %this.stunLengthUpgrades;
		case 4:
			return %this.leapCostsUpgrades;
		case 5:
			return %this.beamGrowthUpgrades;
		case 7:
			return %this.damageUpgrades;
		case 6:
			return %this.speedUpgrades;
		case 8:
			return %this.creditsUpgrades;
	}
}

function Character::resetBlendColor(%this)
{
	if (%this.flashing)
	{
		%this.resetBlendColorSchedule = %this.schedule(16 * $flashTime, resetBlendColor);
		return;
	}
	%this.setBlendColor("0.5 0.5 1");
}