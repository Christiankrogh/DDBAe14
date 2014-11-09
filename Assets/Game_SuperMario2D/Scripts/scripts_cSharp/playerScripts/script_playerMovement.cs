using UnityEngine;
using System.Collections;

public  class script_playerMovement : MonoBehaviour
{
	static float					runSpeed					= script_playerProperties.runSpeed;
	static float					walkSpeed					= script_playerProperties.walkSpeed;
	
	static float					walkJump					= script_playerProperties.walkJump;                     					// jump height from walk    
	static float					runJump						= script_playerProperties.runJump;                     					// jump height from run
	static float					crouchJump					= script_playerProperties.crouchJump; 
	static float					gravity						= script_playerProperties.gravity;
	static float					fallSpeed					= script_playerProperties.fallSpeed;
	static float					collision_repel_above		= script_playerProperties.collision_repel_above;



	#region						Player Movement Functions

	public static void run_movement ( ref Vector3 velocity)
	{
		velocity.x		=	velocity.x * runSpeed;												// player moves left based on runSpeed
	}
	
	public static void set_player_direction ( ref Vector3 velocity, ref int moveDirection)
	{
		//the player character is facing the right
		if ( velocity.x > 0 )
		{
			moveDirection = 1;
		}
		
		//the player character is facing the left
		if (velocity.x < 0 )
		{
			moveDirection = -1;
		}
	}
	
	public static void set_player_ground_velocity ( ref Vector3 velocity, ref CharacterController playerController )
	{
		if ( playerController.isGrounded == true )
		{
			if ( Input.anyKey )
			{
				//Debug.Log ("Movement left/right: controlled by keyboard");
				velocity        =   new Vector3(Input.GetAxis("Horizontal"), 0,  0 );
			}
			else
			{
				//Debug.Log ("Movement left/right: custom controller");
				velocity		=   new Vector3 ( script_gameController.move_Horizontal, 0, 0 );
			}
			velocity            =   playerController.transform.TransformDirection(velocity);
			velocity.x          =   velocity.x * walkSpeed; 
		}
	}
	
	public static void set_player_air_velocity ( ref Vector3 velocity, ref CharacterController playerController )
	{	
		if ( Input.GetButton ("Fire1") )
		{
			velocity.x          =   -Input.GetAxis("Horizontal") * 8.0f;//runSpeed;
		}
				
		else
		{
			//velocity.x          =   -Input.GetAxis("Horizontal") * 6.0f;//walkSpeed;									// the player can change the direction of movement while they're 
		}

		if ( script_gameController.canJump )
		{
			velocity.x          =   -script_gameController.move_Horizontal * 8.0f;
		}
		else
		{
			//velocity.x          =   -script_gameController.move_Horizontal * 6.0f; 
		}
		
		if (playerController.collisionFlags == CollisionFlags.Above)												// if the player's head collides with an object, repel the player downwards
		{
			velocity.y	=	0;
			velocity.y	=	velocity.y - collision_repel_above;
		}
		
	}
	
	public	static void	player_acceleration_from_gravity ( ref Vector3 velocity, ref CharacterController playerController)
	{
		if ( playerController.isGrounded == false )
		{
			velocity.y          =	velocity.y - (gravity * Time.deltaTime);
		}
	}


	//##############
	// Jump_movement
	public static void jump_movement (ref Vector3 velocity)
	{				
		script_playerControls.in_a_jump			=		true;
		// Keyboard controls
		if ( Input.GetButton( "Fire1" ) )																		// player does a run jump
		{	
			velocity.y  =		runJump;
			velocity.x  =		crouchJump * Input.GetAxis ("Horizontal");										// the run jump moves faster in the x direction than the other jumps
		}
		else
		{	
			velocity.y	=		walkJump;																		// player does a walk jump
		}
		// Custom controller 
		if ( script_gameController.canJump )
		{
			velocity.x  =		crouchJump * script_gameController.move_Horizontal;
		}
		else
		{
			velocity.y	=		walkJump;	
		}

		if ( velocity.x == 0 && Input.GetAxis("Vertical") < 0 || velocity.x == 0 && script_gameController.move_Vertical < 0 )														// player does a crouch jump
		{		
			velocity.y	=		crouchJump;		
			velocity.x  =		velocity.x * crouchJump;
		}
	}
	//############


	public static void crouch_movement ( ref Vector3 velocity )
	{
		velocity.x = 0;																								// prevents the player from moving while crouching
	}

	public static void walk_movement ( ref Vector3 velocity )
	{
		if ( script_gameController.move_Horizontal != 0 )//Input.GetAxis ("Horizontal") != 0 )																		// sets player animation to walk left
		{
			velocity.x		=	velocity.x * walkSpeed;															// player moves left based on walk speed
		}
	}
	
	public static void modulate_jump_height	( ref Vector3 velocity )
	{
		if ( Input.GetButtonUp("Jump") )
		{
			velocity.y = velocity.y - fallSpeed;																// subtract current height from 1 if the jump button is up
		}
	}
	
	#endregion
}

















