// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\interface.cs
// Copyright          :  
// Author             :  -
// Created on         :  Mittwoch, 6. November 2013 18:18
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

///create the InterfaceWindow
function createInterfaceWindow()
{
	  if ( !isObject(InterfaceWindow) )
    {
        // Create the scene window.
        new SceneWindow(InterfaceWindow){
			Extent = Canvas.getExtent();
		};
		//InterfaceWindow.checkSize();
		InterfaceWindow.setUseWindowInputEvents(false);

        InterfaceWindow.Profile = GuiInterfaceProfile;

        //Add the Interface to the SceneWindow
        Window.add( InterfaceWindow );
    }
	
    InterfaceWindow.setCameraPosition( 0, 0 );
    InterfaceWindow.setCameraSize( 80, 45 );
    InterfaceWindow.setCameraZoom( 1.0 );
    InterfaceWindow.setCameraAngle( 0 );
	
	// Destroy the scene if it already exists.
    if ( isObject(Interface) )
        destroyInterface();
    
    // Create the scene.
    new Scene(Interface);
	InterfaceWindow.setScene( Interface );
	InterfaceWindow.checkSize();
}

function InterfaceWindow::checkSize(%this)
{
	if (%this.Extent != Canvas.getExtent())
		%this.Extent = Canvas.getExtent();
	%this.schedule(16, checkSize);
}

///destroy the Interface
function destroyInterface()
{
    // Finish if no scene available.
    if ( !isObject(Interface) )
        return;

    // Delete the scene.
    Interface.delete();
}

///create the Interface
function createInterface()
{
	createHPBarOutline();
	createHPBarFill();
	
	createMPBarOutlines();
	createMPBarFill();
	
	createIcons();
	
	createScore();
}

function createScore()
{
	%score = new ImageFont( Score );
	%score.Image = "Game:Font";
	%score.Text = $currentScore;
	%score.TextAlignment = "Right";
	%score.setFontSize("2.5 3");
	%score.setPosition("37.5 -20");
	%score.SceneGroup = 21;
	%score.SceneLayer = 1;
	
	Interface.add(%score);
}

function Score::update(%this)
{
	%this.Text = $currentScore;
}

///create Interface Icons
function createIcons()
{
	createLeapIcon();
	createStunIcon();
	createBeamIcon();
}

///create Leap Icon
function createLeapIcon()
{
	%leap = new Sprite( LeapIcon );
	%leap.Size = "5 5";
	%leap.Position = "-20 17.5";
	%leap.Image = "Game:LeapIcon";
	%leap.setImageFrame(19);
	%leap.SceneGroup = 1;
	%leap.SceneLayer = 1;
	%leap.setFixedAngle(true);
	Interface.add(%leap);
	
	%mouse = new Sprite( MouseIcon );
	%mouse.Size = "2 2";
	%mouse.Position = "-20 21";
	%mouse.Image = "Game:MouseIcon";
	%mouse.SceneGroup = 2;
	%mouse.SceneLayer = 2;
	%mouse.setFixedAngle(true);
	Interface.add(%mouse);
}

///create Stun Icon
function createStunIcon()
{
	%stun = new Sprite( StunIcon );
	%stun.Size = "5 5";
	%stun.Position = "-35 17.5";
	%stun.Image = "Game:StunIcon";
	%stun.setImageFrame(19);
	%stun.SceneGroup = 1;
	%stun.SceneLayer = 1;
	%stun.setFixedAngle(true);
	Interface.add(%stun);
	
	%mouse = new Sprite( QAbilityIcon );
	%mouse.Size = "2 2";
	%mouse.Position = "-35 21";
	%mouse.Image = "Game:QIcon";
	%mouse.SceneGroup = 2;
	%mouse.SceneLayer = 2;
	%mouse.setFixedAngle(true);
	Interface.add(%mouse);
}

///create Stun Icon
function createBeamIcon()
{
	%beam = new Sprite( BeamIcon );
	%beam.Size = "5 5";
	%beam.Position = "-27.5 17.5";
	%beam.Image = "Game:BeamIcon";
	%beam.setImageFrame(19);
	%beam.SceneGroup = 1;
	%beam.SceneLayer = 1;
	%beam.setFixedAngle(true);
	Interface.add(%beam);
	
	%mouse = new Sprite( EAbilityIcon );
	%mouse.Size = "2 2";
	%mouse.Position = "-27.5 21";
	%mouse.Image = "Game:EIcon";
	%mouse.SceneGroup = 2;
	%mouse.SceneLayer = 2;
	%mouse.setFixedAngle(true);
	Interface.add(%mouse);
}

function LeapIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.leapingCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

function StunIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.stunningCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

function BeamIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.beamCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

///create HP Bar Outline
function createHPBarOutline()
{
	%hp = new ScriptObject( HPMeter );
	%hp.outline = new ShapeVector( HPMeterOutline );
	%hp.outline.setLineColor( 0.5, 1, 0.5);
	//create Graphics
	%hp.outline.setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
	%hp.outline.Size = 15 SPC 16 * $character.maxHP / 100;
	%hp.outline.Position = 32.5 SPC 20 - getWord(%hp.outline.Size, 1) / 2;
	%hp.outline.SceneGroup = 1;
	%hp.outline.SceneLayer = 1;
	
	//physics
	%hp.outline.setFixedAngle(true);
	
	//add to Scene
	Interface.add(%hp.outline);
}

///create MP Bar Outlines
function createMPBarOutlines()
{
	%mp = new ScriptObject( MPMeter );
	for (%i = 0; %i < $character.maxMP; %i++)
	{
		%mp.outline[%i] = new ShapeVector( MPMeterOutline );
		%mp.outline[%i].setLineColor( 0.5, 0.5, 1);
		//create Graphics
		%mp.outline[%i].setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
		%mp.outline[%i].Size = "15 5";
		%mp.outline[%i].Position = 36.5 SPC (17.5 - 5.5 * %i);
		%mp.outline[%i].SceneGroup = 1;
		%mp.outline[%i].SceneLayer = 1;
		
		//physics
		%mp.outline[%i].setFixedAngle(true);
		
		//add to Scene
		Interface.add(%mp.outline[%i]);
	}
}

///create HP Bar Fill
function createHPBarFill()
{
	HPMeter.fill = new ShapeVector( HPMeterFill );
	HPMeter.fill.setLineColor( 0.5, 1, 0.5);
	//create Graphics
	HPMeter.fill.setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
	HPMeter.fill.Size = 10 SPC 16 * $character.maxHP / 100 - 0.5; //15.5 * 100 / $character.maxHP
	HPMeter.fill.Position = 32.5 SPC 8;
	HPMeter.fill.SceneGroup = 1;
	HPMeter.fill.SceneLayer = 1;
	
	//physics
	HPMeter.fill.setFixedAngle(true);
	HPMeter.fill.setFillMode(true);
	HPMeter.fill.setFillColor( "0.5 1 0.5" );
	
	//add to Scene
	Interface.add(HPMeter.fill);
	
	HPMeter.fill.update();
}

///create MP Bar Fill
function createMPBarFill()
{
	for (%i = 0; %i < $character.maxMP; %i++)
	{
		MPMeter.fill[%i] = new ShapeVector( MPMeterFill );
		MPMeter.fill[%i].setLineColor( 0.5, 0.5, 1);
		//create Graphics
		MPMeter.fill[%i].setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
		MPMeter.fill[%i].Size = "10 4.5";
		MPMeter.fill[%i].Position = 36.5 SPC (19.25 - 5.5 * %i);
		MPMeter.fill[%i].SceneGroup = 1;
		MPMeter.fill[%i].SceneLayer = 1;
		
		MPMeter.fill[%i].setFillMode(true);
		MPMeter.fill[%i].setFillColor( "0.5 0.5 1");
		
		//physics
		MPMeter.fill[%i].setFixedAngle(true);
		
		//add to Scene
		Interface.add(MPMeter.fill[%i]);
	}
	MPMeter.fill[0].update(0, $character.MP);
}

///update HP Meter Fill Size
function HPMeterFill::update(%this)
{
	%this.Size = 10 SPC clipToAnotherSize($character.HP, $character.maxHP, 16 * $character.maxHP / 100 - 0.5);
	%this.Position = 32.5 SPC 19.75 - getWord(%this.Size, 1) / 2;
}

///update MP Meter Fill Size
function MPMeterFill::update(%this, %number, %mp)
{
	//calculate the value for the current Bar
	%value = 1;
	if (%mp < 1)
	{
		%value = %mp;
		%mp = 0;
	}
	else
	{
		%mp--;
	}
	//clip to the Size of the current Bar
	%this.Size = 10 SPC clipToAnotherSize(%value, 1, 4.5);
	//set the corresponding Position
	%this.Position = 36.5 SPC (19.75 - 5.5 * %number) - getWord(%this.Size, 1) / 2;
	
	//set Alpha to 0, if Bar should not be drawn
	if (getWord(%this.Size, 1) == 0)
		%this.setLineAlpha(0);
	else
		%this.setLineAlpha(1);
	
	//update the next Bar
	if (%number < $character.maxMP - 1)
		MPMeter.fill[%number + 1].update(%number + 1, %mp);
}

