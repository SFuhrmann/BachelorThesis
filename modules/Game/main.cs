function Game::create()
{
	// Load GUI profiles.
	exec("./gui/guiProfiles.cs");
	
	createSceneWindow();
}

function Game::destroy()
{

	//charcontrols.pop();
	
	destroySceneWindow();
	
	//CharaMovement.delete();

}