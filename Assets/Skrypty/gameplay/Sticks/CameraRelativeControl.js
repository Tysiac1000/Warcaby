//////////////////////////////////////////////////////////////
// CameraRelativeControl.js
// Penelope iPhone Tutorial
//
// CameraRelativeControl creates a control scheme similar to what
// might be found in 3rd person platformer games found on consoles.
// The left stick is used to move the character, and the right
// stick is used to rotate the camera around the character.
// A quick double-tap on the right joystick will make the 
// character jump. 
//////////////////////////////////////////////////////////////

#pragma strict

var cursorJoystick : Joystick;
var rotateJoystick : Joystick;

var cameraPivot : Transform;						// The transform used for camera rotation
var cameraTransform : Transform;				// The actual transform of the camera

var speed : float = 5;								// Ground speed
var rotationSpeed : Vector2 = Vector2( 50, 25 );	// Camera rotation speed for each axis

private var thisTransform : Transform;

function Start()
{
	// Cache component lookup at startup instead of doing this every frame	
	thisTransform = GetComponent( Transform );	
}

function OnEndGame()
{
	// Disable joystick when the game ends	
	cursorJoystick.Disable();
	rotateJoystick.Disable();
	
	// Don't allow any more control changes when the game ends
	this.enabled = false;
}

function Update()
{
	// Scale joystick input with rotation speed
	var camRotation = rotateJoystick.position;
	camRotation.x *= rotationSpeed.x;
	camRotation.y *= rotationSpeed.y;
	camRotation *= Time.deltaTime;
	
	// Rotate around the character horizontally in world, but use local space
	// for vertical rotation
	cameraPivot.Rotate( 0, -camRotation.x, 0, Space.World );
	cameraPivot.Rotate( camRotation.y, 0, 0 );
}