﻿///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////
//
// Player Properties - Super Mario 64 Clone (2D)
//
// - Description: Set and stores pickups and state of player
//
///////////////////////////////////////////////////////////////////////
/*
using UnityEngine;
using System.Collections;

public enum PlayerState 
{
	MarioDead	= 0,													// The player is dead
	MarioSmall	= 1,													// sets the size of mario to small
	MarioLarge	= 2, 													// sets the size of mario to large
	MarioFire	= 3														// enable the fireball power for mario 
}

public class script_playerProperties : MonoBehaviour 
{
    PlayerState playerState = PlayerState.MarioSmall;							// Set the display state of mario in the inspector 
	
	public int        lives 					= 3;
	public int        key						= 0;
	public int        bigCoins 					= 0;
	public GameObject projectileFire;
	public Transform  projectileSocketRight;
	public Transform  projectileSocketLeft;
	public Material   materialMarioStandard;
	public Material   materialMarioFire;
	
	public bool changeMario					= false;
	public bool hasFire						= false;
	
	private int  coinLife 				= 100;
	private bool canShoot				= false;


	void Update () 
	{
		script_playerControls playerControls = GetComponent<script_playerControls>();
		
		if ( changeMario )
		{
			SetPlayerState ();
		}	
		
		if ( canShoot )
		{
			GameObject clone;

			if ( Input.GetButtonDown( "Fire2" ) && projectileFire && playerControls.moveDirection == 0 )	// checks if: player pushes button, projectileFire gameobject has been assigned and what direction mario is facing
			{
				clone = Instantiate ( projectileFire, projectileSocketLeft.transform.position, Quaternion.identity ) as GameObject; 
				clone.rigidbody.AddForce ( - 90, 0, 0 );
			} 
			if ( Input.GetButtonDown( "Fire2" ) && projectileFire && playerControls.moveDirection == 1 )	// checks if: player pushes button, projectileFire gameobject has been assigned and what direction mario is facing
			{
				clone = Instantiate ( projectileFire, projectileSocketRight.transform.position, Quaternion.identity ) as GameObject; 
				clone.rigidbody.AddForce ( 90, 0, 0 );
			}  
		}
	}
	
	void Addcoins ( int numKey )
	{
		key += numKey;
	}
	
	void AddCoin ( int numCoin )
	{
		bigCoins += numCoin;
	}

	void SetPlayerState ()
	{
		script_playerControls playerControls = GetComponent<script_playerControls>();
		CharacterController charController = GetComponent<CharacterController>();
		
		switch ( playerState )
		{
		case PlayerState.MarioSmall : 
			
			Debug.Log("State: Mario small");
			playerControls.gravity 	= 0.0f;
			transform.Translate ( 0, 0, - 1.3f );									// moves the player up a little bit
			transform.localScale 	= new Vector3 ( 0.5f, 0.5f, 0.5f );
			charController.height 	= 5.92f;
			transform.renderer.material = materialMarioStandard;
			playerControls.gravity 	= 20.0f;
			canShoot 				= false;
			changeMario 			= false;
			
			break;
			
		case PlayerState.MarioLarge : 
			
			Debug.Log("State: Mario Large");
			
			playerControls.gravity 	= 0.0f;
			transform.Translate ( 0, 0, - 1.3f );
			transform.localScale 	= new Vector3 ( 0.5f, 0.7f, 0.5f );
			charController.height 	= 7.0f;
			transform.renderer.material = materialMarioStandard;
			playerControls.gravity 	= 20.0f;
			canShoot 				= false;
			changeMario 			= false;
			
			break;
			
		case PlayerState.MarioFire  : 
			
			Debug.Log("State: Mario Fire");
			
			playerControls.gravity 	= 0.0f;
			transform.Translate ( 0, 0, - 1.3f );
			transform.localScale 	= new Vector3 ( 0.5f, 0.7f, 0.5f );
			charController.height 	= 7.0f;
			transform.renderer.material = materialMarioFire;
			playerControls.gravity 	= 20.0f;
			canShoot 				= true;
			changeMario 			= false;
			
			break;
			
		case PlayerState.MarioDead  : 
			
			Debug.Log("State: Mario Dead");
			Destroy ( gameObject );
			changeMario = false;
			
			break;	
		}
		
	}

}

*/


using UnityEngine;
using System.Collections;
using UnityEditor;

[AddComponentMenu("Mario Clone/Actor/Player Properties Script")]

public enum		PlayerState		// Placed outside class to make it accessable from anywhere
{
	MarioDead	=	0,														// the player is dead
	MarioSmall	=	1,														// sets the size of the player to small
	MarioLarge	=	2,														// ses the size of the player to large
	MarioFire	=	3,														// enable the fireball power
}

public class script_playerProperties : MonoBehaviour
{
	
	#region									_Fields_
	
	#region Player Movement Speeds
	
	static public float					walkSpeed           			= 4.0f;                     					// speed of the standard walk
	static public float					runSpeed						= 6.0f;                     					// speed of the run
	
	static public float					walkJump            			= 14.0f;                     					// jump height from walk    
	static public float					runJump							= 18.0f;                     					// jump height from run
	static public float					crouchJump          			= 25.0f;                    					// jump height from crouch 
	
