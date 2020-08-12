using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PixelEngine;

namespace Pilot {
    class Game : PixelEngine.Game {
        /// <summary>
        /// Responsible for constructing and starting an instance of the game.
        /// </summary>
        static void Main() {
            new Game().Start();
        }

        #region     Constants
        const int WIDTH = 400,
                  HEIGHT = 200,
                  FRAMERATE_CAP = -1;
        #endregion  Constants

        #region     Game State Management
        /// <summary>
        /// Current scene of the game.
        /// </summary>
        public Scene currentScene;
        /// <summary>
        /// Whether the game will close at the end of this frame.
        /// </summary>
        public bool quit = false;

        void UpdateScene(float elapsed) {
            // If there's no scene, end the game.
            if(currentScene == null) {
                quit = true;
                return;
            }

            currentScene.Update(elapsed);
        }
        #endregion  Game State Management

        #region     Loop
        public override void OnUpdate(float elapsed) {
            base.OnUpdate(elapsed);

            UpdateScene(elapsed);

            if (quit)
                Finish();
        }
        #endregion  Loop

        Game() {
            Construct(WIDTH, HEIGHT, 3, 3, FRAMERATE_CAP);

            currentScene = new SplashScreen(this, new MainMenu(this));
        }
    }
}
