using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace USMS_Source
{
	public class AppMain
	{	
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		
		//private static Obstacle[]	 obstacles;
		private static Player			player;
		private static Background	background;
		private static Bullet[]		bulletList;
		private static Enemy[]		   enemies;
		private static float 		 bulletTimer;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer timer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();
		private static float		previousTime;
				
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update();
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			player.Dispose();
			background.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			bulletTimer = GameConstants.BulletDelay;
			previousTime = 0.0f;
			
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the player
			player = new Player(gameScene);
			
			// Create the bullet array
			bulletList = new Bullet[GameConstants.NumBullets];
			
			
			//Test map loading
			Level level = new Level("maps/desert.tmx");
			
			
			// Create the bullets
			for (int i = 0; i < GameConstants.NumBullets; i++)
			{
				bulletList[i] = new Bullet(gameScene, player);
			}
			
			// Create the enemy array
			enemies = new Enemy[GameConstants.EnemyCount];
			
			// Create the enemies
			for(int i = 0; i < GameConstants.EnemyCount; i++)
			{
				enemies[i] = new Enemy(gameScene);
			}
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			
			
		}
		
		public static void Update()
		{
			float currentTime = (float)timer.Milliseconds();
			float deltaTime = currentTime-previousTime;
			previousTime = currentTime;
			
			//Update the player.
			player.Update(deltaTime);
			
			if(player.IsActive)
			{
				//Move the background.
				background.Update(deltaTime);
			
				// Update the bullets
				for (int i = 0; i < GameConstants.NumBullets; i++)
	            {
					bulletList[i].Update(deltaTime);
	            }
					
				// Are we shooting?
				bulletTimer += deltaTime;
				if(Input2.GamePad0.Cross.Press)
	            {	
					if(bulletTimer >= GameConstants.BulletDelay)
					{
						for(int i = 0; i < GameConstants.NumBullets; i++)
						{
							if (!bulletList[i].IsActive)
							{
								bulletList[i].Activate(player.Direction, player.Position);
								bulletTimer = 0.0f;
								break;
							}
						}
					}
	            }
				
				for(int i = 0; i < GameConstants.EnemyCount; i++)
				{
					enemies[i].ChasePlayer(player, deltaTime);
					// Check for Player collision with Enemy
					enemies[i].Collision(player);
					// Check for Bullet collision with Enemy
					for(int j = 0; j < GameConstants.NumBullets; j++)
					{
						if(bulletList[j].IsActive)
						{
							bulletList[j].BulletCollision(enemies[i]);
						}
					}
				}
            }	
		}
		
	}
}
