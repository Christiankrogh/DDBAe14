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
	#region Fields
	


	#region Player Sounds
	

	
	#endregion	
	
	public static 	int					moveDirection       		=	1;                							// direction the player is facing, 1 = right, -1 = left
	static private 	Vector3				velocity            		=	Vector3.zero;      							// direction and speed of player
	public static 	float				startPosition       		=	0.0f;             							// coordinate of start postioon
	public static 	bool				in_a_jump					=	false;
	static			CharacterController	playerController			=	script_playerProperties.playerController;
	static			AudioSource			playerAudio;
	
	#region Particles
	
	// particle for feet hitting the ground
	static private Vector3				particlePlacement;		
	

	#endregion
	
	static 			script_playerProperties playerProps;

	#endregion
	
	private 			bool 				active 						= false;
    
   
	void Update	()
	{	
		CharacterController playerController		=		GetComponent<CharacterController>	 ();
		AudioSource			playerAudio				=		GetComponent<AudioSource>			 ();
		playerProps									=		GetComponent<script_playerProperties>();

		player_actions											( ref playerController, ref playerAudio );
		
		script_playerMovement.player_acceleration_from_gravity	( ref velocity, ref playerController);
		script_playerMovement.set_player_direction				( ref velocity, ref moveDirection);
		
		playerController.Move									( velocity * Time.deltaTime );
	}

	
	#region				Player Action Functions
	
	void player_actions			( ref CharacterController playerController, ref AudioSource playerAudio )
	{
		air_actions		( ref playerController, ref playerAudio );
		ground_actions	( ref playerController, ref playerAudio );
	}	
	
	void ground_actions			( ref CharacterController playerController, ref AudioSource playerAudio )
	{
		if	(playerController.isGrounded == true)												// movements available to the player on the ground
		{
			in_a_jump					=		false;
			startPosition				=		transform.position.y;
			script_playerMovement.set_player_ground_velocity		( ref velocity, ref playerController );
			crouch_action									( ref playerController, ref playerAudio );
			run_action										( ref playerController, ref playerAudio );
			jump_action										( ref playerController, ref playerAudio );
			walk_action										( ref playerController, ref playerAudio );
			idle_action										( ref playerController, ref playerAudio );
		}
	}
	
	static void	air_actions		( ref CharacterController playerController, ref AudioSource playerAudio )				// movements available to the player in the air
	{
		if ( playerController.isGrounded == false )
		{
			script_playerMovement.modulate_jump_height		( ref velocity);

			script_playerMovement.set_player_air_velocity	( ref velocity, ref playerController );

			if (in_a_jump)
			{ 
				script_playerAnimation.jump_animation	( ref playerController, ref velocity, moveDirection );
			}
		}
	}


	//############
	// Jump Action 
	static void	jump_action		( ref CharacterController playerController, ref AudioSource playerAudio )
	{
		if ( Input.GetButton( "Jump" ) || script_gameController.canJump )															// controls which type of jump the player character will execute
		{				
			// The way jumping behaves:
			script_playerMovement.jump_movement		( ref velocity);

			// Jumping animation:
			script_playerAnimation.jump_animation	( ref playerController, ref velocity, moveDirection);

			// Jumping sound
			script_playerSounds.use_jump_audio		( ref playerAudio, playerProps.jumpSound, playerProps.crouchJumpSound, ref velocity);

			// Jumping particle 
			jump_particle							( playerController );
		}
	}
	//###########


	//###########
	// Run Action
	static void	run_action 		( ref CharacterController playerController, ref AudioSource playerAudio )
	{
		if ( velocity.x != 0 && Input.GetButton ("Fire1") || velocity.x != 0 && script_gameController.canRun )										// sets player animation to run left()
		{
			// The way running behaves:
			script_playerMovement.run_movement			( ref velocity );

			// Running animation:
			script_playerAnimation.run_animation		( ref playerController, ref velocity );
		}
	}
	//##########


	static void	crouch_action 	( ref CharacterController playerController, ref AudioSource playerAudio )
	{
        
        if (velocity.x == 0 && Input.GetAxis("Vertical") < 0 )
		{
            //Debug.Log("crouch_action_keyboard");
			script_playerMovement.crouch_movement	( ref velocity);
			script_playerAnimation.crouch_animation	( ref playerController, ref velocity, moveDirection);
		}

        float   playerControllerHeight = playerController.height;
        Vector3 playerControllerCenter = playerController.center;
        PlayerState playerState = script_playerProperties.active_player_state;

        if ( velocity.x == 0 && script_gameController.move_Vertical < 0 )
        {
            //Debug.Log( "crouch_action" );
            script_playerMovement.crouch_movement( ref velocity );
            script_playerAnimation.crouch_animation( ref playerController, ref velocity, moveDirection );

            if ( playerState == PlayerState.MarioLarge )
            {     
                playerControllerCenter.z = 2.2f;
                playerControllerHeight = 3.0f;
                playerController.center = playerControllerCenter;
                playerController.height = playerControllerHeight;
            } 
        }
        else
        {
            if ( playerState == PlayerState.MarioLarge )
            {  
                playerControllerCenter.z = 0.8f;
                playerControllerHeight = 5.92f;
                playerController.center = playerControllerCenter;
                playerController.height = playerControllerHeight;
            }
        }
	}
	
	static void idle_action		( ref CharacterController playerController, ref AudioSource playerAudio )
	{
        /*
		if	( velocity.x == 0 && velocity.y == 0 && Input.GetAxis ("Vertical") >= 0 && Input.GetAxis ("Horizontal") == 0 )
		{
			script_playerAnimation.idle_animation		( ref playerController, moveDirection );
		}
        */
        if (velocity.x == 0 && velocity.y == 0 && script_gameController.move_Vertical >= 0 && script_gameController.move_Horizontal == 0)
        {
            script_playerAnimation.idle_animation(ref playerController, moveDirection);
        }
	}		
	
	static void	walk_action		( ref CharacterController playerController, ref AudioSource playerAudio )
	{
		if (velocity.x != 0 && velocity.y == 0 && Input.GetButton ("Fire1") == false && !script_gameController.canRun )																		// sets player animation to walk left
		{
			script_playerMovement.walk_movement			( ref velocity);
			script_playerAnimation.walk_animation		( ref playerController, ref velocity);
		}
	}

	#endregion

	#region Particle Functions
	
	static void	jump_particle ( CharacterController playerController )
	{
		Vector3 playerPosition 	= playerController.transform.position;
		particlePlacement 		= new Vector3 ( playerPosition.x, (playerPosition.y - 0.5f) , playerPosition.z );

		Instantiate( playerProps.particleJump, particlePlacement, playerController.transform.rotation );
	}
	
	#endregion
}




















