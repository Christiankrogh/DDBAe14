#pragma strict

	//the collider of the main visible platform
	//var collider : BoxCollider;
	//this variable is true when the players is just below the platform so that its Box collider can be disabled that will allow the player to pass through the platform
	var oneway : boolean;

	var parentCollider : BoxCollider;

function Start () 
{
	parentCollider = this.transform.parent.transform.GetComponent(BoxCollider);
}

function Update () 
{
	//Enabling or Disabling the platform's Box collider to allowing player to pass
	if (oneway)
	  parentCollider.enabled=false;
	 if (!oneway)
	  parentCollider.enabled=true;   
}
	//Checking the collison of the gameobject we created in step 2 for checking if the player is just below the platform and nedded to ignore the collison to the platform
function OnTriggerStay(other: Collider) 
{
     oneway = true;
}

function OnTriggerExit(other: Collider) 
{
	//Just to make sure that the platform's Box Collider does not get permantly disabled and it should be enabeled once the player get its through
	oneway = false;
}