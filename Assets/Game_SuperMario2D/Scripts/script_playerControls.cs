///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////

// Player Controller - Super Mario World Clone (2D)

// Controls: 
// a or left arrow 		- Move left
// d or right arrow 	- Move right
// s or down arrow		- Crouch
// spacebar 			- Standard Jump ( see 'Jump' in input setting)
// ctrl + spacebar 		- Run Jump 		( See 'Fire1' in input setting)
// s + spacebar		 	- Crouch Jump 

///////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class script_playerControls : MonoBehaviour 
{
	public float walkSpeed 				= 1.5f;
	public float runSpeed 				= 2.0f;
	public float fallSpeed 				= 2.0f;
	public float walkJump 				= 6.2f;
	public float runJump 				= 9.0f;
	public float crouchJump 			= 10.0f;
	public float gravity 				= 20.0f;
	       float startPos 				= 0.0f;	
	public int   moveDirection 			= 1;												// Look direction, fx 1 = right, 0 = left	
	
	public Transform particleJump;
	public Transform particleJumpFromCrouch;
	
	public AudioClip soundJump;
	public AudioClip soundCrouchJump;
	
	private float soundRate 			= 0.0f;
	//private float soundDelay 			= 0.0f;
	private Vector3 velocity 		 	= Vector3.zero;
	private bool  jumpEnabled 			= false;
	private bool  runJumpEnabled 		= false;
	private bool  crouchJumpEnabled 	= false;
	private float afterHitForceDown 	= 1.0f;											// force player down if head hits anything

	IEnumerator PlaySound ( AudioClip soundName, float soundDelay )
	{
		if ( !audio.isPlaying && Time.time > soundRate )
		{
			soundRate = Time.time + soundDelay;
			audio.clip = soundName;
			audio.Play ();
			yield return new WaitForSeconds ( audio.clip.length );
		}
	}
	
	void Update ()
	{
		Vector3 particlePlacement = new Vector3( transform.position.x, transform.position.y - 1.5f, transform.position.z );	// Set particle at the base of sprite
		
		script_aniSprite aniPlay = GetComponent<script_aniSprite>();
		
		CharacterController controller = GetComponent<CharacterController>();
		
		if ( controller.isGrounded )
		{
			jumpEnabled 		= false;
			runJumpEnabled 		= false;
			crouchJumpEnabled 	= false;
			
			startPos 			= transform.position.y;											// this if ro camera to subtract from on zooming in and out 
			
			velocity = new Vector3( Input.GetAxis("Horizontal"), 0, 0);
			
			if ( velocity.x == 0 && moveDirection == 1 )										// idle right
			{	//Debug.Log ("Idle right");
				aniPlay.aniSprite ( 16, 16, 0, 0, 1, 12 );
				//aniPlay.aniSprite ( 16, 16, 0, 0, 16, 12 );										// Animation call to sprite sheet
			}	
			if ( velocity.x == 0 && moveDirection == 0 )										// idle left
			{	//Debug.Log ("Idle left");
				aniPlay.aniSprite ( 16, 16, 0, 0, 1, 12 );
				//aniPlay.aniSprite ( 16, 16, 0, 1, 16, 12 );
			}	
			if ( velocity.x < 0 )																// walk left
			{	//Debug.Log ("walk left");
				velocity *= walkSpeed;															// Move left based on walkspeed
				aniPlay.aniSprite( 16, 16, 0, 3, 4, 8 );
				//aniPlay.aniSprite ( 16, 16, 0, 3, 10, 15 );	
			}
			if ( velocity.x > 0 )																// walk right
			{	//Debug.Log ("walk right");
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
}























