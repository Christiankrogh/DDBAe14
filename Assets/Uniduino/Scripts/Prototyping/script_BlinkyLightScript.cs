using UnityEngine;
using System.Collections;
using Uniduino;

public class script_BlinkyLightScript : MonoBehaviour 
{
	public Arduino arduino; 

	const int button_01 = 2;					// the number of the pushbutton pin
	const int button_02 = 3;

	const int button_01_LEDgreen = 13;
	const int button_02_LEDgreen = 12;
	const int button_01_LEDred   = 11;
	const int button_02_LEDred 	 = 10;

	public int button_01_state = 0;					// Variable for reading the pushbutton status
	public int button_02_state = 0;



	void Start () 
	{
		arduino = Arduino.global;				// Searches for the one and only Arduino connected to Unity
		arduino.Setup (ConfigurePins);		// Set up pins
	
	
		//StartCoroutine ( BlinkLoop() );
	}

	void ConfigurePins()						// In here we can define what each pin does
	{
		arduino.pinMode (button_01,  		 PinMode.INPUT);	// Sends stuff to pin 13 on the arduino board. In this case it sends a 'output'
		arduino.pinMode (button_02,  		 PinMode.INPUT);
		arduino.pinMode (button_01_LEDgreen, PinMode.OUTPUT);
		arduino.pinMode (button_02_LEDgreen, PinMode.OUTPUT);
		arduino.pinMode (button_01_LEDred, 	 PinMode.OUTPUT);
		arduino.pinMode (button_02_LEDred,   PinMode.OUTPUT);

		arduino.reportDigital ((byte)(button_01 / 8), 1);
		arduino.reportDigital ((byte)(button_02 / 8), 1);		// (byte)(pin/8) is basically a way of finding the port your pin is part of. Ports are groups of 8, with 3 total ports on the arduino Uno. 
																// Pin 2 (from example) is on port 0.
																// The seconds number in the parameters (1) is the enabled option. 1 = on, 0 = off
	}
	/*
	IEnumerator BlinkLoop()
	{
		while (true) 
		{
			arduino.digitalWrite(13, Arduino.HIGH);	// Turns on pin 13
			yield return new WaitForSeconds(1);
			arduino.digitalWrite(13, Arduino.LOW);
			yield return new WaitForSeconds(1);
		}
	}
	*/


	void Update () 
	{
		CheckButtonState ();

		//Debug.Log ("Button 1: " + button_01_state);
		//Debug.Log ("Button 2: " + button_02_state);
	}

	void CheckButtonState()
	{
		button_01_state = arduino.digitalRead (button_01);
		button_02_state = arduino.digitalRead (button_02);

		// Button # 1
		if (button_01_state == Arduino.HIGH) 
		{
			arduino.digitalWrite(button_01_LEDgreen, Arduino.HIGH );
			arduino.digitalWrite(button_01_LEDred,   Arduino.LOW );
		}
		else
		{	
			arduino.digitalWrite(button_01_LEDgreen, Arduino.LOW );
			arduino.digitalWrite(button_01_LEDred,   Arduino.HIGH );
		}

		// Button # 2
		if (button_02_state == Arduino.HIGH) 
		{
			arduino.digitalWrite(button_02_LEDgreen, Arduino.HIGH );
			arduino.digitalWrite(button_02_LEDred, 	 Arduino.LOW );
		}
		else
		{
			arduino.digitalWrite(button_02_LEDgreen, Arduino.LOW );
			arduino.digitalWrite(button_02_LEDred,   Arduino.HIGH );
		}
	}

}













