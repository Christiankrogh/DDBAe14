using UnityEngine;
using System.Collections;

public class script_cube_questionMark : MonoBehaviour 
{

	public bool	rewardRedMushroom		=	false;
	public bool rewardGreenMushroom		=	false;
	public bool rewardFire				=	false;

	private Animator anim;
	


	void Start()
	{
		anim = GetComponent<Animator>();
	}


	void OnCollisionEnter ( Collision col )
	{
		if ( col.gameObject.tag == "Player" )
		{
			Debug.Log ("Hit!");
			anim.SetBool("shouldPop", true);
		}
	}


	

}
