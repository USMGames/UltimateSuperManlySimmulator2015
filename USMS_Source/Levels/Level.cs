using System;
//using TiledMax;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace USMS_Source
{
	public class Level
	{
		//private Map 			map;
		private Player			player;
		private static Bullet[]	bulletList;
		private static Enemy[]	enemies;
		private static float	previousTime;
		private static float 	bulletTimer;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer timer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();

		public Level (Scene gameScene, String fileName)
		{
			Initialize(gameScene);
			
			//this.map = Map.Open(fileName);
			//if(map.Loaded.Successful)
			//{
			//	drawMap(map);
			//}
		}
		
		public void Initialize(Scene gameScene)
		{
			bulletTimer = GameConstants.BulletDelay;
			
			//Create the player
			player = new Player(gameScene);
			
			// Create the bullet array
			bulletList = new Bullet[GameConstants.NumBullets];
			
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

		}
		
		public void Update()
		{
			float currentTime = (float)timer.Milliseconds();
			float deltaTime = currentTime-previousTime;
			previousTime = currentTime;
			
			//Update the player.
			player.Update(deltaTime);
			
			if(player.IsActive)
			{	
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
								//bulletList[i].Activate(player.Direction);
								bulletList[i].StartPosition(player, player.Position);
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
		
		
		//public void drawMap(Map map)
		//{
		//	Console.WriteLine("Map Loaded!");
		//}
		
	}
}

