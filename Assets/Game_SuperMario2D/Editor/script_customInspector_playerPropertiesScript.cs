using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(script_playerProperties))]
public class script_customInspector_playerPropertiesScript : Editor 
{
	public override void OnInspectorGUI()
	{
		script_playerProperties playerProperties = (script_playerProperties)target;
		
		EditorGUILayout.HelpBox("[PlayerProperties script] \nDescription: Controls all the assets associated with the character. \n- Assign the assets to the various variables below.", MessageType.Info );
		
		DrawDefaultInspector();

		//EditorGUILayout.Space();

		EditorGUILayout.LabelField("");
	}
}
