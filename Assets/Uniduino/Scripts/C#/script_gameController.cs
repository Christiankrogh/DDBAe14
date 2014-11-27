using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Uniduino;

#region Description
	// Note: To change the layout of the variables in this class, edit the editorScript called: script_customInspector_gameControllerScript
#endregion

//[ExecuteInEditMode] 
public class script_gameController : MonoBehaviour 
{
	private 		Arduino 	arduino;

    #region (function)ProgramZapping variables
    Transform                   GUI;
    bool                        program_1_active    = false;
    bool                        program_2_active    = false;
    bool                        program_3_active    = false;
    bool                        program_4_active    = false;
    #endregion 

    #region Action variables
    // Global action variables
    public static   int         move_Horizontal         = 0;
    public static   int         move_Vertical           = 0;

    public static   bool        canJump                 = false;
    public static   bool        canRun                  = false;
    public static   bool        canShoot                = false;

    // Program 1 specific: 
                  // -

    // Program 2 specific: 
                    int         commandRotation         = 0;
                    bool        changeCommand           = true;	
	
    // Program 3 specific: 
    private         List<string>indexArray              = new List<string>();
    public static   bool        reMap                   = true; 

    // Program 4 specific: 
    public static   bool        canDoSomethingSpecial   = false;

	#endregion

	#region LED'pins' 		
                    int         redVal 		            = 0;
                    int         blueVal 	            = 0;
                    int         greenVal 	            = 0;
    
    private			int 		led_01_red				= 31;
    private 		int 		led_01_green			= 33;
    private 		int 		led_01_blue				= 35;

    private 		int 		led_02_red				= 39;
    private 		int 		led_02_green			= 41;
    private 		int 		led_02_blue				= 43;

    private 		int 		led_03_red				= 47;
    private 		int 		led_03_green			= 49;
    private 		int 		led_03_blue				= 51;

    private 		int 		led_04_red				= 10;
    private 		int 		led_04_green			= 9;
    private 		int 		led_04_blue				= 8;
	#endregion

	#region Button'pins' 	[ControlPad]
    private 		int 		button_up 				= 58;   // 57 // A3
    private 		int 		button_down				= 59;   // 56 // A2
    private 		int 		button_left				= 61;   // 55 // A1
    private 		int 		button_right			= 60; 	// 54 // A0
	#endregion

	#region Button'pins' 	[Actions]
    private 		int 		button_B 				= 54;    // 58 // A4 / run   / Yellow
    private 		int 		button_Y 				= 55;    // 59 // A5 / jump  / Green
    private 		int 		button_X 				= 56;    // 60 // A6 / --    / Blue
    private 		int 		button_A 				= 57;    // 61 // shoot / Red
	#endregion

	#region Button'states' 	
	private			int 		buttom_up_state 		    = 0;						// Variable for reading the pushbutton status
	private			int 		button_down_state 		    = 0;
    private			int 		button_left_state 		    = 0;
	private			int 		button_right_state 		    = 0;

	private			int 		button_A_state 			    = 0;
	private			int 		button_B_state 			    = 0;
	private			int 		button_X_state 			    = 0;
	private			int 		button_Y_state 			    = 0;

                    float       debounce_count              = 0.1f;
                    float       timer_controlPad            = 0.0f;                
                    float       timer_actions               = 0.0f;

                    int         button_up_currentState      = Arduino.LOW;
                    int         button_down_currentState    = Arduino.LOW;
                    int         button_left_currentState    = Arduino.LOW;
                    int         button_right_currentState   = Arduino.LOW;

                    int         button_A_currentState       = Arduino.LOW;
                    int         button_B_currentState       = Arduino.LOW;
                    int         button_X_currentState       = Arduino.LOW;
                    int         button_Y_currentState       = Arduino.LOW;
	#endregion
	
