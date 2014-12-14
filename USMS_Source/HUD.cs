using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace USMS_Source
{
	public class HUD
	{
				
		public HUD (Scene scene)
		{			
			TextureInfo hudTexture = new TextureInfo("/Application/textures/HUD.png");
			SpriteUV hudSprite = new SpriteUV(hudTexture);
			hudSprite.Quad.S = hudTexture.TextureSizef;
			hudSprite.Position = new Sce.PlayStation.Core.Vector2(0, 0);	
			scene.AddChild(hudSprite);
		}
	}
}

