using System;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace USMS_Source
{
	public class TestLevel : Scene
	{
		
		private Level level;
		
		public TestLevel ()
		{
			this.Camera.SetViewFromViewport();
			level = new Level(this, "test.tmx");
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
		}
		
		
		public override void Update(float dt)
		{
			Console.WriteLine("Update!");
			level.Update();
			base.Update(dt);
		}
	}
}

