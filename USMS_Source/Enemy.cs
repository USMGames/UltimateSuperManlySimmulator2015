using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace USMS_Source
{
	public class Enemy
	{
		//Private variables.
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static bool			alive;
		private static float 		spriteWidth;
		private static float		spriteHeight;
		private static bool			isActive;
		private static Vector2      direction;
		private static Vector2		enemyDirection;
		
		public float PositionX{ get{return sprite.Position.X;}}
		public float PositionY{ get{return sprite.Position.Y; }}
		public Vector2 Position{ get{return sprite.Position;} set {sprite.Position = value;}}
		public float SpriteWidth{ get{return spriteWidth;}}
		public float SpriteHeight{ get{return spriteHeight;}}
		public Vector2 Direction{ get{return direction;} set{ direction = value; }}
		public bool IsActive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Enemy (Scene scene)
		{
			Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand = 
				new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			textureInfo  = new TextureInfo("/Application/textures/enemy.gif");
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			//sprite.Position = new Vector2(150.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			
			//sprite.Position = new Vector2((float)(Rand.NextDouble()*Director.Instance.GL.Context.GetViewport().Width), (float)(Rand.NextDouble()*Director.Instance.GL.Context.GetViewport().Height));
			sprite.Position = new Vector2(rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Width - 10), 
			                              rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Height - 10));
			alive = true;
			direction = new Vector2(0.0f,0.0f);
			
			// Create a parameter bounding box around the enemy to detect the player.
			Bounds2 enem = sprite.Quad.Bounds2();
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{		
			
		}	
		
		public void ChasePlayer(Player player, float deltaTime)
		{
			//Console.WriteLine("deltaTime = " + deltaTime);
			// Follow player
			//if((sprite.Position.Y + SpriteHeight) < Director.Instance.GL.Context.GetViewport().Height - 4)
			//{
			
			const float kEnemySpeed = 0.01f;
			
			// If the player is further North-East than the Enemy, move the enemy North-East.
			if(player.Position.X > sprite.Position.X && player.Position.Y > sprite.Position.Y)	
			{
				//sprite.Position = new Vector2(sprite.Position.X + (kEnemySpeed * deltaTime), sprite.Position.Y + (kEnemySpeed * deltaTime));
				enemyDirection = new Vector2(0.5f,0.5f);
			}
			
			// If the player is further North-West than the Enemy, move the enemy North-West.
			if(player.Position.X < sprite.Position.X && player.Position.Y > sprite.Position.Y)	
			{
				//sprite.Position = new Vector2(sprite.Position.X + (kEnemySpeed * deltaTime), sprite.Position.Y + (kEnemySpeed * deltaTime));
				enemyDirection = new Vector2(-0.5f,0.5f);
			}
			
			// If the player is further South-West than the Enemy, move the enemy South-West.
			if(player.Position.X < sprite.Position.X && player.Position.Y < sprite.Position.Y)	
			{
				//sprite.Position = new Vector2(sprite.Position.X + (kEnemySpeed * deltaTime), sprite.Position.Y + (kEnemySpeed * deltaTime));
				enemyDirection = new Vector2(-0.5f,-0.5f);
			}
			
			// If the player is further South-East than the Enemy, move the enemy South-East.
			if(player.Position.X > sprite.Position.X && player.Position.Y < sprite.Position.Y)	
			{
				//sprite.Position = new Vector2(sprite.Position.X + (kEnemySpeed * deltaTime), sprite.Position.Y + (kEnemySpeed * deltaTime));
				enemyDirection = new Vector2(0.5f,-0.5f);
			}
			
//			// If the player is further North than the Enemy, move the enemy North.
//			if(player.Position.Y > sprite.Position.Y)
//			{
//				playerDirection = new Vector2(0.0f,1.0f);
//			}
//			// If the player is further East than the Enemy, move the enemy East.
//			if(player.Position.X > sprite.Position.X)
//			{
//				playerDirection = new Vector2(1.0f,0.0f);
//			}
//			// If the player is further West than the Enemy, move the enemy West.
//			if(player.Position.X < sprite.Position.X)
//			{
//			    playerDirection = new Vector2(-1.0f, 0.0f);
//			}
//			// If the player is further South than the Enemy, move the enemy South.
//			if(player.Position.Y < sprite.Position.Y)
//			{
//			    playerDirection = new Vector2(0.0f, -1.0f);
//			}
			
			sprite.Position += enemyDirection * deltaTime * kEnemySpeed;
			//}
		}
	}
}


