using UnityEngine;
using System.Collections;

public class script_playerCollision : MonoBehaviour 
{
	public  GameObject reward_redMushRoom;
	public  GameObject reward_greenMushRoom;
	
	private Animator anim_cube_questionMark;
	private Animator anim_cube_Zspin;

	private script_cube_questionMark cubeQuestionMark;

	
	void OnControllerColliderHit ( ControllerColliderHit col )
	{
		if ( col.gameObject.tag == "cubeQuestionMark" )
		{
			Debug.Log ( "Player hit: " + col.gameObject.name );
			anim_cube_questionMark = col.gameObject.GetComponent<Animator>();
			anim_cube_questionMark.SetBool("shouldPop", true);

			AddReward(col.gameObject);
		}

		if ( col.gameObject.tag == "cubeZspin" )
		{
			Debug.Log ( "Player hit: " + col.gameObject.name );
			anim_cube_Zspin 	= col.gameObject.GetComponent<Animator>();
			anim_cube_Zspin.SetBool("shouldSpin", true);
			col.transform.parent.GetComponent<BoxCollider>().enabled = false;
			col.gameObject.GetComponent<BoxCollider>().enabled = false;
		  
		}
	}


	void AddReward ( GameObject gameObjectCollidedWith )
	{
		cubeQuestionMark 	= gameObjectCollidedWith.GetComponent<script_cube_questionMark>();
		Vector3 goPos 		= gameObjectCollidedWith.transform.position;

		GameObject clone;

		if ( cubeQuestionMark.rewardRedMushroom )	// Red Mushroom
		{
			Debug.Log ("RedMushroomSpawn");
			clone = Instantiate ( reward_redMushRoom, goPos + new Vector3(0f, 2f, 0f), Quaternion.identity) as GameObject; 
		}
		if ( cubeQuestionMark.rewardGreenMushroom )	// Green Mushroom
		{
			Debug.Log ("GreenMushroomSpawn");
			clone = Instantiate ( reward_greenMushRoom, goPos + new Vector3(0f, 2f, 10), Quaternion.identity) as GameObject; 
		}

	}

}


