	#region ConfigurePins
	void ConfigurePins()											// In here we can define what each pin does
	{
		arduino.pinMode (led_01_green	, 	PinMode.OUTPUT);			// Sends stuff to pin 13 on the arduino board. In this case it sends a 'output'
		arduino.pinMode (led_01_blue	,  	PinMode.OUTPUT);
		arduino.pinMode (led_01_red		, 	PinMode.OUTPUT);

		arduino.pinMode (led_02_green	, 	PinMode.OUTPUT);			
		arduino.pinMode (led_02_blue	,  	PinMode.OUTPUT);
		arduino.pinMode (led_02_red		, 	PinMode.OUTPUT);

		arduino.pinMode (led_03_green	, 	PinMode.OUTPUT);			
		arduino.pinMode (led_03_blue	,  	PinMode.OUTPUT);
		arduino.pinMode (led_03_red		, 	PinMode.OUTPUT);

		arduino.pinMode (led_04_green	, 	PinMode.OUTPUT);			
		arduino.pinMode (led_04_blue	,  	PinMode.OUTPUT);
		arduino.pinMode (led_04_red		, 	PinMode.OUTPUT);

		arduino.pinMode (button_up		,  	PinMode.INPUT);				// Sends stuff to pin 13 on the arduino board. In this case it sends a 'output'
		arduino.pinMode (button_down	,  	PinMode.INPUT);	
		arduino.pinMode (button_left	,  	PinMode.INPUT);	
		arduino.pinMode (button_right	,  	PinMode.INPUT);	

		arduino.pinMode (button_A		,  	PinMode.INPUT);
		arduino.pinMode (button_B		,  	PinMode.INPUT);
		arduino.pinMode (button_X		,  	PinMode.INPUT);
		arduino.pinMode (button_Y		,  	PinMode.INPUT);

		arduino.reportDigital ( ( byte ) ( button_up 	/ 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_down 	/ 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_left 	/ 8 ), 1 );
        arduino.reportDigital ( ( byte ) ( button_right / 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_A 	/ 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_B 	/ 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_X 	/ 8 ), 1 );
		arduino.reportDigital ( ( byte ) ( button_Y 	/ 8 ), 1 );

		#region Description of (byte)(pin/8) 	
		// (byte)(pin/8) is basically a way of finding the port your pin is part of. Ports are groups of 8, with 3 total ports on the arduino Uno. 
		// Pin 2 (from example) is on port 0.
		// The seconds number in the parameters (1) is the enabled option. 1 = on, 0 = off
		#endregion
	}
	#endregion

	void Start () 
	{
        AddIndexNumberToArray ();                                   // Used as part of program 3
		arduino = Arduino.global;									// Searches for the one and only Arduino connected to Unity
        arduino.Setup (ConfigurePins);								// Set up pins
        GUI = GameObject.FindGameObjectWithTag("GUI").transform;
    }
   
	void Update () 													
	{
        CheckButtonState();

        ProgramZapping  ();

        //canRunDelay();
	}

	#region CheckButtonState

