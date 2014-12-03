using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.HighLevel.UI;

namespace USMS_Source
{
	 public class MenuScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
    {
        private Sce.PlayStation.HighLevel.UI.Scene _uiScene;
        
        public MenuScene ()
        {
            this.Camera.SetViewFromViewport();
            Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
            dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
            dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
            
            ImageBox ib = new ImageBox();
            ib.Width = dialog.Width;
            ib.Image = new ImageAsset("/Application/Images/menuscreen.png",false);
            ib.Height = dialog.Height;
            ib.SetPosition(0.0f,0.0f);
            
            Button buttonUI1 = new Button();
            buttonUI1.Name = "buttonPlay";
            buttonUI1.Text = "Play Game";
            buttonUI1.Width = 300;
            buttonUI1.Height = 50;
            buttonUI1.Alpha = 0.8f;
            buttonUI1.SetPosition(dialog.Width/2 - 150,200.0f);
            buttonUI1.TouchEventReceived += (sender, e) => {
            Director.Instance.ReplaceScene(new TestLevel());
            };
            
            Button buttonUI2 = new Button();
            buttonUI2.Name = "buttonOptions";
            buttonUI2.Text = "Options";
            buttonUI2.Width = 300;
            buttonUI2.Height = 50;
            buttonUI2.Alpha = 0.8f;
            buttonUI2.SetPosition(dialog.Width/2 - 150,250.0f);
            buttonUI2.TouchEventReceived += (sender, e) => {
			Director.Instance.ReplaceScene(new OptionsScene());
			};  
			
			Button buttonUI3 = new Button();
            buttonUI3.Name = "buttonQuit";
            buttonUI3.Text = "Quit";
            buttonUI3.Width = 300;
            buttonUI3.Height = 50;
            buttonUI3.Alpha = 0.8f;
            buttonUI3.SetPosition(dialog.Width/2 - 150,300.0f);
            buttonUI3.TouchEventReceived += (sender, e) => {
				// Insert quit action.
			};
                
            dialog.AddChildLast(ib);
            dialog.AddChildLast(buttonUI1);
            dialog.AddChildLast(buttonUI2);
			dialog.AddChildLast(buttonUI3);
            _uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
            _uiScene.RootWidget.AddChildLast(dialog);
            UISystem.SetScene(_uiScene);
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
        }
        public override void Update (float dt)
        {
            base.Update (dt);
            UISystem.Update(Touch.GetData(0));
        }
        
        public override void Draw ()
        {
            base.Draw();
            UISystem.Render ();
        }
        
        ~MenuScene()
        {
            
        }
    }
}