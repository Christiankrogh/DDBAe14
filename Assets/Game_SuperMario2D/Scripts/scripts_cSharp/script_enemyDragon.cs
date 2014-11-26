using UnityEngine;
using System.Collections;

public class script_enemyDragon : MonoBehaviour 
{
    private Transform   playerPos;
    public float        playerTriggerDistance = 40;

	public 	bool        dragonDead = false;
			bool        startWalk	= false;
			bool        walkLeft 	= true;

    private bool        grantScore = true;
	public  GameObject  guiText;

            Vector3     startPos; 

    void Start()
    {
        startPos  = transform.position;
        playerPos = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }

	void Update () 
	{
        if ( script_playerProperties.marioDead || script_sceneManager.level_restart )
        {
            transform.GetComponent<Rigidbody>().isKinematic  = true;
            transform.GetComponent<Rigidbody>().useGravity   = false;
            //transform.GetComponent<SpriteRenderer>().enabled = true;
            transform.position = startPos;
            startWalk = false;
            dragonDead = false;
        }
        
        DistanceToPlayer();

		if ( dragonDead )
		{
			StartCoroutine( WaitASecond (3.0f) );
		}

		if ( startWalk )
		{
			if ( walkLeft ) // Walk left
			{
				this.transform.Translate ( -5.0f * Time.deltaTime, 0f, 0f );
                transform.Rotate(0, 0, 0);
			}
			else 			// Walk right
			{
				this.transform.Translate ( 5.0f * Time.deltaTime, 0f, 0f );
                transform.Rotate( 0, 180, 0 );
			}
		}
	}

    void DistanceToPlayer()
    {
        float dist = Vector3.Distance( playerPos.position, transform.position );

        if ( dist <= playerTriggerDistance )
        {
            Debug.Log("Player triggered dragon!");

            startWalk = true;

            StartCoroutine( WaitASecond( 30.0f ) );
        }
    }

  
    void OnTriggerEnter( Collider other )
	{
		if ( other.gameObject.tag == "fireball" )
		{
			Destroy(other.gameObject);

            if ( grantScore )
            {
                script_playerProperties.AddToTotalCoinCollected( 200 );
                PopGuiText( "+200", this.gameObject );
                grantScore = false;
            }
			//Debug.Log ("Hit Dragon!");
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity  = true;
			transform.Translate ( 0f, 0f, -15.0f, Space.World );
		}

        
        if ( other.gameObject.tag == "changeDirection" )
        {
            Debug.Log( "Dragon changes direction" );
            walkLeft = !walkLeft;
        }
        
	}
	
	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);

        startWalk = false;
        //transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Rigidbody>().useGravity = false;
		//Destroy(gameObject);
	}

	void PopGuiText ( string scoreText, GameObject GO )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( guiText, GO.transform.position, Quaternion.identity); 
		
		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));
		
		cloneComponent.SetScoreText(scoreText);
	}
	
}

















