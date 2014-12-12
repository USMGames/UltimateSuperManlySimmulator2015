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
		private SpriteUV 		sprite;
		private TextureInfo		textureInfo;
		private bool			isActive;
		private Vector2			enemyDirection;
		private Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
		
		//Accessors.
		public SpriteUV Sprite   { get{return sprite;} }
		public Vector2 Position  { get{return sprite.Position;} set {sprite.Position = value;}}
		public bool IsActive     { get{return isActive;} set{isActive = value;} }
		
		//Public functions.
		public Enemy (Scene scene)
		{
			rand            = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			textureInfo     = new TextureInfo("/Application/textures/EnemySprite.png");
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Width - textureInfo.TextureSizef.X), 
			                              rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Height - textureInfo.TextureSizef.Y));
			isActive = true;
			
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
			// If the player is further North-East than the Enemy, move the enemy North-East.
			if(player.Position.X > sprite.Position.X && player.Position.Y > sprite.Position.Y)	
			{
				enemyDirection = new Vector2(0.5f,0.5f);
			}
			
			// If the player is further North-West than the Enemy, move the enemy North-West.
			if(player.Position.X < sprite.Position.X && player.Position.Y > sprite.Position.Y)	
			{
				enemyDirection = new Vector2(-0.5f,0.5f);
			}
			
			// If the player is further South-West than the Enemy, move the enemy South-West.
			if(player.Position.X < sprite.Position.X && player.Position.Y < sprite.Position.Y)	
			{
				enemyDirection = new Vector2(-0.5f,-0.5f);
			}
			
			// If the player is further South-East than the Enemy, move the enemy South-East.
			if(player.Position.X > sprite.Position.X && player.Position.Y < sprite.Position.Y)	
			{
				enemyDirection = new Vector2(0.5f,-0.5f);
			}
			
			sprite.Position += enemyDirection * deltaTime * GameConstants.EnemySpeed;
		}
		
		public void Collision(Player player)
		{
			Bounds2 playerBounds = new Bounds2();
			Bounds2 enemyBounds  = new Bounds2();
			
			sprite.GetContentWorldBounds(ref enemyBounds);
			player.Tiles.GetContentWorldBounds(ref playerBounds);
			
			if(playerBounds.Overlaps(enemyBounds))
        	{
          		Position = new Vector2(rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Width - textureInfo.TextureSizef.X), 
			                           rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Height - textureInfo.TextureSizef.Y));
        	}

		}
	}
}


