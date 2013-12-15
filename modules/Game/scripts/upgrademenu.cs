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
	
	%start.desFont = %des;
	%start.staFont = %sta;
	%start.incFont = %inc;
}

function UpgradeDescriptionFont::update(%this, %i)
{
	%this.desFont.Text = getField(getUpgradeDescription(%i), 0);
	%this.staFont.Text = getField(getUpgradeDescription(%i), 1);
	%this.incFont.Text = getField(getUpgradeDescription(%i), 2);
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
	MainMenu.add(%start);
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
		
		if (%item.number < 2 && %item.level > 5)
		{
			%item.setBlendColor("0.5 0.5 0.5");
			%item.inactiveUpgrade = true;
		}
		
		MainMenu.add(%item);
		
		createUpgradeCosts(%item);
		createUpgradeLevel(%item);
	}
}

function UpgradeSprite::update(%this)
{
	if (%this.number > 2 || %this.level < 6)
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
	return mFloor(mPow(%i, 2) * 1000);
}

function getUpgradeDescription(%i)
{
	switch(%i)
	{
		case 0:
			return "Increases Max LP" TAB "Start: 100 LP" TAB "Per Level: +20 LP";
		case 1:
			return "Increases Max AP" TAB "Start: 3 AP" TAB "Per Level: +1 AP";
		case 2:
			return "Increases Projectile Speed" TAB "Start: 15 m/s" TAB "Per Level: +2 m/s";
		case 3:
			return "Increases Stun Duration" TAB "Start: 5 s" TAB "Per Level: +1 s";
		case 4:
			return "Decreases Leap AP Costs" TAB "Start: 1 AP" TAB "Per Level: -10% of current";
		case 5:
			return "Increases Beam Growth Rate" TAB "Start: 0.3 dmg/s" TAB "Per Level: +0.05 dmg/s";
		case 6:
			return "Increases Stun Radius" TAB "Start: 10 m" TAB "Per Level: +2 m";
		case 7:
			return "Deacreases Leap Cooldown" TAB "Start: 10 s" TAB "Per Level: -10% of current";
		case 8:
			return "Increases Beam Speed" TAB "Start: 15 m/s" TAB "Per Level: +2 m/s";
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
			return "StunRadius";
		case 7:
			return "LeapCooldown";
		case 8:
			return "BeamSpeed";
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
			return $saveGame.StunRadius;
		case 7:
			return $saveGame.LeapCooldown;
		case 8:
			return $saveGame.BeamSpeed;
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
				$saveGame.StunRadius++;
			case 7:
				$saveGame.LeapCooldown++;
			case 8:
				$saveGame.BeamSpeed++;
		}
		$currentScore -= %costs;
		saveGame();
		%obj.level++;
		%obj.update();
		UpgradeCreditsFont.update();
	}
}


///Enter Method for Upgrade Sprites
function UpgradeSprite::onTouchEnter(%this)
{
	if (%this.inactiveUpgrade)
		return;
	
	%this.setBlendColor("0.5 0.5 1");
	UpgradeDescriptionFont.update(%this.number);
}

///Mouse Leave Method for Upgrade Sprites
function UpgradeSprite::onTouchLeave(%this)
{
	if (%this.inactiveUpgrade)
		return;
	
	%this.setBlendColor("1 1 1");
}