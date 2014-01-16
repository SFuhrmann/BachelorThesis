// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Elements\enemy.cs
// Copyright          :  
// Author             :  -
// Created on         :  Montag, 18. November 2013 22:43
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Enemy. Similiar to the Character. Uses Behaviors instead
//                    :  of a set of own functions, though.
//                    :  
// ============================================================

function createEnemy(%pos)
{
	//Sprite Properties
	%enemy = new Sprite( Enemy );
	%enemy.setBodyType( dynamic );
	%enemy.Position = %pos;
	%enemy.Size = "6 6";
	%enemy.SceneLayer = 10;
	%enemy.SceneGroup = 1;
	%enemy.setCollisionGroups( None );
	%enemy.Image = "Game:Character";
	%enemy.createCircleCollisionShape(2);
	%enemy.setBlendColor( "1 0.5 0.5" );
	%enemy.setFixedAngle( true );
	%enemy.setLinearDamping(2);
	Level.add( %enemy );
	
	//States Properties:
	//Movement
	%enemy.maxSpeed = $enemyMaxSpeed;
	%enemy.saveMaxSpeed = %enemy.maxSpeed;
	%enemy.acceleration = $enemyMaxSpeed / 5;
	
	//Shooting
	%enemy.shootingFrequency = 350;
	%enemy.projectileSpeed = $enemyProjectileSpeed;
	%enemy.projectileDamage = $enemyProjectileDamage;
	
	//Values
	%enemy.maxHP = $enemyMaxHP;
	%enemy.maxMP = $enemyMaxMP;
	%enemy.HP = %enemy.maxHP;
	%enemy.MP = %enemy.maxMP;
	%enemy.MPCostFactor = 1;
	
	//Invisibility
	%enemy.invisibilityLength = $enemyInvisibilityLength;
	
	//Mine
	%enemy.mineRadius = $enemyMineRadius;
	%enemy.mineDamage = $enemyMineDamage;
	
	//GravitPoint
	%enemy.gravitPointDuration = $gravitPointDuration;
	
	//Cooldowns
	%enemy.cooldownTime = 10000;
	
	
	$enemy = %enemy;
	%enemy.AIBehavior = GOAPBehavior.createInstance();
	%enemy.addBehavior(%enemy.AIBehavior);
	
	%enemy.updateAngle();
	
	
	%enemy.createEnemyLifeBar();
	%enemy.shoot();
}

function Enemy::addHP(%this, %amount)
{
	%this.HP += %amount;
	
	//if HP exceeds maxHP
	if (%this.HP > %this.maxHP)
		%this.HP = %this.maxHP;
	
	//if enemy HP is zero
	if (%this.HP < 1)
	{
		%this.HP = 0;
		%this.die();
	}
	//schedule next generation survive function of genetic module
	if (%amount < 0 && isEventPending($enemySameHPSurviveSchedule))
	{
		cancel($enemySameHPSurviveSchedule);
		$enemySameHPSurviveSchedule = $geneticModule.schedule(20000, createNextGenerationSurvive, 0.7);
	}
}

function Enemy::createEnemyLifeBar(%this)
{
	%bar = new ShapeVector( EnemyHPMeterOutline );
	%bar.setLineColor( 1, 0.5, 0.5);
	//create Graphics
	%bar.setPolyCustom(4, "-0.5 0.05 0.5 0.05 0.5 -0.05 -0.5 -0.05");
	%bar.Size = "12 10";
	%bar.Position = getWord(%this.Position, 0) SPC getWord(%this.Position, 1) + 4;
	%bar.SceneGroup = 30;
	%bar.SceneLayer = 1;
	
	//physics
	%bar.setFixedAngle(true);
	
	//add to Scene
	Level.add(%bar);
	%this.barOutline = %bar;
	
	%meter = new ShapeVector( EnemyHPMeterFill );
	%meter.setLineColor( 1, 0.5, 0.5);
	//create Graphics
	%meter.setPolyCustom(4, "-0.5 0.05 0.5 0.05 0.5 -0.05 -0.5 -0.05");
	%meter.Size = "11.5 5";
	%meter.Position = getWord(%this.Position, 0) SPC getWord(%this.Position, 1) + 4;
	%meter.SceneGroup = 30;
	%meter.SceneLayer = 1;
	
	//physics
	%meter.setFixedAngle(true);
	%meter.setFillMode(true);
	%meter.setFillColor( "1 0.5 0.5" );
	
	//add to Scene
	Level.add(%meter);
	
	%this.barFill = %meter;
	
	%meter.update();
}
function EnemyHPMeterFill::update(%this)
{
	%this.Size = clipToAnotherSize($enemy.HP, $enemy.maxHP, 11.5) SPC 5;
	%this.Position = $enemy.Position - 5.75 + getWord(%this.Size, 0) / 2 SPC getWord($enemy.Position, 1) + 4;
	
	$enemy.barOutline.Position = getWord($enemy.Position, 0) SPC getWord($enemy.Position, 1) + 4;
	
	$enemy.barUpdateSchedule = %this.schedule(16, update);
}

