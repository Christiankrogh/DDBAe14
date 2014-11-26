using UnityEngine;
using System.Collections;

public class script_cannonBall : MonoBehaviour 
{
    private Transform   playerPos;
    public  float       playerTriggerDistance   = 40;
			bool 		fireNow 		        = false;
            bool        playSound               = true;
            Vector3     startPos;
            float       dist;
    void Start()
    {
        startPos    = transform.position;
        playerPos   = GameObject.FindGameObjectWithTag( "Player" ).transform;
    }

	void Update () 
	{
        DistanceToPlayer();

		if ( fireNow )
		{
			this.transform.Translate ( 10.0f * Time.deltaTime, 0f, 0f );
		}

        if ( script_playerProperties.marioDead || script_sceneManager.level_restart)
        {
            //transform.GetComponent<SpriteRenderer>().enabled = true;
            transform.position = startPos;
            fireNow     = false;
            if ( dist <= playerTriggerDistance )
            {
                playSound = false;
            }
            else
            {
                playSound = true;
            }
        }
	}


	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);

        fireNow = false;
        //transform.GetComponent<SpriteRenderer>().enabled = false;

		//Destroy(gameObject);
	}

    void DistanceToPlayer()
    {
        dist = Vector3.Distance( playerPos.position, transform.position );

        if ( dist <= playerTriggerDistance )
        {
            //Debug.Log ( "Player activated cannonball!" );

            if ( playSound && !script_playerProperties.marioDead || playSound && !script_sceneManager.level_restart )
            {
                audio.Play();
                playSound = false;
            }
            
            fireNow = true;

            StartCoroutine( WaitASecond( 20.0f ) ); 
        }
    }
}
