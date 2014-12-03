using System;
using TiledMax;

namespace USMS_Source
{
	public class Level
	{
		private Map map;
		
		public Level (String fileName)
		{
			this.map = Map.Open(fileName);
			if(map.Loaded.Successful)
			{
				drawMap(map);
			}
		}
		
		
		public void drawMap(Map map)
		{
			Console.WriteLine("Map Loaded!");
		}
		
	}
}