///clip Percentage value of HP to the Size
function clipToAnotherSize(%value, %maxValue, %newMax)
{
	return %value / %maxValue * %newMax;
}

function destroyHPBar()
{
	if (!isObject(HPMeter.Fill))
		return;
	HPMeter.fill.delete();
	HPMeter.outline.delete();
	HPMeter.delete();
}

function destroyMPBars()
{
	if (!isObject(MPMeter.Fill))
		return;
	for(%i = 0; %i < $character.maxMP; %i++)
	{
		MPMeter.fill[%i].delete();
		MPMeter.outline[%i].delete();
	}
	MPMeter.delete();
}

///creates the Screen for the Next Stage
function createNextStage()
{	
	saveGame();
	%next = new Sprite( NextStageScreen );
	%next.Position = "0 0";
	%next.Size = "80 45";
	%next.SceneGroup = 20;
	%next.SceneLayer = 1;
	%next.Image = "Game:NextStage";
	Interface.add(%next);
	
	createNextStageIcons();
	
	createNextStageCurrentLevelIcons();
	createNextStageCurrentLevel();
}

///shows a text showing the next Level
function createNextStageCurrentLevel()
{
	%level = new ImageFont( NextStageLevelFont );
	%level.Image = "Game:Font";
	%level.Position = "0 -14";
	%level.FontSize = "2 3";
	%level.TextAlignment = "Center";
	%level.Text = "Level" SPC $level + 1;
	%level.SceneGroup = 27;
	%level.SceneLayer = 0;
	Interface.add(%level);
}

///shows all Upgrade Icons with their current and max Level
function createNextStageCurrentLevelIcons()
{
	for (%i = 0; %i < 9; %i++)
	{
		createNextStageLevelIcon(%i);
		createNextStageLevelFont(%i);
	}
}

///creates a single Icon for the next Stage Window
function createNextStageLevelIcon(%i)
{
	%icon = new Sprite( NextStageCurrentLevelSymbol );
	%icon.Position = 30 SPC 20.5 - %i * 5;
	%icon.Size = "4 4";
	%icon.SceneGroup = 1;
	%icon.SceneLayer = 0;
	%icon.Image = "Game:" @ getUpgradeName(%i) @ "Icon";
	Interface.add(%icon);
}

function createNextStageLevelFont(%i)
{
	%level = new ImageFont( NextStageLevelFont );
	%level.Image = "Game:Font";
	%level.Position = 38 SPC 22.5 - %i * 5;
	%level.FontSize = "1.5 2.25";
	%level.TextAlignment = "Right";
	%level.Text = $character.getCurrentUpgradeLevel(%i) @ "/" @ getUpgradeLevel(%i);
	%level.SceneGroup = 27;
	%level.SceneLayer = 0;
	Interface.add(%level);
}

