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
	
	
	//DEBUG
	//Sprite Properties
	%debug = new Sprite( Character );
	%debug.setBodyType( dynamic );
	%debug.Position = "0 0";
	%debug.Size = "6 6";
	%debug.SceneLayer = 10;
	%debug.SceneGroup = 1;
	%debug.setCollisionGroups( None );
	%debug.Image = "Game:Character";
	%debug.createCircleCollisionShape(2);
	%debug.setCollisionCallback(true);
	%debug.setFixedAngle(false);
	%debug.setLinearDamping(2);
	Level.add( %debug );
	
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
