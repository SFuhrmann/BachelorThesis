// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\Structures\world_projection.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Sonntag, 5. Januar 2014 15:34
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  Holds the class for all World Projections.
//                    :  A World Projection holds all information about the world
//                    :  that the agent has access to.
// ============================================================

///create a new World Projection of the current State of the World
function createCurrentWorldProjection()
{
	%wp = new ScriptObject( WorldProjection );
	
	//declare all world properties
	%wp.ownPosition = $enemy.Position;
	%wp.enemyPosition = $character.Position;
	%wp.ownHP = $enemy.HP;
	%wp.enemyHP = $character.HP;
	%wp.ownMP = $enemy.MP;
	%wp.gravitPointProjectileExists = isObject($gravitPointProjectile);
	%wp.gravitPointFiredExists = isObject($enemy.gravitPointFired);
	if (%wp.gravitPointProjectileExists)
		%wp.gravitPointProjectilePosition = $gravitPointProjectile.Position;
	if (%wp.gravitPointFiredExists)
		%wp.gravitPointPosition = $enemy.gravitPointFired.Position;
	%wp.mineExists = isObject($mine);
	if (%wp.mineExists)
		%wp.minePosition = $mine.Position;
	%wp.invisibility = $enemy.invisible;
	%wp.invisibilityCooldown = $enemy.invisibilityCooldown;
	%wp.mineCooldown = $enemy.mineCooldown;
	%wp.gravitPointCooldown = $enemy.gravitPointCooldown;
	
	//declare all properties for goal calculation
	%wp.createProps();
	
	//norm all properties for goal calculation
	%wp.normProperties();
	
	return %wp;
}

///creates all relevant properties for the worldpropjection and saves them inside a list
function WorldProjection::createProps(%this)
{
	%nextPackageDistance = getDistanceNearestPackage(%this.ownPosition);
	%skillsOnCooldown = %this.invisibilityCooldown + %this.mineCooldown + %this.gravitPointCooldown;
	%distanceEnemy = VectorDist(%this.enemyPosition, %this.ownPosition);
	if (%this.gravitPointFiredExists)
		%enemyInGravitPoint = VectorDist(%this.enemyPosition, %this.gravitPointPosition) < $attractionPointSize;
	else
		%enemyInGravitPoint = 0;
	if (%this.mineExists)
		%distanceEnemyMine = VectorDist(%this.minePosition, %this.enemyPosition);
	else
		%distanceEnemyMine = 200;
	if (%this.gravitPointFiredExists && %this.mineExists)
		%distanceMineGravitPoint = VectorDist(%this.gravitPointPosition, %this.minePosition);
	else
		%distanceMineGravitPoint = 200;
	
	
	%this.props = %this.enemyHP SPC %this.ownHP SPC %nextPackageDistance SPC %skillsOnCooldown SPC %this.invisible SPC %this.ownMP SPC %distanceEnemy SPC %this.gravitPointFiredExists SPC %this.mineExists SPC %enemyInGravitPoint SPC %distanceEnemyMine SPC %distanceMineGravitPoint;
}


///returns the satisfaction of the "Survive"-Goal
///satisfaction is always a value between -1 and 1
function WorldProjection::convertToGoalSurvive(%this)
{
	//add all properties of the worldprojection while multiplying them with their weights
	%sum = 0;
	%weights = getAverageWeightsSurvive();
	%invert = getInvertWeightsSurvive();
	
	%sumSurvive = getSumWeightsSurvive();
	
	for (%i = 0; %i < %this.norm_props.count; %i++)
	{
		%next = 0;
		%weight = getWord(%weights, %i);
		%invertNext = getWord(%invert, %i);
		if (%invertNext)
			%next = 1 - getWord(%this.norm_props, %i);
		else
			%next = getWord(%this.norm_props, %i);
		%sum += %next;
	}
	//divide them by their amount to get the average satisfaction
	%sum /= %sumSurvive;
	return %sum;
}