	void CheckButtonState ()
	{
        timer_controlPad += Time.deltaTime;
        timer_actions    += Time.deltaTime;

		if ( arduino != null )
		{
            buttom_up_state     = arduino.digitalRead( button_up    );
            button_down_state   = arduino.digitalRead( button_down  );
            button_left_state   = arduino.digitalRead( button_left  );
            button_right_state  = arduino.digitalRead( button_right );
            
            button_A_state      = arduino.digitalRead( button_A     );
            button_B_state      = arduino.digitalRead( button_B     );
            button_X_state      = arduino.digitalRead( button_X     );
            button_Y_state      = arduino.digitalRead( button_Y     );

            float counter       = 0;

            #region Buttons [ControlPad] - rebounce
            if ( timer_controlPad >= debounce_count )
            {
                // Control pad */
                if ( buttom_up_state == button_up_currentState && counter > 0 )
                { counter--; }
                if ( button_down_state == button_down_currentState && counter > 0 )
                { counter--; }
                if ( button_left_state == button_left_currentState && counter > 0 )
                { counter--; }
                if ( button_right_state == button_right_currentState && counter > 0 )
                { counter--; }

                if ( buttom_up_state != button_up_currentState )
                { counter++; }
                if ( button_down_state != button_down_currentState )
                { counter++; }
                if ( button_left_state != button_left_currentState )
                { counter++; }
                if ( button_right_state != button_right_currentState )
                { counter++; }
                // Control pad */

                if ( counter >= debounce_count )
                {
                    counter = 0;

                    // Control pad */
                    button_up_currentState = buttom_up_state;
                    button_down_currentState = button_down_state;
                    button_left_currentState = button_left_state;
                    button_right_currentState = button_right_state;
                    // Control pad */

                    timer_controlPad = Time.deltaTime;
                }
            }
            #endregion

            #region Button [Actions] - rebounce
            if ( timer_actions >= debounce_count )
            {
                // Action buttons */
                if ( button_A_state     == button_A_currentState && counter > 0 )
                {   counter--;  }
                if ( button_B_state     == button_B_currentState && counter > 0 )
                {   counter--;  }
                if ( button_X_state     == button_X_currentState && counter > 0 )
                {   counter--;  }
                if ( button_Y_state     == button_Y_currentState && counter > 0 )
                {   counter--;  }

                if ( button_A_state     != button_A_currentState )
                {   counter++;  }
                if ( button_B_state     != button_B_currentState )
                {   counter++;  }
                if ( button_X_state     != button_X_currentState )
                {   counter++;  }
                if ( button_Y_state     != button_Y_currentState )
                {   counter++;  }
                // Action buttons */

                if ( counter >= debounce_count )
                {
                    counter = 0;

                    // Action buttons */
                    button_A_currentState       = button_A_state;
                    button_B_currentState       = button_B_state;
                    button_X_currentState       = button_X_state;
                    button_Y_currentState       = button_Y_state;
                    // Action buttons */

                    timer_actions = Time.deltaTime;
                }
            }
            #endregion
        }  
	}
	#endregion

	#region SetColor
	public void SetColor ( int ledIndex, string colorName )
	{
		if ( colorName == "Red" )
		{
			redVal    = 255;
            greenVal  = 0;
			blueVal   = 0;
		}
		if ( colorName == "Green" )
		{
			redVal    = 0;
            greenVal  = 0;
			blueVal   = 255;	
		}
		if ( colorName == "Blue" )
		{
			redVal    = 0;
            greenVal  = 255;
			blueVal   = 0;	
		}
		if ( colorName == "Yellow" )
		{
			redVal    = 255;
            greenVal  = 0;
			blueVal   = 255;	
		}
		if ( colorName == "Off" )
		{
			redVal    = 0;
            greenVal  = 0;
			blueVal   = 0;	
		}

		if ( arduino != null )
		{
			if ( ledIndex == 1 )										// Maps the physical placement of the LEDs to a index number
			{
				arduino.digitalWrite( led_01_red  , redVal      - 255 );
                arduino.digitalWrite( led_01_green, greenVal    - 255 );	
				arduino.digitalWrite( led_01_blue , blueVal     - 255 );	
			}

			if ( ledIndex == 2 )
			{
				arduino.digitalWrite( led_02_red  , redVal      - 255 );
                arduino.digitalWrite( led_02_green, greenVal    - 255 );
				arduino.digitalWrite( led_02_blue , blueVal     - 255 );	
			}

			if ( ledIndex == 3 )
			{
				arduino.digitalWrite( led_03_red  , redVal      - 255 );
                arduino.digitalWrite( led_03_green, greenVal    - 255 );
				arduino.digitalWrite( led_03_blue , blueVal     - 255 );
			}

			if ( ledIndex == 4 )
			{
				arduino.digitalWrite( led_04_red  , redVal      - 255 );
                arduino.digitalWrite( led_04_green, greenVal    - 255 );
				arduino.digitalWrite( led_04_blue , blueVal     - 255 );	
			}
		}
	}
	#endregion

    #region Notifications

    void Notification_main(string text, bool showControls )
    {
        Transform notifications_main            = GUI.transform.GetChild(7).transform.GetChild(0);
        Text notifications_main_text            = notifications_main.GetComponent<Text>();
        notifications_main_text.text            = text;

        StartCoroutine( Notification_Decay_in( 10.0f, notifications_main_text, showControls ) );
    }

