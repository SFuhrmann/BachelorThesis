function createSceneWindow()
{
    // Sanity!
    if ( !isObject(Window) )
    {
        // Create the scene window.
        new SceneWindow(Window);

       // Set Gui profile. If you omit the following line, the program will still run as it uses                
       // GuiDefaultProfile by default

        Window.Profile = GuiDefaultProfile;

        // Place the sceneWindow on the Canvas
        Canvas.setContent( Window );             
    }

    Window.setCameraPosition( 0, 0 );
    Window.setCameraSize( 80, 45 );
    Window.setCameraZoom( 1 );
    Window.setCameraAngle( 0 );
	
	createSceneLevel();
}

function destroySceneWindow()
{
    // Finish if no window available.
    if ( !isObject(Window) )
        return;
    
    // Delete the window.
    Window.delete();
}