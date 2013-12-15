

function Game::create()
{
	//set a random seed
	setRandomSeed(getRealTime()); 
	// Load GUI profiles.
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
	
	createMenu();
}

function createMenu()
{
	
	alxStop($bgm);
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

function destroyMainMenuItems()
{
	TitleFont.delete();
	StartFont.delete();
	UpgradeFont.delete();
}

function createGame()
{
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

function saveGame()
{
	$saveGame.currentScore = $currentScore;
	TamlWrite( $saveGame, "save.taml");
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}


function createMenuItems()
{
	%title = new ImageFont( TitleFont );
	%title.Image = "Game:Font";
	%title.FontSize = "6 9";
	%title.Position = "0 15";
	%title.Text = "Title";
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
	MainMenu.add(%start);
	
	%upgrade = new ImageFont( UpgradeFont );
	%upgrade.Image = "Game:Font";
	%upgrade.FontSize = "3 4.5";
	%upgrade.Position = "0 -10";
	%upgrade.Text = "Upgrade";
	%upgrade.SceneGroup = 29;
	%upgrade.SceneLayer = 2;
	MainMenu.add(%upgrade);
}