    void Notification_description(string text, bool showControls )
    {
        Transform notifications_description     = GUI.transform.GetChild(7).transform.GetChild(1);
        Text notifications_description_text     = notifications_description.GetComponent<Text>();
        notifications_description_text.text     = text;

        StartCoroutine( Notification_Decay_in( 10.0f, notifications_description_text, showControls ) );
    }

    IEnumerator Notification_Decay_in( float seconds, Text textToGoAway, bool showControl )
    {
        Transform controlsDisplay   = GUI.transform.GetChild(7).transform.GetChild(2);

            //StartCoroutine( FadeColorOverTime( textToGoAway, 1.0f, 0.0f, 2.0f ) );

            textToGoAway.enabled    = true;

            if ( showControl )
            {
                controlsDisplay.gameObject.SetActive( true );
            }

        yield return new WaitForSeconds(seconds);

            textToGoAway.enabled    = false;

            if ( showControl )
            {
                controlsDisplay.gameObject.SetActive( false );
            }
    }
    /*
    IEnumerator FadeColorOverTime ( Text text, float startLevel, float endLevel, float time )
    {
        float speed = 1.0f / time;

        for ( float t = 0.0f; t < 1.0; t += Time.deltaTime * speed )
        {
            float a         = Mathf.Lerp( startLevel, endLevel, t );
            text.color      = new Color( text.color.r, text.color.g, text.color.b, a ); 
            yield return 0;
        }
      
    }
    */
    #endregion

    #region ProgramZapping  // Real-time switching between programs

    void ProgramZapping()
    {
        if ( !program_1_active && !program_2_active && !program_3_active && !program_4_active )
        {
            Debug.Log("Please select a program...");
            SetColor( 1, indexArray[ 0 ] );
            SetColor( 2, indexArray[ 1 ] );
            SetColor( 3, indexArray[ 2 ] );
            SetColor( 4, indexArray[ 3 ] );
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            program_2_active = false;
            program_3_active = false;
            program_4_active = false;
            program_1_active = true;
            Notification_main("Program 1 - Normal-mapping", true);
            Notification_description("Description: Controls behave as you would expect.", true);
            RecalibrateColorArray();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            program_1_active = false;
            program_3_active = false;
            program_4_active = false;
            program_2_active = true;
            Notification_main("Program 2 - Konstant-mapping", true);
            Notification_description("Description: On input, controls rotate right.", true);
            RecalibrateColorArray();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            program_1_active = false;
            program_2_active = false;
            program_4_active = false;
            program_3_active = true;
            Notification_main("Program 3 - Random-mapping", true);
            Notification_description("Description: On input, random button layout.", true);
            RecalibrateColorArray();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            program_1_active = false;
            program_2_active = false;
            program_3_active = false;
            program_4_active = true;
            Notification_main("Program 4 - Gameplay'Special-mapping", true);
            Notification_description("Description: ---- ", true);
            RecalibrateColorArray();
        }
        if (program_1_active)
        {   
            //Debug.Log("Program_1();");
            Program_1();
        }
        if (program_2_active)
        {
            //Debug.Log("Program_2();");
            Program_2();
        }
        if (program_3_active)
        {     
            //Debug.Log("Program_3();");
            Program_3();
        }
        if (program_4_active)
        {  
            //Debug.Log("Program_4();");
            Program_4();
        }
    }

    #endregion

    #region Program 1   [Standard-mapping]

    void Program_1()
    {
        SetColor( 1, indexArray[ 0 ] );
        SetColor( 2, indexArray[ 1 ] );
        SetColor( 3, indexArray[ 2 ] );
        SetColor( 4, indexArray[ 3 ] );

        #region Button up & down
        if ( button_up_currentState == Arduino.LOW && button_down_currentState == Arduino.LOW )
        {
            move_Vertical = 0;
        }

        if ( button_up_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical = 1;
        }

        if ( button_down_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );

            move_Vertical = -1;
        }
        #endregion

        #region Button left & right
        if (button_left_currentState == Arduino.LOW && button_right_currentState == Arduino.LOW)
        {
            move_Horizontal = 0;
        }

