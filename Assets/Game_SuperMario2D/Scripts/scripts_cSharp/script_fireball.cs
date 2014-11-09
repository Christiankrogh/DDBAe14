using UnityEngine;
using System.Collections;

public class script_fireball : MonoBehaviour 
{

	void Start () 
	{
		StartCoroutine(WaitFor(2.0f));
	}

	IEnumerator WaitFor ( float seconds )
	{
		yield return new WaitForSeconds (seconds);

		Destroy (gameObject);
	}

}
