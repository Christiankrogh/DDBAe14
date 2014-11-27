using UnityEngine;
using System.Collections;

public class script_cube_questionMark : MonoBehaviour 
{

	public bool	rewardRedMushroom		=	false;
	public bool rewardGreenMushroom		=	false;
	public bool rewardFire				=	false;

    bool temp_reward_1;
    bool temp_reward_2;
    bool temp_reward_3;

    void Start()
    {
        temp_reward_1 = rewardRedMushroom;
        temp_reward_2 = rewardGreenMushroom;
        temp_reward_3 = rewardFire;
    }

    void Update()
    {
       
        if ( script_sceneManager.level_restart )
        {
            rewardRedMushroom       = temp_reward_1;
            rewardGreenMushroom     = temp_reward_2;
            rewardFire              = temp_reward_3;

            Animator anim_cube_questionMark = GetComponent<Animator>();
            anim_cube_questionMark.Rebind();
            anim_cube_questionMark.SetBool( "shouldPop", false ); 
        }
    }
}
