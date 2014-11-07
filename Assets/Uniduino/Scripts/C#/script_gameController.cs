using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Uniduino;

#region Description
	// Note: To change the layout of the variables in this class, edit the editorScript called: script_customInspector_gameControllerScript
#endregion

//[ExecuteInEditMode] 
public class script_gameController : MonoBehaviour 
{
	private 		Arduino 	arduino;
					int 		commandRotation 		= 0;
					bool 		changeCommand 			= true;

	#region Action variables
					bool 		button_1_run 			= false;
					bool 		button_2_run 			= false;
					bool 		button_3_run 			= false;
					bool 		button_4_run 			= false;
					
					bool 		button_2_jump 			= false;
					bool 		button_3_jump 			= false;
					bool 		button_4_jump			= false;
					bool 		button_1_jump 			= false;
	
	public static 	bool 		canJump 				= false;
	public static 	bool 		canRun 					= false;
	
	public static 	int 		move_Horizontal 		= 0;
	public static 	int 		move_Vertical 			= 0;
	#endregion

	#region LED'pins' 		
	public 			int 		led_01_red				= 50;
	public 			int 		led_01_green			= 51;
	public 			int 		led_01_blue				= 52;

	public 			int 		led_02_red				= 42;
	public 			int 		led_02_green			= 43;
	public 			int 		led_02_blue				= 44;

	public 			int 		led_03_red				= 34;
	public 			int 		led_03_green			= 35;
	public 			int 		led_03_blue				= 36;

	public 			int 		led_04_red				= 26;
	public 			int 		led_04_green			= 27;
	public 			int 		led_04_blue				= 28;
	#endregion

	#region Button'pins' 	[ControlPad]
	public 			int 		button_up 				= 10;						// the number of the pushbutton pin
	public 			int 		button_down				= 12;
	public 			int 		button_left				= 5;
	public 			int 		button_right			= 6;		 
	#endregion

	#region Button'pins' 	[Actions]
	public 			int 		button_B 				= 7;
	public 			int 		button_Y 				= 2;
	public 			int 		button_X 				= 3;
	public 			int 		button_A 				= 4;
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
		arduino.reportDigital ( ( byte ) ( button_right	/ 8 ), 1 );
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
		AddIndexNumberToArray ();