function Enemy::addMP(%this, %amount)
{
	%this.MP += %amount;
	
	//if HP exceeds maxHP
	if (%this.MP > %this.maxMP)
		%this.MP = %this.maxMP;
}

function Enemy::die(%this)
{
	if ($nextStage)
		return;
		
	$nextStage = true;
	addScore(1000 * $level);
	$level++;
	schedule(1, 0, deleteObj, %this);
	%this.barOutline.delete();
	%this.barFill.delete();
	$createEnemySchedule = schedule(15000, 0, createEnemy, "0 0");
	$character.stopMoving();
	$character.setLinearVelocity("0 0");
	createNextStage();
	if ($level < 15)
	{
		increaseBGSpeed();
	}
	%this.increaseOneUpgradeLevel();
	$geneticModule.createNextGenerationSurvive(0);
	$geneticModule.createNextGenerationKill(0.3);
}

function Enemy::updateAngle(%this)
{
	if (!%this.stunned)
	
	{
		//get differences on X and Y axes between Character and MousePointer
		%dX = getWord($character.Position, 0) - getWord(%this.Position, 0);
		%dY = getWord($character.Position, 1) - getWord(%this.Position, 1);
		
		//get angle
		%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
		
		//align character to angle
		%this.Angle = %targetAngle;
	}
	%this.updateAngleSchedule = %this.schedule(31, updateAngle);
}

///flash Enemy Sprite
function Enemy::flash(%this)
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
function Enemy::updateFlash(%this)
{	
	%this.setBlendColor(%this.saveColor);
	%this.flashing = false;
}

function Enemy::stunned(%this, %i)
{
	%standStill = new ScriptObject( StandStillAction);
	%standStill.initialize( $character.stunLength / 250 );
	//save current Behavior 
	%this.AIBehavior.actionQueue.push(%this.AIBehavior.currentAction);
	//call stand still Behavior
	%this.AIBehavior.actionQueue.push(%standStill);
	
	%this.AIBehavior.executeNextBehavior();
	
	%this.stunned = true;
	%this.saveMaxSpeed = %this.maxSpeed;
	%this.maxSpeed = 0;
	%this.stunnedSchedule = %this.schedule($character.stunLength, endStun);
}

function Enemy::endStun(%this)
{
	%this.stunned = false;
	%this.maxSpeed = %this.saveMaxSpeed;
}

function Enemy::shoot(%this)
{
	if (!%this.stunned)
	{	
		//convert local position of top-middle point of the Character to World Coordinates
		%position = %this.getWorldPoint(0, getWord(%this.Size, 1) / 2);
		
		//get differences on X and Y axes between Character and MousePointer
		%dX = getWord($character.Position, 0) - getWord(%this.Position, 0);
		%dY = getWord($character.Position, 1) - getWord(%this.Position, 1);
		//calculate the direction from that
		%direction = mRadToDeg(mAtan(%dY, %dX)) + 90;
		
		//copy projectile speed to the speed of this projectile
		%speed = %this.projectileSpeed;
		
		//get Linear Velocity along the axes and multiply it with a constant
		%veloX = %this.getLinearVelocityX() * $speedInfluenceOnProjectiles;
		%veloY = %this.getLinearVelocityY() * $speedInfluenceOnProjectiles;
		
		//create the Projectile
		createProjectile(%position, %direction, %speed, %veloX, %veloY, %this);
		
		//turn on cooldown
		%this.attackOnCooldown = true;
		%this.coolDownSchedule = %this.schedule(%this.shootingFrequency - 1, turnOffCooldown);
	}
	//re-schedule this
	%this.shootSchedule = %this.schedule(%this.shootingFrequency, shoot);
}

///reset cooldown
function Enemy::turnOffCooldown(%this)
{
	%this.attackOnCooldown = false;
}

function Enemy::resetBlendColor(%this)
{
	if (%this.flashing)
	{
		%this.resetBlendColorSchedule = %this.schedule(16 * $flashTime, resetBlendColor);
		return;
	}
	%this.setBlendColor("1 0.5 0.5");
}

//------------------------------------------//
//                                          //
//                                          //
//                                          //
//           AI Help Functions              //
//                                          //
//                                          //
//                                          //
//------------------------------------------//

