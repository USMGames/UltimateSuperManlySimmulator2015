using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace USMS_Source
{
	public class Bullet
	{
		// Private variables
		private Vector2 position;
		private Vector2 direction;
		private float speed;
		private bool isActive;
		private float 		spriteWidth;
		private float		spriteHeight;
		private SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
		
		public bool IsActive { get{ return isActive; } set{ isActive = value; } }
		public Vector2 Position { get{return sprite.Position;} }
		
		public Bullet(Scene scene, Player player)
		{
			rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			
			textureInfo  = new TextureInfo("/Application/textures/bullet.gif");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			
			// Initialise variables
			isActive = false;
			speed = 0.5f;
			//damage = 50.0f;
			direction = player.Direction;
			
			// Initialise the bulletTimer
			//Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer bulletTimer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();
			
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Activate(Vector2 newDirection, Vector2 newPosition)
		{
				isActive = true;
				direction = newDirection;
				sprite.Position = newPosition;
		}
		
		public void Update(float deltaTime)
		{
			// deltaTime causes the sprite not to draw
			sprite.Position += direction * deltaTime * speed;
			
				// If the bullet goes off the edge of the screen, set it to inactive.
				if(sprite.Position.X > Director.Instance.GL.Context.GetViewport().Width |
			   	   sprite.Position.X < -20 |
			   	   sprite.Position.Y > Director.Instance.GL.Context.GetViewport().Height |
			   	   sprite.Position.Y < -10)
				{
					isActive = false;
					
				}
		}
		
		public void BulletCollision(Enemy enemy)
		{
			Bounds2 bulletBounds = new Bounds2();
			Bounds2 enemyBounds = new Bounds2();
			
			sprite.GetContentWorldBounds(ref bulletBounds);
			enemy.Sprite.GetContentWorldBounds(ref enemyBounds);
			
			if(bulletBounds.Overlaps(enemyBounds))
        	{
          		enemy.Position = new Vector2(rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Width - textureInfo.TextureSizef.X), 
			                              rand.NextFloat(0, Director.Instance.GL.Context.GetViewport().Height - textureInfo.TextureSizef.Y));		
//          		player.Sprite.Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
//                                    Director.Instance.GL.Context.GetViewport().Height/2);
        	}
		}
		
		
	}
}

