

function Game::create()
{
	//set a random seed
	setRandomSeed(getRealTime()); 
	// Load all script files
	exec("./gui/guiProfiles.cs");
	exec("./scripts/scene.cs");
	exec("./scripts/scenewindow.cs");
	exec("./scripts/controls/charamovement.cs");
	exec("./scripts/controls/menumovement.cs");
	exec("./scripts/elements/character.cs");
	exec("./scripts/elements/powerups.cs");
	exec("./scripts/elements/projectile.cs");
	exec("./scripts/elements/beam.cs");
	exec("./scripts/elements/enemy.cs");
	exec("./scripts/elements/packages.cs");
	exec("./scripts/common scripts/constants.cs");
	exec("./scripts/common scripts/functions.cs");
	exec("./scripts/common scripts/effects.cs");
	exec("./scripts/map/mapcreator.cs");
	exec("./scripts/interface.cs");
	exec("./scripts/upgrademenu.cs");
	exec("./scripts/Artificial Intelligence/Kinematic Movement/stand_still.cs");
	exec("./scripts/Artificial Intelligence/Kinematic Movement/shoot_character.cs");
	exec("./scripts/Artificial Intelligence/ai_core.cs");
	exec("./scripts/Artificial Intelligence/Structures/actionqueue.cs");
	
	//play bgm with and without percussion and adjust Volume Channels accordingly
	//Channel 2: Menu BGM
	//Channel 3: InGame BGM
	$gamebgm = alxPlay("Game:BGM");
	$menubgm = alxPlay("Game:MenuBGM");
	//workaround for looping problem: replay the BGMs after their length
	schedule(213300, 0, playBGMS);
	
	createMenu();
}

function playBGMS()
{
	alxStop($gamebgm);
	alxStop($menubgm);
	//save Channel Volumes
	%ch2 = alxGetChannelVolume(2);
	%ch3 = alxGetChannelVolume(3);
	//set Volumes to 1
	alxSetChannelVolume(2, 1);
	alxSetChannelVolume(3, 1);
	//play bgms
	$gamebgm = alxPlay("Game:BGM");
	$menubgm = alxPlay("Game:MenuBGM");
	//reset Channel Volumes
	alxSetChannelVolume(2, %ch2);
	alxSetChannelVolume(3, %ch3);
	//schedule again
	schedule(213300, 0, playBGMS);
}

///create the Main Menu
function createMenu()
{
	//Adjust Volume Channels -> Menu BGM
	alxSetChannelVolume(2, 1);
	alxSetChannelVolume(3, 0);
	if(isObject(CharaMovement))
	{
		Window.removeInputListener(CharaMovement);
		CharaMovement.delete();
	}
		
	destroySceneLevel();
	destroyInterface();
		
	$saveGame = TamlRead("save.taml");
	
	if (!$saveGame.existing)
	{
		createSaveGame();
		TamlWrite( $saveGame, "save.taml");
	}
	$currentScore = $saveGame.currentScore;
	
	createSceneWindow();
	createSceneMenu();
	
	createMenuItems();
}

///destroy all items in the Main Menu (Title, Start, Upgrade)
function destroyMainMenuItems()
{
	TitleFont.delete();
	StartFont.delete();
	UpgradeFont.delete();
}

///create the Game Screen
function createGame()
{
	//Adjust Volume Channels -> InGame BGM
	alxSetChannelVolume(2, 0);
	alxSetChannelVolume(3, 1);
	if(isObject(MenuMovement))
	{
		Window.removeInputListener(MenuMovement);
		MenuMovement.delete();
	}
	
	destroySceneLevel();
	destroyInterface();
	destroySceneMenu();
	
	$saveGame = TamlRead("save.taml");
	
	if (!$saveGame.existing)
	{
		createSaveGame();
		TamlWrite( $saveGame, "save.taml");
	}
	
	$currentScore = $saveGame.currentScore;
	
	//create Main Scene
	createSceneWindow();
	createSceneLevel();
	
	//create Character
	createCharacter("10 0");
	
	//create Interface
	createInterfaceWindow();
	createInterface();
	
	createPowerUp();
	
	//create Enemy
	createEnemy("-10 0");
	
	//Create Map
	createMap($mapSize SPC $mapSize);
	
	$gameOver = false;
	
	//debug
	//schedule(4000, $character, changeMaxHP, 50);
	
	$level = 0;
}

///save the Current Score and all Upgrade Levels
function saveGame()
{
	$saveGame.currentScore = $currentScore;
	TamlWrite( $saveGame, "save.taml");
}

function Game::destroy()
{
	alxStopAll();
	destroySceneWindow();

}


///create all Main Menu Items
function createMenuItems()
{
	%title = new ImageFont( TitleFont );
	%title.Image = "Game:Font";
	%title.FontSize = "6 9";
	%title.Position = "0 15";
	%title.Text = "Genius Baus";
	%title.SceneGroup = 31;
	%title.SceneLayer = 2;
	MainMenu.add(%title);
	
	%start = new ImageFont( StartFont );
	%start.Image = "Game:Font";
	%start.Position = "0 0";
	%start.FontSize = "3 4.5";
	%start.Text = "Start Game";
	%start.SceneGroup = 30;
	%start.SceneLayer = 2;
	%start.setUseInputEvents(true);
	MainMenu.add(%start);
	
	%upgrade = new ImageFont( UpgradeFont );
	%upgrade.Image = "Game:Font";
	%upgrade.FontSize = "3 4.5";
	%upgrade.Position = "0 -10";
	%upgrade.Text = "Upgrade";
	%upgrade.SceneGroup = 29;
	%upgrade.SceneLayer = 2;
	%upgrade.setUseInputEvents(true);
	MainMenu.add(%upgrade);
}

function StartFont::onTouchEnter(%this)
{
	%this.setBlendColor("0.5 0.5 1");
	alxPlay("Game:MenuMove");
}
function StartFont::onTouchLeave(%this)
{
	%this.setBlendColor("1 1 1");
}

function UpgradeFont::onTouchEnter(%this)
{
	%this.setBlendColor("0.5 1 0.5");
	alxPlay("Game:MenuMove");
}
function UpgradeFont::onTouchLeave(%this)
{
	%this.setBlendColor("1 1 1");
}