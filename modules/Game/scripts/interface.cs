// ============================================================
// Project            :  BachelorThesis
// File               :  .\modules\Game\scripts\interface.cs
// Copyright          :  
// Author             :  -
// Created on         :  Mittwoch, 6. November 2013 18:18
//
// Editor             :  TorqueDev v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

///create the InterfaceWindow
function createInterfaceWindow()
{
	  if ( !isObject(InterfaceWindow) )
    {
        // Create the scene window.
        new SceneWindow(InterfaceWindow){
			Extent = Canvas.getExtent();
		};
		//InterfaceWindow.checkSize();
		InterfaceWindow.setUseWindowInputEvents(false);

        InterfaceWindow.Profile = GuiInterfaceProfile;

        //Add the Interface to the SceneWindow
        Window.add( InterfaceWindow );
    }
	
    InterfaceWindow.setCameraPosition( 0, 0 );
    InterfaceWindow.setCameraSize( 80, 45 );
    InterfaceWindow.setCameraZoom( 1.0 );
    InterfaceWindow.setCameraAngle( 0 );
	
	// Destroy the scene if it already exists.
    if ( isObject(Interface) )
        destroyInterface();
    
    // Create the scene.
    new Scene(Interface);
	InterfaceWindow.setScene( Interface );
}

///destroy the Interface
function destroyInterface()
{
    // Finish if no scene available.
    if ( !isObject(Interface) )
        return;

    // Delete the scene.
    Interface.delete();
}

///create the Interface
function createInterface()
{
	createHPBarOutline();
	createHPBarFill();
	
	createMPBarOutlines();
	createMPBarFill();
	
	createIcons();
}

///create Interface Icons
function createIcons()
{
	createLeapIcon();
	createStunIcon();
	createBeamIcon();
}

///create Leap Icon
function createLeapIcon()
{
	%leap = new Sprite( LeapIcon );
	%leap.Size = "5 5";
	%leap.Position = "-20 17.5";
	%leap.Image = "Game:LeapIcon";
	%leap.setImageFrame(19);
	%leap.SceneGroup = 1;
	%leap.SceneLayer = 1;
	%leap.setFixedAngle(true);
	Interface.add(%leap);
}

///create Stun Icon
function createStunIcon()
{
	%stun = new Sprite( StunIcon );
	%stun.Size = "5 5";
	%stun.Position = "-35 17.5";
	%stun.Image = "Game:LeapIcon";
	%stun.setImageFrame(19);
	%stun.SceneGroup = 1;
	%stun.SceneLayer = 1;
	%stun.setFixedAngle(true);
	Interface.add(%stun);
}

///create Stun Icon
function createBeamIcon()
{
	%beam = new Sprite( BeamIcon );
	%beam.Size = "5 5";
	%beam.Position = "-27.5 17.5";
	%beam.Image = "Game:LeapIcon";
	%beam.setImageFrame(19);
	%beam.SceneGroup = 1;
	%beam.SceneLayer = 1;
	%beam.setFixedAngle(true);
	Interface.add(%beam);
}

function LeapIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.leapingCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

function StunIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.stunningCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

function BeamIcon::updateCooldown(%this)
{
	%this.setImageFrame(%this.getImageFrame() + 1);
	if (%this.getImageFrame() < 19)
		%this.cooldownSchedule = %this.schedule($character.cooldownTime / 20, updateCooldown);
	else
	{
		$character.beamCooldown = false;
		%glare = showGlare(%this.Position, %this.Size, 200);
		
		Interface.add(%glare);
	}
}

///create HP Bar Outline
function createHPBarOutline()
{
	%hp = new ScriptObject( HPMeter );
	%hp.outline = new ShapeVector( HPMeterOutline );
	%hp.outline.setLineColor( 0.5, 1, 0.5);
	//create Graphics
	%hp.outline.setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
	%hp.outline.Size = "15 24";
	%hp.outline.Position = 32.5 SPC 7.5;
	%hp.outline.SceneGroup = 1;
	%hp.outline.SceneLayer = 1;
	
	//physics
	%hp.outline.setFixedAngle(true);
	
	//add to Scene
	Interface.add(%hp.outline);
}