function createNextStageIcons()
{
	%list = "";
	for (%i = 0; %i < $character.availableItems.count; %i++)
	{
		%list = addWord(%list, %i);
	}
	if (%list.count > 0)
	{
		%x = getRandom(0, %list.count - 1);
		%qInt = getWord(%list, %x);
		%list = removeWord(%list, %x);
		%qSymbolType = getSymbolType(%qInt);
		
		%qIcon = new Sprite( QIcon );
		%qIcon.type = %qSymbolType;
		%qIcon.id = %qInt;
		%qIcon.Position = "-20 -5";
		%qIcon.Size = "7.5 7.5";
		%qIcon.SceneGroup = 11;
		%qIcon.SceneLayer = 0;
		%qIcon.Image = "Game:" @ %qSymbolType @ "Icon";
		Interface.add(%qIcon);
		
		%q = new Sprite( QButton );
		%q.Position = "-20 -10";
		%q.Size = "3 3";
		%q.SceneGroup = 11;
		%q.SceneLayer = 0;
		%q.Image = "Game:QIcon";
		Interface.add(%q);
	}
	if (%list.count > 0)
	{
		%x = getRandom(0, %list.count - 1);
		%wInt = getWord(%list, %x);
		%list = removeWord(%list, %x);
		%wSymbolType = getSymbolType(%wInt);
		
		%wIcon = new Sprite( WIcon );
		%wIcon.type = %wSymbolType;
		%wIcon.id = %wInt;
		%wIcon.Position = "0 -5";
		%wIcon.Size = "7.5 7.5";
		%wIcon.SceneGroup = 12;
		%wIcon.SceneLayer = 0;
		%wIcon.Image = "Game:" @ %wSymbolType @ "Icon";
		Interface.add(%wIcon);
		
		%w = new Sprite( WButton );
		%w.Position = "0 -10";
		%w.Size = "3 3";
		%w.SceneGroup = 11;
		%w.SceneLayer = 0;
		%w.Image = "Game:WIcon";
		Interface.add(%w);
	}
	if (%list.count > 0)
	{
		%x = getRandom(0, %list.count - 1);
		%eInt = getWord(%list, %x);
		
		%eSymbolType = getSymbolType(%eInt);
		%eIcon = new Sprite( EIcon );
		%eIcon.type = %eSymbolType;
		%eIcon.id = %eInt;
		%eIcon.Position = "20 -5";
		%eIcon.Size = "7.5 7.5";
		%eIcon.SceneGroup = 13;
		%eIcon.SceneLayer = 0;
		%eIcon.Image = "Game:" @ %eSymbolType @ "Icon";
		Interface.add(%eIcon);
		
		%e = new Sprite( EButton );
		%e.Position = "20 -10";
		%e.Size = "3 3";
		%e.SceneGroup = 11;
		%e.SceneLayer = 0;
		%e.Image = "Game:EIcon";
		Interface.add(%e);
	}
}

function destroyNextStage()
{
	NextStageScreen.delete();
	EIcon.delete();
	EButton.delete();
	WIcon.delete();
	WButton.delete();
	QIcon.delete();
	QButton.delete();
	NextStageLevelFont.delete();
	
	//delete all Side Icons
	for (%i = 0; %i < 9; %i++)
	{
		NextStageLevelFont.delete();
		NextStageCurrentLevelSymbol.delete();
	}
}

function getSymbolType(%i)
{
	return getWord($character.availableItems, %i);
}

function openInGameMenu()
{
	if ($gameMenu)
		return;
	
	stopAllInGameSchedules();
	
	$gameMenu = true;
	alxPlay("Game:MenuSelect");
	Level.setScenePause(true);
	
	%menu = new Sprite( GameMenuScreen );
	%menu.Position = $character.Position;
	%menu.Size = "80 45";
	%menu.SceneGroup = 20;
	%menu.SceneLayer = 4;
	%menu.Image = "Game:GameMenu";
	Level.add(%menu);
	
	%back = new ImageFont( GameMenuBackFont );
	%back.Image = "Game:Font";
	%back.FontSize = "2 3";
	%back.Position = getWord($character.Position, 0) SPC getWord($character.Position, 1) + 5;
	%back.Text = "Back to Game";
	%back.SceneGroup = 26;
	%back.SceneLayer = 2;
	%back.setBlendColor("1 1 1");
	%back.setUseInputEvents(true);
	Level.add(%back);
	
	gameMenuSetActiveItem(%back);
	
	%quit = new ImageFont( GameMenuQuitFont );
	%quit.Image = "Game:Font";
	%quit.FontSize = "2 3";
	%quit.Position = getWord($character.Position, 0) SPC getWord($character.Position, 1) - 2.5;
	%quit.Text = "Quit to Title";
	%quit.SceneGroup = 27;
	%quit.SceneLayer = 2;
	%quit.setBlendColor("1 1 1");
	%quit.setUseInputEvents(true);
	Level.add(%quit);
	
	%end = new ImageFont( GameMenuEndFont );
	%end.Image = "Game:Font";
	%end.FontSize = "2 3";
	%end.Position = getWord($character.Position, 0) SPC getWord($character.Position, 1) - 10;
	%end.Text = "Quit Game";
	%end.SceneGroup = 28;
	%end.SceneLayer = 2;
	%end.setBlendColor("1 1 1");
	%end.setUseInputEvents(true);
	Level.add(%end);
}

function gameMenuSetActiveItem(%item)
{
	if (isObject($gameMenuActiveItem))
	{
		$gameMenuActiveItem.deactivateItem();
	}
	$gameMenuActiveItem = %item;
	$gameMenuActiveItem.setRightColor();
}

function menuConfirmJoystick()
{
	gameMenuClick($gameMenuActiveItem);
}