	#endregion
	
	#region Enivornmental Forces
	
	static public float					fallSpeed           			= script_environmentalProperties.fallSpeed;                     					// speed of falling down
	static public float					gravity             			= script_environmentalProperties.gravity;                    					// downward force applied on the character      
	static public float					collision_repel_above			= script_environmentalProperties.collision_repel_above;	 
	
	#endregion
	
	
	private AudioSource								playerAudio;	
	public AudioClip								jumpSound;
	public AudioClip								crouchJumpSound;
	public AudioClip								coinSound;
	public AudioClip								pickUpSound;
	public AudioClip 								powerUpSound;
	public AudioClip 								plusOneLifeSound;

	public Transform								particleJump;	
	public Transform								particleCoin;
	
	
	public static int								lives							=	3;
	public static int								coins							=	0;
	public static int								bigCoins						=	0;
	
	public 	Rigidbody								projectileFire;
	
	public 	Transform								projectile_socket_left;
	public 	Transform								projectile_socket_right;
	
	public 	Material								material_player_MarioSmall;
	public  Material 								material_player_MarioLarge;
	public 	Material								material_player_MarioFire;
	
	public bool										changeMario						=	false;
	public bool										marioLarge						= 	false;
	public bool										hasFire							=	false;
	
	private static int								coinLife						=	20;
	private static bool								canShoot						=	false;
	
	public static CharacterController				playerController;		
	public static Transform							playerTransform;
	public static MeshRenderer						playerMeshRender;
	
	public PlayerState								active_player_state				=	PlayerState.MarioSmall;
	

	public  GameObject 								reward_redMushRoom;
	public  GameObject 								reward_greenMushRoom;
	public	GameObject								reward_coin;

	private Animator 								anim_cube_questionMark;
	private Animator 								anim_cube_Zspin;

	private script_cube_questionMark 				cubeQuestionMark;
	private script_cube_Zspin						cubeZspin;


	#endregion
	
	
	#region			_Properties_
	

	
	#endregion
	
	
	#region			UnityEngine Functions
	
	
	// Update is called once per frame
	void Update()
	{
		playerController				=	GetComponent		<CharacterController>	();
		playerTransform					=	GetComponent		<Transform>				();
		playerMeshRender				=	GetComponent		<MeshRenderer>			();		
		playerAudio						=	GetComponent		<AudioSource>			();

		change_player_state		();
		Shoot					();
		
	}
	
	
	
	
	#endregion
	
	
	
	
	void			Shoot					()
	{	
		float playerDirection	=	script_playerControls.moveDirection;
		
		Rigidbody		clone;
		if ( canShoot && Input.GetButtonDown ("Fire1") &&  playerDirection < 0)
		{
			
			Vector3			left_socket			=	projectile_socket_left.transform.position;
			Quaternion		player_rotation		=	playerController.transform.rotation;
			
			clone = Instantiate ( projectileFire, left_socket, player_rotation) as Rigidbody;
			clone.AddForce		( -90, 0, 0);
			
		}
		
		if ( canShoot && Input.GetButtonDown ("Fire1") && playerDirection > 0)
		{
			
			Vector3			right_socket		=	projectile_socket_right.transform.position;
			Quaternion		player_rotation		=	playerController.transform.rotation;
			
			clone = Instantiate ( projectileFire, right_socket, player_rotation) as Rigidbody;
			clone.AddForce		( 90, 0, 0);
		}
	}

	void OnControllerColliderHit ( ControllerColliderHit col )
	{
		if ( col.gameObject.tag == "cubeQuestionMark" )
		{
			Check_cubeQuestionMark(col.gameObject);
		}
		
		if ( col.gameObject.tag == "cubeZspin" )
		{
			Check_cubeZspin(col.gameObject);
		}
	}

	void OnTriggerEnter ( Collider other )
	{
		if ( other.tag == "coin" )
		{
			Addcoins(1);
			script_playerSounds.play_sound ( ref playerAudio, coinSound, 0f);
			Transform clone;
			clone = Instantiate ( particleCoin, other.transform.position, Quaternion.identity) as Transform;
			Destroy(other.gameObject);
		}

		if ( other.tag == "bigCoin" )
		{
			AddBigCoins(1);
			script_playerSounds.play_sound ( ref playerAudio, coinSound, 0f);
			Transform clone;
			clone = Instantiate ( particleCoin, other.transform.position, Quaternion.identity) as Transform;
			Destroy(other.gameObject);
		}

		if ( other.gameObject.tag == "redMushRoom" )
		{
			if ( active_player_state == PlayerState.MarioLarge )
			{
				lives += 1;
				script_playerSounds.play_sound ( ref playerAudio, plusOneLifeSound, 0f);
				// Instantiate particle effect saying +1 
			}

			//Debug.Log ( "Player picked up: " + other.gameObject.name );
			script_playerSounds.play_sound ( ref playerAudio, powerUpSound, 0f);
			Destroy( other.gameObject );
			UpdatePlayerState ( PlayerState.MarioLarge );
		}
	}

