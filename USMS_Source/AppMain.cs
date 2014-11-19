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
		const float kBulletDelay = 0.01f;
		
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Obstacle[]	 obstacles;
		private static Player			player;
		private static Background	background;
		private static Bullet[]		bulletList;
		private static Enemy[]		   enemies;
		private static double 	  MS_PER_FRAME;
		private static bool			 timerDone;
		private static float 		 bulletTimer;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer timer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();
		private static float		previousTime;
		
		private static GamePadData gamePadData;
		public static GamePadData PadData 
		{ 
			get { return gamePadData;}
		}
		
		
				
		public static void Main (string[] args)
		{
			Initialize();
			
		
			
			//Game loop
			bool quitGame = false;
			bool timerDone = true;
			while (!quitGame) 
			{
//				Stopwatch timer = Stopwatch.StartNew();
//				double start = timer.ElapsedMilliseconds;
				Update();
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
				
				//Thread.Sleep ((int)(start + MS_PER_FRAME) - (int)(timer.ElapsedMilliseconds));
			}
			
			//Clean up after ourselves.
			player.Dispose();
			foreach(Obstacle obstacle in obstacles)
				obstacle.Dispose();
			background.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			bulletTimer = kBulletDelay;
			previousTime = 0.0f;
			MS_PER_FRAME = 16.6666666667;
			
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.Text = "0";
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the flappy douche
			player = new Player(gameScene);
			
			// Create the bullet array
			bulletList = new Bullet[GameConstants.NumBullets];
			
			// Create the enemy array
			enemies = new Enemy[GameConstants.EnemyCount];
			
			for (int i = 0; i < GameConstants.NumBullets; i++)
			{
				bulletList[i] = new Bullet(gameScene, player);
			}
			
			// Create the enemies
			for(int i = 0; i < GameConstants.EnemyCount; i++)
			{
				enemies[i] = new Enemy(gameScene);
			}
			
			//Create some obstacles.
//			obstacles = new Obstacle[2];
//			obstacles[0] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width*0.5f, gameScene);	
//			obstacles[1] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width, gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			
			
		}
		
		public static void Update()
		{
			float currentTime = (float)timer.Milliseconds();
			float deltaTime = currentTime-previousTime;
			previousTime = currentTime;
			
			// Gets the time between the current frame, and the last frame.
			//float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			//Console.WriteLine("timeDelta = " + timeDelta);
			
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			//If tapped, inform the player.
//			if(touches.Count > 0)
//				player.Tapped();
//			
			//Update the player.
			player.Update(deltaTime);
			
			if(player.IsActive)
			{
				//Move the background.
				background.Update(deltaTime);
			
				// Update the bullets
				for (int i = 0; i < GameConstants.NumBullets; i++)
	            {
	                if (bulletList[i].IsActive)
	                {
						bulletList[i].Update(deltaTime);
	                }
	            }
					
				Console.WriteLine("Width: " + Director.Instance.GL.Context.GetViewport().Width + " Height: " + Director.Instance.GL.Context.GetViewport().Height);
			
//				// Bullet-enemies collision check
//	            for (int i = 0; i < GameConstants.NumBullets; i++)
//	            {
//	                if (enemies[i].IsActive)
//	                {			
//						//Bounds2 enem = enemies[i].Quad.S.Bounds2();
//						Bounds2 enem = new Bounds2(enemies[i].Position);
//						float enemiesWidth  = enem.Point10.X;
//						float enemiesHeight = enem.Point01.Y;
//				
//						float enemiesLeft = enemies[i].Position.X;
//						float enemiesRight = enemies[i].Position.X + (enemiesWidth);
//						float enemiesBottom = enemies[i].Position.Y;
//						float enemiesTop = enemiesBottom + enemiesHeight;
//							
//	                    for (int j = 0; j < GameConstants.NumBullets; j++)
//	                    {
//	                        if (bulletList[j].IsActive)
//	                        {
//								//Bounds2 bull = bulletList[j].Quad.Bounds2();
//								Bounds2 bull = new Bounds2(bulletList[j].Position);
//								float bulletWidth  = bull.Point10.X;
//								float bulletHeight = bull.Point01.Y;
//				
//								float bulletLeft = bulletList[i].Position.X;
//								float bulletRight = bulletList[i].Position.X + (bulletWidth);
//								float bulletBottom = bulletList[i].Position.Y;
//								float bulletTop = bulletBottom + bulletHeight;	
//									
//	                            if ( (bulletBottom < enemiesTop) && (bulletTop > enemiesBottom) &&
//					     (bulletRight > enemiesLeft) && (bulletLeft < enemiesRight) )
//	                            {
//	                                //soundBank.PlayCue("explosion2");
//	                                //enemiesList[i].IsActive = false;
//	                                bulletList[j].IsActive = false;
//	                                //score += GameConstants.KillBonus;
//	                                break; //no need to check other bullets
//	                            }
//	                        }
//	                    }
//	                }
//	            }
					
				// Are we shooting?
				bulletTimer += deltaTime;
				if(Input2.GamePad0.Cross.Press)
	            {
					//if(Bullet.BulletTimer.)
					//{
						// start it
					//}
					
					if(bulletTimer >= kBulletDelay)
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
					

	                //add another bullet.  Find an inactive bullet slot and use it
	                //if all bullets slots are used, ignore the user input
	            }
				
				// Check for bullet collision with enemy
//				for(int i = 0; i < GameConstants.NumBullets; i++)
//				{
//					if(bulletList[i].IsActive)
//					{
//							
//					}
//				}
				
				// Check for bullet collision with enemy
				
				
				for(int i = 0; i < GameConstants.EnemyCount; i++)
				{
					Console.WriteLine("Inside ChasePlayer For Loop. I = " + i);
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
					//break;
					Console.WriteLine ("Enemy " + i + " moved!");
				}
            }
							
				//Update the obstacles.
//				foreach(Obstacle obstacle in obstacles)
//					obstacle.Update(0.0f);
			//}
			
			
			
		}
		
	}
}
