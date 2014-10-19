// Animation Sprite Sheet 
//
// Description: Plays an animated sprite using a sprite sheet
// Instruction: Assign script to a gameObject with a material/texture (sprite sheet) 
// Function arguments: 
// columnSize      - number of frames across (horizontal)
// rowSize         - number of frames down (vertical)
// colFrameStart   - where frame starts (remember 0 is first number in counting)
// rowFrameStart   - where frame starts (remember 0 is first number in counting)
// totalFrames     - number of frames in the animation (count regular)
// framesPerSecond - how fast do you want it to play through (Standard: 12 - 30 fps)

using UnityEngine;
using System.Collections;

public class script_aniSprite : MonoBehaviour 
{
	public void aniSprite ( int columnSize, int rowSize, int colFrameStart, int rowFrameStart, int totalFrames, int framesPerSecond )	// function for animating sprites
	{
		int index = (int)Time.time * framesPerSecond;													// time control fps
		index = index % totalFrames;																	// modulate to total number of frames

		Vector2 size = new Vector2 ( 1.0f / columnSize, 1.0f / rowSize);											// scale for column and row size
		
		var u = index % columnSize;																		// u gets current x coordinate from column size
		var v = index / columnSize;																		// v gets current y coordinate by dividing by column size
		
		Vector2 offset = new Vector2 ((u + colFrameStart) * size.x, (1.0f - size.y) - (v + rowFrameStart) * size.y); // offset equals column and row
		
		renderer.material.mainTextureOffset = offset;													// texture offset for diffuse map
		renderer.material.mainTextureScale  = size;														// texture scale  for diffuse map
		
		//renderer.material.SetTextureOffset ("_BumpMap", offset);										// texture offset for bump (normal map)
		//renderer.material.SetTextureScale  ("_BumpMap", size);											// texture scale  for bump (normal map) 
	}
}
