using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace USMS_Source
{
	public class TestLevel : Scene
	{
		private Level level;
		
		public TestLevel ()
		{
			this.Camera.SetViewFromViewport();
			TextureInfo tiBackground = new TextureInfo("/Application/textures/City1.png");
			SpriteUV bgSprite = new SpriteUV(tiBackground);
			bgSprite.Quad.S = tiBackground.TextureSizef;
			//bgSprite.CenterSprite();
			bgSprite.Position = new Sce.PlayStation.Core.Vector2(0, 0);
			this.AddChild(bgSprite);
			level = new Level(this, "test.tmx");
			
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
		}
		
		
		public override void Update(float dt)
		{
			//Console.WriteLine("Update!");
			level.Update();
			base.Update(dt);
		}
	}
}