// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\upgrademenu.cs
// Copyright          :  
// Author             :  -
// Created on         :  Dienstag, 3. Dezember 2013 13:40
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

function createUpgradeMenu()
{
	createUpgradeMenuItems();
}

function createUpgradeMenuItems()
{
	createCreditsAmount();
	createDescriptionWindow();
	createUpgradeItems();
	createTitle();
}


function createCreditsAmount()
{
	%start = new ImageFont( UpgradeCreditsFont );
	%start.Image = "Game:Font";
	%start.Position = "20 18";
	%start.FontSize = "1.5 2.25";
	%start.Text = "Cr." SPC $currentScore;
	%start.SceneGroup = 31;
	%start.SceneLayer = 2;
	MainMenu.add(%start);
}

function UpgradeCreditsFont::update(%this)
{
	%this.Text = "Cr." SPC $currentScore;
}

function createDescriptionWindow()
{
	%start = new ImageFont( UpgradeDescriptionFont );
	%start.Image = "Game:Font";
	%start.Position = "14 11";
	%start.FontSize = "1.5 2.25";
	%start.TextAlignment = "Left";
	%start.Text = "Description:";
	%start.SceneGroup = 31;
	%start.SceneLayer = 2;
	MainMenu.add(%start);
	
	%des = new ImageFont( DiscriptFontChild );
	%des.Image = "Game:Font";
	%des.Position = "14 8";
	%des.FontSize = "1 1.5";
	%des.TextAlignment = "Left";
	%des.Text = "";
	%des.SceneGroup = 31;
	%des.SceneLayer = 2;
	MainMenu.add(%des);
	
	%sta = new ImageFont( DiscriptFontChild );
	%sta.Image = "Game:Font";
	%sta.Position = "14 3";
	%sta.FontSize = "1 1.5";
	%sta.TextAlignment = "Left";
	%sta.Text = "";
	%sta.SceneGroup = 31;
	%sta.SceneLayer = 2;
	MainMenu.add(%sta);
	
	%inc = new ImageFont( DiscriptFontChild );
	%inc.Image = "Game:Font";
	%inc.Position = "14 -2";
	%inc.FontSize = "1 1.5";
	%inc.TextAlignment = "Left";
	%inc.Text = "";
	%inc.SceneGroup = 31;
	%inc.SceneLayer = 2;
	MainMenu.add(%inc);
	
	%max = new ImageFont( DiscriptFontChild );
	%max.Image = "Game:Font";
	%max.Position = "14 -7";
	%max.FontSize = "1 1.5";
	%max.TextAlignment = "Left";
	%max.Text = "";
	%max.SceneGroup = 31;
	%max.SceneLayer = 2;
	MainMenu.add(%max);
	
	%start.maxFont = %max;
	%start.desFont = %des;
	%start.staFont = %sta;
	%start.incFont = %inc;
}

function UpgradeDescriptionFont::update(%this, %i)
{
	%this.desFont.Text = getField(getUpgradeDescription(%i), 0);
	%this.staFont.Text = getField(getUpgradeDescription(%i), 1);
	%this.incFont.Text = getField(getUpgradeDescription(%i), 2);
	%this.maxFont.Text = getField(getUpgradeDescription(%i), 3);
}

function createTitle()
{
	%start = new ImageFont( UpgradeTitleFont );
	%start.Image = "Game:Font";
	%start.Position = "-28 18";
	%start.FontSize = "3 4.5";
	%start.Text = "Upgrades";
	%start.SceneGroup = 31;
	%start.SceneLayer = 2;
	MainMenu.add(%start);
	
	%start = new ImageFont( UpgradeBackFont );
	%start.Image = "Game:Font";
	%start.Position = "35 -18";
	%start.FontSize = "2 3";
	%start.Text = "Back";
	%start.SceneGroup = 28;
	%start.SceneLayer = 2;
	%start.setUseInputEvents(true);
	MainMenu.add(%start);
}

function UpgradeBackFont::onTouchEnter(%this)
{
	%this.setBlendColor("1 0.5 0.5");
	alxPlay("Game:MenuMove");
}
function UpgradeBackFont::onTouchLeave(%this)
{
	%this.setBlendColor("1 1 1");
}

