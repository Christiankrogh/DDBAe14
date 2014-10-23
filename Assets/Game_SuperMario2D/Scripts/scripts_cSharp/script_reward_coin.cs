using UnityEngine;
using System.Collections;

public class script_reward_coin : MonoBehaviour 
{
	public  GameObject 		coinText;

	public  GameObject 		poofParticle;  
	

	void Start () 
	{
		int coins = script_playerProperties.coins;

		coins += 1; 

		PopGuiText ("+1");

		this.transform.Translate	(0f, -1.2f, 0f);
	
		StartCoroutine(DestroyGO(0.6f));
	}

	IEnumerator DestroyGO ( float waitTime )
	{
		GameObject clone;
		yield return new WaitForSeconds(waitTime);
		clone = Instantiate ( poofParticle, transform.position, Quaternion.identity) as GameObject; 
		yield return new WaitForSeconds(0.1f);
		Destroy (gameObject);
	}


	void PopGuiText ( string scoreText )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( coinText, transform.position, Quaternion.identity); 
		clone.transform.parent = this.transform;

		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));

		cloneComponent.SetScoreText(scoreText);
	}
}

















