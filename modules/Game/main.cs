

function Game::create()
{
	// Load GUI profiles.
	exec("./gui/guiProfiles.cs");
	exec("./scripts/scene.cs");
	exec("./scripts/scenewindow.cs");
	exec("./scripts/controls/charamovement.cs");
	exec("./scripts/elements/character.cs");
	exec("./scripts/elements/projectile.cs");
	exec("./scripts/constants.cs");
	
	createSceneWindow();
	
	createCharacter();
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}