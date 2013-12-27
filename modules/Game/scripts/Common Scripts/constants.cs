// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\constants.cs
// Copyright          :  
// Author             :  -
// Created on         :  Samstag, 2. November 2013 22:40
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Projectiles
$speedInfluenceOnProjectiles = 0.5;

//Map
$mapSize = 200;

//Obstacles
$obstacleAveragePointAmount = 5;
$obstacleMaxSize = 20;
$amountOfObstaclesOnMap = 50;

//Heal Packages
$packageHealAmountHP = 2.5;
$packageHealAmountMP = 0.08;

//Stunning
$callStunTime = 2000;

//Effects
$flashTime = 10;

//PowerUps
$averagePowerUpPause = 15000;


function createSaveGame()
{
	$saveGame = new ScriptObject( SaveGame );
	$saveGame.existing = true;
	$saveGame.HP = 2;
	$saveGame.MP = 2;
	$saveGame.shotSpeed = 2;
	$saveGame.stunLength = 0;
	//$saveGame.stunRadius = 0;
	$saveGame.leapCosts = 0;
	//$saveGame.leapCooldown = 0;
	$saveGame.beamGrowth = 0;
	//$saveGame.beamSpeed = 0;
	$saveGame.currentScore = 0;
	$saveGame.speed = 0;
	$saveGame.damage = 0;
	$saveGame.credits = 0;
}