using UnityEngine;
using System.Collections;

public class script_reward_Mushroom : MonoBehaviour 
{	
	void Start () 
	{
		this.transform.Translate	(0f, -0.5f, 0f);
	}

	void Update () 
	{
		StartCoroutine(WaitASecond(0.5f));
		
		this.transform.Translate 	( -5.0f * Time.deltaTime, 0f, 0f );
	}


	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);
	}
}