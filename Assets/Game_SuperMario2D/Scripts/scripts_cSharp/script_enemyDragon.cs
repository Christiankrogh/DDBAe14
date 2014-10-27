using UnityEngine;
using System.Collections;

public class script_enemyDragon : MonoBehaviour 
{
	public 	bool dragonDead = false;
			bool startWalk	= false;
			bool walkLeft 	= true;

	void Update () 
	{
		if ( dragonDead )
		{
			Destroy ( gameObject );
		}

		if ( startWalk )
		{
			if ( walkLeft ) // Walk left
			{
				this.transform.Translate ( -5.0f * Time.deltaTime, 0f, 0f );
			}
			else 			// Walk right
			{
				this.transform.Translate ( 5.0f * Time.deltaTime, 0f, 0f );
			}
		}
	}

	void OnTriggerEnter ( Collider other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			//Debug.Log ( "Player activated Dragon!" );
			
			startWalk = true;
			
			StartCoroutine( WaitASecond (10.0f) );
		}

		if ( other.gameObject.tag == "changeDirection" )
		{
			Debug.Log ("Dragon changes direction");
			//walkLeft = !walkLeft;
		}
	}

	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);
		
		Destroy(gameObject);
	}

	void OnCollisionEnter ( Collision other )
	{
	
	}
}

















