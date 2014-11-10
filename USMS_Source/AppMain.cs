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
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Obstacle[]	 obstacles;
		private static Player			player;
		private static Background	background;
		private static Bullet[]		bulletList;
		private static double 	  MS_PER_FRAME;
		
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
			while (!quitGame) 
			{
//				Stopwatch timer = Stopwatch.StartNew();
//				double start = timer.ElapsedMilliseconds;
				
				Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer timer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();
				
				Update ((float)timer.Milliseconds());
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
				
				timer.Reset();
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
			
			for (int i = 0; i < GameConstants.NumBullets; i++)
			{
				bulletList[i] = new Bullet(gameScene);
			}
			
			//Create some obstacles.
//			obstacles = new Obstacle[2];
//			obstacles[0] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width*0.5f, gameScene);	
//			obstacles[1] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width, gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
			
			
		}
		
		public static void Update(float deltaTime)
		{
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
				
			// Bullet-Enemy collision check
//            for (int i = 0; i < enemyList.Length; i++)
//            {
//                if (enemyList[i].isActive)
//                {			
//					Bounds2 enem = enemyList[i].Quad.Bounds2();
//					float enemyWidth  = enem.Point10.X;
//					float enemyHeight = enem.Point01.Y;
//			
//					float enemyLeft = enemyList[i].Position.X + (enemyWidth);
//					float enemyRight = enemyList[i].Position.X + (enemyWidth);
//					float enemyBottom = enemyList[i].Position.Y;
//					float enemyTop = enemyBottom + enemyHeight;
//						
//                    for (int j = 0; j < bulletList.Length; j++)
//                    {
//                        if (bulletList[j].isActive)
//                        {
//							Bounds2 bull = bulletList[j].Quad.Bounds2();
//							float bulletWidth  = bull.Point10.X;
//							float bulletHeight = bull.Point01.Y;
//			
//							float bulletLeft = bulletList[i].Position.X + (bulletWidth);
//							float bulletRight = bulletList[i].Position.X + (bulletWidth);
//							float bulletBottom = bulletList[i].Position.Y;
//							float bulletTop = bulletBottom + bulletHeight;	
//								
//                            if ( (bulletBottom < enemyTop) && (bulletTop > enemyBottom) &&
//				     (bulletRight > enemyLeft) && (bulletLeft < enemyRight) )
//                            {
//                                //soundBank.PlayCue("explosion2");
//                                enemyList[i].isActive = false;
//                                bulletList[j].isActive = false;
//                                //score += GameConstants.KillBonus;
//                                break; //no need to check other bullets
//                            }
//                        }
//                    }
//                }
//            }
				
			// Are we shooting?
			if(player.IsActive && Input2.GamePad0.Cross.Down)
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumBullets; i++)
                {
                    if (!bulletList[i].IsActive)
                    {
							// bulletList[i].Activate(vector2 direction);
							 bulletList[i].Activate(player.Direction, player.Position);
//                        bulletList[i].direction = player.RotationMatrix.Forward;
//                        bulletList[i].speed = GameConstants.BulletSpeedAdjustment;
//                        bulletList[i].position = player.Position +
//                  		(200 * bulletList[i].direction);
//                        bulletList[i].isActive = true;
                        //score -= GameConstants.ShotPenalty;
                        //soundBank.PlayCue("tx0_fire1");
							Console.WriteLine("i = " + i);

                        break; //exit the loop     
                    }
						  //break; //exit the loop  
                }
            }
        }
							
				//Update the obstacles.
//				foreach(Obstacle obstacle in obstacles)
//					obstacle.Update(0.0f);
			//}
			
			
			
		}
		
	}
}
