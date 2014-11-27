using UnityEngine;
using System.Collections;

public class script_loadingScene : MonoBehaviour 
{
    public string sceneToLoad;
    public float  loadingTime;

	void Start () 
    {
        StartCoroutine( WaitFor( loadingTime ) );
	}

    IEnumerator WaitFor( float seconds )
    { 
        yield return new WaitForSeconds(seconds);

        Application.LoadLevel( sceneToLoad );
    }
}