///move the enemy towards %pos
function Enemy::moveTowards(%this, %pos)
{
	//get X and Y difference between goal and agent
	%dX = getWord(%pos, 0) - getWord(%this.Position, 0);
	%dY = getWord(%pos, 1) - getWord(%this.Position, 1);
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) + 90;
	
	%this.accelerateTowards(5, %targetAngle);
}

///move the enemy towards %pos
function Enemy::moveAwayFrom(%this, %pos)
{
		//get X and Y difference between goal and agent
	%dX = getWord(%pos, 0) - getWord(%this.Position, 0);
	%dY = getWord(%pos, 1) - getWord(%this.Position, 1);
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 90;
	
	%this.accelerateTowards(5, %targetAngle);
}

///move the enemy towards %pos
function Enemy::moveAroundCW(%this, %pos)
{
	//get X and Y difference between goal and agent
	%dX = getWord(%pos, 0) - getWord(%this.Position, 0);
	%dY = getWord(%pos, 1) - getWord(%this.Position, 1);
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX)) - 180;
	
	%this.clampspeed();
	%this.accelerateTowards(5, %targetAngle);
}

///move the enemy towards %pos
function Enemy::moveAroundCCW(%this, %pos)
{
	//get X and Y difference between goal and agent
	%dX = getWord(%pos, 0) - getWord(%this.Position, 0);
	%dY = getWord(%pos, 1) - getWord(%this.Position, 1);
	
	//get angle
	%targetAngle = mRadToDeg(mAtan(%dY, %dX));
	
	%this.accelerateTowards(5, %targetAngle);
}

///accelerate the enemy towards %targetAngle using a speed clamp
function Enemy::accelerateTowards(%this, %i, %targetAngle)
{
	if (%i < 0)
	{
		return;
	}
	%currentAngle = getWord(%this.getLinearVelocityPolar(), 0);
	%sign = mSign(%targetAngle - %currentAngle);
	
	//calculate new Angle
	%newAngle = %currentAngle + %sign * mMin(5, mAbs(%targetAngle - %currentAngle));
	
	%this.setLinearVelocityPolar(%newAngle, getWord(%this.getLinearVelocityPolar(), 1) + %this.acceleration / 2);
	%this.clampspeed();
	%this.accelerationSchedule = %this.schedule(50, accelerateTowards, %i - 1, %targetAngle);
}

///clamp velocity to maxSpeed
function Enemy::clampspeed(%this)
{
	//calculate length of velocity vector
	if(getWord(%this.getLinearVelocityPolar(), 1) > %this.maxSpeed)
	{
		//set to maxspeed if it exceeds it
		%this.setLinearVelocityPolar(getWord(%this.getLinearVelocityPolar(), 0), %this.maxSpeed);	
	}
}

///enemy sets a mine at its position
function Enemy::createMine(%this)
{
	if (%this.MP < 1 * %this.MPCostFactor || %this.mineCooldown)
		return;
	
	createMine(%this);
	%this.mineCooldown = true;
	%this.mineCooldownSchedule = %this.schedule(10000, mineCooldown);
	
	%this.addMP(-1 * %this.MPCostFactor);
}

///lets enemy become invisible
function Enemy::becomeInvisible(%this)
{
	if (%this.MP < 1 * %this.MPCostFactor || %this.invisibilityCooldown)
		return;
	
	%this.invisibilityCooldown = true;
	%this.setSrcBlendFactor(ZERO);
	EnemyHPMeterOutline.setSrcBlendFactor(ZERO);
	EnemyHPMeterFill.setSrcBlendFactor(ZERO);
	%this.resetInvisibilitySchedule = %this.schedule(%this.InvisibilityLength, resetInvisibility);
	
	%this.invisibilityCooldownSchedule = %this.schedule(10000, invisibilityCooldown);
	
	%this.addMP(-1 * %this.MPCostFactor);
	
	%this.invisible = true;
}

function Enemy::resetInvisibility(%this)
{
	%this.setSrcBlendFactor(SRC_ALPHA);
	EnemyHPMeterOutline.setSrcBlendFactor(SRC_ALPHA);
	EnemyHPMeterFill.setSrcBlendFactor(SRC_ALPHA);
	%this.invisible = false;
}

///shoots out a gravit point from enemys position
function Enemy::createGravitPoint(%this)
{
	if (%this.MP < 1 * %this.MPCostFactor || %this.gravitPointCooldown)
		return;
		
	shootGravitPoint(%this);
	this.gravitPointCooldown = true;
	alxPlay("Game:beamshoot");
	%this.addMP(-1 * %this.MPCostFactor);
	%this.gravitPointCooldownSchedule = %this.schedule(10000, gravitPointCooldown);
}

