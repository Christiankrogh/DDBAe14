using UnityEngine;
using System.Collections;

public class script_cannonBall : MonoBehaviour 
{
			bool 		fireNow 		= false;

	void Update () 
	{
		if ( fireNow )
		{
			this.transform.Translate ( 10.0f * Time.deltaTime, 0f, 0f );
		}
	}

	void OnTriggerEnter2D ( Collider2D other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			//Debug.Log ( "Player activated cannonball!" );
			audio.Play();

			fireNow = true;

			StartCoroutine( WaitASecond (10.0f) );
		}
	}


	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);

		Destroy(gameObject);
	}
}
