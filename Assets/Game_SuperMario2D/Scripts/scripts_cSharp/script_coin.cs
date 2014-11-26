using UnityEngine;
using System.Collections;

public class script_coin : MonoBehaviour 
{
    void Update()
    {
        if ( script_sceneManager.level_restart )
        {
            if ( GetComponent<BoxCollider>() )
            {
                GetComponent<BoxCollider>().enabled = true;
            }
            if ( GetComponent<SphereCollider>() )
            {
                GetComponent<SphereCollider>().enabled = true;
            }
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
