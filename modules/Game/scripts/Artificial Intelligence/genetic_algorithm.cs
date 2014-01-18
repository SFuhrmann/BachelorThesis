// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\Artificial Intelligence\genetic_algorithm.cs
// Copyright          :  
// Author             :  Stephen
// Created on         :  Dienstag, 14. Januar 2014 22:26
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  This file holds the class for the genetic algorithm.
//                    :  The class has n dna strings and saves their average values.
//                    :  Also it holds a function that creates a new generation of dna strings.
//                    :  The class must be given a goal value for the validation function inside the algorithm.
// ============================================================

///create a genetic module that will step by step enhance the world projections weights that are used
///to convert their properties into goal values
function createGeneticModule()
{
	$geneticModule = new ScriptObject( GeneticModule );
	
	$geneticModule.createNewDNAStrings();
}

///erases all Strings and generates new DNA Strings
function GeneticModule::createNewDNAStrings(%this)
{
	%this.surviveAverage = "0 0 0 0 0 0 0 0 0 0 0 0";
	%this.killAverage = "0 0 0 0 0 0 0 0 0 0 0 0";
	//go through all DNA Strings
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		%this.surviveStrings[%i] = "";
		%this.killStrings[%i] = "";
		for (%j = 0; %j < 12; %j++)
		{
			//create new random Numbers for the current DNA String's weights
			%newS = getRandom(0, 100) / 100;
			%newK = getRandom(0, 100) / 100;
			
			//save the new values
			%this.surviveStrings[%i] = addWord(%this.surviveStrings[%i], %newS);
			%this.killStrings[%i] = addWord(%this.killStrings[%i], %newK);
			
			//calculate the new average values
			%this.surviveAverage = setWord(%this.surviveAverage, %j, (%newS - getWord(%this.surviveAverage, %j) / (%i + 1)));
			%this.killAverage = setWord(%this.killAverage, %j, (%newK - getWord(%this.killAverage, %j) / (%i + 1)));
		}
	}
	echo("SURVIVE:" SPC %this.surviveAverage);
	echo("KILL:" SPC %this.killAverage);
}

///creates the new generation of Strings for Survive
function GeneticModule::createNextGenerationSurvive(%this, %goalValue)
{
	//create the current World Projection to determine which goal value would be calculated now
	//so then we have a validation formula for the dna strings
	%wp = createCurrentWorldProjection();
	%e[-1] = 0;
	%newStrings[-1] = 0;
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		//this is the value of each string (+ the values of all strings before,
		//this way the intervals for the dna picking process already exist
		%e[%i] = 1 - mAbs(%wp.convertToGoalSurvive(%this.surviveStrings[%i]) - %goalValue) + %e[%i - 1];
	}
	
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		//create a random number with two decimal numbers in range of all intervals
		%rnd1 = getRandom(0, mFloor(%e[49] * 100)) / 100;
		
		//iteratively determine which string is chosen
		%index = 24;
		%change = 12;
		%result = 24;
		%j = 0;
		while (%j < 50)
		{
			//test if interval is found
			if (%e[%index - 1] < %rnd1 && %e[%index] >= %rnd1)
			{
				//if so save the index of the dna string in result
				%result = %index;
				break;
			}
			else
			{
				//otherwise determine wether interval lies too low or high and go on with the testing
				if (%e[%index] > %rnd1)
				{
					%index = %index - %change;
					%change =mMax(1,  mFloor(%change / 2));
				}
				else
				{
					%index = %index + %change;
					%change = mMax(1, mFloor(%change / 2));
				}
			}
			%j++;
		}
		if (%j == 49)
			return;
		%string1 = %this.surviveStrings[%result];
		
		//do the same for a second string that will be the mating partner of string1
		%rnd2 = getRandom(0, mFloor(%e[49] * 100)) / 100;
		
		//iteratively determine which string is chosen
		%index = 24;
		%change = 12;
		%result = 24;
		%j = 0;
		while (true)
		{
			//test if interval is found
			if (%e[%index - 1] < %rnd2 && %e[%index] >= %rdn2)
			{
				//if so save the index of the dna string in result
				%result = %index;
				break;
			}
			else
			{
				//otherwise determine wether interval lies too low or high and go on with the testing
				if (%e[%index] > %rnd2)
				{
					%index = %index - %change;
					%change = mMax(1, mFloor(%change / 2));
				}
				else
				{
					%index = %index + %change;
					%change = mMax(1, mFloor(%change / 2));
				}
			}
			%j++;
		}
		if (%j == 49)
			return;
		%string2 = %this.surviveStrings[%result];
		
		//determine the position where the two strings should be paired
		%rnd3 = getRandom(0, 11);
		
		for (%j = 0; %j < %rnd3; %j++)
		{
			%string2 = setWord(%string2, %j, getWord(%string1, %j));
		}
		//create a last random number to see wether there is a mutation or not
		%rnd4 = getRandom(0, $geneticAlgorithmMutationRate);
		if (%rnd4 == 0)
		{
			%string2 = setWord(%string2, %rnd3, 1 - (getWord(%string2, %rnd3)));
		}
		//save the new String in an array that will replace the old dna strings later on
		%newStrings[%i] = %string2;
	}
	%this.surviveAverage = "0 0 0 0 0 0 0 0 0 0 0 0";
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		%this.surviveStrings[%i] = %newStrings[%i];
		for (%j = 0; %j < 12; %j++)
		{
			%current = getWord(%this.surviveAverage, %j);
			%new = getWord(%newStrings[%j], %j);
			
			%this.surviveAverage = setWord(%this.surviveAverage, %j, %current + (%new - %current) / %i);
		}
	}
	echo("SURVIVE:" SPC %this.surviveAverage);
}

