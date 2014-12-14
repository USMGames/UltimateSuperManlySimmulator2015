using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.UI;
	
namespace USMS_Source
{
	public class AppMain
	{		
		 
   	 	private static Dictionary<string,Bgm> _songs;
    	private static BgmPlayer _bgmPlayer;
    
   	 	private static void PlaySong(string filename){
      	if(_bgmPlayer != null)
		{
        if(_bgmPlayer.Status == BgmStatus.Playing)
          	_bgmPlayer.Stop();
        	_bgmPlayer.Dispose();
      	}
      	
			if(!_songs.ContainsKey(filename))
        	_songs.Add(filename,new Bgm(filename));

     		_bgmPlayer = _songs[filename].CreatePlayer();
     	 	_bgmPlayer.Play();
	
		}
		

		public static void Main (string[] args)
		{
			Director.Initialize();
            UISystem.Initialize(Director.Instance.GL.Context);
            Director.Instance.RunWithScene(new TitleScene()); 
			
			
      		_songs = new Dictionary<string, Bgm>();
       
      CallFunc playSong1 = new CallFunc(
        ()=>  PlaySong ("/Application/ItHasToBeThisWay")
        );
      
      CallFunc playSong2 = new CallFunc(
        ()=>  PlaySong ("/Application/ItHasToBeThisWay.mp3")
        );
      
			Sequence seq = new Sequence();
      		seq.Add(playSong1);
      		seq.Add (new DelayTime(30));
      		seq.Add(playSong2);
      
  
      seq.Run();
      
      

		}
		
	}
}
