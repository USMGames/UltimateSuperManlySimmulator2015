using System;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.UI;
	
namespace USMS_Source
{
	public class AppMain
	{				
		public static void Main (string[] args)
		{
			Director.Initialize();
            UISystem.Initialize(Director.Instance.GL.Context);
            Director.Instance.RunWithScene(new TitleScene());  
		}
		
	}
}
