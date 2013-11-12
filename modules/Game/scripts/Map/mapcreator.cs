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
function createMap(%size)
{
	//create a global instance to store all data about the map
	$map = new ScriptObject( Map );
	$map.Size = %size; //size of the map
	$map.SizeX = getWord(%size, 0);
	$map.SizeY = getWord(%size, 1);
	$map.obstaclesAmount = $amountOfObstaclesOnMap; //amount of obstacles on the map
	$map.obstacles = "";
	
	//create the border
	$map.createBorders(%size);
	
	//create %obstacles random shapes on the map
	for (%i = 0; %i < $amountOfObstaclesOnMap; %i++)
	{
		$map.createRandomObstacle(%i);
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
	%border = new ShapeVector(MapObject);
	%border.Size = %size;
	%border.setBodyType( static );
	%border.Position = "0 0";
	%border.PolyList = "-0.5 0.5 0.5 0.5 0.5 -0.5 -0.5 -0.5";
	%border.SceneLayer = 9;
	%border.SceneGroup = 4;
	%border.setCollisionGroups( "1 2 3" );
	%border.createChainCollisionShape( %borderPoints );
	%border.setFixedAngle(true);
	
	//save in the map structure
	$map.border = %border;
	
	//add to Scene
	Level.add( %border );
}

///creates a single Obstacle
function Map::createRandomObstacle(%this, %id)
{
	//create a random number of points located somewhere in a space between
	//(-0.5/0.5) and (0.5/-0.5):
	
	//declare an alpha angle, a random amount will be added to it each round
	//the next point lays on the circle around the center of the obstacle with a radius of r
	//and at the angle of alpha
	%averageAngle = 360 / $obstacleAveragePointAmount;
	%alpha = getRandom(%averageAngle / 2, %averageAngle * 1.5);
	%radius = 0.5;
	%points = "";
	%point1 = "";
	while (%alpha <= 359)
	{
		//calculate the radiant of %alpha
		%radiant = mDegToRad(%alpha);
		//calculate the local %X and %Y coordinates from that:
		%x = mCos(%radiant) * %radius;
		%y = mSin(%radiant) * %radius;
		
		%point = %x SPC %y;
		//add to list
		if (%points $= "")
		{
			%points = %point;
			%point1 = %point;
		}
		else
		{
			%points = %point SPC %points;
		}
		//add a random value to alpha
		%alpha += getRandom(%averageAngle / 2, %averageAngle * 1.5);
	}
	//create the obstacle
	%obstacle = new ShapeVector(MapObject);
	%obstacle.id = %id;
	//get a random Size for X and Y
	%size = getRandom($obstacleMaxSize / 2, $obstacleMaxSize);
	%obstacle.Size =  %size SPC %size;
	%obstacle.SizeX = %size;
	%obstacle.SizeY = %size;
	%obstacle.setBodyType( static );
	//calculate a Random Position, consider the size of this obstacle as well
	%posX = getRandom((-$map.SizeX + %obstacle.SizeX) / 2, ($map.SizeX - %obstacle.SizeX) / 2);
	%posY = getRandom((-$map.SizeY + %obstacle.SizeY) / 2, ($map.SizeY - %obstacle.SizeY) / 2);
	%obstacle.Position = %posX SPC %posY;
	%obstacle.PolyList = %points;
	%obstacle.SceneLayer = 8;
	%obstacle.SceneGroup = 4;
	%obstacle.setCollisionGroups( "1 2 3 4" );
	%obstacle.setCollisionCallback(true);
	%points = multiplyString(%point1 SPC %points, %size);
	%obstacle.createPolygonCollisionShape( %points );
	%obstacle.setFillMode(true);
	%obstacle.setFillColor("0 0 0 1");
	%obstacle.setFixedAngle(true);
	
	//add to Scene
	Level.add(%obstacle);
	
	if ($map.obstacles $= "")
	{
		$map.obstacles = %obstacle;
	}
	else
	{
		$map.obstacles = $map.obstacle SPC %obstacle;
	}
	
	//create a random number of health and magic packages around the obstacle
	%packages = %size * %size / 12;
	
	for (%i = 0; %i < %packages; %i++)
	{
		createPackage(%obstacle);
	}
}

function MapObject::onCollision(%this, %obj, %details)
{
	//delete a Projectile
	//this must happen in this onCollision-function and not the one in the projectile class because fuck you, that's why.
	if (%obj.SceneGroup == 3)
	{
		%glare = showGlare(%obj.Position, %obj.Size, 200);
		Level.add(%glare);
		schedule(16, 0, deleteObj, %obj);
	}
}