//Input Functions for Menu Elements:
function GameMenuBackFont::onTouchDown(%this)
{
	schedule(1, 0, closeInGameMenu);
	alxPlay("Game:MenuSelect");
}
function GameMenuBackFont::onTouchEnter(%this)
{
	gameMenuSetActiveItem(%this);
}
function GameMenuBackFont::setRightColor(%this)
{
	alxPlay("Game:MenuMove");
	%this.setBlendColor("0.5 0.5 1");
}
function GameMenuBackFont::deactivateItem(%this)
{
	%this.setBlendColor("1 1 1");
}

function GameMenuQuitFont::onTouchDown(%this)
{
	saveGame();
	$gameMenu = false;
	schedule(1, 0, createMenu);
	cancel($createEnemySchedule);
	alxPlay("Game:MenuSelect");
}
function GameMenuQuitFont::onTouchEnter(%this)
{
	gameMenuSetActiveItem(%this);
}
function GameMenuQuitFont::setRightColor(%this)
{
	alxPlay("Game:MenuMove");
	%this.setBlendColor("1 1 0.5");
}
function GameMenuQuitFont::deactivateItem(%this)
{
	%this.setBlendColor("1 1 1");
}

function GameMenuEndFont::onTouchDown(%this)
{
	saveGame();
	alxPlay("Game:MenuSelect");
	
	schedule(500, 0, quit);
}
function GameMenuEndFont::onTouchEnter(%this)
{
	gameMenuSetActiveItem(%this);
}
function GameMenuEndFont::setRightColor(%this)
{
	alxPlay("Game:MenuMove");
	%this.setBlendColor("1 0.5 0.5");
}
function GameMenuEndFont::deactivateItem(%this)
{
	%this.setBlendColor("1 1 1");
}

function closeInGameMenu()
{
	$gameMenu = false;
	Level.setScenePause(false);
	
	GameMenuBackFont.delete();
	GameMenuQuitFont.delete();
	GameMenuEndFont.delete();
	GameMenuScreen.delete();
	
	recallAllInGameSchedules();
}

