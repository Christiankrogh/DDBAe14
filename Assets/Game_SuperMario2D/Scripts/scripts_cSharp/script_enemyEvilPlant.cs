using UnityEngine;
using System.Collections;

public class script_enemyEvilPlant : MonoBehaviour 
{
	public  GameObject guiText;
    bool       grantScore = true;


    void Update()
    {
        if ( script_sceneManager.level_restart )
        {
            Visible( true );
        }
    }

	void OnCollisionEnter ( Collision other )
	{
		if ( other.gameObject.tag == "fireball" )
		{
            Visible( false );
			//Destroy(other.gameObject);

            if ( grantScore )
            {
                script_playerProperties.AddToTotalCoinCollected( 500 );
                PopGuiText( "+500", this.gameObject );
                grantScore = false;
            }
			transform.Translate ( 0f, -50.0f * Time.deltaTime, 0f, Space.World );

			StartCoroutine(WaitASecond(2.0f));
		}
	}

	void PopGuiText ( string scoreText, GameObject GO )
	{
		GameObject clone;
		clone = (GameObject)Instantiate ( guiText, GO.transform.position, Quaternion.identity); 
		
		script_text cloneComponent = (script_text)clone.GetComponent(typeof(script_text));
		
		cloneComponent.SetScoreText(scoreText);
	}

	IEnumerator WaitASecond ( float waitFor )
	{
		yield return new WaitForSeconds (waitFor);
		
        Visible( false );
		//Destroy(gameObject);
	}

    void Visible( bool visible )
    {
        GetComponent<BoxCollider>().enabled     = visible;
        GetComponent<SpriteRenderer>().enabled  = visible;
    }
}
