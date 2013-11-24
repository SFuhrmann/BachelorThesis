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
	echo(enemydead);
	
	schedule(1, 0, deleteObj, %this);
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
		return;
	
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