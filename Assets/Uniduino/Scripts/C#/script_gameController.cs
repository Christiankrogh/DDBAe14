using UnityEngine;
using System.Collections;
using Uniduino;

public class script_gameController : MonoBehaviour 
{
	private Arduino arduino;

	#region LED'pins' 		

	const 	int 	GREEN 				= 9;
	const 	int 	BLUE  				= 10;
	const 	int 	RED   				= 11;

	#endregion

	#region Button'pins' 	[ControlPad]

	const 	int 	button_up 			= 12;						// the number of the pushbutton pin
	const 	int 	button_down			= 2;
	const 	int 	button_left			= 3;
	const 	int 	button_right		= 4;

	#endregion

	#region Button'pins' 	[Actions]

	const 	int 	button_A 			= 5;
	const 	int 	button_B 			= 6;
	const 	int 	button_X 			= 7;
	const 	int 	button_Y 			= 8;

	#endregion

	#region Button'states' 	

	public 	int 	buttom_up_state 	= 0;						// Variable for reading the pushbutton status
	public 	int 	button_down_state 	= 0;
	public 	int 	button_left_state 	= 0;
	public 	int 	button_right_state 	= 0;

	public 	int 	button_A_state 		= 0;
	public 	int 	button_B_state 		= 0;
	public 	int 	button_X_state 		= 0;
	public 	int 	button_Y_state 		= 0;

	#endregion
	

	void ConfigurePins()											// In here we can define what each pin does
	{
		arduino.pinMode (GREEN			, 	PinMode.OUTPUT);			// Sends stuff to pin 13 on the arduino board. In this case it sends a 'output'
		arduino.pinMode (BLUE			,  	PinMode.OUTPUT);
		arduino.pinMode (RED			, 	PinMode.OUTPUT);

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
	

	void Start () 
	{
		arduino = Arduino.global;									// Searches for the one and only Arduino connected to Unity
		arduino.Setup (ConfigurePins);								// Set up pins
	}


	void Update () 													// Checks for button'pushes
	{
		CheckButtonState ();
	}


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

		#region Buttom_up
		if ( buttom_up_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_up [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_down
		if 		( button_down_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_down [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_left
		if 		( button_left_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_left [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_right
		if 		( button_right_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_right [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_A
		if 		( button_A_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_A [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_B
		if 		( button_B_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_B [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_X
		if 		( button_X_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_X [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

		#region Button_Y
		if 		( button_Y_state == Arduino.HIGH ) 
		{	
			Debug.Log ( "buttom_Y [pressed]" );
			SetColor ("Green");		
		}
		else 	
		{	
			SetColor ("Red");		
		}
		#endregion

	}


	public void SetColor ( string colorName )
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
			blueVal   = 0;
			greenVal  = 255;
		}
		if ( colorName == "Off" )
		{
			redVal    = 0;
			blueVal   = 0;
			greenVal  = 0;
		}
	
		arduino.digitalWrite( RED  , 255 - redVal   );
		arduino.digitalWrite( BLUE , 255 - blueVal  );
		arduino.digitalWrite( GREEN, 255 - greenVal );
	}
	
}

