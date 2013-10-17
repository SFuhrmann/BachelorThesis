function createSceneLevel( %levelnumber )
{
	$currentLevelNumber = %levelnumber;
    // Destroy the scene if it already exists.
    if ( isObject(Level) )
        destroySceneLevel();
    
    // Create the scene.
    new Scene(Level);

	
	Window.setScene( Level );
	
	Window.mount( $character );
	
	if (!isObject(CharaMovement))
	{
		new ScriptObject(CharaMovement);
		Window.addInputListener(CharaMovement);
		
		CharaMovement.Init_controls();
	}
	
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
