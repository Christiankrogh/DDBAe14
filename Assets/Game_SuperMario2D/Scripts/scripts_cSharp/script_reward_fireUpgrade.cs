using UnityEngine;
using System.Collections;

public class script_reward_fireUpgrade : MonoBehaviour 
{
	void Start () 
	{
		this.transform.Translate	(0f, -0.4f, 0f);
	}
    void Update()
    {
        if ( script_sceneManager.level_restart )
        {
            Destroy( gameObject );
        }
    }
}