function destroyUpgradeMenuItems()
{
	UpgradeBackFont.delete();
	UpgradeTitleFont.delete();
	for (%i = 0; %i < 9; %i++)
	{
		UpgradeSprite.delete();
		CostsFont.delete();
		LevelFont.delete();
		NextLevelFont.delete();
	}
}

function createUpgradeItems()
{
	for (%i = 0; %i < 9; %i++)
	{
		%item = new Sprite( UpgradeSprite );
		%item.Image = "Game:" @ getUpgradeName(%i) @ "Icon";
		%item.number = %i;
		%item.id = getUpgradeName(%i);
		%item.level = getUpgradeLevel(%i);
		%item.Size = "5 5";
		%item.Position = -30 + (%i % 3) * 15 SPC 10 - (mFloor(%i / 3)) * 10;
		%item.SceneGroup = %i;
		%item.SceneLayer = 1;
		%item.setUseInputEvents(true);
		
		if (%item.level > 4)
		{
			%item.setBlendColor("0.5 0.5 0.5");
			%item.inactiveUpgrade = true;
		}
		
		MainMenu.add(%item);
		
		createUpgradeCosts(%item);
		createUpgradeLevel(%item);
		
		%item.update();
	}
}

function UpgradeSprite::update(%this)
{
	if (%this.level < 5)
	{
		%this.costsFont.Text = "$" @ calculateCosts(%this.level);
		%this.currentLevelFont.Text = "Lv.:" SPC getUpgradeLevel(%this.number);
		%this.nextLevelFont.Text = "Next:" SPC getUpgradeLevel(%this.number) + 1;
	}
	else
	{
		%this.costsFont.Text = "$---";
		%this.currentLevelFont.Text = "Lv.:" SPC getUpgradeLevel(%this.number);
		%this.nextLevelFont.Text = "Next: ---";
		
		%this.setBlendColor("0.5 0.5 0.5");
		%this.inactiveUpgrade = true;
	}
}

function createUpgradeCosts(%item)
{
	%costs = new ImageFont( CostsFont );
	%costs.Image = "Game:Font";
	%costs.Position = getWord(%item.Position, 0) + 3 SPC getWord(%item.Position, 1) + 2;
	%costs.FontSize = "1 1.5";
	%costs.TextAlignment = "Left";
	%costs.Text = "$" @ calculateCosts(%item.level);
	%costs.SceneGroup = 27;
	%costs.SceneLayer = 2;
	MainMenu.add(%costs);
	
	%item.costsFont = %costs;
}

function createUpgradeLevel(%item)
{
	%costs = new ImageFont( LevelFont );
	%costs.Image = "Game:Font";
	%costs.Position = getWord(%item.Position, 0) + 3 SPC getWord(%item.Position, 1) + 0.5;
	%costs.FontSize = "1 1.5";
	%costs.TextAlignment = "Left";
	%costs.Text = "Lv.:" SPC getUpgradeLevel(%item.number);
	%costs.SceneGroup = 27;
	%costs.SceneLayer = 2;
	MainMenu.add(%costs);
	
	%item.currentLevelFont = %costs;
	
	%costs = new ImageFont( NextLevelFont );
	%costs.Image = "Game:Font";
	%costs.Position = getWord(%item.Position, 0) + 3 SPC getWord(%item.Position, 1) - 1;
	%costs.FontSize = "1 1.5";
	%costs.TextAlignment = "Left";
	%costs.Text = "Next:" SPC getUpgradeLevel(%item.number) + 1;
	%costs.SceneGroup = 27;
	%costs.SceneLayer = 2;
	MainMenu.add(%costs);
	
	%item.nextLevelFont = %costs;
}

function calculateCosts(%i)
{
	%i++;
	return mFloor(mPow(%i, 2) * 6500 - %i * 14500 + 10000);
}

