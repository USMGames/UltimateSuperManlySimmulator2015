using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace USMS_Source
{
	public class Player
	{
		//Private variables.
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static int			pushAmount = 100;
		private static float		yPositionBeforePush;
		private static bool			rise;
		private static float		angle;
		private static bool			alive;
		private static float 		spriteWidth;
		private static float		spriteHeight;
		private static bool			isActive;
		private static Vector2      direction;
		
		public float PositionX{ get{return sprite.Position.X;}}
		public float PositionY{ get{return sprite.Position.Y; }}
		public Vector2 Position{ get{return sprite.Position;} set {sprite.Position = value;}}
		public float SpriteWidth{ get{return spriteWidth;}}
		public float SpriteHeight{ get{return spriteHeight;}}
		public Vector2 Direction{ get{return direction;} set{ direction = value; }}
		
		public bool IsActive { get{return alive;} set{alive = value;} }
		
		// Sets up our game pad
		GamePadData gamePadData;
		public GamePadData PadData 
		{ 
			get { return gamePadData;}
		}
		
		//Accessors.
		//public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Player (Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/ManLEE front.png");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);	
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(50.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			//sprite.Pivot 	= new Vector2(0.5f,0.5f);
			angle = 0.0f;
			rise  = false;
			alive = true;
			//isActive = true;
			spriteHeight = 31.0f;
			spriteWidth = 21.0f;
			//Add to the current scene.
			direction = new Vector2(1.0f,0.0f);
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{		
			
			gamePadData = GamePad.GetData(0);
			
			// If we move left
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				// Changes which way the player is facing.
				direction = new Vector2(-1.0f,0.0f);
				// If the sprite is within the left of the screen, move. Else, do not move.
				if(sprite.Position.X > 3)
				{
					sprite.Position = new Vector2(sprite.Position.X - 3, sprite.Position.Y);
				}
			}
			
			// If we move right
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				direction = new Vector2(1.0f,0.0f);
				// If the sprite is within the right of the screen, move. Else, do not move.
				if((sprite.Position.X + SpriteWidth) < (Director.Instance.GL.Context.GetViewport().Width - 3))
				{
					sprite.Position = new Vector2(sprite.Position.X + 3, sprite.Position.Y);
				}
			}
			
			// If we move up
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				direction = new Vector2(0.0f,1.0f);
				if((sprite.Position.Y + SpriteHeight) < Director.Instance.GL.Context.GetViewport().Height - 4)
				{
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 3);
				}
			}
			
			// If we move down
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				direction = new Vector2(0.0f,-1.0f);
				if((sprite.Position.Y) > 4)
				{
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 3);
				}
			}
		}	
		
//		public void Tapped()
//		{
//			if(!rise)
//			{
//				rise = true;
//				yPositionBeforePush = sprite.Position.Y;
//			}
//		}
	}
}


