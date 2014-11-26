using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class script_sceneManager : MonoBehaviour 
{
	public Transform player; 
	public script_playerProperties playerProperties;
	public script_gameTimer	time;

	public int		lives;	
	public Text		guiLives;

	public int		coins;		
	public Text		guiCoins;

	public int		bigCoins;							

	public float	totalCoinCollected;
	public Text		guiTotalCoinsCollected;

	public static bool		level_completed = false;
    public static bool      level_restart   = false;

	private float 	remainingTime; 
	private bool	calculateScore = false;

    

	void Start ()
	{
		playerProperties = player.GetComponent<script_playerProperties>();
		time			 = GetComponent<script_gameTimer>();
	}
	
	void Update () 
	{
		UpdateGUI ();

		CheckPlayerState ();

		if ( level_completed )
		{
			StartCoroutine(levelComplete());
		}

        if ( Input.GetKey( KeyCode.Alpha9 ) )
        {
            //Debug.Log("Restarting level!");
            level_restart = true;
        }
	}

	void CheckPlayerState ()
	{
		// Checks if mario is large or not. If large, change icon to indicate he is
		Transform guiContainerChild 		= transform.GetChild(0);
			Transform guiIndicatorState 		= guiContainerChild.transform.GetChild(1);
				
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

        Transform guiIndicatorStateChild_3 	= guiIndicatorState.transform.GetChild( 3 );
        Image indicator_3_Image 		    = guiIndicatorStateChild_3.gameObject.GetComponent<Image>();

        if ( playerProperties.marioFire )
        {
            indicator_3_Image.enabled = true;
        }
        else
        {
            indicator_3_Image.enabled = false;
        }
	}

	void UpdateBigCoinGui ()
	{
		bigCoins									= script_playerProperties.bigCoins;
		Transform guiContainerChild 				= transform.GetChild(0);
		Transform guiIndicator_bigCoinsContainer 	= guiContainerChild.transform.GetChild(3);

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

	IEnumerator levelComplete ()
	{
		Transform guiContainerChild  	= transform.GetChild(0);
		Transform guiLevel			 	= guiContainerChild.GetChild(0);

		// Manage remaining time
		time.stopTime = true;
		remainingTime = time.playTime;

		Debug.Log (remainingTime);

		yield return new WaitForSeconds (1.0f);

		// Activate blackscreen
		Transform guiLevel_childOne 	= guiLevel.GetChild(0);
			
				  guiLevel_childOne.GetComponent<Image>().enabled 	 = true;
				  guiLevel_childOne.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds (1.5f);

		// Activate Header
		Transform guiLevel_childTwo 	= guiLevel.GetChild(1);

				  guiLevel_childTwo.GetComponent<Text>().enabled  	 = true;

		yield return new WaitForSeconds (1.5f);

		// Activate score
		calculateScore = true;

		CalculateScore ();

		yield return new WaitForSeconds (1.5f);


		// Activate Credits 
		// Tak fordi i spillede med, meh. :) 

		level_completed = false;
	}


	void CalculateScore ()
	{
		if ( calculateScore )
		{
			//Debug.Log (remainingTime);
			Transform guiContainerChild  		= transform.GetChild(0);
			Transform guiTime			 		= guiContainerChild.GetChild(5);
			Transform guiTime_childOne 			= guiTime.GetChild(0);
			Text 	  guiTimeDisplay 			= guiTime_childOne.GetComponent<Text>();

					  guiTimeDisplay.text 		= remainingTime.ToString("f0");

			Transform guiLevel			 		= guiContainerChild.GetChild(0);
			Transform guiLevel_childThree 		= guiLevel.GetChild(2);
			Text 	  guiTimeBonusScore 		= guiLevel_childThree.GetComponent<Text>();

			float 	  timeBonusScore			= remainingTime * 50;

			guiTimeBonusScore.enabled 			= true;
			guiTimeBonusScore.text				= "Time Bonus\n\n" + remainingTime.ToString("f0") + " x 50 = " + timeBonusScore.ToString("f0"); 

			script_playerProperties.totalCoinCollected	+= Time.deltaTime * timeBonusScore;

			Transform coinCollected		 		= guiContainerChild.GetChild(6);
			Transform coinCollected_total		= coinCollected.GetChild(3);
			Text	  guiTotalCoinCollected		= coinCollected_total.GetComponent<Text>();

			guiTotalCoinCollected.text 			= script_playerProperties.totalCoinCollected.ToString("f0");

			calculateScore = false;
		}
	
	}

}






















