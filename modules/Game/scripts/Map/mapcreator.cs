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

///create Map with %size and a number of %obstacles
///%size = "width height"
///%obstacles = int
function createMap(%size, %obstacles)
{
	//calculate the borders of the map
	%b1 = -(getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topleft
	%b2 = (getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topright
	%b3 = (getWord(%size, 0) / 2) SPC -(getWord(%size, 1) / 2); //bottomright
	%b4 = -(getWord(%size, 0) / 2) SPC -(getWord(%size, 1) / 2); //bottomleft
	%b5 = -(getWord(%size, 0) / 2) SPC (getWord(%size, 1) / 2); //topleft
	//store them in a single String
	%borderPoints = %b1 SPC %b2 SPC %b3 SPC %b4 SPC %b5;
	
	%border = new ShapeVector(MapBorder);
	%border.Size = %size;
	%border.setBodyType( static );
	%border.Position = "0 0";
	%border.PolyList = "-0.5 0.5 0.5 0.5 0.5 -0.5 -0.5 -0.5";
	%border.SceneLayer = 9;
	%border.SceneGroup = 0;
	%border.setCollisionGroups( "1 2 3" );
	%border.createChainCollisionShape( %borderPoints );
	echo(%border.getPoly());
	%border.setFixedAngle(true);
	
	//add to Scene
	Level.add( %border );
}