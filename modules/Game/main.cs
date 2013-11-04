

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
	exec("./scripts/constants.cs");
	exec("./scripts/map/mapcreator.cs");
	
	createSceneWindow();
	
	createCharacter();
	
	createMap("200 200", 5);
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}