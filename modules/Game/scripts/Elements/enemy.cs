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
	%enemy.maxSpeed = 15;
	%enemy.saveMaxSpeed = %enemy.maxSpeed;
	%enemy.acceleration = 3;
	
	//Shooting
	%enemy.shootingFrequency = 350;
	%enemy.projectileSpeed = 15;
	%enemy.projectileDamage = 2;
	
	//Values
	%enemy.maxHP = 100;
	%enemy.maxMP = 3;
	%enemy.HP = %enemy.maxHP;
	%enemy.MP = %enemy.maxMP;
	
	//Cooldowns
	%enemy.cooldownTime = 10000;
	
	%enemy.AIBehavior = GOAPBehavior.createInstance();
	%enemy.addBehavior(%enemy.AIBehavior);
	
	%enemy.updateAngle();
	$enemy = %enemy;
	
	%enemy.createEnemyLifeBar();
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
	$currentScore += 5000;
	schedule(1, 0, deleteObj, %this);
	%this.barOutline.delete();
	%this.barFill.delete();
	schedule(5000, 0, createEnemy, "0 0");
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
	%this.setBlendColor( "1 1 1" );
	
	//schedule flash update
	%this.flashSchedule = %this.schedule(16, updateFlash, $flashTime);
}

///update Flash
function Enemy::updateFlash(%this, %i)
{
	//if old Color reached
	if (%i == 0)
	{
		%this.flashing = false;
		return;
	}
	
	//calculate new Color and set Color
	%newVal = getWord(%this.getBlendColor(), 1) - (0.5 / $flashTime);
	%this.setBlendColor( 1 SPC %newVal SPC %newVal );
	
	//re-schedule
	%this.flashSchedule = %this.schedule(16, updateFlash, %i - 1);
}

function Enemy::stunned(%this, %i)
{
	%standStill = new ScriptObject( StandStillAction);
	%standStill.initialize( $stunDuration / 250 );
	//save current Behavior 
	%this.AIBehavior.actionQueue.push(%this.AIBehavior.currentAction);
	//call stand still Behavior
	%this.AIBehavior.actionQueue.push(%standStill);
	
	%this.AIBehavior.executeNextBehavior();
	
	%this.stunned = true;
	%this.stunnedSchedule = %this.schedule($stunDuration, endStun);
}

function Enemy::endStun(%this)
{
	%this.stunned = false;
}