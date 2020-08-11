using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PixelEngine;

namespace Pilot {
    class SettingsMenu : Scene {
        Sprite backButton;
        Point backLocation;
        
        Scene back;
        
        void Back() {
            game.currentScene = back;
        }
        
        public override void Update(float elapsed) {
            base.Update(elapsed);
            game.Clear(Pixel.Empty);
            
            game.DrawSprite(backLocation, backButton);
            
            int mx = game.MouseX;
            int my = game.MouseY;

            if (game.GetMouse(Mouse.Left).Down) {
                if (IsButtonPressed(mx, my, backLocation, backButton))
                    Back();
            }
        }
        
        public SettingsMenu(Game game, Scene back) : base(game) {
            this.back = back;
            
            backButton = Sprite.Load("data/settings/back.png");
            
            backLocation = new Point(20, 20);
        }
    }
}