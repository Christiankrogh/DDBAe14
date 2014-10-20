


		var cameraTarget		: GameObject;
		var player				: GameObject;

		var smoothTime 			: float			= 0.1;			// time for camera dampen
		var cameraFollowX		: boolean		= true;
		var cameraFollowY		: boolean		= true;
		var cameraFollowHeight	: boolean		= false;		// camera follow cameraTarget object height
		var cameraHeight		: float			= 2.5;			// height of camera adjustable in the 
		var cameraZoom 			: boolean		= false;		// toggle for zoom in and out on ortho-layer
		var cameraZoomMax		: float			= 2.5;			// minimum amount that camera can pull in
		var cameraZoomMin		: float			= 4.0;			// maximim amount that camera can pull out
		var cameraZoomTime		: float			= 0.03;			// speed for camera zooming
		var velocity 			: Vector2;						// speed of the camera movement
		
private var thisTransform 		: Transform;					// cameras transform
private var curPos 				: float			= 0.0;			// current position of cameraTarget
private var playerJumpHeight	: float			= 0.0;			// store jump heigh of player 


function Start ()
{
	thisTransform = transform;	
}

function Update ()
{
	if ( cameraFollowX )
	{
		thisTransform.position.x = Mathf.SmoothDamp ( thisTransform.position.x, cameraTarget.transform.position.x, velocity.x, smoothTime );
	}
	if ( cameraFollowY )
	{
		thisTransform.position.y = Mathf.SmoothDamp ( thisTransform.position.y, cameraTarget.transform.position.y, velocity.y, smoothTime );
	}
	if ( !cameraFollowY && cameraFollowHeight )
	{
		camera.transform.position.y = cameraHeight;
	}
	
	var playerControl = player.GetComponent( "playerControls" );
	if ( cameraZoom )
	{
		curPos 				= player.transform.position.y;				// set current position to players current y position
		playerJumpHeight 	= curPos - playerControl.startPos; 			// subtract current height from playercontrol start position 
		
		if ( playerJumpHeight < 0 )
		{
			playerJumpHeight *= -1;										// anything negative will be positive 
		}
		if ( playerJumpHeight > cameraZoomMax )
		{
			playerJumpheight = cameraZoomMax;
		}
		
		this.camera.orthographicSize = Mathf.Lerp ( this.camera.orthographicSize, playerJumpheight + cameraZoomMin, Time.time * cameraZoomTime );
	}
}





























