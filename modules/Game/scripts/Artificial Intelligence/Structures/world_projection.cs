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
	%wp = new ScriptObject(WorldProjection);
	
	//declare world properties that are important for action selection
	%wp.invisibilityCooldown = $enemy.invisibilityCooldown;
	%wp.mineCooldown = $enemy.mineCooldown;
	%wp.gravitPointCooldown = $enemy.gravitPointCooldown;
	%wp.powerUpExists = isObject($powerup);
	%wp.gravitPointProjectileExists = isObject($gravitPointProjectile);
	
	//declare all world properties
	%wp.enemyHP = $character.HP;
	%wp.ownHP = $enemy.HP;
	
	%wp.distancePackage = getDistanceNearestPackage($enemy.Position);
	
	%wp.skillsOnCooldown = $enemy.getSkillsOnCooldown();
	%wp.visible = $enemy.getVisible();
	%wp.enemyMP = $character.MP;
	%wp.ownMP = $enemy.MP;
	%wp.enemyInSight = isInSight($character.Position, $enemy);
	
	
	
	if (%wp.enemyInSight)
	{
		%wp.distanceEnemy = VectorDist($character.Position, $enemy.Position);
		%wp.distanceEnemyPackage = getDistanceNearestPackage($character.Position);
		%wp.enemyInGravitPoint = $enemy.getCharacterInGravitPoint();
		%wp.distanceEnemyMine = VectorDist($character.Position, $mine.Position);
	}
	else
	{
		%wp.distanceEnemy = -1;
		%wp.distanceEnemyPackage = -1;
		%wp.enemyInGravitPoint = -1;
		%wp.distanceEnemyMine = -1;
	}
	
	
	
	//save all properties inside an array
	%wp.props = %wp.enemyHP SPC %wp.ownHP SPC %wp.distancePackage SPC %wp.skillsOnCooldown SPC %wp.visible SPC %wp.enemyMP SPC %wp.ownMP SPC %wp.enemyInSight SPC %wp.distanceEnemy SPC %wp.distanceEnemyPackage SPC %wp.enemyInGravitPoint SPC %wp.distanceEnemyMine;
	%wp.normProperties();
	
	return %wp;
}


///returns the satisfaction of the "Survive"-Goal
///satisfaction is always a value between -1 and 1
function WorldProjection::convertToGoalSurvive(%this)
{
	//add all properties of the worldprojection while multiplying them with their weights
	%sum = 0;
	%weights = getAverageWeightsSurvive();
	for (%i = 0; %i < %this.norm_props.count; %i++)
	{
		%sum += getWord(%this.norm_props, %i) * getWord(%weights, %i);
	}
	//divide them by their amount to get the average satisfaction
	%sum /= %this.props.count;
	return %sum;
}

///returns the satisfaction of the "Kill"-Goal
///satisfaction is always a value between -1 and 1
function WorldProjection::convertToGoalKill(%this)
{
	//add all properties of the worldprojection while multiplying them with their weights
	%sum = 0;
	%weights = getAverageWeightsKill();
	for (%i = 0; %i < %this.props.count; %i++)
	{
		%sum += getWord(%this.norm_props, %i) * getWord(%weights, %i);
	}
	%sum /= %this.props.count;
	return %sum;
}

function getAverageWeightsSurvive()
{
	return "0 0.3 -0.1 -0.1 -1 -0.7 0 -0.2 1 0 1 0";
	//return "0 0 0 0 0 0 0 0 0 0 0 0";
}

function getAverageWeightsKill()
{
	return "-1 0 0 -0.2 -1 0 0.5 1 -1 1 1 -1";
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
		case 5:
			%a = $character.maxMP;
		case 6:
			%a = $enemy.maxMP;
		case 8:
			%a = 46;
		case 9:
			%a = 200;
		case 11:
			%a = 200;
	}
	return %prop / %a;
}

///creates the World Projection that results, when %action is performed inside the old World Projection
function WorldProjection::createNewWorldProjection(%this, %action)
{
	//get basic changes of current action
	%changes = %action.getChanges();
	
	//for every weak property index
	for (%i = 0; %i < $weakIndices; %i++)
	{
		//get Index of next weak property
		%j = getWord($weakIndices, %i);
		
		//calculate the changes regarding the changes over time
		%newVal = getWord(%changes, %j) + $saveWorldProjections.averages[%j];
		
		//set new change
		%changes = setWord(%changes, %j, %newVal);
	}
	
	%wp = new ScriptObject(WorldProjection);
	
	%wp.props = %this.props;
	
	//add all changes through action (%changes)
	for (%i = 0; %i < %wp.props.count; %i++)
	{
		%wp.props = setWord(%wp.props, %i, getWord(%wp.props, %i) + getWord(%changes, %i));
	}
	
	//set all "real" properties that are need for action selection
	%wp.ownMP = %this.ownMP;
	%wp.distanceEnemy = %this.distanceEnemy;
	%wp.invisibilityCooldown = %this.invisibilityCooldown;
	%wp.mineCooldown = %this.mineCooldown;
	%wp.gravitPointCooldown = %this.gravitPointCooldown;
	%wp.gravitPointProjectileExists = %this.gravitPointProjectileExists;
	%wp.powerUpExists = %this.powerUpExists;
	
	//apply changes to all "real" properties that are needed for action selection
	%wp.ownMP = getWord(%wp.props, 6);
	%wp.distanceEnemy = getWord(%wp.props, 8);
	if (%action.id $= BecomeInvisibleBehavior)
	{
		%wp.invisibilityCooldown = 1;
	}
	if (%action.id $= SetMineBehavior)
	{
		%wp.mineCooldown = 1;
	}
	if (%action.id $= ShootGravitPointBehavior)
	{
		%wp.gravitPointCooldown = 1;
		%wp.gravitPointProjectileExists = 1;
	}
	if (%action.id $= GetPowerupBehavior)
	{
		%wp.powerUpExists = false;
	}
	if (%action.id $= UseGravitPointBehavior)
	{
		%wp.gravitPointProjectileExists = 0;
	}
	
	%wp.normProperties();
	
	return %wp;
}