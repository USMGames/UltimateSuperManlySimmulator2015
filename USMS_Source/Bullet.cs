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
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer bulletTimer = new Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer();
		
		public bool IsActive { get{ return isActive; } }
		public static Sce.PlayStation.HighLevel.GameEngine2D.Base.Timer BulletTimer { get{ return bulletTimer; }}
		
		public Bullet(Scene scene, Player player)
		{
			textureInfo  = new TextureInfo("/Application/textures/bullet.gif");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			//sprite.Position = new Vector2(1.0f,0.0f);
			//spriteHeight = 31.0f;
			//spriteWidth = 21.0f;
			
			// Initialise variables
			isActive = false;
			speed = 500.0f;
			//position = new Vector2(0.0f, 0.0f);
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
			sprite.Position += direction * (deltaTime * speed);
			
				// If the bullet goes off the edge of the screen, set it to inactive.
				if(sprite.Position.X > Director.Instance.GL.Context.GetViewport().Width |
			   	   sprite.Position.X < -20 |
			   	   sprite.Position.Y > Director.Instance.GL.Context.GetViewport().Height |
			   	   sprite.Position.Y < -10)
				{
					isActive = false;
				}
		}
		
		
	}
}

