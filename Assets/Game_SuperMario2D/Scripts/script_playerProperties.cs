///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////
//
// Player Properties - Super Mario 64 Clone (2D)
//
// - Description: Set and stores pickups and state of player
//
///////////////////////////////////////////////////////////////////////

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
	public int        coins 					= 0;
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
	
	void AddKeys ( int numKey )
	{
		key += numKey;
	}
	
	void AddCoin ( int numCoin )
	{
		coins += numCoin;
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






















