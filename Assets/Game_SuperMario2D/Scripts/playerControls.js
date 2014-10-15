///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////

// Player Controller - Super Mario Clone (2D)

// Controls: 
// a or left arrow 		- Move left
// d or right arrow 	- Move right
// s or down arrow		- Crouch
// spacebar 			- Standard Jump ( see 'Jump' in input setting)
// ctrl + spacebar 		- Run Jump 		( See 'Fire1' in input setting)
// s + spacebar		 	- Crouch Jump 

///////////////////////////////////////////////////////////////////////

		var walkSpeed 				: float		= 1.5;
		var runSpeed 				: float		= 2.0;
		var fallSpeed 				: float		= 2.0;
		var walkJump 				: float 	= 6.2;
		var runJump 				: float 	= 9.0;
		var crouchJump 				: float		= 10.0;
		var gravity 				: float 	= 20.0;
		var startPos 				: float		= 0.0;	
		var moveDirection 			: int		= 1;												// Look direction, fx 1 = right, 0 = left	

		var particleJump 			: Transform;	
		var particleJumpFromCrouch 	: Transform;
						
		var soundJump				: AudioClip;
		var soundCrouchJump 		: AudioClip;

private var soundRate 				: float		= 0.0;
private var soundDelay 				: float		= 0.0;
private var velocity 				: Vector3 	= Vector3.zero;
private var jumpEnabled 			: boolean	= false;
private var runJumpEnabled 			: boolean	= false;
private var crouchJumpEnabled 		: boolean	= false;
private var afterHitForceDown   	: float		= 1.0;											// force player down if head hits anything


function PlaySound ( soundName, soundDelay )
{
	if ( !audio.isPlaying && Time.time > soundRate )
	{
		soundRate = Time.time + soundDelay;
		audio.clip = soundName;
		audio.Play ();
		yield WaitForSeconds ( audio.clip.length );
	}
}

