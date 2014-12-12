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
		private Vector2 	direction;
		private float 			speed;
		private bool 		 isActive;
		private SpriteUV 	   sprite;
		private static TextureInfo	textureInfo;
		private Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator rand;
		
		// Accessors
		public bool IsActive    { get{ return isActive; } set{ isActive = value; } }
		public Vector2 Position { get{return sprite.Position;} set{sprite.Position = value;}}
		
		public Bullet(Scene scene, Player player)
		{
			rand = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.RandGenerator(DateTime.Now.Millisecond);
			
			textureInfo  = new TextureInfo("/Application/textures/Bullet.png");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			
			// Initialise variables
			isActive  = false;
			speed     = 0.5f;
			//damage  = 50.0f;
			direction = player.Direction;
			
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
		
		public void StartPosition(Player player, Vector2 newPosition)
		{
				if(player.SPlayerDir == "Left")
				{
 					//sprite.Position.X = 0.0f;//set position of bullet to his hands on the left hand side
					//sprite.Position.Y = 0.5f;
					Vector2 bla = new Vector2(newPosition.X, newPosition.Y + 0.5f);
					sprite.Position = bla;
				}
		}
		
		public void Update(float deltaTime)
		{
			if(isActive)
			{
				// See which way ManLEE is facing and set the bullet to the correct position:
				
				
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
			else
			{
				sprite.Position = new Vector2(-999.0f, -999.0f);
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
				isActive = false;
        	}
		}
		
		
	}
}

