

function Game::create()
{
	//set a random seed
	setRandomSeed(getRealTime()); 
	// Load GUI profiles.
	exec("./gui/guiProfiles.cs");
	exec("./scripts/scene.cs");
	exec("./scripts/scenewindow.cs");
	exec("./scripts/controls/charamovement.cs");
	exec("./scripts/elements/character.cs");
	exec("./scripts/elements/projectile.cs");
	exec("./scripts/elements/beam.cs");
	exec("./scripts/elements/enemy.cs");
	exec("./scripts/elements/packages.cs");
	exec("./scripts/common scripts/constants.cs");
	exec("./scripts/common scripts/functions.cs");
	exec("./scripts/common scripts/effects.cs");
	exec("./scripts/map/mapcreator.cs");
	exec("./scripts/interface.cs");
	exec("./scripts/Artificial Intelligence/Kinematic Movement/stand_still.cs");
	exec("./scripts/Artificial Intelligence/Kinematic Movement/shoot_character.cs");
	exec("./scripts/Artificial Intelligence/ai_core.cs");
	exec("./scripts/Artificial Intelligence/Structures/actionqueue.cs");
	
	createGame();
}

function createGame()
{
	destroySceneLevel();
	destroyInterface();
	
	//create Main Scene
	createSceneWindow();
	
	//create Character
	createCharacter("10 0");
	
	//create Interface
	createInterfaceWindow();
	createInterface();
	
	//create Enemy
	createEnemy("-10 0");
	
	//Create Map
	createMap($mapSize SPC $mapSize);
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}