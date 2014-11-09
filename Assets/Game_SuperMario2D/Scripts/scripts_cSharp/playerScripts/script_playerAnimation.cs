using UnityEngine;
using System.Collections;

public static class script_playerAnimation
{
	#region							Player Animation Functions

	public static void 			dead_animation 					( CharacterController playerController )
	{
		script_aniSprite.aniSprite( playerController, 16, 16, 0, 6, 1, 1);
	}

	public static void			idle_animation					(ref CharacterController playerController, float moveDirection)
	{
		if (moveDirection == 1)												// sets player animation to idle right
		{
			script_aniSprite.aniSprite( playerController, 16, 16, 0, 0, 1, 1);
		}
		
		if (moveDirection == -1)												// sets player animation to idle left
		{
			script_aniSprite.aniSprite( playerController, 16, 16, 0, 1, 1, 1);
		}
	}
	
	public static void			walk_animation					(ref CharacterController playerController, ref Vector3 velocity)
	{
		if (velocity.x < 0)																		// sets player animation to walk left
		{								
			script_aniSprite.aniSprite( playerController, 16, 16, 0, 3, 3, 9);
		}
		
		if (velocity.x > 0)																		// sets player animation to walk right
		{
			script_aniSprite.aniSprite( playerController, 16, 16, 0, 2, 3, 9);
		}
	}
	
	public static void			run_animation					(ref CharacterController playerController, ref Vector3 velocity)
	{
		if (velocity.x < 0  && Input.GetButton ("Fire1"))										// sets player animation to run left()
		{					
			script_aniSprite.aniSprite( playerController, 16, 16, 0, 3, 3, 15);
		}
		
		if (velocity.x > 0 && Input.GetButton ("Fire1"))										// sets player animation to run right
		{
			script_aniSprite.aniSprite( playerController,  16, 16, 0, 2, 3, 15);
		}
	}
	
	public static void			crouch_animation				(ref CharacterController playerController, ref Vector3 velocity, float moveDirection)
	{
		if	( velocity.x == 0 && Input.GetAxis ("Vertical") < 0)
		{
			if (moveDirection == -1)														// player is facing left
			{
				script_aniSprite.aniSprite( playerController, 16, 16, 0, 9, 1, 1);				// sets player animation to crouch left
			}
			if (moveDirection == 1)															// player is facing right
			{
				script_aniSprite.aniSprite( playerController, 16, 16, 0, 8, 1, 1);				// sets player animation to crouch right
			}
		}
	}
	
	public static void			jump_animation					(ref CharacterController playerController, ref Vector3 velocity, float moveDirection)
	{
		if (moveDirection == -1)														// use a jump animation facing the left
		{
			if (velocity.x == 0 && Input.GetAxis("Vertical") < 0)					// use the left crouch jump animation
			{
				script_aniSprite.aniSprite(playerController, 16, 16, 12, 11, 1, 1);
			}
			else
			{
				script_aniSprite.aniSprite( playerController, 16, 16, 11, 3, 4, 12);			// use the left normal jump animtion
			}
		}
		
		if (moveDirection == 1)															// use a jump animation facing the right
		{
			if (velocity.x == 0 && Input.GetAxis("Vertical") < 0)					// use the right crouch jump animation
			{
				script_aniSprite.aniSprite(playerController, 16, 16, 12, 10, 1, 1);
			}
			else
			{
				script_aniSprite.aniSprite( playerController, 16, 16, 11, 2, 4, 12);			// use the right normal jump animtion
			}
		}		
	}
	
	#endregion
}