function stopAllInGameSchedules()
{
	if (isEventPending($createEnemySchedule))
	{
		$createEnemyScheduleDuration = getEventTimeLeft($createEnemySchedule);
		cancel($createEnemySchedule);
		$createEnemyScheduleSave = true;
	}
	
	if (isEventPending($enemy.gravitPointCooldownSchedule))
	{
		$enemy.gravitPointCooldownScheduleDuration = getEventTimeLeft($enemy.gravitPointCooldownSchedule);
		cancel($enemy.gravitPointCooldownSchedule);
		$enemy.gravitPointCooldownScheduleSave = true;
	}
	
	if (isEventPending($enemy.mineCooldownSchedule))
	{
		$enemy.mineCooldownScheduleDuration = getEventTimeLeft($enemy.mineCooldownSchedule);
		cancel($enemy.mineCooldownSchedule);
		$enemy.mineCooldownScheduleSave = true;
	}
	
	if (isEventPending($enemy.invisibilityCooldownSchedule))
	{
		$enemy.invisibilityCooldownScheduleDuration = getEventTimeLeft($enemy.invisibilityCooldownSchedule);
		cancel($enemy.invisibilityCooldownSchedule);
		$enemy.invisibilityCooldownScheduleSave = true;
	}
	
	if (isEventPending($beamUpdateSchedule))
	{
		$beamUpdateScheduleDuration = getEventTimeLeft($beamUpdateSchedule);
		cancel($beamUpdateSchedule);
		$beamUpdateScheduleSave = true;
	}
	
	if (isEventPending($character.UpdateSchedule))
	{
		$character.UpdateScheduleDuration = getEventTimeLeft($character.UpdateSchedule);
		cancel($character.UpdateSchedule);
		$character.UpdateScheduleSave = true;
	}
	
	if (isEventPending($character.walkDschedule))
	{
		$character.walkDscheduleDuration = getEventTimeLeft($character.walkDschedule);
		cancel($character.walkDschedule);
		$character.walkDscheduleSave = true;
	}
	
	if (isEventPending($character.walkUschedule))
	{
		$character.walkUscheduleDuration = getEventTimeLeft($character.walkUschedule);
		cancel($character.walkUschedule);
		$character.walkUscheduleSave = true;
	}
	
	if (isEventPending($character.walkLschedule))
	{
		$character.walkLscheduleDuration = getEventTimeLeft($character.walkLschedule);
		cancel($character.walkLschedule);
		$character.walkLscheduleSave = true;
	}
	
	if (isEventPending($character.walkRschedule))
	{
		$character.walkRscheduleDuration = getEventTimeLeft($character.walkRschedule);
		cancel($character.walkRschedule);
		$character.walkRscheduleSave = true;
	}
	
	if (isEventPending($character.turnOffNoMovingSchedule))
	{
		$character.turnOffNoMovingScheduleDuration = getEventTimeLeft($character.turnOffNoMovingSchedule);
		cancel($character.turnOffNoMovingSchedule);
		$character.turnOffNoMovingScheduleSave = true;
	}
	
	if (isEventPending($character.shootSchedule))
	{
		$character.shootScheduleDuration = getEventTimeLeft($character.shootSchedule);
		cancel($character.shootSchedule);
		$character.shootScheduleSave = true;
	}
	
	if (isEventPending($character.coolDownSchedule))
	{
		$character.coolDownScheduleDuration = getEventTimeLeft($character.coolDownSchedule);
		cancel($character.coolDownSchedule);
		$character.coolDownScheduleSave = true;
	}
	
	if (isEventPending($character.leapingSchedule))
	{
		$character.leapingScheduleDuration = getEventTimeLeft($character.leapingSchedule);
		cancel($character.leapingSchedule);
		$character.leapingScheduleSave = true;
	}
	
	if (isEventPending(LeapIcon.cooldownSchedule))
	{
		LeapIcon.cooldownScheduleDuration = getEventTimeLeft(LeapIcon.cooldownSchedule);
		cancel(LeapIcon.cooldownSchedule);
		LeapIcon.cooldownScheduleSave = true;
	}
	
	if (isEventPending($character.callStunSchedule))
	{
		$character.callStunScheduleDuration = getEventTimeLeft($character.callStunSchedule);
		cancel($character.callStunSchedule);
		$character.callStunScheduleSave = true;
	}
	
	if (isEventPending(StunIcon.cooldownSchedule))
	{
		StunIcon.cooldownScheduleDuration = getEventTimeLeft(StunIcon.cooldownSchedule);
		cancel(StunIcon.cooldownSchedule);
		StunIcon.cooldownScheduleSave = true;
	}
	
	if (isEventPending(BeamIcon.cooldownSchedule))
	{
		BeamIcon.cooldownScheduleDuration = getEventTimeLeft(BeamIcon.cooldownSchedule);
		cancel(BeamIcon.cooldownSchedule);
		BeamIcon.cooldownScheduleSave = true;
	}
	
	if (isEventPending($character.flashSchedule))
	{
		$character.flashScheduleDuration = getEventTimeLeft($character.flashSchedule);
		cancel($character.flashSchedule);
		$character.flashScheduleSave = true;
	}
	
	if (isEventPending($character.resetBlendColorSchedule))
	{
		$character.resetBlendColorScheduleDuration = getEventTimeLeft($character.resetBlendColorSchedule);
		cancel($character.resetBlendColorSchedule);
		$character.resetBlendColorScheduleSave = true;
	}
	
	if (isEventPending($enemy.flashSchedule))
	{
		$enemy.flashScheduleDuration = getEventTimeLeft($enemy.flashSchedule);
		cancel($enemy.flashSchedule);
		$enemy.flashScheduleSave = true;
	}
	
	if (isEventPending($enemy.stunnedSchedule))
	{
		$enemy.stunnedScheduleDuration = getEventTimeLeft($enemy.stunnedSchedule);
		cancel($enemy.stunnedSchedule);
		$enemy.stunnedScheduleSave = true;
	}
	
	if (isEventPending($enemy.coolDownSchedule))
	{
		$enemy.coolDownScheduleDuration = getEventTimeLeft($enemy.coolDownSchedule);
		cancel($enemy.coolDownSchedule);
		$enemy.coolDownScheduleSave = true;
	}
	
	if (isEventPending($enemy.shootSchedule))
	{
		$enemy.shootScheduleDuration = getEventTimeLeft($enemy.shootSchedule);
		cancel($enemy.shootSchedule);
		$enemy.shootScheduleSave = true;
	}
	
	if (isEventPending($enemy.resetBlendColorSchedule))
	{
		$enemy.resetBlendColorScheduleDuration = getEventTimeLeft($enemy.resetBlendColorSchedule);
		cancel($enemy.resetBlendColorSchedule);
		$enemy.resetBlendColorScheduleSave = true;
	}
	
	if (isEventPending($enemy.accelerationSchedule))
	{
		$enemy.accelerationScheduleDuration = getEventTimeLeft($enemy.accelerationSchedule);
		cancel($enemy.accelerationSchedule);
		$enemy.accelerationScheduleSave = true;
	}
	
	if (isEventPending($enemy.resetInvisibilitySchedule))
	{
		$enemy.resetInvisibilityScheduleDuration = getEventTimeLeft($enemy.resetInvisibilitySchedule);
		cancel($enemy.resetInvisibilitySchedule);
		$enemy.resetInvisibilityScheduleSave = true;
	}
	
	if (isEventPending($firedGravitPointUpdateSchedule))
	{
		$firedGravitPointUpdateScheduleDuration = getEventTimeLeft($firedGravitPointUpdateSchedule);
		cancel($firedGravitPointUpdateSchedule);
		$firedGravitPointUpdateScheduleSave = true;
	}
	
	if (isEventPending($powerUpSchedule))
	{
		$powerUpScheduleDuration = getEventTimeLeft($powerUpSchedule);
		cancel($powerUpSchedule);
		$powerUpScheduleSave = true;
	}
	
	if (isEventPending($deactivateDoubleDamageSchedule))
	{
		$deactivateDoubleDamageScheduleDuration = getEventTimeLeft($deactivateDoubleDamageSchedule);
		cancel($deactivateDoubleDamageSchedule);
		$deactivateDoubleDamageScheduleSave = true;
	}
	
	if (isEventPending($deactivateHalfMPSchedule))
	{
		$deactivateHalfMPScheduleDuration = getEventTimeLeft($deactivateHalfMPSchedule);
		cancel($deactivateHalfMPSchedule);
		$deactivateHalfMPScheduleSave = true;
	}
	
	if (isEventPending($deactivateDoubleArmorSchedule))
	{
		$deactivateDoubleArmorScheduleDuration = getEventTimeLeft($deactivateDoubleArmorSchedule);
		cancel($deactivateDoubleArmorSchedule);
		$deactivateDoubleArmorScheduleSave = true;
	}
	
	if (isEventPending($deactivateIncreasedSpeedSchedule))
	{
		$deactivateIncreasedSpeedScheduleDuration = getEventTimeLeft($deactivateIncreasedSpeedSchedule);
		cancel($deactivateIncreasedSpeedSchedule);
		$deactivateIncreasedSpeedScheduleSave = true;
	}
	
	if (isEventPending($deactivatePowerUpResetColorSchedule))
	{
		$deactivatePowerUpResetColorScheduleDuration = getEventTimeLeft($deactivatePowerUpResetColorSchedule);
		cancel($deactivatePowerUpResetColorSchedule);
		$deactivatePowerUpResetColorScheduleSave = true;
	}
}

