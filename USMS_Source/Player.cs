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
		
		// For Spritesheet
		private static Texture2D 	m_texture;
		private static SpriteTile	tiles;
		private static TextureInfo	textureInfo;
		private static int 			numIndices = 2;
		private static int          index = 0;
		private static Vector2      sPosition;
		
		
		// Other
		//private static SpriteUV 	tiles;
		private static bool			isActive;
		private static Vector2      playerDirection;
		private static Vector2      bulletDirection;
		private static float 		speed;
		private static string 		sPlayerDir;
		private static int 			pHealth;
		
		// Accessors
		//public Vector2 Position  { get{return tiles.Position;} set {tiles.Position = value;}}
		public Vector2 Position  { get{return tiles.Position;} set {tiles.Position = value;}}
		public Vector2 Direction { get{return bulletDirection;} set{ bulletDirection = value; }}
		public bool IsActive     { get{return isActive;} set{isActive = value;} }
		//public SpriteUV Sprite   { get{return tiles;} }
		public SpriteTile Tiles   { get{return tiles;} }
		public string SPlayerDir { get{return sPlayerDir;}}
		
		
		// Sets up our game pad
		GamePadData gamePadData;
		public GamePadData PadData 
		{ 
			get { return gamePadData;}
		}
		
		//Public functions.
		public Player (Scene scene)
		{
			m_texture = new Texture2D("/Application/textures/ManLeeSpriteSheet.png", false);
			textureInfo = new TextureInfo(m_texture, new Vector2i(numIndices, 4)); // numIndices is the column, 1 is the rows
			tiles = new SpriteTile(textureInfo);
			tiles.Quad.S = new Vector2(textureInfo.TextureSizef.X / (float)numIndices, textureInfo.TextureSizef.Y / 4); // Bounding box of which tiles to use
			tiles.TileIndex2D = new Vector2i(0, 0);
			tiles.CenterSprite();
			//textureInfo     = new TextureInfo("/Application/textures/ManLEE front.png");
			//textureInfo     = new TextureInfo("/Application/textures/ManLeeSpriteSheet.png");
//			tiles	 		= new SpriteUV();
//			tiles 			= new SpriteUV(textureInfo);
//			tiles.Quad.S 	= textureInfo.TextureSizef;
			tiles.Position = new Vector2(50.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			isActive 		= true;
			bulletDirection = new Vector2(1.0f, 0.0f);
			playerDirection = new Vector2(0.0f, 0.0f);
			
			speed 			= 0.2f;
			pHealth = 100;
			//Add to the current scene.
			//scene.AddChild(tiles);
			scene.AddChild (tiles);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		
					
		public int getPlayerHealth()
		{
			return pHealth;
		}
			
		public void setPlayerHealth(int health)
		{
			pHealth = health;
		}
		
		
		public void Update(float deltaTime)
		{		
			playerDirection = new Vector2(0.0f, 0.0f);
			
			gamePadData = GamePad.GetData(0);
			
			if(gamePadData.Buttons == 0)
			{
				tiles.TileIndex2D = new Vector2i(1, index);
			}
			
			if(pHealth <= 0)
			{
            Director.Instance.ReplaceScene(new GameOver());	
			}
				
			
			
			// If we move left
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				sPlayerDir = "Left";
				// Changes which way the player is facing.
				playerDirection = new Vector2(-1.0f,0.0f);
				bulletDirection = new Vector2(-1.0f,0.0f);
				tiles.TileIndex2D = new Vector2i(0, 2);
				index = 2;
				
				// If the tiles is within the left of the screen, move. Else, do not move.
				if(tiles.Position.X < 0)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move right
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				sPlayerDir = "Right";
				playerDirection = new Vector2(1.0f,0.0f);
				bulletDirection = new Vector2(1.0f,0.0f);
				tiles.TileIndex2D = new Vector2i(0, 1);
				index = 1;
				// If the tiles is within the right of the screen, move. Else, do not move.
				if((tiles.Position.X + tiles.TextureInfo.TileSizeInPixelsf.X) > (Director.Instance.GL.Context.GetViewport().Width))
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move up
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				sPlayerDir = "Up";
				playerDirection = new Vector2(0.0f,1.0f);
				bulletDirection = new Vector2(0.0f,1.0f);
				tiles.TileIndex2D = new Vector2i(0, 0);
				index = 0;
				
				if((tiles.Position.Y + tiles.TextureInfo.TileSizeInPixelsf.Y) > Director.Instance.GL.Context.GetViewport().Height)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move down
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				sPlayerDir = "Down";
				playerDirection = new Vector2(0.0f,-1.0f);
				bulletDirection = new Vector2(0.0f,-1.0f);
				tiles.TileIndex2D = new Vector2i(0, 3);
				index = 3;
				
				if((tiles.Position.Y) < 0)
				{
					playerDirection = new Vector2(0.0f, 0.0f);
				}
			}
			
			// If we move north-east
			if((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerDirection = new Vector2(0.5f,0.5f);
				bulletDirection = new Vector2(0.5f,0.5f);
				
				if((tiles.Position.Y + tiles.TextureInfo.TileSizeInPixelsf.Y) > (Director.Instance.GL.Context.GetViewport().Height) 
				  || (tiles.Position.X + tiles.TextureInfo.TileSizeInPixelsf.X) > (Director.Instance.GL.Context.GetViewport().Width))
				{
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			// North-west
			if((gamePadData.Buttons & GamePadButtons.Up) != 0 && (gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				playerDirection = new Vector2(-0.5f,0.5f);
				bulletDirection = new Vector2(-0.5f,0.5f);
				
				if((tiles.Position.Y + tiles.TextureInfo.TileSizeInPixelsf.Y) > (Director.Instance.GL.Context.GetViewport().Height) 
				  || (tiles.Position.X) < 0)
				{
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
				
				if((tiles.Position.Y) < 0 
				  || (tiles.Position.X) < 0)
				{
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
				
				if((tiles.Position.Y) < 0 
				  || (tiles.Position.X + tiles.TextureInfo.TileSizeInPixelsf.X) > Director.Instance.GL.Context.GetViewport().Width)
				{
					playerDirection = new Vector2(0.0f,0.0f);
				}
			}
			
			tiles.Position += playerDirection * deltaTime * speed;
			
		}		
	}
}


