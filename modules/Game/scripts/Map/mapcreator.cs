// ============================================================
// Project            :  Bachelor Thesis
// File               :  ..\GitHub\BachelorThesis\modules\Game\scripts\Map\mapcreator.cs
// Copyright          :  
// Author             :  -
// Created on         :  Sonntag, 3. November 2013 17:47
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

//classes:
//Map -> ScriptObject
/*
Fields:
border - contains the border of the map
size - contains the size of the map "x y", also sizeX and sizeY possible
obstacles - contains a list of all obstacles on the map
obstaclesAmount - the number of obstacles
*/

///create Map with %size and a number of %obstacles
///%size = "width height"
///%obstacles = int
function createMap(%size, %obstacles)
{
	//create a global instance to store all data about the map
	$map = new ScriptObject( Map );
	$map.Size = %size; //size of the map
	$map.SizeX = getWord(%size, 0);
	$map.SizeY = getWord(%size, 1);
	$map.obstaclesAmount = %obstacles; //amount of obstacles on the map
	
	//create the border
	$map.createBorders(%size);
	
	//create %obstacles random shapes on the map
	for (%i = 0; %i < %obstacles; %i++)
	{
		$map.createRandomObstacle();
	}
}

///create the map borders
function Map::createBorders(%this, %size)
{	
	//calculate the borders of the map
	%b1 = -(getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topleft
	%b2 = (getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topright
	%b3 = (getWord(%size, 0) / 2) SPC -(getWord(%size, 1) / 2); //bottomright
	%b4 = -(getWord(%size, 0) / 2) SPC -(getWord(%size, 1) / 2); //bottomleft
	%b5 = -(getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topleft
	
	//store them in a single String
	%borderPoints = %b1 SPC %b2 SPC %b3 SPC %b4 SPC %b5;
	
	//create VectorShape
	%border = new ShapeVector(MapBorder);
	%border.Size = %size;
	%border.setBodyType( static );
	%border.Position = "0 0";
	%border.PolyList = "-0.5 0.5 0.5 0.5 0.5 -0.5 -0.5 -0.5";
	%border.SceneLayer = 9;
	%border.SceneGroup = 0;
	%border.setCollisionGroups( "1 2 3" );
	%border.createChainCollisionShape( %borderPoints );
	%border.setFixedAngle(true);
	
	//save in the map structure
	$map.border = %border;
	
	//add to Scene
	Level.add( %border );
}

///creates a single Obstacle
function Map::createRandomObstacle(%this)
{
	//create a random number of points located somewhere in a space between
	//(-0.5/0.5) and (0.5/-0.5)
	%amountPoints = getRandom(0, $obstacleMaxPointAmount);
	%points = "";
	%pointno1 = "";
	for (%i = 0; %i < %amountPoints; %i++)
	{
		%point = getRandom(-0.5, 0.5) SPC getRandom(-0.5, 0.5);
		%points = %point SPC %points;
		if (%i == 0)
		{
			%pointno1 = %point;
		}
	}
	%points = %pointno1 SPC %points;
	
	//create the obstacle
	%obstacle = new ShapeVector(Obstacle);
	%obstacle.Size = getRandom(1, $obstacleMaxSize) SPC getRandom(1, $obstacleMaxSize);
	%obstacle.SizeX = getWord(%obstacle.Size, 0);
	%obstacle.SizeY = getWord(%obstacle.Size, 1);
	%obstacle.setBodyType( static );
	//calculate a Random Position, consider the size of this obstacle as well
	%posX = getRandom((-$map.SizeX + %obstacle.SizeX) / 2, ($map.SizeX - %obstacle.SizeX) / 2);
	%posY = getRandom((-$map.SizeY + %obstacle.SizeY) / 2, ($map.SizeY - %obstacle.SizeY) / 2);
	%obstacle.Position = %posX SPC %posY;
	%obstacle.PolyList = %points;
	%obstacle.SceneLayer = 9;
	%obstacle.SceneGroup = 0;
	%obstacle.setCollisionGroups( "1 2 3" );
	%obstacle.createChainCollisionShape( %points );
	%obstacle.setFixedAngle(true);
	
	//add to Scene
	Level.add(%obstacle);
}