///create MP Bar Outlines
function createMPBarOutlines()
{
	for (%i = 0; %i < $character.maxMP; %i++)
	{
		%mp = new ScriptObject( MPMeter );
		%mp.outline = new ShapeVector( MPMeterOutline );
		%mp.outline.setLineColor( 0.5, 0.5, 1);
		//create Graphics
		%mp.outline.setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
		%mp.outline.Size = "15 7.5";
		%mp.outline.Position = 36.5 SPC (15.75 - 8.25 * %i);
		%mp.outline.SceneGroup = 1;
		%mp.outline.SceneLayer = 1;
		
		//physics
		%mp.outline.setFixedAngle(true);
		
		//add to Scene
		Interface.add(%mp.outline);
	}
}

///create HP Bar Fill
function createHPBarFill()
{
	HPMeter.fill = new ShapeVector( HPMeterFill );
	HPMeter.fill.setLineColor( 0.5, 1, 0.5);
	//create Graphics
	HPMeter.fill.setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
	HPMeter.fill.Size = "10 23.5";
	HPMeter.fill.Position = 32.5 SPC 7.5;
	HPMeter.fill.SceneGroup = 1;
	HPMeter.fill.SceneLayer = 1;
	
	//physics
	HPMeter.fill.setFixedAngle(true);
	HPMeter.fill.setFillMode(true);
	HPMeter.fill.setFillColor( "0.5 1 0.5" );
	
	//add to Scene
	Interface.add(HPMeter.fill);
	
	HPMeter.fill.update();
}

///create MP Bar Fill
function createMPBarFill()
{
	for (%i = 0; %i < $character.maxMP; %i++)
	{
		MPMeter.fill[%i] = new ShapeVector( MPMeterFill );
		MPMeter.fill[%i].setLineColor( 0.5, 0.5, 1);
		//create Graphics
		MPMeter.fill[%i].setPolyCustom(4, "-0.05 0.5 0.05 0.5 0.05 -0.5 -0.05 -0.5");
		MPMeter.fill[%i].Size = "10 7";
		MPMeter.fill[%i].Position = 36.5 SPC (15.75 - 8.25 * %i);
		MPMeter.fill[%i].SceneGroup = 1;
		MPMeter.fill[%i].SceneLayer = 1;
		
		MPMeter.fill[%i].setFillMode(true);
		MPMeter.fill[%i].setFillColor( "0.5 0.5 1");
		
		//physics
		MPMeter.fill[%i].setFixedAngle(true);
		
		//add to Scene
		Interface.add(MPMeter.fill[%i]);
	}
	MPMeter.fill[0].update(0, $character.MP);
}

///update HP Meter Fill Size
function HPMeterFill::update(%this)
{
	%this.Size = 10 SPC clipToAnotherSize($character.HP, $character.maxHP, 23.5);
	%this.Position = 32.5 SPC 19.25 - getWord(%this.Size, 1) / 2;
}

///update MP Meter Fill Size
function MPMeterFill::update(%this, %number, %mp)
{
	//calculate the value for the current Bar
	%value = 1;
	if (%mp < 1)
	{
		%value = %mp;
		%mp = 0;
	}
	else
	{
		%mp--;
	}
	//clip to the Size of the current Bar
	%this.Size = 10 SPC clipToAnotherSize(%value, 1, 7);
	//set the corresponding Position
	%this.Position = 36.5 SPC (19.25 - 8.25 * %number) - getWord(%this.Size, 1) / 2;
	
	//set Alpha to 0, if Bar should not be drawn
	if (getWord(%this.Size, 1) == 0)
		%this.setLineAlpha(0);
	else
		%this.setLineAlpha(1);
	
	//update the next Bar
	if (%number < 2)
		MPMeter.fill[%number + 1].update(%number + 1, %mp);
}

///clip Percentage value of HP to the Size
function clipToAnotherSize(%value, %maxValue, %newMax)
{
	return %value / %maxValue * %newMax;
}