///creates the new generation of Strings for Kill
function GeneticModule::createNextGenerationKill(%this, %goalValue)
{
	//create the current World Projection to determine which goal value would be calculated now
	//so then we have a validation formula for the dna strings
	%wp = createCurrentWorldProjection();
	%e[-1] = 0;
	%newStrings[-1] = 0;
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		//this is the value of each string (+ the values of all strings before,
		//this way the intervals for the dna picking process already exist
		%e[%i] = 1 - mAbs(%wp.convertToGoalKill(%this.KillStrings[%i]) - %goalValue) + %e[%i - 1];
	}
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		//create a random number with two decimal numbers in range of all intervals
		%rnd1 = getRandom(0, %e[49] * 100) / 100;
		
		//iteratively determine which string is chosen
		%index = 24;
		%change = 12;
		%result = 24;
		%j = 0;
		while (%j < 50)
		{
			//test if interval is found
			if (%e[%index - 1] < %rnd1 && %e[%index] > %rnd1)
			{
				//if so save the index of the dna string in result
				%result = %index;
				break;
			}
			else
			{
				//otherwise determine wether interval lies too low or high and go on with the testing
				if (%e[%index] > %rnd1)
				{
					%index = %index - %change;
					%change =mMax(1,  mFloor(%change / 2));
				}
				else
				{
					%index = %index + %change;
					%change = mMax(1, mFloor(%change / 2));
				}
			}
		}
		if (%j == 49)
			return;
		%string1 = %this.KillStrings[%result];
		
		//do the same for a second string that will be the mating partner of string1
		%rnd2 = getRandom(0, %e[49] * 100) / 100;
		
		//iteratively determine which string is chosen
		%index = 24;
		%change = 12;
		%result = 24;
		%j = 0;
		while (%j < 50)
		{
			//test if interval is found
			if (%e[%index - 1] < %rnd2 && %e[%index] > %rdn2)
			{
				//if so save the index of the dna string in result
				%result = %index;
				break;
			}
			else
			{
				//otherwise determine wether interval lies too low or high and go on with the testing
				if (%e[%index] > %rnd2)
				{
					%index = %index - %change;
					%change = mMax(1, mFloor(%change / 2));
				}
				else
				{
					%index = %index + %change;
					%change = mMax(1, mFloor(%change / 2));
				}
			}
		}
		if (%j == 49)
			return;
		%string2 = %this.KillStrings[%result];
		
		//determine the position where the two strings should be paired
		%rnd3 = getRandom(0, 11);
		
		for (%j = 0; %j < %rnd3; %j++)
		{
			%string2 = setWord(%string2, %j, getWord(%string1, %j));
		}
		
		//create a last random number to see wether there is a mutation or not
		%rnd4 = getRandom(0, $geneticAlgorithmMutationRate);
		if (%rnd4 == 0)
		{
			%string2 = setWord(%string2, %rnd3, 1 - (getWord(%string2, %rnd3)));
		}
		
		//save the new String in an array that will replace the old dna strings later on
		%newStrings[%i] = %string2;
	}
	//save the string inside the Module
	%this.KillAverage = "0 0 0 0 0 0 0 0 0 0 0 0";
	for (%i = 0; %i < $amountDNAStrings; %i++)
	{
		%this.KillStrings[%i] = %newStrings[%i];
		for (%j = 0; %j < 12; %j++)
		{
			%current = getWord(%this.KillAverage, %j);
			%new = getWord(%newStrings[%j], %j);
			
			%this.KillAverage = setWord(%this.KillAverage, %j, %current + (%new - %current) / %i);
		}
	}
	echo("KILL:" SPC %this.KillAverage);
}