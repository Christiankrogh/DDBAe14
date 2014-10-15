#pragma strict
#pragma downcast

import System.Collections.Generic;

		var scale 			: float = 1; 
		var heightScale 	: float	= 1;
		var timeModifier	: float	= 0;
		var heightModifier 	: float	= 0;

private var side_size 		: int = 50;
		var cube 			: GameObject;
	
		var colorList 		: List.<Color> = new List.<Color>();

function Start () 
{
	/*
	colorList.Add(Color.red);
	colorList.Add(Color.blue);
	colorList.Add(Color.green);
	colorList.Add(Color.yellow);
	*/
	colorList.Add(Color.gray);
	colorList.Add(Color.white);

	for ( var x = 0; x < side_size; x++ )
	{
		for ( var z = 0; z < side_size; z++ )
		{	
			//for ( var y = 0; y < side_size; y++ )
			//{
				//var newBlock = Instantiate (cube, Vector3( x, y, z ), Quaternion.identity );
				var newBlock = Instantiate (cube, Vector3( x, 0, z ), Quaternion.identity );
			
				newBlock.transform.parent = transform;

			//}
		}
	}
	
	//UpdateGeometry ();
}

function Update () 
{
	UpdateGeometry ();
}



function UpdateGeometry ()
{
	TimeModifier 	();
	HeightModifier 	();
	
	var t = Time.time * 0.1;
	
	for ( var child : Transform in transform )
	{
		//child.transform.position.y = heightScale * Mathf.PerlinNoise ( Time.time + ( child.transform.position.x * scale ), Time.time + ( child.transform.position.z * scale ) );
		
		child.transform.position.y = ( heightScale + heightModifier ) * Mathf.PerlinNoise ( (t + timeModifier)  + ( child.transform.position.x * scale ), (t + timeModifier) + ( child.transform.position.z * scale ) );
		
		//UpdateGeomtryColor ( child );
	}
}


function UpdateGeomtryColor ( child : Transform )
{
	var index : int = Random.Range(0, colorList.Count );
	
	var newColor : Color = colorList[index];
	
	child.transform.renderer.material.color = newColor;
}


function TimeModifier ()
{
	Debug.Log("TimeModifier: " + timeModifier);
		
	if ( Input.GetMouseButton(1) )
	{
		timeModifier += 0.01;
	}
	else
	{
		timeModifier = Mathf.Lerp( timeModifier, 0, Time.deltaTime * 0.5 );
	}
}

function HeightModifier ()
{
	Debug.Log("HeightModifier: " + heightModifier);

	if ( Input.GetMouseButton(0) )
	{
		heightModifier += 0.01;
	}
	else
	{
		heightModifier = Mathf.Lerp( heightModifier, 0, Time.deltaTime );
	}
}





























