/*
	Camera with borders for movement
	
	- Ability to have the character move to the edge of the screen and the camera
	moves over revealing a new screen and level

	What the script will do: 
	- Check if the player is on either the right or left edge
	- Make a border amount on the check (through the inspector)
	- If the player is moving to the right (or left), then adjust camera position
*/


using UnityEngine;
using System.Collections;

public class script_cameraBorderFollow2D : MonoBehaviour 
{
	public GameObject cameraTarget;
	public GameObject player;
	
	public float cameraHeight		= 0.0f;
	public float smoothTime			= 0.2f;
	public float borderX			= 2.0f;
	public float borderY			= 2.0f;
	
	private Vector2 velocity;
	private bool moveScreenRight	= false;
	private bool moveScreenLeft		= false; 

	void Start ()
	{
		cameraHeight = camera.transform.position.y;
	}
	
	
	void Update ()
	{
		float moveDirection = script_playerControls.moveDirection;

		float cameraTransformX = camera.transform.position.x;
		float cameraTransformY = camera.transform.position.y;

		if ( cameraTarget.transform.position.x > camera.transform.position.x + borderX && moveDirection == 1 )	// if player is at the right edge
		{
			moveScreenRight = true;
		}

			if ( moveScreenRight )
			{
				cameraTransformX = Mathf.SmoothDamp ( camera.transform.position.x, camera.transform.position.x + borderX, ref velocity.y, smoothTime );
			}

		if ( cameraTarget.transform.position.x < camera.transform.position.x - borderX && moveDirection == 1 )	// if player is not at border 
		{
			moveScreenRight = false;
		}


		if ( cameraTarget.transform.position.x < camera.transform.position.x - borderX && moveDirection == -1 )
		{
			moveScreenLeft 	= true;
		}

			if ( moveScreenLeft )
			{
				cameraTransformX = Mathf.SmoothDamp ( camera.transform.position.x, camera.transform.position.x - borderX, ref velocity.y, smoothTime );
			}

		if ( cameraTarget.transform.position.x > camera.transform.position.x + borderX && moveDirection == -1 )
		{
			moveScreenLeft = false;
		}

		this.camera.transform.position = new Vector3(cameraTransformX, cameraTransformY, camera.transform.position.z);

		cameraTransformY = cameraHeight;
	}


}
