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
					bool 		button_1_run 			= false;
					bool 		button_2_run 			= false;
					bool 		button_3_run 			= false;
					bool 		button_4_run 			= false;
					
					bool 		button_2_jump 			= false;
					bool 		button_3_jump 			= false;
					bool 		button_4_jump			= false;
					bool 		button_1_jump 			= false;

                    bool        button_1_shoot          = false;
                    bool        button_2_shoot          = false;
                    bool        button_3_shoot          = false;
                    bool        button_4_shoot          = false;
	
    // Program 3 specific: 
    private         List<string>indexArray              = new List<string>();
    public static   bool        reMap                   = true; 

    // Program 4 specific: 
    //public static   bool        reShuffle               = false;                      // can be called from other scripts to reShuffle mapping
    public static   bool        canDoSomethingSpecial   = false;

	#endregion

	#region LED'pins' 		
	private			int 		led_01_red				= 37;
    private 		int 		led_01_green			= 35;
    private 		int 		led_01_blue				= 33;

    private 		int 		led_02_red				= 40;
    private 		int 		led_02_green			= 42;
    private 		int 		led_02_blue				= 44;

    private 		int 		led_03_red				= 48;
    private 		int 		led_03_green			= 50;
    private 		int 		led_03_blue				= 52;

    private 		int 		led_04_red				= 10;
    private 		int 		led_04_green			= 9;
    private 		int 		led_04_blue				= 8;
	#endregion

	#region Button'pins' 	[ControlPad]
    private 		int 		button_up 				= 57;   // A3
    private 		int 		button_down				= 56;   // A2
    private 		int 		button_left				= 55;   // A1
    private 		int 		button_right			= 54; 	// A0
	#endregion

	#region Button'pins' 	[Actions]
    private 		int 		button_B 				= 58;    // A4 / run   / Yellow
    private 		int 		button_Y 				= 59;    // A5 / jump  / Green
    private 		int 		button_X 				= 60;    // A6 / --    / Blue
    private 		int 		button_A 				= 61;    // A7 / shoot / Red
	#endregion

	#region Button'states' 	
	private			int 		buttom_up_state 		= 0;						// Variable for reading the pushbutton status
	private			int 		button_down_state 		= 0;
    private			int 		button_left_state 		= 0;
	private			int 		button_right_state 		= 0;

	private			int 		button_A_state 			= 0;
	private			int 		button_B_state 			= 0;
	private			int 		button_X_state 			= 0;
	private			int 		button_Y_state 			= 0;
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
	}
    
	#region CheckButtonState
	void CheckButtonState ()
	{
		if ( arduino != null )
		{
			buttom_up_state 	= arduino.digitalRead ( button_up 		);
			button_down_state 	= arduino.digitalRead ( button_down 	);
			button_left_state 	= arduino.digitalRead ( button_left 	);  
			button_right_state 	= arduino.digitalRead ( button_right 	);

			button_A_state 		= arduino.digitalRead ( button_A 		);
			button_B_state 		= arduino.digitalRead ( button_B 		);
			button_X_state 		= arduino.digitalRead ( button_X 		);
			button_Y_state 		= arduino.digitalRead ( button_Y 		);
		}
	}
	#endregion

	#region SetColor
	public void SetColor ( int ledIndex, string colorName )
	{
		int redVal 		= 0;
		int blueVal 	= 0;
		int greenVal 	= 0;

		if ( colorName == "Red" )
		{
			redVal    = 255;
			blueVal   = 0;
			greenVal  = 0;
		}
		if ( colorName == "Green" )
		{
			redVal    = 0;
			blueVal   = 0;
			greenVal  = 255;
		}
		if ( colorName == "Blue" )
		{
			redVal    = 0;
			blueVal   = 255;
			greenVal  = 0;
		}
		if ( colorName == "Yellow" )
		{
			redVal    = 255;
			blueVal   = 255;
			greenVal  = 0;
		}
		if ( colorName == "Off" )
		{
			redVal    = 0;
			blueVal   = 0;
			greenVal  = 0;
		}

		if ( arduino != null )
		{
			if ( ledIndex == 1 )										// Maps the physical placement of the LEDs to a index number
			{
				arduino.digitalWrite( led_01_red  , 255 - redVal   );	
				arduino.digitalWrite( led_01_blue , 255 - blueVal  );	
				arduino.digitalWrite( led_01_green, 255 - greenVal );	
			}

			if ( ledIndex == 2 )
			{
				arduino.digitalWrite( led_02_red  , 255 - redVal   );
				arduino.digitalWrite( led_02_blue , 255 - blueVal  );
				arduino.digitalWrite( led_02_green, 255 - greenVal );
			}

			if ( ledIndex == 3 )
			{
				arduino.digitalWrite( led_03_red  , 255 - redVal   );
				arduino.digitalWrite( led_03_blue , 255 - blueVal  );
				arduino.digitalWrite( led_03_green, 255 - greenVal );
			}

			if ( ledIndex == 4 )
			{
				arduino.digitalWrite( led_04_red  , 255 - redVal   );
				arduino.digitalWrite( led_04_blue , 255 - blueVal  );
				arduino.digitalWrite( led_04_green, 255 - greenVal );
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
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            program_1_active = false;
            program_3_active = false;
            program_4_active = false;
            program_2_active = true;
            Notification_main("Program 2 - Konstant-mapping", true);
            Notification_description("Description: On input, controls rotate right.", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            program_1_active = false;
            program_2_active = false;
            program_4_active = false;
            program_3_active = true;
            Notification_main("Program 3 - Random-mapping", true);
            Notification_description("Description: On input, random button layout.", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            program_1_active = false;
            program_2_active = false;
            program_3_active = false;
            program_4_active = true;
            Notification_main("Program 4 - Gameplay'Special-mapping", true);
            Notification_description("Description: ---- ", true);
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
        SetColor(1, "Yellow");
        SetColor(2, "Green");
        SetColor(3, "Blue");
        SetColor(4, "Red");
        
        #region Button up & down
        if (buttom_up_state == Arduino.LOW && button_down_state == Arduino.LOW)
        {
            move_Vertical = 0;
        }

        if (buttom_up_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical = 1;
        }

        if (button_down_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_down [pressed]" );

            move_Vertical = -1;
        }
        #endregion

        #region Button left & right
        if (button_left_state == Arduino.LOW && button_right_state == Arduino.LOW)
        {
            move_Horizontal = 0;
        }

        if (button_left_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal = 1;
        }

        if (button_right_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal = -1;
        }
        #endregion

        #region Action disable check
        if (button_A_state == Arduino.LOW || button_B_state == Arduino.LOW || button_X_state == Arduino.LOW || button_Y_state == Arduino.LOW)
        {
            canJump     = false;
            canRun      = false;
            canShoot    = false;
        }
        #endregion

        #region Button_B
        if (button_B_state == Arduino.HIGH)
        {
            //Debug.Log("buttom_B (1) [pressed]");
            canRun = true;    
        }
        #endregion

        #region Button_Y
        if (button_Y_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );
            // --
        }
        #endregion

        #region Button_X
        if (button_X_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_X [pressed]" );
            canJump = true;
        }
        #endregion

        #region Button_A
        if (button_A_state == Arduino.HIGH)
        {
            //Debug.Log ( "buttom_A [pressed]" );
            canShoot = true;
        }
        #endregion
    }

    #endregion

    #region Program 2   ['konsekvent'-mapping]

    void Program_2 ()
    {
        #region Standard colors
        if (commandRotation == 0)
        {
            SetColor( 1, "Yellow"   );
            SetColor( 2, "Green"    );
            SetColor( 3, "Blue"     );
            SetColor( 4, "Red"      );
        }
        #endregion

        #region Button up & down
        if ( buttom_up_state    == Arduino.LOW     &&     button_down_state == Arduino.LOW )
        {
            move_Vertical   = 0;
        }

        if ( buttom_up_state    == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical   = 1;
        }

        if ( button_down_state  == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );

            move_Vertical   = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_state  == Arduino.LOW   &&   button_right_state == Arduino.LOW )
        {
            move_Horizontal     = 0;
        }

        if ( button_left_state  == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal     = 1;
        }

        if ( button_right_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal     = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_state == Arduino.LOW   &&   button_B_state == Arduino.LOW   &&   button_X_state == Arduino.LOW   &&   button_Y_state == Arduino.LOW )
        {
            changeCommand = true;
        }
        if ( !button_1_jump   ||   !button_2_jump   ||   !button_3_jump   ||   !button_4_jump  )
        {
            canJump = false;
        }
        if ( !button_1_run    ||   !button_2_run    ||   !button_3_run    ||   !button_4_run   )
        {
            canRun  = false;
        }
        if (!button_1_shoot || !button_2_shoot || !button_3_shoot || !button_4_shoot)
        {
            canShoot = false;
        }
        #endregion

        #region Button_B
        if ( button_B_state == Arduino.HIGH )
        {
            //Debug.Log("buttom_B (1) [pressed]");
            ChangeCommand ();

            if ( button_1_run  )
            {
                canRun  = true;
            }
            if ( button_1_jump )
            {
                canJump = true;
            }
            if ( button_1_shoot )
            {
                canShoot = true;
            }
        }
        #endregion

        #region Button_Y
        if ( button_Y_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );
            ChangeCommand ();	

            if ( button_2_run  )
            {
                canRun  = true;
            }
            if ( button_2_jump )
            {
                canJump = true;
            }
            if ( button_2_shoot )
            {
                canShoot = true;
            }
        }
        #endregion

        #region Button_X
        if ( button_X_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );
            ChangeCommand ();	

            if ( button_3_run )
            {
                canRun  = true;
            }
            if ( button_3_jump )
            {
                canJump = true;
            }
            if ( button_3_shoot )
            {
                canShoot = true;
            }
        }
        #endregion

        #region Button_A
        if ( button_A_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_A [pressed]" );
            ChangeCommand ();

            if ( button_4_run )
            {
                canRun  = true;
            }
            if ( button_4_jump )
            {
                canJump = true;
            }
            if ( button_4_shoot )
            {
                canShoot = true;
            }
        }
        #endregion
    }

    void ChangeCommand()	                // Function used within program 2
    {
        if ( changeCommand )
        {
            commandRotation += 1;
            changeCommand   = false;
        }
        if ( commandRotation == 4 )         // CommandRotation Reset
        {
            commandRotation  = 0;
        }

        //###################//
        // button_1 = yellow //
        // button_2 = green	 // 
        // button_3 = blue	 //
        // button_4 = red	 //
        //###################//

        button_1_run    = false;
        button_2_run    = false;
        button_3_run    = false;
        button_4_run    = false;

        button_1_jump   = false;
        button_2_jump   = false;
        button_3_jump   = false;
        button_4_jump   = false;

        button_1_shoot  = false;
        button_2_shoot  = false;
        button_3_shoot  = false;
        button_4_shoot  = false;

        // Command specifications 
        if ( commandRotation == 0 )
        {
            button_1_run    = true;
            button_2_jump   = true;
            button_4_shoot  = true;

            SetColor( 1, "Yellow"   );
            SetColor( 2, "Green"    );
            SetColor( 3, "Blue"     );
            SetColor( 4, "Red"      );
        }

        if ( commandRotation == 1 )
        {
            button_2_run    = true;
            button_3_jump   = true;
            button_1_shoot  = true;

            SetColor( 1, "Red"      );
            SetColor( 2, "Yellow"   );
            SetColor( 3, "Green"    );
            SetColor( 4, "Blue"     );
        }

        if ( commandRotation == 2 )
        {
            button_3_run    = true;
            button_4_jump   = true;
            button_2_shoot  = true;

            SetColor( 1, "Blue"     );
            SetColor( 2, "Red"      );
            SetColor( 3, "Yellow"   );
            SetColor( 4, "Green"    );
        }

        if ( commandRotation == 3 )
        {
            button_4_run    = true;
            button_1_jump   = true;
            button_3_shoot  = true;

            SetColor( 1, "Green"    );
            SetColor( 2, "Blue"     );
            SetColor( 3, "Red"      );
            SetColor( 4, "Yellow"   );
        }
    }

    #endregion 

    #region Program 3   [random-mapping]

    void Program_3 ()
    {
        #region Button up & down
        if ( buttom_up_state    == Arduino.LOW   &&   button_down_state == Arduino.LOW )
        {
            move_Vertical   = 0;
        }

        if ( buttom_up_state    == Arduino.HIGH  )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical   = 1;
        }

        if ( button_down_state  == Arduino.HIGH  )
        {
            //Debug.Log ( "buttom_down [pressed]" );
            move_Vertical   = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_state  == Arduino.LOW    &&    button_right_state == Arduino.LOW )
        {
            move_Horizontal     = 0;
        }

        if ( button_left_state  == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal     = 1;
        }

        if ( button_right_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal     = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_state == Arduino.LOW    &&    button_B_state == Arduino.LOW    &&    button_X_state == Arduino.LOW    &&    button_Y_state == Arduino.LOW )
        {
            reMap       = true;
            canJump     = false;
            canRun      = false;
            canShoot    = false;
        }
        #endregion

        string      color_run       = "Yellow";
        string      color_jump      = "Blue";
        string      color_shoot     = "Red";
        float       delay           = 0.1f;

        #region Button_B
        if ( button_B_state == Arduino.HIGH )
        {
            //Debug.Log("buttom_B (1) [pressed]");

            if ( indexArray[0] == color_run  )
            {
                canRun      = true;
            }
            if ( indexArray[0] == color_jump )
            {
                canJump     = true;
            }
            if (indexArray[0] == color_shoot )
            {
                canShoot    = true;
            }
            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_Y
        if ( button_Y_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );

            if ( indexArray[1] == color_run )
            {
                canRun  = true;
            }
            if ( indexArray[1] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[1] == color_shoot )
            {
                canShoot = true;
            }
            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_X
        if ( button_X_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );

            if ( indexArray[2] == color_run )
            {
                canRun  = true;
            }
            if ( indexArray[2] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[2] == color_shoot )
            {
                canShoot = true;
            }
            StartCoroutine( WaitFor( delay ) );
        }
        #endregion

        #region Button_A
        if ( button_A_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_A [pressed]" );

            if ( indexArray[3] == color_run )
            {
                canRun  = true;
            }
            if ( indexArray[3] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[3] == color_shoot )
            {
                canShoot = true;
            }
            StartCoroutine( WaitFor( delay ) );
        }
        #endregion
    }

    IEnumerator WaitFor( float seconds )
    {
        yield return new WaitForSeconds( seconds );
        
        RandomMapping();
    }

    void AddIndexNumberToArray ()
	{
		indexArray.Add( "Yellow" );
		indexArray.Add( "Blue"   );
        indexArray.Add( "Green"  );
		indexArray.Add( "Red"    );
		//Debug.Log ( "IndexArray: " + indexArray[0] + ", " + indexArray[1] + ", " + indexArray[2] + ", " + indexArray[3] );
	}

	void RandomMapping ()
	{
		if ( reMap )
		{
			for ( int x = 0; x < indexArray.Count; x++ )
			{
				string 	tempString	            = indexArray[ x ];
				int 	random 		            = Random.Range( x, indexArray.Count );
				        indexArray[ x ] 		= indexArray[ random ];
				        indexArray[ random ] 	= tempString;
				break;
			}

            SetColor( 1, indexArray[ 0 ] );
            SetColor( 2, indexArray[ 1 ] );
            SetColor( 3, indexArray[ 2 ] );
            SetColor( 4, indexArray[ 3 ] );

            if ( program_4_active )
            {
                Notification_description( "Action buttons remapped!", false);
            }

			reMap = false;
		}
    }

    #endregion

    #region Program 4   [GameplaySpecial-mapping]

    void Program_4()
    {
        // This program is almost identical to program 3
        // It uses the same functions as program 3: WaitFor(), AddIndexNumberToArray() and RandomMapping()
        // The diffenrent being the reShuffle boolean, which can be triggered from other scripts 

        string      color_run       = "Yellow";
        string      color_jump      = "Blue";   // means green
        string      color_shoot     = "Red";
        string      color_blue      = "Green";  // Actually means blue
        float       delay           = 0.1f;

        #region Button up & down
        if ( buttom_up_state == Arduino.LOW && button_down_state == Arduino.LOW )
        {
            move_Vertical = 0;
        }

        if ( buttom_up_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_up [pressed]" );
            move_Vertical = 1;
        }

        if ( button_down_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_down [pressed]" );
            move_Vertical = -1;
        }
        #endregion

        #region Button left & right
        if ( button_left_state == Arduino.LOW && button_right_state == Arduino.LOW )
        {
            move_Horizontal = 0;
        }

        if ( button_left_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_left [pressed]" );
            move_Horizontal = 1;
        }

        if ( button_right_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_right [pressed]" );
            move_Horizontal = -1;
        }
        #endregion

        #region Action disable check
        if ( button_A_state == Arduino.LOW && button_B_state == Arduino.LOW && button_X_state == Arduino.LOW && button_Y_state == Arduino.LOW )
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
            Debug.Log( "Mapping reShuffles!" );
            //Debug.Log( "IndexArray: " + indexArray[ 0 ] + "," + indexArray[ 1 ] + "," + indexArray[ 2 ] + "," + indexArray[ 3 ] );

            StartCoroutine( WaitFor( delay ) );
        }

        #region Button_B
        if ( button_B_state == Arduino.HIGH )
        {
            //Debug.Log("buttom_B (1) [pressed]");

            if ( indexArray[ 0 ] == color_run )
            {
                canRun = true;
            }
            if ( indexArray[ 0 ] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[ 0 ] == color_shoot )
            {
                canShoot = true;
            }
            if ( indexArray[ 0 ] == color_blue )
            {
                // Something special happens
                canDoSomethingSpecial = true;
            }
        }
        #endregion

        #region Button_Y
        if ( button_Y_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_Y (2) [pressed]" );

            if ( indexArray[ 1 ] == color_run )
            {
                canRun = true;
            }
            if ( indexArray[ 1 ] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[ 1 ] == color_shoot )
            {
                canShoot = true;
            }
            if ( indexArray[ 1 ] == color_blue )
            {
                // Something special happens
                canDoSomethingSpecial = true;
            }
        }
        #endregion

        #region Button_X
        if ( button_X_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_X [pressed]" );

            if ( indexArray[ 2 ] == color_run )
            {
                canRun = true;
            }
            if ( indexArray[ 2 ] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[ 2 ] == color_shoot )
            {
                canShoot = true;
            }
            if ( indexArray[ 2 ] == color_blue )
            {
                // Something special happens
                canDoSomethingSpecial = true;
            }
        }
        #endregion

        #region Button_A
        if ( button_A_state == Arduino.HIGH )
        {
            //Debug.Log ( "buttom_A [pressed]" );

            if ( indexArray[ 3 ] == color_run )
            {
                canRun = true;
            }
            if ( indexArray[ 3 ] == color_jump )
            {
                canJump = true;
            }
            if ( indexArray[ 3 ] == color_shoot )
            {
                canShoot = true;
            }
            if ( indexArray[ 3 ] == color_blue )
            {
                // Something special happens
                canDoSomethingSpecial = true;
            }
        }
        #endregion
    }

    #endregion
}


















