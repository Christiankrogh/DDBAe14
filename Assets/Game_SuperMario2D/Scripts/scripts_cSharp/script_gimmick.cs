using UnityEngine;
using System.Collections;

public class script_gimmick : MonoBehaviour 
{
    public  GameObject 		coinText;
    public  GameObject 		poofParticle;
    bool            grantScore = true;


    void OnTriggerEnter( Collider col )
    {
        if ( col.tag == "Player" )
        {
            GetComponent<SpriteRenderer>().enabled  = true;
            GetComponent<Animator>()      .enabled  = true;

            StartCoroutine(ActivateChild( 1.0f ));

            if ( grantScore )
            {
                script_playerProperties.AddToTotalCoinCollected( 5000 );
                PopGuiText( "+5000" );
                grantScore = false;
            } 
        }
    }

    IEnumerator ActivateChild( float seconds )
    {
        yield return new WaitForSeconds( seconds );

        Transform child_01 = transform.GetChild( 0 ).transform;

        child_01.GetComponent<SpriteRenderer>().enabled = true;
        child_01.GetComponent<Animator>()      .enabled = true;

        audio.Play();
    }

    void PopGuiText( string scoreText )
    {
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(0, 0, 0);
        GameObject clone;
        clone = (GameObject)Instantiate( coinText, transform.position, rot );
        //clone.transform.parent = this.transform;

        script_text cloneComponent = (script_text)clone.GetComponent( typeof( script_text ) );

        cloneComponent.SetScoreText( scoreText );
    }

}