	void Check_cubeZspin ( GameObject gameObjectCollidedWith )
	{
		anim_cube_Zspin 	= gameObjectCollidedWith.gameObject.GetComponent<Animator>();
		cubeZspin 			= gameObjectCollidedWith.GetComponent<script_cube_Zspin>();
		Vector3 goPos 		= gameObjectCollidedWith.transform.position;

		GameObject clone;

		if ( cubeZspin.rewardCoin && reward_coin != null && !cubeZspin.zSpin )
		{
			Addcoins( 1 );
			script_playerSounds.play_sound ( ref playerAudio, coinSound, 0f);
			clone = Instantiate ( reward_coin, goPos + new Vector3(0f, 1.6f, 0f), Quaternion.identity) as GameObject; 
			cubeZspin.rewardCoin = false;
		}

		if ( cubeZspin.staticForm )
		{
			anim_cube_Zspin.SetBool("shouldSpin", false);
			anim_cube_Zspin.SetBool("shouldPop", true);
		}

		if ( cubeZspin.zSpin )
		{
			anim_cube_Zspin.SetBool("shouldPop", false);
			anim_cube_Zspin.SetBool("shouldSpin", true);
			gameObjectCollidedWith.transform.parent.GetComponent<BoxCollider>().enabled = false;
			gameObjectCollidedWith.gameObject.GetComponent<BoxCollider>().enabled 		= false;
		}
		
	}


	void Check_cubeQuestionMark ( GameObject gameObjectCollidedWith )
	{
		anim_cube_questionMark = gameObjectCollidedWith.gameObject.GetComponent<Animator>();
		anim_cube_questionMark.SetBool("shouldPop", true);

		cubeQuestionMark 	= gameObjectCollidedWith.GetComponent<script_cube_questionMark>();
		Vector3 goPos 		= gameObjectCollidedWith.transform.position;
		GameObject clone;
		
		if ( cubeQuestionMark.rewardRedMushroom )	// Red Mushroom
		{
			script_playerSounds.play_sound ( ref playerAudio, pickUpSound, 0f);

			clone = Instantiate ( reward_redMushRoom, goPos + new Vector3(0f, 1.6f, 0f), Quaternion.identity) as GameObject; 

			cubeQuestionMark.rewardRedMushroom = false;
		}
		if ( cubeQuestionMark.rewardGreenMushroom )	// Green Mushroom
		{
			script_playerSounds.play_sound ( ref playerAudio, pickUpSound, 0f);

			clone = Instantiate ( reward_greenMushRoom, goPos + new Vector3(0f, 1.6f, 10), Quaternion.identity) as GameObject; 

			cubeQuestionMark.rewardGreenMushroom = false;
		}
	}

	void Addcoins ( int numCoin )
	{
		coins		=	coins + numCoin;
	}
	
	void AddBigCoins ( int numBigCoins )
	{
		bigCoins	=	bigCoins + numBigCoins;
	}



	void UpdatePlayerState ( PlayerState playerState )
	{
		active_player_state = playerState;

		changeMario = true;

		change_player_state();

		SetPlayerState ();
	}

	public void change_player_state()
	{
		if (changeMario == true)
		{ 
			SetPlayerState();
		}
	}

	public void	SetPlayerState ()
	{
		
		switch ( active_player_state )
		{
		case	PlayerState.MarioDead:
			Destroy ( gameObject );
			changeMario					= false;
			break;
			
		case	PlayerState.MarioSmall:
			
			player_scale_small		();
			canShoot					=	false;
			changeMario					=	false;
			marioLarge					= 	false;
			playerMeshRender.material	=	material_player_MarioSmall;
			break;			
			
		case	PlayerState.MarioLarge:
			player_scale_normal		();
			canShoot					=	false;
			changeMario					=	false;
			marioLarge					= 	true;
			playerMeshRender.material	=	material_player_MarioLarge;
			break;
			
		case	PlayerState.MarioFire:
			player_scale_normal		();
			canShoot					=	true;
			changeMario					=	false;
			marioLarge					= 	false;
			playerMeshRender.material	=	material_player_MarioFire;
			break;
			
		}
	}
	

	
	void player_scale_small			()
	{	
		playerTransform.localScale	=	new Vector3	( 0.5f, 0.5f, 0.5f );
		playerTransform.Translate	(0f, 0f, - 1.3f);
		playerController.height		=	5.92f;

		Vector3 playerControllerCenter = playerController.center;
		playerControllerCenter.z = 2.0f;
		playerController.center = playerControllerCenter;
	}
	
	void player_scale_normal			()
	{
		playerTransform.Translate	(0f, 0f, - 1.3f);
		playerTransform.localScale	=	new Vector3	( 0.5f, 0.7f, 0.5f);
		playerController.height		=	6.0f;

		Vector3 playerControllerCenter = playerController.center;
		playerControllerCenter.z = 0.8f;
		playerController.center = playerControllerCenter;
	}
	
	
	public AudioClip get_jump_sound				()
	{
		return this.jumpSound;
	}
	
	public AudioClip get_crouch_jump_sound		()
	{
		return this.crouchJumpSound;
	}


}

