function Update ()
{
	var particlePlacement : Vector3 = Vector3( transform.position.x, transform.position.y - 1.5, transform.position.z );	// Set particle at the base of sprite

	var aniPlay = GetComponent ( "aniSprite" );

	var controller : CharacterController = GetComponent (CharacterController);
	
	if ( controller.isGrounded )
	{
		jumpEnabled 		= false;
		runJumpEnabled 		= false;
		crouchJumpEnabled 	= false;
		
		velocity = Vector3( Input.GetAxis("Horizontal"), 0, 0 );

		if ( velocity.x == 0 && moveDirection == 1 )										// idle right
		{
			aniPlay.aniSprite ( 16, 16, 0, 0, 1, 12 );
			//aniPlay.aniSprite ( 16, 16, 0, 0, 16, 12 );										// Animation call to sprite sheet
		}	
		if ( velocity.x == 0 && moveDirection == 0 )										// idle left
		{
			aniPlay.aniSprite ( 16, 16, 0, 0, 1, 12 );
			//aniPlay.aniSprite ( 16, 16, 0, 1, 16, 12 );
		}	
		if ( velocity.x < 0 )																// walk left
		{
			velocity *= walkSpeed;															// Move left based on walkspeed
			aniPlay.aniSprite( 16, 16, 0, 3, 4, 8 );
			//aniPlay.aniSprite ( 16, 16, 0, 3, 10, 15 );	
		}
		if ( velocity.x > 0 )																// walk right
		{
			velocity *= walkSpeed;															// Move right based on walkspeed
			aniPlay.aniSprite( 16, 16, 0, 2, 4, 8 );
			//aniPlay.aniSprite ( 16, 16, 0, 2, 10, 15 ); 
		}
		if ( velocity.x < 0 && Input.GetButton( "Fire1" ) )									// run left
		{
			velocity *= runSpeed;															// Move left based on runSpeed
			aniPlay.aniSprite( 16, 16, 0, 3, 4, 8 );
			//aniPlay.aniSprite ( 16, 16, 0, 5, 16, 24 ); 
		}
		if ( velocity.x > 0 && Input.GetButton( "Fire1" ) )									// run right
		{
			velocity *= runSpeed;															// Move right based on runSpeed
			aniPlay.aniSprite( 16, 16, 0, 2, 4, 8 );
			//aniPlay.aniSprite ( 16, 16, 0, 4, 16, 24 ); 
		}
		if ( velocity.x == 0 && Input.GetAxis( "Vertical" ) < 0 )
		{
			if ( moveDirection == 0 )														// crouch left
			{
				velocity.x = 0;																// keep player from moving while crouching
				aniPlay.aniSprite ( 16, 16, 0, 9, 1, 1 ); 
				//aniPlay.aniSprite ( 16, 16, 0, 9, 16, 24 ); 								// crouching left
			}
			if ( moveDirection == 1 )														// crouch right
			{
				velocity.x = 0;
				aniPlay.aniSprite ( 16, 16, 0, 8, 1, 1 ); 
				//aniPlay.aniSprite ( 16, 16, 0, 8, 16, 24 ); 								// crouching 
			}										
		}
		if ( Input.GetButton 	 ("Jump")   && ( !Input.GetButton( "Fire1" ) || Input.GetButton ( "Fire1" ) && velocity.x == 0 ) && Input.GetAxis( "Vertical" ) >= 0 )
		{
			velocity.y  		= walkJump;
			Instantiate ( particleJump, particlePlacement, Quaternion.identity );
			PlaySound ( soundJump, 0 );
			jumpEnabled 		= true;
		}
		if ( Input.GetButtonDown ( "Jump" ) && Input.GetButton ( "Fire1" ) && velocity.x != 0 )
		{
			velocity.y 			= runJump;	
			Instantiate ( particleJump, particlePlacement, Quaternion.identity );
			PlaySound ( soundJump, 0 );
			runJumpEnabled 		= true; 
		}
		if ( Input.GetButtonDown ( "Jump" ) && velocity.x == 0 && Input.GetAxis( "Vertical" ) < 0 )
		{
			velocity.y 			= crouchJump;
			Instantiate ( particleJumpFromCrouch, particlePlacement, Quaternion.identity );
			PlaySound ( soundCrouchJump, 0 );
			crouchJumpEnabled 	= true;
		}
	}
	
	if ( !controller.isGrounded )
	{
		velocity.x = Input.GetAxis( "Horizontal" );
		
		if ( Input.GetButtonUp( "Jump" ) )														// Jump controlled height
		{
			velocity.y = velocity.y - fallSpeed;												// subtract current height from 1 if jump button is up 
		}
		if ( moveDirection == 0 )																// facing left
		{
			if  ( jumpEnabled )
			{
				velocity.x *= walkSpeed;														// walk left speed * current move speed 
				//aniPlay.aniSprite ( 16, 16, 0, 8, 3, 8 ); 
				aniPlay.aniSprite ( 16, 16, 11, 3, 4, 12 );
			}
			if  ( runJumpEnabled )
			{
				velocity.x *= runSpeed;	
				//aniPlay.aniSprite ( 16, 16, 0, 8, 3, 8 );
				aniPlay.aniSprite ( 16, 16, 11, 3, 4, 12 );
			}
			if  ( crouchJumpEnabled )
			{
				velocity.x *= walkSpeed;
				aniPlay.aniSprite ( 16, 16, 12, 11, 1, 1 );
				//aniPlay.aniSprite ( 16, 16, 12, 11, 4, 12 );	
			}
		}
		if ( moveDirection == 1 )																// facing right
		{
			if  ( jumpEnabled )
			{
				velocity.x *= walkSpeed;														// walk right speed * current move speed 
				//aniPlay.aniSprite ( 16, 16, 0, 9, 3, 8 ); 
				aniPlay.aniSprite ( 16, 16, 11, 2, 4, 12 );
			}
			if  ( runJumpEnabled )
			{
				velocity.x *= runSpeed;	
				//aniPlay.aniSprite ( 16, 16, 0, 9, 3, 8 ); 
				aniPlay.aniSprite ( 16, 16, 11, 2, 4, 12 );
			}
			if  ( crouchJumpEnabled )
			{
				velocity.x *= walkSpeed;
				aniPlay.aniSprite ( 16, 16, 12, 10, 1, 1 ); 
				//aniPlay.aniSprite ( 16, 16, 12, 10, 4, 12 );	
			}
		}			
	}
	
	if ( velocity.x < 0 )																		// get last move direction 
	{
		moveDirection = 0;
	}
	if ( velocity.x > 0 )
	{
		moveDirection = 1;
	}
	
	if ( controller.collisionFlags == CollisionFlags.Above )
	{
		velocity.y = 0;
		velocity.y -= afterHitForceDown;														// apply force downward so player doesnt hang in air
	}
	
	velocity.y -= gravity * Time.deltaTime;
	controller.Move ( velocity * Time.deltaTime );
}



















































