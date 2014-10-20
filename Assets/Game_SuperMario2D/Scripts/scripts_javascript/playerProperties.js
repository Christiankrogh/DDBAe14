///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////
//
// Player Properties - Super Mario 64 Clone (2D)
//
// - Description: Set and stores pickups and state of player
//
///////////////////////////////////////////////////////////////////////

		enum PlayerState 
		{
			MarioDead	= 0,													// The player is dead
			MarioSmall	= 1,													// sets the size of mario to small
			MarioLarge	= 2, 													// sets the size of mario to large
			MarioFire	= 3														// enable the fireball power for mario 
		}

		var playerState = PlayerState.MarioSmall;							// Set the display state of mario in the inspector 
	
		var lives 					: int 			= 3;
		var key						: int			= 0;
		var coins 					: int			= 0;
		var projectileFire  		: GameObject;
		var projectileSocketRight 	: Transform;
		var projectileSocketLeft 	: Transform;
		var materialMarioStandard 	: Material;
		var materialMarioFire		: Material;

		var changeMario				: boolean		= false;
		var hasFire					: boolean		= false;
	
private var coinLife 				: int			= 100;
private var canShoot				: boolean		= false;



function Update () 
{
	var playerControls = GetComponent ( "playerControls" );

	if ( changeMario )
	{
		SetPlayerState ();
	}	
	
	if ( canShoot )
	{
		var clone;
		if ( Input.GetButtonDown( "Fire2" ) && projectileFire && playerControls.moveDirection == 0 )	// checks if: player pushes button, projectileFire gameobject has been assigned and what direction mario is facing
		{
			clone = Instantiate ( projectileFire, projectileSocketLeft.transform.position, Quaternion.identity ); 
			clone.rigidbody.AddForce ( - 90, 0, 0 );
		} 
		if ( Input.GetButtonDown( "Fire2" ) && projectileFire && playerControls.moveDirection == 1 )	// checks if: player pushes button, projectileFire gameobject has been assigned and what direction mario is facing
		{
			clone = Instantiate ( projectileFire, projectileSocketRight.transform.position, Quaternion.identity ); 
			clone.rigidbody.AddForce ( 90, 0, 0 );
		}  
	}
}

function AddKeys ( numKey : int )
{
	key += numKey;
}

function AddCoin ( numCoin : int )
{
	coins += numCoin;
}

function SetPlayerState ()
{
	var playerControls = GetComponent( "playerControls" );
	var charController = GetComponent( CharacterController );
	
	switch ( playerState )
	{
		case PlayerState.MarioSmall : 
			
				Debug.Log("State: Mario small");
				playerControls.gravity 	= 0.0;
				transform.Translate ( 0, 0, - 1.3 );									// moves the player up a little bit
				transform.localScale 	= Vector3 ( 0.5, 0.5, 0.5 );
				charController.height 	= 5.92;
				transform.renderer.material = materialMarioStandard;
				playerControls.gravity 	= 20.0;
				canShoot 				= false;
				changeMario 			= false;
			
			break;
			
		case PlayerState.MarioLarge : 
			
				Debug.Log("State: Mario Large");
			
				playerControls.gravity 	= 0.0;
				transform.Translate ( 0, 0, - 1.3 );
				transform.localScale 	= Vector3 ( 0.5, 0.7, 0.5 );
				charController.height 	= 7.0;
				transform.renderer.material = materialMarioStandard;
				playerControls.gravity 	= 20.0;
				canShoot 				= false;
				changeMario 			= false;
			
			break;
			
		case PlayerState.MarioFire  : 
		
				Debug.Log("State: Mario Fire");
				
				playerControls.gravity 	= 0.0;
				transform.Translate ( 0, 0, - 1.3 );
				transform.localScale 	= Vector3 ( 0.5, 0.7, 0.5 );
				charController.height 	= 7.0;
				transform.renderer.material = materialMarioFire;
				playerControls.gravity 	= 20.0;
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


@script AddComponentMenu ( "Christian/Actor/Player Properties Script" )		// Assign this script to the menu







































