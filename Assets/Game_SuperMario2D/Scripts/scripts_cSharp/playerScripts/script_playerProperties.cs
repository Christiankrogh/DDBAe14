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
//using UnityEditor;
using UnityEngine.UI;

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
	
	static public float					walkSpeed           			= 3.2f;                     					// speed of the standard walk
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
	public AudioClip 								powerDown;
	public AudioClip 								plusOneLifeSound;
	public AudioClip 								jumpOnEnemySound;
	public AudioClip								marioDied;
	public AudioClip 								fireball;
	public AudioClip 								levelBackMusic;
    public AudioClip                                levelCompleteSound;

	public Transform								particleJump;	
	public Transform								particleCoin;
	
	
	public static int								lives							=	3;
	public static int								coins							=	0;
	public static int								bigCoins						= 	0;
	public static float								totalCoinCollected				=	0;
	
	public 	Rigidbody								projectileFire;
	
	public 	Transform								projectile_socket_left;
	public 	Transform								projectile_socket_right;
	
	public 	Material								material_player_MarioSmall;
	public  Material 								material_player_MarioLarge;
	public 	Material								material_player_MarioFire;

	public bool										changeMario						=	false;
    public static bool                              marioDead                       =   false;
	public bool										marioLarge						= 	false;
    public bool                                     marioFire                       =   false;
	public bool										hasFire							=	false;
	
	private static int								coinLife						=	20;
	private static bool								canShoot						=	false;
                   bool                             shootNow                        =   true;

	public static CharacterController				playerController;		
	public static Transform							playerTransform;
	public static MeshRenderer						playerMeshRender;
    script_gameTimer                                time;

	public static PlayerState					    active_player_state				=	PlayerState.MarioSmall;
	
	public  GameObject 								guiText;

	public  GameObject 								reward_redMushRoom;
	public  GameObject 								reward_greenMushRoom;
	public	GameObject								reward_coin;
	public  GameObject								reward_fireUpgrade;

	private Animator 								anim_cube_questionMark;
	private Animator 								anim_cube_Zspin;

	private script_cube_questionMark 				cubeQuestionMark;
	private script_cube_Zspin						cubeZspin;
	private bool 									cannotBeKilled 					= false;
    private Vector3                                 startPos;
    private Vector3 								spawnPos;

	#endregion
	
	
	#region			UnityEngine Functions

	void Start()
	{
        startPos    = transform.position;
		spawnPos 	= transform.position;
	}


	// Update is called once per frame
	void Update()
	{
		playerController				=	GetComponent		<CharacterController>	();
		playerTransform					=	GetComponent		<Transform>				();
		playerMeshRender				=	GetComponent		<MeshRenderer>			();		
		playerAudio						=	GetComponent		<AudioSource>			();
        time                            =   GameObject.FindGameObjectWithTag("sceneManager").GetComponent<script_gameTimer>();

		change_player_state		();
		Shoot					();

        if ( script_sceneManager.level_restart )
        { 
            spawnPos            = startPos;
            StartCoroutine( MarioRespawn( 0.0f ) );
            lives = 3;
            coins = 0;
            bigCoins = 0;
            totalCoinCollected = 0;
            time.setTime = true;
        }
	}
	
	
	#endregion

   
	void                Shoot                   ()
	{	
		float 		playerDirection	=	script_playerControls.moveDirection;
       
		Rigidbody	clone;

		if ( canShoot && Input.GetButtonDown ("Fire1") &&  playerDirection < 0 || canShoot && script_gameController.canShoot && playerDirection < 0 )
		{
            if (shootNow)
            {
                shootNow = false;

                Vector3     left_socket = projectile_socket_left.transform.position;
                Quaternion  player_rotation = playerController.transform.rotation;

                script_playerSounds.play_sound(ref playerAudio, fireball, 0f);

                clone = Instantiate(projectileFire, left_socket, Quaternion.identity) as Rigidbody;
                clone.AddForce(-90, 0, 0);

                StartCoroutine(BulletDelay( 0.2f));
            }	
		}

        if (canShoot && Input.GetButtonDown("Fire1") && playerDirection > 0 || canShoot && script_gameController.canShoot && playerDirection > 0)
		{
            if (shootNow)
            {
                shootNow = false;
                Vector3 right_socket = projectile_socket_right.transform.position;
                Quaternion player_rotation = playerController.transform.rotation;

                script_playerSounds.play_sound(ref playerAudio, fireball, 0f);

                clone = Instantiate(projectileFire, right_socket, Quaternion.identity) as Rigidbody;
                clone.AddForce(90, 0, 0);

                StartCoroutine(BulletDelay(0.2f));
            }
		}
	}

    IEnumerator         BulletDelay             (float seconds )
    {
        yield return new WaitForSeconds(seconds);

        shootNow = true;
    }

    void                OnControllerColliderHit ( ControllerColliderHit col )
    {
         GameObject otherPos = col.gameObject;

         if ( col.gameObject.tag == "cubeQuestionMark" )
         {
             Check_cubeQuestionMark(col.gameObject);
         }

         if ( col.gameObject.tag == "cubeZspin" )
         {
             Check_cubeZspin(col.gameObject);
         }

         if ( col.gameObject.tag == "enemyDragonHead" )
         {
             MarioHitDragonHead( col, otherPos );
         }

         if ( col.gameObject.tag == "enemyEvilPlant" )
         {
             Debug.Log ("Hit by: " + col.gameObject.name + " - Mario is dead!" );

             UpdateMarioLifeSituation ();
         }
    }

    void                OnTriggerEnter          ( Collider other )
	{
		GameObject otherPos = other.gameObject;

        if ( other.gameObject.tag == "enemyDragon" )
        {
            Debug.Log( "Hit by: " + other.gameObject.name + " - Mario is dead!" );

            UpdateMarioLifeSituation();
        }

        if ( other.gameObject.tag == "enemyCannonBall" )
        {
            Debug.Log( "Hit by: " + other.gameObject.name + " - Mario is dead!" );

            UpdateMarioLifeSituation();
        }

        if ( other.tag == "levelComplete" )
		{
            script_playerSounds.play_sound( ref playerAudio, levelCompleteSound, 0f );
            AddToTotalCoinCollected(5000);
			PopGuiText ( "+5000", otherPos );
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
			script_sceneManager.level_completed = true;
		}

		if ( other.tag == "savePoint" )
		{
            script_playerSounds.play_sound( ref playerAudio, jumpOnEnemySound, 0f );
			spawnPos = transform.position;
			PopGuiText ( "Checkpoint!", otherPos );
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
			//Destroy(other.gameObject);
		}

		if ( other.tag == "killbox" )
		{
			UpdatePlayerState ( PlayerState.MarioDead );
		}
		
		if ( other.tag == "coin" )
		{
			Addcoins(1);

			PopGuiText ( "+1", otherPos );

			if ( coins == 100 )
			{
				lives += 1;
				PopGuiText ( "+1", otherPos );
			}

			script_playerSounds.play_sound ( ref playerAudio, coinSound, 0f);
			Transform clone;
			clone = Instantiate ( particleCoin, other.transform.position, Quaternion.identity) as Transform;

            RendererAndColliderDisabling(other);
            //Destroy(other.gameObject);
		}

		if ( other.tag == "bigCoin" )
		{
			AddToTotalCoinCollected(1);
			AddBigCoins(1);
			//Debug.Log ("BigCoins: " + bigCoins );
			if ( bigCoins == 1 )	// x1 multiplier
			{	AddToTotalCoinCollected(1000 * 1); PopGuiText ( "+1000", otherPos ); 	}
			if ( bigCoins == 2 )	// x2 multiplier
			{	AddToTotalCoinCollected(1000 * 2); PopGuiText ( "+2000", otherPos );	}
			if ( bigCoins == 3 )	// x3 multiplier
			{	AddToTotalCoinCollected(1000 * 4); PopGuiText ( "+4000", otherPos );	}
			if ( bigCoins == 4 )	// x4 multiplier
			{	AddToTotalCoinCollected(1000 * 8); PopGuiText ( "+8000", otherPos );	}
			if ( bigCoins == 5 )	// +1 lives and reset of bigCoins counter 
			{	
				lives += 1; 
				bigCoins = 0;	
				script_playerSounds.play_sound ( ref playerAudio, plusOneLifeSound, 0f);
			}

			script_playerSounds.play_sound ( ref playerAudio, coinSound, 0f);
			Transform clone;
			clone = Instantiate ( particleCoin, other.transform.position, Quaternion.identity) as Transform;

            RendererAndColliderDisabling( other );
            //Destroy(other.gameObject);
		}

		if ( other.gameObject.tag == "redMushRoom" )
		{
			if ( active_player_state == PlayerState.MarioLarge || active_player_state == PlayerState.MarioFire )
			{
				lives += 1;
				PopGuiText ( "+1", otherPos );
				script_playerSounds.play_sound ( ref playerAudio, plusOneLifeSound, 0f);
				// Instantiate particle effect saying +1 
			}

			//Debug.Log ( "Player picked up: " + other.gameObject.name );
			script_playerSounds.play_sound ( ref playerAudio, powerUpSound, 0f);
			Destroy( other.gameObject );

			if ( active_player_state != PlayerState.MarioFire )
			{
				UpdatePlayerState ( PlayerState.MarioLarge );
			}
		}

		if ( other.gameObject.tag == "fireUpgrade" )
		{
			if ( active_player_state == PlayerState.MarioLarge || active_player_state == PlayerState.MarioFire )
			{
				lives += 1;
				PopGuiText( "+1", otherPos );
				script_playerSounds.play_sound ( ref playerAudio, plusOneLifeSound, 0f);
			}

			script_playerSounds.play_sound ( ref playerAudio, powerUpSound, 0f);
			Destroy( other.gameObject );
			UpdatePlayerState ( PlayerState.MarioFire );
		}
	}

    public void MarioHitDragonHead( ControllerColliderHit col, GameObject otherPos )
    {
        Debug.Log( "Mario killed dragon!" );

        script_gameController.reMap = true; // Enabled program 4 if active!

        AddToTotalCoinCollected( 200 );
        PopGuiText( "+200", otherPos );
        script_playerSounds.play_sound( ref playerAudio, jumpOnEnemySound, 0f );

        transform.Translate( 0f, 100.0f * Time.deltaTime, 0f, Space.World );

        script_enemyDragon otherDragon = col.gameObject.transform.parent.transform.GetComponent<script_enemyDragon>();
        otherDragon.dragonDead = true;

        otherDragon.GetComponent<Rigidbody>().isKinematic = false;
        otherDragon.GetComponent<Rigidbody>().useGravity = true;
        otherDragon.transform.Translate( 0f, 0f, -15.0f, Space.World );
    }

    void RendererAndColliderDisabling ( Collider other )
    {
        Transform otherGO = other.gameObject.transform;

        if ( otherGO.GetComponent<BoxCollider>() )
        {
             otherGO.GetComponent<BoxCollider>().enabled     = false;
        }
        if ( otherGO.GetComponent<SphereCollider>() )
        {
             otherGO.GetComponent<SphereCollider>().enabled  = false;
        }
        otherGO.GetComponent<SpriteRenderer>().enabled       = false;
    }

	public void         Check_cubeZspin         ( GameObject gameObjectCollidedWith )
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

	public void Check_cubeQuestionMark  ( GameObject gameObjectCollidedWith )
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

		if ( cubeQuestionMark.rewardFire )			// Fire Upgrade
		{
			script_playerSounds.play_sound ( ref playerAudio, pickUpSound, 0f);

			clone = Instantiate ( reward_fireUpgrade, goPos + new Vector3(0f, 1.6f, 0f), Quaternion.identity) as GameObject;

			cubeQuestionMark.rewardFire = false;
		}
	}

	public static void  Addcoins                ( int numCoin )
	{
		coins		+= numCoin;
	}
	
	public static void  AddToTotalCoinCollected ( float CoinCollected )
	{
		totalCoinCollected	+= CoinCollected;
	}

	void                AddBigCoins             ( int bigCoin )
	{
		bigCoins += bigCoin;
	}

	void                PopGuiText              ( string scoreText, GameObject GO )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( guiText, GO.transform.position, Quaternion.identity); 
		
		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));
		
		cloneComponent.SetScoreText(scoreText);
	}

	void                UpdatePlayerState       ( PlayerState playerState )
	{
		active_player_state = playerState;

		changeMario = true;

		change_player_state();

		SetPlayerState ();
	}

	public void         change_player_state     ()
	{
		if (changeMario == true)
		{ 
			SetPlayerState();
		}
	}

	IEnumerator         MarioRespawn            ( float seconds )
	{
		Transform parent = this.transform.parent;
		parent.transform.GetChild(1).GetComponent<script_cameraSmoothFollow2D>().enabled = true;
		transform.GetComponent<Rigidbody>().isKinematic = true;

		yield return new WaitForSeconds (seconds);

        script_sceneManager.level_restart = false;
        UpdatePlayerState( PlayerState.MarioSmall );
        transform.position = spawnPos;
	}

    public void         UpdateMarioLifeSituation()
	{
		if ( active_player_state == PlayerState.MarioSmall && cannotBeKilled == false )
		{
			Debug.Log ("Mariostate = MarioSmall, so Mario dies!");

			script_playerSounds.play_sound ( ref playerAudio, marioDied, 0f);

			Transform parent = this.transform.parent;
			parent.transform.GetChild(1).GetComponent<script_cameraSmoothFollow2D>().enabled = false;

			script_playerAnimation.dead_animation( playerController );
			transform.GetComponent<Rigidbody>().isKinematic 	 	= false;

			transform.Translate ( -0.2f, 100.0f * Time.deltaTime, -5.0f, Space.World );

			StartCoroutine( WaitFor(2.0f) );

			UpdatePlayerState ( PlayerState.MarioDead );
		}
		if ( active_player_state == PlayerState.MarioLarge && cannotBeKilled == false )
		{
			Debug.Log ("Mariostate = MarioLarge, so Mario turns small instead!");
			UpdatePlayerState ( PlayerState.MarioSmall );

			script_playerSounds.play_sound ( ref playerAudio, powerDown, 0f);

			StartCoroutine( WaitFor(2.0f) );
		}
		if ( active_player_state == PlayerState.MarioFire && cannotBeKilled == false )
		{
			Debug.Log ("Mariostate = MarioFire, so Mario turns large instead!");
			UpdatePlayerState ( PlayerState.MarioLarge );

			script_playerSounds.play_sound ( ref playerAudio, powerDown, 0f);

			StartCoroutine( WaitFor(2.0f) );
		}
	}

	IEnumerator         WaitFor                 ( float seconds )
	{
		cannotBeKilled = true;

		yield return new WaitForSeconds(seconds);

		cannotBeKilled = false;
	}

	public void	        SetPlayerState          ()
	{
		
		switch ( active_player_state )
		{
		case	PlayerState.MarioDead:
			//Destroy ( gameObject );
			//gameObject.transform.renderer.enabled = false;
            marioDead                   = true;
            StartCoroutine(MarioRespawn ( 2.0f )); 
			changeMario					= false;
            if ( lives > 0 )
            {
                lives -= 1;
            }
            if ( lives == 0 )
            {
                Debug.Log("Game Over!");
            }

			break;
			
		case	PlayerState.MarioSmall:
			
			player_scale_small		();
            marioDead                   =   false;
            canShoot					=	false;
			changeMario					=	false;
			marioLarge					= 	false;
            marioFire                   =   false;
			playerMeshRender.material	=	material_player_MarioSmall;
			break;			
			
		case	PlayerState.MarioLarge:
			player_scale_normal		();
            marioDead                   =   false;
			canShoot					=	false;
			changeMario					=	false;
			marioLarge					= 	true;
            marioFire                   =   false;
			playerMeshRender.material	=	material_player_MarioLarge;
			break;
			
		case	PlayerState.MarioFire:
			player_scale_normal		();
            marioDead                   =   false;
			canShoot					=	true;
			changeMario					=	false;
			marioLarge					= 	true;
            marioFire                   =   true;
			playerMeshRender.material	=	material_player_MarioFire;
			break;
			
		}
	}
	
	void                player_scale_small	    ()
	{	
		playerTransform.localScale	=	new Vector3	( 0.5f, 0.5f, 0.5f );
		playerTransform.Translate	(0f, 0f, - 1.3f);
         
        float   playerControllerHeight = playerController.height;
        Vector3 playerControllerCenter = playerController.center;
       
        playerControllerCenter.z = 2.2f;
        playerControllerHeight = 3.0f;
      
        playerController.center = playerControllerCenter;
        playerController.height = playerControllerHeight;
	}
	
	void                player_scale_normal		()
	{
		playerTransform.Translate	(0f, 0f, - 1.3f);
		playerTransform.localScale	=	new Vector3	( 0.5f, 0.7f, 0.5f);

        float   playerControllerHeight = playerController.height;
		Vector3 playerControllerCenter = playerController.center;

        playerControllerCenter.z = 0.8f;
        playerControllerHeight = 5.92f;
        
        playerController.center = playerControllerCenter;
        playerController.height = playerControllerHeight;
	}
	
	public AudioClip    get_jump_sound			()
	{
		return this.jumpSound;
	}
	
	public AudioClip    get_crouch_jump_sound	()
	{
		return this.crouchJumpSound;
	}


}


















