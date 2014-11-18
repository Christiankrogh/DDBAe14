using UnityEngine;
using System.Collections;

public class script_enemyEvilPlant : MonoBehaviour 
{

	public  GameObject guiText;

	void OnCollisionEnter2D ( Collision2D other )
	{
		if ( other.gameObject.tag == "fireball" )
		{
			Destroy(other.gameObject);

			script_playerProperties.AddToTotalCoinCollected(500);
			PopGuiText ("+500", this.gameObject);

			transform.Translate ( 0f, -50.0f * Time.deltaTime, 0f, Space.World );

			StartCoroutine(WaitASecond(2.0f));
		}
	}

	void PopGuiText ( string scoreText, GameObject GO )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( guiText, GO.transform.position, Quaternion.identity); 
		
		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));
		
		cloneComponent.SetScoreText(scoreText);
	}

	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);
		
		Destroy(gameObject);
	}
	
}
