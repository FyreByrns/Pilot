using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sprite = PixelEngine.Sprite;

namespace Pilot {
    /// <summary>
    /// Fades between a few images, then advances the scene.
    /// </summary>
    class SplashScreen : Scene {
        Scene next;
        float currentTimer = 0;
        Stack<Sprite> imagesToDisplay;

        void GoToNextScene() {
            game.currentScene = next;
        }

        public override void Update(float elapsed) {
            base.Update(elapsed);

            if (imagesToDisplay.Count == 0 || game.GetKey(PixelEngine.Key.Any).Down) {
                GoToNextScene();
                return;
            }

            currentTimer += elapsed;
            if (currentTimer <= 0.5f) {
                game.DrawSprite(PixelEngine.Point.Origin, imagesToDisplay.Peek());
            }
            else if (currentTimer > 0.5f) {
                game.PixelMode = PixelEngine.Pixel.Mode.Alpha;
                game.Clear(new PixelEngine.Pixel(0, 0, 0, 1));
                game.PixelMode = PixelEngine.Pixel.Mode.Normal;
            }

            if (currentTimer > 0.55f) {
                currentTimer = 0;
                imagesToDisplay.Pop();
            }
        }

        public SplashScreen(Game game, Scene next) : base(game) {
            this.next = next;

            // Get the images to display
            imagesToDisplay = new Stack<Sprite>();
            foreach (string s in File.ReadAllLines("data/splashscreen.txt").Reverse())
                imagesToDisplay.Push(Sprite.Load(s));
        }
    }
}
