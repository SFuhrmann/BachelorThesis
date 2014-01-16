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
$amountOfObstaclesOnMap = 25;

//Heal Packages
$packageHealAmountHP = 2.5;
$packageHealAmountMP = 0.08;

//Stunning
$callStunTime = 2000;

//Effects
$flashTime = 10;

//PowerUps
$averagePowerUpPause = 15000;

//Mine
$mineDamage = 50;

//Attraction
$attractionPointSize = 60;
$gravitPointInfluence = 0.85;

//AI Structures
$weakIndices = "0 1 6 10";
$AIDLSDEPTH = 2;
$amountDNAStrings = 50;
$geneticAlgorithmMutationRate = 20;

//EnemyValues
function setEnemyValues()
{
	$enemyMaxSpeed = 20;
	$enemyProjectileSpeed = 25;
	$enemyProjectileDamage = 2;
	$enemyMaxHP = 100;
	$enemyMaxMP = 3;
	$enemyInvisibilityLength = 3000;
	$enemyMineRadius = 5;
	$enemyMineDamage = 50;
	$gravitPointDuration = 3000;
	
	for (%i = 0; %i < 8; %i++)
	{
		$enemyUpgradeLevel[%i] = 0;
	}
}

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