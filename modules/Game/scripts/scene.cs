function createSceneLevel()
{
    // Destroy the scene if it already exists.
    if ( isObject(Level) )
        destroySceneLevel();
    
    // Create the scene.
    new Scene(Level);
	Window.setScene( Level );
	
	if (!isObject(CharaMovement))
	{
		new ScriptObject(CharaMovement);
		Window.addInputListener(CharaMovement);
		
		CharaMovement.Init_controls();
	}
	
	Window.setUseObjectInputEvents(false);
	//DEBUG
	
}


function destroySceneLevel()
{
    // Finish if no scene available.
    if ( !isObject(Level) )
        return;

    // Delete the scene.
    Level.delete();
}

//-------------------------------------------------------------------------------

function createSceneMenu()
{
	// Destroy the scene if it already exists.
    if ( isObject(MainMenu) )
        destroySceneMenu();
    
    // Create the scene.
    new Scene(MainMenu);
	Window.setScene( MainMenu );
	
	if (!isObject(MenuMovement))
	{
		new ScriptObject(MenuMovement);
		Window.addInputListener(MenuMovement);
		
		MenuMovement.Init_controls();
	}
	
	Window.setUseObjectInputEvents(true);
}

function destroySceneMenu()
{
    // Finish if no scene available.
    if ( !isObject(MainMenu) )
        return;

    // Delete the scene.
    MainMenu.delete();
}