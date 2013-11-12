

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
	exec("./scripts/elements/packages.cs");
	exec("./scripts/common scripts/constants.cs");
	exec("./scripts/common scripts/functions.cs");
	exec("./scripts/common scripts/effects.cs");
	exec("./scripts/map/mapcreator.cs");
	exec("./scripts/interface.cs");
	
	//create Main Scene
	createSceneWindow();
	
	//create Character
	createCharacter();
	
	//create Interface
	createInterfaceWindow();
	createInterface();
	
	//Create Map
	createMap($mapSize SPC $mapSize);
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}