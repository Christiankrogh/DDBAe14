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


		var startPoint 			: Transform;
		var soundDie			: AudioClip;

private var soundRate 			: float		= 0.0;
private var soundDelay 			: float		= 0.0;
private var curSavePos 			: Vector3;




function PlaySound ( soundName, soundDelay )
{
	if ( !audio.isPlaying && Time.time > soundRate )
	{
		soundRate = Time.time + soundDelay;
		audio.clip = soundName;
		audio.Play ();
		yield WaitForSeconds ( audio.clip.length );
	}
}

function OnTriggerEnter ( other : Collider )
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

function Start ()
{
	if ( startPoint != null )
	{
		this.transform.position = startPoint.position;
	}	
}

function Update ()
{

}










