		arduino = Arduino.global;									// Searches for the one and only Arduino connected to Unity
		arduino.Setup (ConfigurePins);								// Set up pins
	}


	void Update () 													// Checks for button'pushes
	{
		Debug.Log( "LED 1 = " + indexArray[0] );
		Debug.Log( "LED 2 = " + indexArray[1] );
		Debug.Log( "LED 3 = " + indexArray[2] );
		Debug.Log( "LED 4 = " + indexArray[3] );
		
		SetColor ( 1, indexArray[0] );	
		SetColor ( 2, indexArray[1] );
		SetColor ( 3, indexArray[2] );	
		SetColor ( 4, indexArray[3] );

		CheckButtonState ();
		/*
		if ( commandRotation == 0 )
		{
			SetColor ( 1, "Yellow" 	);	
			SetColor ( 2, "Green" 	);
			SetColor ( 3, "Blue" 	);	
			SetColor ( 4, "Red" 	);	
		}
		*/
		/*
		Debug.Log ( "New indexArray: " 
			   				+ indexArray[0] + 
			           ", " + indexArray[1] + 
			           ", " + indexArray[2] + 
			           ", " + indexArray[3] );
		*/
	}

	#region ChangeCommand
	void ChangeCommand ()	// function which return int value
	{
		if ( changeCommand )
		{
			commandRotation += 1;
			changeCommand 	 = false;
		}

		// CommandRotation Reset
		if ( commandRotation == 4 )
		{
			 commandRotation = 0;
		}

		//////////////////////////////////////////////////////

		//###################//
		// button_1 = yellow //
		// button_2 = green	 // 
		// button_3 = blue	 //
		// button_4 = red	 //
		//###################//

		button_1_run  = false;
		button_2_run  = false;
		button_3_run  = false;
		button_4_run  = false;

		button_1_jump = false;
		button_2_jump = false;
		button_3_jump = false;
		button_4_jump = false;

		// Command specifications 
		if ( commandRotation == 0 )
		{
			button_1_run  = true;
			button_2_jump = true;

			SetColor ( 1, "Yellow" 	);	
			SetColor ( 2, "Green" 	);
			SetColor ( 3, "Blue" 	);	
			SetColor ( 4, "Red" 	);	
		}

		if ( commandRotation == 1 )
		{	
			button_2_run  = true;
			button_3_jump = true;

			SetColor ( 1, "Red" 	);	
			SetColor ( 2, "Yellow" 	);
			SetColor ( 3, "Green" 	);	
			SetColor ( 4, "Blue" 	);
		}

		if ( commandRotation == 2 )
		{	
			button_3_run  = true;
			button_4_jump = true;

			SetColor ( 1, "Blue" 	);	
			SetColor ( 2, "Red" 	);
			SetColor ( 3, "Yellow" 	);	
			SetColor ( 4, "Green" 	);	
		}

		if ( commandRotation == 3 )
		{	
			button_4_run  = true;
			button_1_jump = true;

			SetColor ( 1, "Green" 	);	
			SetColor ( 2, "Blue" 	);
			SetColor ( 3, "Red" 	);	
			SetColor ( 4, "Yellow"	);
		}
	}
	#endregion

	#region CheckButtonState
	void CheckButtonState ()
	{
		#region buttons [digitalRead]
		buttom_up_state 	= arduino.digitalRead ( button_up 		);
		button_down_state 	= arduino.digitalRead ( button_down 	);
		button_left_state 	= arduino.digitalRead ( button_left 	);
		button_right_state 	= arduino.digitalRead ( button_right 	);

		button_A_state 		= arduino.digitalRead ( button_A 		);
		button_B_state 		= arduino.digitalRead ( button_B 		);
		button_X_state 		= arduino.digitalRead ( button_X 		);
		button_Y_state 		= arduino.digitalRead ( button_Y 		);

		#endregion

		#region Button up & down
		if ( buttom_up_state == Arduino.LOW && button_down_state == Arduino.LOW )
		{
			move_Vertical = 0;
		}

		if ( buttom_up_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_up [pressed]" );

			move_Vertical = 1;
		}

		if ( button_down_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_down [pressed]" );

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
			Debug.Log ( "buttom_left [pressed]" );

			move_Horizontal = 1;	
		}
	
		if ( button_right_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_right [pressed]" );
	
			move_Horizontal = -1;
		}
		#endregion
	
		#region Action disable check 
		if ( button_A_state == Arduino.LOW && button_B_state == Arduino.LOW && button_X_state == Arduino.LOW	&& button_Y_state == Arduino.LOW )
		{
			changeCommand 	= true;

			reMap			= true;
		}
		if ( !button_1_jump || !button_2_jump || !button_3_jump || !button_4_jump )
		{
			canJump 		= false;
		}
		if ( !button_1_run || !button_2_run || !button_3_run || !button_4_run )
		{
			canRun 			= false;
		}
		#endregion

		#region Button_B
		if ( button_B_state == Arduino.HIGH		||	Input.GetKey( KeyCode.Q ) ) 
		{	
			//Debug.Log ( "buttom_B (1) [pressed]" );
			//ChangeCommand ();

			RandomMapping ();

			if ( button_1_run )
			{
				canRun = true;
			}

			if ( button_1_jump || indexArray[0] == "Green" )
			{
				canJump = true;
			}
		}
		#endregion

		#region Button_Y
		if ( button_Y_state == Arduino.HIGH		||	Input.GetKey( KeyCode.S ) ) 
		{	
			//Debug.Log ( "buttom_Y (2) [pressed]" );
			//ChangeCommand ();	

			RandomMapping ();

			if ( button_2_run )
			{
				canRun = true;
			}

			if ( button_2_jump || indexArray[1] == "Green" )
			{
				canJump = true;
			}
		}
		#endregion

		#region Button_X
		if ( button_X_state == Arduino.HIGH		||	Input.GetKey( KeyCode.W ) ) 
		{	
			//Debug.Log ( "buttom_X [pressed]" );
			//ChangeCommand ();	

			RandomMapping ();

			if ( button_3_run )
			{
				canRun = true;
			}

			if ( button_3_jump || indexArray[2] == "Green" )
			{
				canJump = true;
			}
		}
		#endregion

		#region Button_A
		if ( button_A_state == Arduino.HIGH 	||	Input.GetKey( KeyCode.A )	 ) 
		{	
			//Debug.Log ( "buttom_A [pressed]" );
			//ChangeCommand ();

			RandomMapping ();

			if ( button_4_run )
			{
				canRun = true;
			}

			if ( button_4_jump || indexArray[3] == "Green" )
			{
				canJump = true;
			}
		}
		#endregion

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
	#endregion

	

	public 	List<string> 	indexArray 	= new List<string>();
	private bool 			reMap 		= true; 

	void AddIndexNumberToArray ()
	{
		indexArray.Add("Yellow");
		indexArray.Add("Green");
		indexArray.Add("Blue");
		indexArray.Add("Red");

		Debug.Log ( "IndexArray: " + indexArray[0] + ", " + indexArray[1] + ", " + indexArray[2] + ", " + indexArray[3] );
	}

	void RandomMapping ()
	{
		if ( reMap )
		{
			for ( int x = 0; x < indexArray.Count; x++ )
			{
				string 	tempString	= indexArray[x];
				int 	random 		= Random.Range( x, indexArray.Count );
				indexArray[x] 		= indexArray[random];
				indexArray[random] 	= tempString;
				break;
			}

			reMap = false;
		}
	}


}


















