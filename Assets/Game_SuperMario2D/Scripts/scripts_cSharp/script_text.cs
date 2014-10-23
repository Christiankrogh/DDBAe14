using UnityEngine;
using System.Collections;

public class script_text : MonoBehaviour 
{
	private TextMesh textComponent;

	public void SetScoreText ( string scoreText )
	{
		textComponent 		= GetComponent<TextMesh>();
		textComponent.text 	= scoreText;
	}
	
	void Start () 
	{
		StartCoroutine(DestroyGO(0.6f));
	}

	void Update () 
	{
		this.transform.Translate (0f, 0.07f, 0f);
	}
	
	IEnumerator DestroyGO ( float waitTime )
	{
		yield return new WaitForSeconds(waitTime);

		SetScoreText("");

		Destroy (gameObject);
	}
}