///lets enemy fire the gravit point
function Enemy::fireGravitPoint(%this)
{
	if (isObject(%this.GravitPoint))
		%this.GravitPoint.fire();
}

///changes a stat of the enemy
function Enemy::increaseUpgradeLevel(%this, %i)
{
	switch(%i)
	{
		case 0:
			$enemyMaxHP += 10;
		case 1:
			$enemyMaxMP += 1;
		case 2:
			$enemyProjectileSPeed += 2;
		case 3:
			$enemyMaxSpeed += 5;
		case 4:
			$enemyProjectileDamage += 0.5;
		case 5:
			$enemyMineDamage += 10;
		case 6:
			$gravitPointLength += 1000;
		case 7:
			$enemyInvisibilityLength += 1000;
	}
}

///searches for a levelable upgrade and increases it
function Enemy::increaseOneUpgradeLevel(%this)
{
	while (true)
	{
		%i = getRandom(0, 7);
		if ($enemyUgradeLevel[%i] < 6)
		{
			%this.increaseUpgradeLevel(%i);
			return;
		}
	}
}

function Enemy::mineCooldown(%this)
{
	%this.mineCooldown = false;
}

function Enemy::invisibilityCooldown(%this)
{
	%this.invisibilityCooldown = false;
}

function Enemy::gravitPointCooldown(%this)
{
	%this.gravitPointCooldown = false;
	$gravitPointProjectile.delete();
}

function Enemy::getAvailableActions(%this, %wp)
{
	%result = "";
	if (%wp.ownMP > 1)
	{
		if (!%wp.invisibilityCooldown)
		{
			//invisibility
			%action = new ScriptObject(BecomeInvisibleAction);
			%action.initialize();
			%result = addWord(%result, %action);
		}
		if (!%wp.mineCooldown)
		{
			//mine
			%action = new ScriptObject(SetMineAction);
			%action.initialize();
			%result = addWord(%result, %action);
		}
		if (!%wp.gravitPointCooldown)
		{
			//gravitpoint
			%action = new ScriptObject(ShootGravitPointAction);
			%action.initialize();
			%result = addWord(%result, %action);
		}
	}
	if (isObject($powerup))
	{
		//powerup
		%action = new ScriptObject(GetPowerupAction);
		%action.initialize();
		%result = addWord(%result, %action);
	}
	
	
	if (%this.inAcceptableBorder(%wp))
	{
		//away
		%action = new ScriptObject(FleeFromEnemyAction);
		%action.initialize(5);
		%result = addWord(%result, %action);
		//around_ccw
		%action = new ScriptObject(MoveAroundEnemyCCWAction);
		%action.initialize(5);
		%result = addWord(%result, %action);
		//around_cw
		%action = new ScriptObject(MoveAroundEnemyCWAction);
		%action.initialize(5);
		%result = addWord(%result, %action);
	}
	if (VectorDistSquared(%wp.ownPosition, %wp.enemyPosition) > 4)
	{
		//toward
		%action = new ScriptObject(FollowEnemyAction);
		%action.initialize(5);
		%result = addWord(%result, %action);
	}
	if (%wp.gravitPointProjectileExists)
	{
		//use gravitPoint
		%action = new ScriptObject(UseGravitPointAction);
		%action.initialize();
		%result = addWord(%result, %action);
	}
	//package
	%action = new ScriptObject(FindNearestPackageAction);
	%action.initialize();
	%result = addWord(%result, %action);
	//stand
	%action = new ScriptObject(StandStillAction);
	%action.initialize(5);
	%result = addWord(%result, %action);
	
	return %result;
}

///checks if the enemy is far away enough from the levelborders
function Enemy::inAcceptableBorder(%this, %wp)
{
	return getWord(%wp.ownPosition, 0) > -$mapSize/2 - 5 && getWord(%wp.ownPosition, 0) < $mapSize/2 - 5 && getWord(%wp.ownPosition, 1) > -$mapSize/2 - 5 && getWord(%wp.ownPosition, 1) < $mapSize/2 - 5;
}

//returns the number of Skills that are on Cooldown
function Enemy::getSkillsOnCooldown(%this)
{
	%i = 0;
	if (%this.invisibilityCooldown)
		%i++;
	if (%this.mineCooldown)
		%i++;
	if (%this.gravitPointCooldown)
		%i++;
	return %i;
}

//returns if the Character is inside the Gravit Point
function Enemy::getCharacterInGravitPoint(%this)
{
	return VectorDistSquared($character.Position, %this.GravitPoint.Position) < mPow(getWord(%this.GravitPoint.Size, 0), 2);
}