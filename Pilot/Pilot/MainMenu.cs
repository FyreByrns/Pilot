using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PixelEngine;

namespace Pilot {
    class MainMenu : Scene {
        Sprite playButton, settingsButton, quitButton;
        Point playLocation, settingsLocation, quitLocation;

        AnimatedSprite sprite;

        void Play() {
            Console.WriteLine("Play!~");
        }

        void Settings() {
            Console.WriteLine("Settings!~");
        }

        void Quit() {
            game.quit = true;
        }

        public override void Update(float elapsed) {
            base.Update(elapsed);
            game.Clear(Pixel.Empty);

            game.DrawSprite(playLocation, playButton);
            game.DrawSprite(settingsLocation, settingsButton);
            game.DrawSprite(quitLocation, quitButton);

            int mx = game.MouseX;
            int my = game.MouseY;

            if (game.GetMouse(Mouse.Left).Down) {
                if (mx > playLocation.X && my > playLocation.Y && mx < playLocation.X + playButton.Width && my < playLocation.Y + playButton.Height)
                    Play();
                else if (mx > settingsLocation.X && my > settingsLocation.Y && mx < settingsLocation.X + settingsButton.Width && my < settingsLocation.Y + settingsButton.Height)
                    Settings();
                else if (mx > quitLocation.X && my > quitLocation.Y && mx < quitLocation.X + quitButton.Width && my < quitLocation.Y + quitButton.Height)
                    Quit();
            }

            sprite.Update(elapsed);
            sprite.Draw(game, mx, my);
        }

        public MainMenu(Game game) : base(game) {
            playButton = Sprite.Load("data/mainmenu/play.png");
            settingsButton = Sprite.Load("data/mainmenu/settings.png");
            quitButton = Sprite.Load("data/mainmenu/quit.png");

            playLocation = new Point(game.ScreenWidth / 2 - playButton.Width / 2, 100);
            settingsLocation = new Point(game.ScreenWidth / 2 - settingsButton.Width / 2, playLocation.Y + playButton.Height + 3);
            quitLocation = new Point(game.ScreenWidth / 2 - quitButton.Width / 2, settingsLocation.Y + settingsButton.Height + 3);

            sprite = new AnimatedSprite("data/test", 18);
            sprite.Play("default");
        }
    }
}
