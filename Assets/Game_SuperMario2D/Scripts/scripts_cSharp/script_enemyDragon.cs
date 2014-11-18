using UnityEngine;
using System.Collections;

public class script_enemyDragon : MonoBehaviour 
{
	public 	bool dragonDead = false;
			bool startWalk	= false;
			bool walkLeft 	= true;

	public  GameObject guiText;

	void Update () 
	{
		if ( dragonDead )
		{
			StartCoroutine( WaitASecond (3.0f) );
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

	void OnTriggerEnter2D ( Collider2D other )
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

	void OnCollisionEnter2D ( Collision2D other )
	{
		if ( other.gameObject.tag == "fireball" )
		{
			Destroy(other.gameObject);

			script_playerProperties.AddToTotalCoinCollected(200);
			PopGuiText ("+200", this.gameObject);

			//Debug.Log ("Hit Dragon!");
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity  = true;
			transform.Translate ( 0f, 0f, -15.0f, Space.World );
		}
	}
	
	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);
		
		Destroy(gameObject);
	}

	void PopGuiText ( string scoreText, GameObject GO )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( guiText, GO.transform.position, Quaternion.identity); 
		
		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));
		
		cloneComponent.SetScoreText(scoreText);
	}
	
}

















