// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Elements\packages.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Montag, 4. November 2013 22:10
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//Classes
//Projectile
/*
Fields:
healType - determines whether it heals HP or MP
*/

///create a Package around an obstacle
function createPackage(%obstacle)
{
	//calculate Position
	%posX = "";
	%posY = "";
	%obssize = %obstacle.SizeX;
	%obsPosX = getWord(%obstacle.Position, 0);
	%obsPosY = getWord(%obstacle.Position, 1);
	//create random Position and check if it is inside the map borders
	%posX = %obsPosX + getRandom(-%obssize * 0.6, %obssize * 0.6);
	%posY = %obsPosY + getRandom(-%obssize * 0.6, %obssize * 0.6);
	//if so, return
	if (%posX + 0.5 < $map.SizeX / 2 &&
		%posX - 0.5 > -$map.SizeX / 2 &&
		%posY + 0.5 > $map.SizeY / 2 &&
		%posY - 0.5 > -$map.SizeY / 2)
			return;
		
	//create Package
	%pack = new ShapeVector(Package);
	//determine whether it contains HP or MP
	%pack.healType = "";
	%healType = getRandom(0, 1);
	if (%healType == 0)
	{
		%pack.healType = "HP";
		%pack.setLineColor( 0.5, 1, 0.5);
		%pack.healAmount = $packageHealAmountHP;
	}
	else
	{
		%pack.healType = "MP";
		%pack.setLineColor( 0.5, 0.5, 1);
		%pack.healAmount = $packageHealAmountMP;
	}
	//create Graphics
 	%pack.setIsCircle( true );
	%pack.setCircleRadius( 0.5 );
	%pack.setFillMode(true);
	%pack.setFillColor("0 0 0");
	
	
	%pack.Position = %posX SPC %posY;
	
	//set Scene related Properties
	%pack.SceneGroup = 5;
	%pack.SceneLayer = 15;
	%pack.obstacle = %obstacle;
	
	//physics
	%pack.radius = 0.5;
	%pack.createCircleCollisionShape( %pack.radius );
	%pack.setCollisionGroups( 1, 2, 4, 5 );
	%pack.setCollisionCallback(true);
	%pack.setCollisionShapeIsSensor(0, true);
	%pack.setFixedAngle(true);
	
	//add to Scene
	Level.add(%pack);
}

function Package::onCollision(%this, %obj, %details)
{
	if (%obj.SceneGroup == $character.SceneGroup)
	{
		if(%this.healType $= "HP")
			%obj.addHP(%this.healAmount);
		else
			%obj.addMP(%this.healAmount);
	}
	schedule(30000, 0, createPackage, %this.obstacle);
	schedule(1, 0, deleteObj, %this);
}