function recallAllInGameSchedules()
{
	if ($createEnemyScheduleSave)
	{
		$createEnemySchedule = schedule($createEnemyScheduleDuration, 0, createEnemy, "0 0");
	}
	
	if ($enemy.mineCooldownScheduleSave)
	{
		$enemy.mineCooldownSchedule = $enemy.schedule($createEnemyScheduleDuration, mineCooldown);
	}
	
	if ($enemy.gravitPointCooldownScheduleSave)
	{
		$enemy.gravitPointCooldownSchedule = $enemy.schedule($createEnemyScheduleDuration, gravitPointCooldown);
	}
	
	if ($enemy.invisibilityCooldownScheduleSave)
	{
		$enemy.invisibilityCooldownSchedule = $enemy.schedule($createEnemyScheduleDuration, invisibilityCooldown);
	}
	
	if ($beamUpdateScheduleSave)
	{
		$beamUpdateSchedule = Beam.schedule($beamUpdateScheduleDuration, update);
	}
	
	if ($character.UpdateScheduleSave)
	{
		$character.UpdateSchedule = $character.schedule($character.UpdateScheduleDuration, update);
	}
	
	if ($character.walkDscheduleSave)
	{
		$character.walkDschedule = $character.schedule($character.walkDscheduleDuration, walkdown);
	}
	
	if ($character.walkUscheduleSave)
	{
		$character.walkUschedule = $character.schedule($character.walkUscheduleDuration, walkup);
	}
	
	if ($character.walkLscheduleSave)
	{
		$character.walkLschedule = $character.schedule($character.walkLscheduleDuration, walkleft);
	}
	
	if ($character.walkRscheduleSave)
	{
		$character.walkRschedule = $character.schedule($character.walkRscheduleDuration, walkright);
	}
	
	if ($character.turnOffNoMovingScheduleSave)
	{
		$character.turnOffNoMovingSchedule = $character.schedule($character.turnOffNoMovingScheduleDuration, turnOffNoMoving);
	}
	
	if ($character.shootScheduleSave)
	{
		$character.shootSchedule = $character.schedule($character.shootScheduleDuration, shoot);
	}
	
	if ($character.coolDownScheduleSave)
	{
		$character.coolDownSchedule = $character.schedule($character.coolDownScheduleDuration, turnOffCooldown);
	}
	
	if ($character.leapingScheduleSave)
	{
		$character.leapingSchedule = $character.schedule($character.leapingScheduleDuration, stopLeap);
	}
	
	if (LeapIcon.cooldownScheduleSave)
	{
		LeapIcon.cooldownSchedule = LeapIcon.schedule(LeapIcon.cooldownScheduleDuration, updateCooldown);
	}
	
	if ($character.callStunScheduleSave)
	{
		$character.callStunSchedule = $character.schedule($character.callStunScheduleDuration, stunImpact);
	}
	
	if (StunIcon.cooldownScheduleSave)
	{
		StunIcon.cooldownSchedule = StunIcon.schedule(StunIcon.cooldownScheduleDuration, updateCooldown);
	}
	
	if (BeamIcon.cooldownScheduleSave)
	{
		BeamIcon.cooldownSchedule = BeamIcon.schedule(BeamIcon.cooldownScheduleDuration, updateCooldown);
	}
	
	if ($character.flashScheduleSave)
	{
		$character.flashSchedule = $character.schedule($character.flashScheduleDuration, updateflash);
	}
	
	if ($character.resetBlendColorScheduleSave)
	{
		$character.resetBlendColorSchedule = $character.schedule($character.resetBlendColorScheduleDuration, resetBlendColor);
	}
	
	if ($enemy.flashScheduleSave)
	{
		$enemy.flashSchedule = $enemy.schedule($enemy.flashScheduleDuration, updateFlash);
	}
	
	if ($enemy.stunnedScheduleSave)
	{
		$enemy.stunnedSchedule = $enemy.schedule($enemy.stunnedScheduleDuration, endStun);
	}
	
	if ($enemy.coolDownScheduleSave)
	{
		$enemy.coolDownSchedule = $enemy.schedule($enemy.coolDownScheduleDuration, turnOffCooldown);
	}
	
	if ($enemy.shootScheduleSave)
	{
		$enemy.shootSchedule = $enemy.schedule($enemy.shootScheduleDuration, shoot);
	}
	
	if ($enemy.resetBlendColorScheduleSave)
	{
		$enemy.resetBlendColorSchedule = $enemy.schedule($enemy.resetBlendColorScheduleDuration, resetBlendColor);
	}
	
	if ($enemy.resetInvisibilityScheduleSave)
	{
		$enemy.resetInvisibilitySchedule = $enemy.schedule($enemy.resetInvisibilityScheduleDuration, resetInvisibility);
	}
	
	if ($firedGravitPointUpdateScheduleSave)
	{
		$firedGravitPointUpdateSchedule = FiredGravitPoint.schedule($firedGravitPointUpdateScheduleDuration, update);
	}
	
	if ($powerUpScheduleSave)
	{
		$powerUpSchedule = schedule($powerUpScheduleDuration, 0, createPowerUp);
	}
	
	if ($deactivateDoubleDamageScheduleSave)
	{
		$deactivateDoubleDamageScheduleSave = schedule($deactivateDoubleDamageScheduleDuration, 0, deactivateDoubleDamage, $ddObject);
	}
	
	if ($deactivateHalfMPScheduleSave)
	{
		$deactivateHalfMPScheduleSave = schedule($deactivateHalfMPScheduleDuration, 0, deactivateHalfMP, $hmpObject);
	}
	
	if ($deactivateDoubleArmorScheduleSave)
	{
		$deactivateDoubleArmorScheduleSave = schedule($deactivateDoubleArmorScheduleDuration, 0, deactivateDoubleArmor, $daObject);
	}
	
	if ($deactivateIncreasedSpeedScheduleSave)
	{
		$deactivateIncreasedSpeedScheduleSave = schedule($deactivateIncreasedSpeedScheduleDuration, 0, deactivateIncreasedSpeed, $siObject);
	}
	
	if ($deactivatePowerUpResetColorScheduleSave)
	{
		$deactivatePowerUpResetColorSchedule = $daObject2.schedule($deactivatePowerUpResetColorScheduleDuration. resetBlendColor);
	}
}