///returns the satisfaction of the "Kill"-Goal
///satisfaction is always a value between -1 and 1
function WorldProjection::convertToGoalKill(%this)
{
	//add all properties of the worldprojection while multiplying them with their weights
	%sum = 0;
	%weights = getAverageWeightsKill();
	%invert = getInvertWeightsKill();
	
	%sumKill = getSumWeightsKill();
	
	for (%i = 0; %i < %this.norm_props.count; %i++)
	{
		%next = 0;
		%weight = getWord(%weights, %i);
		%invertNext = getWord(%invert, %i);
		if (%invertNext)
			%next = 1 - getWord(%this.norm_props, %i);
		else
			%next = getWord(%this.norm_props, %i);
		%sum += %next;
	}
	%sum /= %sumKill;
	return %sum;
}

function getAverageWeightsSurvive()
{
	return "0 1 0.1 0.1 1 0.1 1 0.5 0.5 1 1 0.5";
}
function getAverageWeightsKill()
{
	return "1 0 0 1 1 1 1 0.5 0.5 1 1 0.5";
}
function getInvertWeightsSurvive()
{
	return "0 0 1 1 0 0 0 0 0 0 1 1";
}
function getInvertWeightsKill()
{
	return "1 0 0 1 0 0 1 0 0 0 1 1";
}

function getSumWeightsSurvive()
{
	%a = 0;
	%weights = getAverageWeightsSurvive();
	for (%i = 0; %i < %weights.count; %i++)
	{
		%a += getWord(%weights, %i);
	}
	return %a;
}
function getSumWeightsKill()
{
	%a = 0;
	%weights = getAverageWeightsKill();
	for (%i = 0; %i < %weights.count; %i++)
	{
		%a += getWord(%weights, %i);
	}
	return %a;
}

///normalizes the properties and saves them inside an own array
function WorldProjection::normProperties(%this)
{
	%result = "";
	for (%i = 0; %i < %this.props.count; %i++)
	{
		%a = mMax(mMin(%this.getNormalizedProp(getWord(%this.props, %i), %i), 1), 0);
		%result = addWord(%result, %a);
	}
	%this.norm_props = %result;
}

function WorldProjection::getNormalizedProp(%this, %prop, %i)
{
	%a = 1;
	switch(%i)
	{
		case 0:
			%a = $character.maxHP;
		case 1:
			%a = $enemy.maxHP;
		case 2:
			%a = 200;
		case 3:
			%a = 3;
		case 4:
			%a = 1;
		case 5:
			%a = $enemy.maxMP;
		case 6:
			%a = 200;
		case 7:
			%a = 1;
		case 8:
			%a = 1;
		case 9:
			%a = 1;
		case 10:
			%a = 200;
		case 11:
			%a = 200;
	}
	return %prop / %a;
}

///creates the World Projection that results, when %action is performed inside the old World Projection
function WorldProjection::createNewWorldProjection(%this, %action)
{
	//%wp = new ScriptObject(WorldProjection);
	
	%wp = %this.clone(true);
	%wp.class = WorldProjection;
	
	%action.applyChanges(%wp);
	
	%wp.createProps();
	
	//create a word for over time Changes
	%changes = "0 0 0 0 0 0 0 0 0 0 0 0";
	
	//get the changes over time of all weak properties
	//for every weak property index
	for (%i = 0; %i < $weakIndices; %i++)
	{
		//get Index of next weak property
		%j = getWord($weakIndices, %i);
		
		//calculate the changes regarding the changes over time
		%newVal = $saveWorldProjections.averages[%j];
		
		//set new change
		%changes = setWord(%changes, %j, %newVal);
	}
	
	//add all changes over time
	for (%i = 0; %i < %wp.props.count; %i++)
	{
		%wp.props = setWord(%wp.props, %i, getWord(%wp.props, %i) + getWord(%changes, %i));
	}
	
	%wp.normProperties();
	
	return %wp;
}