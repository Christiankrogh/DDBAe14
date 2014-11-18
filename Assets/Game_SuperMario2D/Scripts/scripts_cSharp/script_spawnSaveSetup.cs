///////////////////////////////////////////////////////////////////////
////// Part of Bachelor project in Digital Design 2014 (BADDe14) //////
//
// Spawn Save Player Location Setup - Super Mario 64 Clone (2D)
//
// - Description: Saves the player position / location (Save points) for Spawning after death based on killbox / enemy
//- Instruction: Place save points (gameobjects with collision) in the scene with tag names 'savePoint'
//				 Place killboxes in scene with tag name 'killbox' - currently sends player to most recent save point location 
//
///////////////////////////////////////////////////////////////////////
/*
using UnityEngine;
using System.Collections;

public class script_spawnSaveSetup : MonoBehaviour 
{
	public Transform startPoint;
	public AudioClip soundDie;
	
	private float soundRate 		= 0.0f;
	private float soundDelay		= 0.0f;
	private Vector3 curSavePos;

	IEnumerator PlaySound ( AudioClip soundName, float soundDelay )
	{
		if ( !audio.isPlaying && Time.time > soundRate )
		{
			soundRate = Time.time + soundDelay;
			audio.clip = soundName;
			audio.Play ();
			yield return new WaitForSeconds ( audio.clip.length );
		}
	}
	
	void OnTriggerEnter ( Collider other )
	{
		if ( other.tag == "savePoint" )
		{
			curSavePos = this.transform.position;
		}
		if ( other.tag == "killbox" )
		{
			PlaySound ( soundDie, 0 );
			this.transform.position = curSavePos; 
		}
	}
	
	void Start ()
	{
		if ( startPoint != null )
		{
			this.transform.position = startPoint.position;
		}	
	}
}
*/

using UnityEngine;
using System.Collections;

public class SpawnSaveSetup : MonoBehaviour
{
	
	#region				Fields
	
	public Transform			startPoint;
	public AudioClip			soundDie;
	
	private static float		soundRate				=		0.0f;
	private static float		soundDelay				=		0.0f;
	private Vector3				currentSavePosition;
	
	
	#endregion
	
	void Start ()
	{
		if ( startPoint != null)
		{
			transform.position = startPoint.position;
		}
		
	}
	
	static void	play_sound ( AudioSource soundSource, AudioClip soundName, float soundDelay)
	{
		if	( soundSource.isPlaying == false && Time.time > soundRate )
		{
			soundRate			=	Time.time + soundDelay;
			soundSource.clip	=	soundName;
			soundSource.Play();
		}
	}
	
	void OnTriggerEnter2D	( Collider2D other)		
	{
		if (other.tag	==	"savePoint")
		{
			currentSavePosition = transform.position;
		}
		
		if (other.tag	==	"killbox")
		{
			play_sound ( audio, soundDie, 0);
			transform.position = currentSavePosition;
		}
	}
}

















