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
		private static Vector2      playerDirection;
		private static Vector2      bulletDirection;
		private static float 		speed;
		
		public float PositionX{ get{return sprite.Position.X;}}
		public float PositionY{ get{return sprite.Position.Y; }}
		public Vector2 Position{ get{return sprite.Position;} set {sprite.Position = value;}}
		public float SpriteWidth{ get{return spriteWidth;}}
		public float SpriteHeight{ get{return spriteHeight;}}
		public Vector2 Direction{ get{return bulletDirection;} set{ bulletDirection = value; }}
		
		public bool IsActive { get{return alive;} set{alive = value;} }
		
		public SpriteUV Sprite {get{return sprite;}}
		
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
			alive = true;
			bulletDirection = new Vector2(1.0f, 0.0f);
			playerDirection = new Vector2(0.0f, 0.0f);
			speed = 0.2f;
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{		
			playerDirection = new Vector2(0.0f, 0.0f);
			
			gamePadData = GamePad.GetData(0);
			
			// If we move left
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				// Changes which way the player is facing.
				playerDirection = new Vector2(-1.0f,0.0f);
				bulletDirection = new Vector2(-1.0f,0.0f);
				// If the sprite is within the left of the screen, move. Else, do not move.
				if(sprite.Position.X < 0)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move right
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerDirection = new Vector2(1.0f,0.0f);
				bulletDirection = new Vector2(1.0f,0.0f);
				// If the sprite is within the right of the screen, move. Else, do not move.
				if((sprite.Position.X + sprite.TextureInfo.TileSizeInPixelsf.X) > (Director.Instance.GL.Context.GetViewport().Width))
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move up
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				playerDirection = new Vector2(0.0f,1.0f);
				bulletDirection = new Vector2(0.0f,1.0f);
				
				if((sprite.Position.Y + sprite.TextureInfo.TileSizeInPixelsf.Y) > Director.Instance.GL.Context.GetViewport().Height)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move down
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				playerDirection = new Vector2(0.0f,-1.0f);
				bulletDirection = new Vector2(0.0f,-1.0f);
				
				if((sprite.Position.Y) < 0)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move north-east
			if((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerDirection = new Vector2(0.5f,0.5f);
				bulletDirection = new Vector2(0.5f,0.5f);
				
				if((sprite.Position.Y + sprite.TextureInfo.TileSizeInPixelsf.Y) > (Director.Instance.GL.Context.GetViewport().Height) 
				  || (sprite.Position.X + sprite.TextureInfo.TileSizeInPixelsf.X) > (Director.Instance.GL.Context.GetViewport().Width))
				{
					Console.WriteLine("Out");
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			// North-west
			if((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				playerDirection = new Vector2(-0.5f,0.5f);
				bulletDirection = new Vector2(-0.5f,0.5f);
				
				if((sprite.Position.Y + sprite.TextureInfo.TileSizeInPixelsf.Y) > (Director.Instance.GL.Context.GetViewport().Height) 
				  || (sprite.Position.X) < 0)
				{
					Console.WriteLine("Out");
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			// South-west
			if((gamePadData.Buttons & GamePadButtons.Down) != 0 && (gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				playerDirection.X = -0.5f;
				playerDirection.Y = -0.5f;
				bulletDirection.X = -0.5f;
				bulletDirection.Y = -0.5f;
				
				if((sprite.Position.Y) < 0 
				  || (sprite.Position.X) < 0)
				{
					Console.WriteLine("Out");
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			// South-east
			if((gamePadData.Buttons & GamePadButtons.Down) != 0 && (gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerDirection.X = 0.5f;
				playerDirection.Y = -0.5f;
				bulletDirection.X = 0.5f;
				bulletDirection.Y = -0.5f;
				
				if((sprite.Position.Y) < 0 
				  || (sprite.Position.X + sprite.TextureInfo.TileSizeInPixelsf.X) > Director.Instance.GL.Context.GetViewport().Width)
				{
					Console.WriteLine("Out");
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			sprite.Position += playerDirection * deltaTime * speed;
			
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


