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

	public int		totalCoinCollected;
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

	void UpdateBigCoinGui ()
	{
		bigCoins									= script_playerProperties.bigCoins;
		Transform guiContainerChild 				= transform.GetChild(0);
		Transform guiIndicator_bigCoinsContainer 	= guiContainerChild.transform.GetChild(2);

		Image guiIcon_bigCoin_1 				= guiIndicator_bigCoinsContainer.transform.GetChild(0).GetComponent<Image>();
		Image guiIcon_bigCoin_2 				= guiIndicator_bigCoinsContainer.transform.GetChild(1).GetComponent<Image>();
		Image guiIcon_bigCoin_3 				= guiIndicator_bigCoinsContainer.transform.GetChild(2).GetComponent<Image>();
		Image guiIcon_bigCoin_4 				= guiIndicator_bigCoinsContainer.transform.GetChild(3).GetComponent<Image>();

		if ( bigCoins == 0 )
		{
			guiIcon_bigCoin_1.enabled = false;
			guiIcon_bigCoin_2.enabled = false;
			guiIcon_bigCoin_3.enabled = false;
			guiIcon_bigCoin_4.enabled = false;
		}
		if ( bigCoins == 1 )
		{
			guiIcon_bigCoin_1.enabled = true;

			guiIcon_bigCoin_2.enabled = false;
			guiIcon_bigCoin_3.enabled = false;
			guiIcon_bigCoin_4.enabled = false;
		}
		if ( bigCoins == 2 )
		{
			guiIcon_bigCoin_1.enabled = true;
			guiIcon_bigCoin_2.enabled = true;

			guiIcon_bigCoin_3.enabled = false;
			guiIcon_bigCoin_4.enabled = false;
		}
		if ( bigCoins == 3 )
		{
			guiIcon_bigCoin_1.enabled = true;
			guiIcon_bigCoin_2.enabled = true;
			guiIcon_bigCoin_3.enabled = true;

			guiIcon_bigCoin_4.enabled = false;
		}
		if ( bigCoins == 4 )
		{
			guiIcon_bigCoin_1.enabled = true;
			guiIcon_bigCoin_2.enabled = true;
			guiIcon_bigCoin_3.enabled = true;
			guiIcon_bigCoin_4.enabled = true;
		}
	}

	
	void UpdateGUI ()
	{
		lives 				= script_playerProperties.lives;
		coins 				= script_playerProperties.coins;
		totalCoinCollected	= script_playerProperties.totalCoinCollected;

		guiLives.text 				= lives.ToString	("f0");
		guiCoins.text 				= coins.ToString	("f0");
		guiTotalCoinsCollected.text = totalCoinCollected.ToString ("f0");

		UpdateBigCoinGui ();
	}



}






















