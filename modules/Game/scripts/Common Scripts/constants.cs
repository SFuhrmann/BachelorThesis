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
$packageHealAmountHP = 1.25;
$packageHealAmountMP = 0.04;

//Stunning
$callStunTime = 2000;

//Effects
$flashTime = 10;


function createSaveGame()
{
	$saveGame = new ScriptObject( SaveGame );
	$saveGame.existing = true;
	$saveGame.HP = 0;
	$saveGame.MP = 0;
	$saveGame.shotSpeed = 0;
	$saveGame.stunLength = 2;
	$saveGame.stunRadius = 0;
	$saveGame.leapCosts = 2;
	$saveGame.leapCooldown = 0;
	$saveGame.beamGrowth = 2;
	$saveGame.beamSpeed = 0;
	$saveGame.currentScore = 0;
}