function getUpgradeDescription(%i)
{
	switch(%i)
	{
		case 0:
			return "Increases Max LP" TAB "Start: 100 LP" TAB "Per Level: +20 LP" TAB "Current Max.:" SPC mRound(100 + 20 * getUpgradeLevel(%i)) SPC "LP";
		case 1:
			return "Increases Max AP" TAB "Start: 3 AP" TAB "Per Level: +1 AP" TAB "Current Max.:" SPC mRound(3 + 1 * getUpgradeLevel(%i)) SPC "AP";
		case 2:
			return "Increases Projectile Speed" TAB "Start: 25 m/s" TAB "Per Level: +5 m/s" TAB "Current Max.:" SPC mRound(25 + 5 * getUpgradeLevel(%i)) SPC "m/s";
		case 3:
			return "Increases Stun Duration" TAB "Start: 5 s" TAB "Per Level: +1 s" TAB "Current Max.:" SPC mRound(5 + 1 * getUpgradeLevel(%i)) SPC "s";
		case 4:
			return "Decreases Leap AP Costs" TAB "Start: 1 AP" TAB "Per Level: -10% of current" TAB "Current Min.:" SPC getRounded(mPow(0.9, getUpgradeLevel(%i)), -2) SPC "AP";
		case 5:
			return "Increases Beam Growth Rate" TAB "Start: 0.5 dmg/s" TAB "Per Level: +0.1 dmg/s" TAB "Current Max.:" SPC getRounded(0.5 + 0.1 * getUpgradeLevel(%i), -2) SPC "dmg/s";
		case 6:
			return "Increases Movement Speed" TAB "Start: 20 m/s" TAB "Per Level: +4 m/s" TAB "Current Max.:" SPC mRound(20 + 4 * getUpgradeLevel(%i)) SPC "m/s";
		case 7:
			return "Increases Projectile Dmg" TAB "Start: 2 dmg" TAB "Per Level: +0.5 dmg" TAB "Current Max.:" SPC getRounded(2 + 0.5 * getUpgradeLevel(%i), -1) SPC "dmg";
		case 8:
			return "Increases Point Multiplier" TAB "Start: x1.0" TAB "Per Level: +0.1" TAB "Current Max.: x" @ getRounded(1 + 0.1 * getUpgradeLevel(%i), -1);
	}
}

function getUpgradeName(%i)
{
	switch(%i)
	{
		case 0:
			return "LP";
		case 1:
			return "AP";
		case 2:
			return "ShotSpeed";
		case 3:
			return "StunLength";
		case 4:
			return "LeapCosts";
		case 5:
			return "BeamGrowth";
		case 6:
			return "Speed";
		case 7:
			return "Damage";
		case 8:
			return "Credits";
	}
}

function getUpgradeLevel(%i)
{
	switch(%i)
	{
		case 0:
			return $saveGame.HP;
		case 1:
			return $saveGame.MP;
		case 2:
			return $saveGame.ShotSpeed;
		case 3:
			return $saveGame.StunLength;
		case 4:
			return $saveGame.LeapCosts;
		case 5:
			return $saveGame.BeamGrowth;
		case 6:
			return $saveGame.speed;
		case 7:
			return $saveGame.damage;
		case 8:
			return $saveGame.credits;
	}
}

function increaseUpgradeLevel(%i, %obj)
{
	if (%obj.inactiveUpgrade)
		return;
	
	%costs = calculateCosts(getUpgradeLevel(%i));
	if ($currentScore >= %costs)
	{	
		switch(%i)
		{
			case 0:
				$saveGame.HP++;
			case 1:
				$saveGame.MP++;
			case 2:
				$saveGame.ShotSpeed++;
			case 3:
				$saveGame.StunLength++;
			case 4:
				$saveGame.LeapCosts++;
			case 5:
				$saveGame.BeamGrowth++;
			case 6:
				$saveGame.speed++;
			case 7:
				$saveGame.damage++;
			case 8:
				$saveGame.credits++;
		}
		$currentScore -= %costs;
		saveGame();
		%obj.level++;
		%obj.update();
		UpgradeCreditsFont.update();
		alxPlay("Game:MenuSelect");
		UpgradeDescriptionFont.update(%i);
	}
	else
	{
		alxPlay("Game:MenuBack");
	}
}


///Enter Method for Upgrade Sprites
function UpgradeSprite::onTouchEnter(%this)
{
	if (%this.inactiveUpgrade)
		%this.setBlendColor("0.25 0.25 0.5");
	else
		%this.setBlendColor("0.5 0.5 1");
	UpgradeDescriptionFont.update(%this.number);
	alxPlay("Game:MenuMove");
}

///Mouse Leave Method for Upgrade Sprites
function UpgradeSprite::onTouchLeave(%this)
{
	if (%this.inactiveUpgrade)
		%this.setBlendColor("0.5 0.5 0.5");
	else
		%this.setBlendColor("1 1 1");
}