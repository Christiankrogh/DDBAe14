using UnityEngine;
using System.Collections;

public class script_cube_Zspin : MonoBehaviour 
{
	public bool rewardCoin 		= 	false;
	public bool	staticForm		=	true;
	public bool zSpin			=	false;

    bool temp_reward_1;
    bool temp_reward_2;
    bool temp_reward_3;

    void Start()
    {
        temp_reward_1 = rewardCoin;
        temp_reward_2 = staticForm;
        temp_reward_3 = zSpin;
    }

    void Update()
    {

        if ( script_sceneManager.level_restart )
        {
            rewardCoin  = temp_reward_1;
            staticForm  = temp_reward_2;
            zSpin       = temp_reward_3;

            Animator anim_cube_zSpin = GetComponent<Animator>();
            anim_cube_zSpin.Rebind();
            anim_cube_zSpin.SetBool( "shouldPop", false );
        }
    }
}