        if (button_left_currentState == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal = 1;
        }

        if (button_right_currentState == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal = -1;
        }
        #endregion

        #region Action disable check
        if (button_A_currentState == Arduino.LOW || button_B_currentState == Arduino.LOW || button_X_currentState == Arduino.LOW || button_Y_currentState == Arduino.LOW)
        {
            canJump     = false;
            canRun      = false;
            canShoot    = false;
        }
        #endregion

        #region Button_Y [Button 1]
        if ( button_Y_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (1) [pressed]" );

            SwitchAction( 0 );
        }
        #endregion

        #region Button_X [Button 2]
        if ( button_X_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X (2) [pressed]" );

            SwitchAction( 1 );
        }
        #endregion

        #region Button_B [Button 3]
        if (button_B_currentState == Arduino.HIGH)
        {
            //Debug.Log("buttom_B (3) [pressed]");

            SwitchAction( 2 );
        }
        #endregion

        #region Button_A [Button 4]
        if (button_A_currentState == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_A (4) [pressed]" );

            SwitchAction( 3 );
        }
        #endregion
    }

    #endregion

    #region Program 2   ['konsekvent'-mapping]

    void Program_2 ()
    {
        SetColor( 1, indexArray[ 0 ] );
        SetColor( 2, indexArray[ 1 ] );
        SetColor( 3, indexArray[ 2 ] );
        SetColor( 4, indexArray[ 3 ] );
 
        #region Button up & down
        if ( button_up_currentState == Arduino.LOW && button_down_currentState == Arduino.LOW )
        {
            move_Vertical   = 0;
        }

        if ( button_up_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical   = 1;
        }

        if ( button_down_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );
            move_Vertical   = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_currentState == Arduino.LOW && button_right_currentState == Arduino.LOW )
        {
            move_Horizontal     = 0;
        }

        if ( button_left_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal     = 1;
        }

        if ( button_right_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal     = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_currentState == Arduino.LOW && button_B_currentState == Arduino.LOW && button_X_currentState == Arduino.LOW && button_Y_currentState == Arduino.LOW )
        {
            changeCommand = true;
        }

        if ( button_B_currentState == Arduino.LOW || button_Y_currentState == Arduino.LOW || button_X_currentState == Arduino.LOW || button_A_currentState == Arduino.LOW )
        {
            canJump     = false;
            canRun      = false;
            canShoot    = false;
        }
        #endregion


        #region Button_Y [Button 1]
        if ( button_Y_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );

            ChangeCommand();

            SwitchAction( 1 ); 
        }
        #endregion

        #region Button_X [Button 2]
        if ( button_X_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );

            ChangeCommand();

            SwitchAction( 2 ); 
        }
        #endregion

        #region Button_B [Button 3]
        if ( button_B_currentState == Arduino.HIGH  )
        {
            //Debug.Log("buttom_B (1) [pressed]");

            ChangeCommand();

            SwitchAction( 3 ); // Problem solved - Button_B = physical placement nr. 3 instead of 0
        }
        #endregion   

        #region Button_A [Button 4]
        if ( button_A_currentState == Arduino.HIGH  )
        {
            //Debug.Log ( "buttom_A [pressed]" );

            ChangeCommand();

            SwitchAction( 0 ); // Problem solved - Button_A = physical placement nr. 0 instead of 3
        }
        #endregion
    }

    void ChangeCommand()	                // Function used within program 2
    {
        if ( changeCommand )
        {
            commandRotation += 1;
            changeCommand    = false;
        }
        if ( commandRotation == 4 )         // CommandRotation Reset
        {
            commandRotation  = 0;
        }

        // Command specifications 
        const int index_0 = 0;
        const int index_1 = 1;
        const int index_2 = 2;
        const int index_3 = 3;

        switch ( commandRotation )              // Daniel: Forklaring af switch: http://unity3d.com/learn/tutorials/modules/beginner/scripting/switch
        { 
            case index_0:               
                indexArray[ 0 ] = "Yellow"  ;
                indexArray[ 1 ] = "Green"   ;
                indexArray[ 2 ] = "Blue"    ;
                indexArray[ 3 ] = "Red"     ;
                break;

            case index_1: 
                indexArray[ 0 ] = "Red"     ;
                indexArray[ 1 ] = "Yellow"  ;
                indexArray[ 2 ] = "Green"   ;
                indexArray[ 3 ] = "Blue"    ;
                break;

            case index_2:
                indexArray[ 0 ] = "Blue"    ;
                indexArray[ 1 ] = "Red"     ;
                indexArray[ 2 ] = "Yellow"  ;
                indexArray[ 3 ] = "Green"   ;
                break;

            case index_3:
                indexArray[ 0 ] = "Green"   ;
                indexArray[ 1 ] = "Blue"    ;
                indexArray[ 2 ] = "Red"     ;
                indexArray[ 3 ] = "Yellow"  ;
                break;

            default:
                Debug.Log( "Switch(CommandRotation) - IndexArray: Out of index!" );
                break;
        }
    }

    #endregion 

    #region Program 3   [random-mapping]

    void Program_3 ()
    {
        SetColor( 1, indexArray[ 0 ] );
        SetColor( 2, indexArray[ 1 ] );
        SetColor( 3, indexArray[ 2 ] );
        SetColor( 4, indexArray[ 3 ] );

        #region Button up & down
        if ( button_up_currentState == Arduino.LOW && button_down_currentState == Arduino.LOW )
        {
            move_Vertical   = 0;
        }

        if ( button_up_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical   = 1;
        }

        if ( button_down_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );
            move_Vertical   = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_currentState  == Arduino.LOW    &&    button_right_currentState == Arduino.LOW )
        {
            move_Horizontal     = 0;
        }

        if ( button_left_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal     = 1;
        }

        if ( button_right_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal     = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_currentState == Arduino.LOW    &&    button_B_currentState == Arduino.LOW    &&    button_X_currentState == Arduino.LOW    &&    button_Y_currentState == Arduino.LOW )
        {
            reMap       = true;
            canJump     = false;
            canRun      = false;
            canShoot    = false;
        }
        #endregion

        float delay = 0.1f;

        #region Button_Y [Button 1]
        if ( button_Y_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );

            SwitchAction( 0 );

            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_X [Button 2]
        if ( button_X_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );

            SwitchAction( 1 );

            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_B [Button 3]
        if ( button_B_currentState == Arduino.HIGH )
        {
            //Debug.Log("buttom_B (1) [pressed]");

            SwitchAction( 2 );

            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_A [Button 4]
        if ( button_A_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_A [pressed]" );

            SwitchAction( 3 );
            
            StartCoroutine( WaitFor( delay ) );
        }
        #endregion
    }

    IEnumerator WaitFor( float seconds )
    {
        yield return new WaitForSeconds( seconds );
        
        RandomMapping();
    }
    #endregion

    #region Program 4   [GameplaySpecial-mapping]

    void Program_4()
    {
        SetColor( 1, indexArray[ 0 ] );
        SetColor( 2, indexArray[ 1 ] );
        SetColor( 3, indexArray[ 2 ] );
        SetColor( 4, indexArray[ 3 ] );

        // This program is almost identical to program 3
        // It uses the same functions as program 3: WaitFor(), AddIndexNumberToArray() and RandomMapping()
        // The diffenrent being the reShuffle boolean, which can be triggered from other scripts 

        float       delay           = 0.1f;

        #region Button up & down
        if ( button_up_currentState == Arduino.LOW && button_down_currentState == Arduino.LOW )
        {
            move_Vertical = 0;
        }

        if ( button_up_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical = 1;
        }

        if ( button_down_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );
            move_Vertical = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_currentState == Arduino.LOW && button_right_currentState == Arduino.LOW )
        {
            move_Horizontal = 0;
        }

        if ( button_left_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal = 1;
        }

        if ( button_right_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_currentState == Arduino.LOW && button_B_currentState == Arduino.LOW && button_X_currentState == Arduino.LOW && button_Y_currentState == Arduino.LOW )
        {
            //reMap                 = true;       // In program 4, reMap is set to true elsewhere
            canJump                 = false;
            canRun                  = false;
            canShoot                = false;
            canDoSomethingSpecial   = false;      // turns false here? :/ maybe somewhere else. 
        }
        #endregion 

        if ( program_4_active )
        {
            //Debug.Log( "Mapping reShuffles!" );

            StartCoroutine( WaitFor( delay ) );
        }

        #region Button_Y [Button 1]
        if ( button_Y_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );

            SwitchAction( 0 );
        }
        #endregion

        #region Button_X [Button 2]
        if ( button_X_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );

            SwitchAction( 1 );
        }
        #endregion

        #region Button_B [Button 3]
        if ( button_B_currentState == Arduino.HIGH )
        {
            //Debug.Log("buttom_B (1) [pressed]");

            SwitchAction( 2 );
        }
        #endregion

        #region Button_A [Button 4]
        if ( button_A_currentState == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_A [pressed]" );

            SwitchAction( 3 );
        }
        #endregion
    }

    #endregion

    #region Add colors to indexArray
    void AddIndexNumberToArray()
    {
        indexArray.Add( "Yellow" );
        indexArray.Add( "Green" );
        indexArray.Add( "Blue" );
        indexArray.Add( "Red" );
        //Debug.Log ( "IndexArray: " + indexArray[0] + ", " + indexArray[1] + ", " + indexArray[2] + ", " + indexArray[3] );
    }
    #endregion

    #region Function: RandomMapping()   / Shuffles IndexArray
    void RandomMapping()
    {
        if ( reMap )
        {
            for ( int x = 0; x < indexArray.Count; x++ )
            {
                string 	tempString	            = indexArray[ x ];
                int 	random 		            = Random.Range( x, indexArray.Count );
                indexArray[ x ] = indexArray[ random ];
                indexArray[ random ] = tempString;
                break;
            }
            if ( program_4_active )
            {
                Notification_description( "Action buttons remapped!", false );
            }
            reMap = false;
        }
    }
    #endregion

    #region Function: Recalibrate colors in indexArray  / reset color order
    void RecalibrateColorArray()
    {
        indexArray[ 0 ] = "Yellow";
        indexArray[ 1 ] = "Green";
        indexArray[ 2 ] = "Blue";
        indexArray[ 3 ] = "Red";

        SetColor( 1, indexArray[ 0 ] );
        SetColor( 2, indexArray[ 1 ] );
        SetColor( 3, indexArray[ 2 ] );
        SetColor( 4, indexArray[ 3 ] );
    }
    #endregion

    #region Function: Switch action of a specific button
    void SwitchAction( int ledColor )
    {
        const string action_run     = "Green";  // Variables, used in a switch statement has to be constants
        const string action_jump    = "Yellow";
        const string action_shoot   = "Red";
        const string action_special = "Blue";

        switch ( indexArray[ ledColor ] )
        {
            case action_run:                    // Cases are what is supposed to be compared to the switch parameter - in this case - the indexArray[x]
                canRun = true;
                //Debug.Log( "Run" );
                break;

            case action_jump:
                canJump = true;
                //Debug.Log( "Jump" );
                break;

            case action_shoot:
                canShoot = true;
                //Debug.Log( "Shoot" );
                break;

            case action_special:
                canDoSomethingSpecial = true;
                //Debug.Log( "Special" );
                break;

            default:                            // Default doesnt need anything to be compared to. If the switch is compared to something else than the cases, the default will pick it up.
                Debug.Log( "The action is not known..." );
                break;
        }
    }
    #endregion
    /*
    void canRunDelay ()
    {
        if ( canRun )
        {
            runJumpVelocity_delay = true;
        }
        if ( runJumpVelocity_delay )
        {
            StartCoroutine( WaitForRunDelay( 1.5f ) );
        }
    }
    
    public static bool runJumpVelocity_delay = false;

    IEnumerator WaitForRunDelay( float seconds )
    {
        Debug.Log( "Jump delay begins" );
        yield return new WaitForSeconds( seconds );
        Debug.Log( "Jump delay ends" );
        runJumpVelocity_delay = false;
    }
    */
}


















