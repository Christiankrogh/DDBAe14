using UnityEngine;
using System.Collections;
using UnityEditor;

// Tutorial for this: http://unity3d.com/learn/tutorials/modules/intermediate/editor/building-custom-inspector

[CustomEditor(typeof(script_gameController))]
public class script_customInspector_gameControllerScript : Editor 
{
	public override void OnInspectorGUI()
	{
		script_gameController gameController = (script_gameController)target;
	
		EditorGUILayout.HelpBox("Description: PIN-numbers for all input/output", MessageType.Info );

		//DrawDefaultInspector();

		EditorGUILayout.LabelField("Control buttons");
		gameController.button_up 		= EditorGUILayout.IntField("  - Up\t: "		, gameController.button_up);
		gameController.button_down 		= EditorGUILayout.IntField("  - Down\t: "	, gameController.button_down);
		gameController.button_left 		= EditorGUILayout.IntField("  - Left\t: "	, gameController.button_left);
		gameController.button_right 	= EditorGUILayout.IntField("  - Right\t: "	, gameController.button_right);

		EditorGUILayout.LabelField("Action buttons");
		gameController.button_B 		= EditorGUILayout.IntField("  - B\t: "		, gameController.button_B);
		gameController.button_Y 		= EditorGUILayout.IntField("  - Y\t: "		, gameController.button_Y);
		gameController.button_X 		= EditorGUILayout.IntField("  - X\t: "		, gameController.button_X);
		gameController.button_A 		= EditorGUILayout.IntField("  - A\t: "		, gameController.button_A);

		EditorGUILayout.LabelField("RGB-LED[1]");
		gameController.led_01_red 		= EditorGUILayout.IntField("  - Red\t: "	, gameController.led_01_red);
		gameController.led_01_green 	= EditorGUILayout.IntField("  - Green\t: "	, gameController.led_01_green);
		gameController.led_01_blue 		= EditorGUILayout.IntField("  - Blue\t: "	, gameController.led_01_blue);

		EditorGUILayout.LabelField("RGB-LED[2]");
		gameController.led_02_red 		= EditorGUILayout.IntField("  - Red\t: "	, gameController.led_02_red);
		gameController.led_02_green 	= EditorGUILayout.IntField("  - Green\t: "	, gameController.led_02_green);
		gameController.led_02_blue 		= EditorGUILayout.IntField("  - Blue\t: "	, gameController.led_02_blue);

		EditorGUILayout.LabelField("RGB-LED[3]");
		gameController.led_03_red 		= EditorGUILayout.IntField("  - Red\t: "	, gameController.led_03_red);
		gameController.led_03_green 	= EditorGUILayout.IntField("  - Green\t: "	, gameController.led_03_green);
		gameController.led_03_blue 		= EditorGUILayout.IntField("  - Blue\t: "	, gameController.led_03_blue);

		EditorGUILayout.LabelField("RGB-LED[4]");
		gameController.led_04_red 		= EditorGUILayout.IntField("  - Red\t: "	, gameController.led_04_red);
		gameController.led_04_green 	= EditorGUILayout.IntField("  - Green\t: "	, gameController.led_04_green);
		gameController.led_04_blue 		= EditorGUILayout.IntField("  - Blue\t: "	, gameController.led_04_blue);

		EditorGUILayout.LabelField("");
	}
}
