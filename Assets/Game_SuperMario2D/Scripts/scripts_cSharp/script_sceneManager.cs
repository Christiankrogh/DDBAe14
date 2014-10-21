using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class script_sceneManager : MonoBehaviour 
{
	public Transform player; 
	public script_playerProperties playerProperties;

	public int		lives;	
	public Text		guiLives;
	public int		coins;		
	public Text		guiCoins;
	public int		bigCoins;							
	public Text		guiBigCoins;
	public int		totalCoinsCollected;
	public Text		guiTotalCoinsCollected;
					  

	void Start ()
	{
		playerProperties = player.GetComponent<script_playerProperties>();
	}
	
	void Update () 
	{
		UpdateGUI ();

		CheckPlayerState ();
	}

	void CheckPlayerState ()
	{
		// Checks if mario is large or not. If large, change icon to indicate he is
		Transform guiContainerChild 		= transform.GetChild(0);
			Transform guiIndicatorState 		= guiContainerChild.transform.GetChild(0);
				Transform guiIndicatorStateChild 	= guiIndicatorState.transform.GetChild(2);
					Image indicatorImage 				= guiIndicatorStateChild.gameObject.GetComponent<Image>();

		if ( playerProperties.marioLarge )
		{
			indicatorImage.enabled = true;
		}
		else
		{
			indicatorImage.enabled = false;
		}
	}
	
	void UpdateGUI ()
	{
		lives 				= script_playerProperties.lives;
		coins 				= script_playerProperties.coins;
		bigCoins 			= script_playerProperties.bigCoins;

		guiLives.text 		= lives.ToString	("f0");
		guiCoins.text 		= coins.ToString	("f0");
		//guiBigCoins.text 	= bigCoins.ToString	("f0");
		guiTotalCoinsCollected.text = totalCoinsCollected.ToString ("f0");
	}



}






















