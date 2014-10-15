using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class script_perlinNoise_geometry : MonoBehaviour 
{

	public 	float scale 			= 1;
	public 	float heightScale 		= 1;
			float timeModifier  	= 0;
			float heightModifier	= 0;

	public 	int side_size 			= 50;
	public 	GameObject cube;
	
	private script_BlinkyLightScript arduinoScript;

	void Start () 
	{
		arduinoScript = GameObject.FindGameObjectWithTag("ArduinoLogic").GetComponent<script_BlinkyLightScript>();

		for ( int x = 0; x < side_size; x++ )
		{
			for ( int z = 0; z < side_size; z++ )
			{	
				GameObject newBlock = Instantiate (cube, new Vector3( x, 0, z ), Quaternion.identity ) as GameObject;

				newBlock.transform.parent = transform;
			}
		}
	}


	void Update () 
	{
		UpdateGeometry ();
	}


	void UpdateGeometry ()
	{
		TimeModifier 	();
		HeightModifier 	();

		float t = Time.time * 0.1f;

		foreach (Transform child in transform)
		{
			float posY;

			posY = ( heightScale + heightModifier ) * Mathf.PerlinNoise ( (t + timeModifier)  + ( child.transform.position.x * scale ), (t + timeModifier) + ( child.transform.position.z * scale ) );

			child.transform.position = new Vector3 (child.transform.position.x, posY, child.transform.position.z);
		}
	}

	void TimeModifier ()
	{
		//Debug.Log("TimeModifier: " + timeModifier);
		
		if ( Input.GetMouseButton(1) || arduinoScript.button_01_state == 1 )
		{
			timeModifier += 0.1f; // 0.01f
		}
		else
		{
			timeModifier = Mathf.Lerp( timeModifier, 0, Time.deltaTime * 0.5f );
		}
	}
	
	void HeightModifier ()
	{
		//Debug.Log("HeightModifier: " + heightModifier);
		
		if ( Input.GetMouseButton(0) || arduinoScript.button_02_state == 1 )
		{
			heightModifier += 0.1f; // 0.01f
		}
		else
		{
			heightModifier = Mathf.Lerp( heightModifier, 0, Time.deltaTime );
		}
	}
}










