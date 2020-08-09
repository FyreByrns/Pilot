using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class PlayState : Scene {
        World level;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            if (game.GetKey(PixelEngine.Key.Left).Down) level.cameraX--;
            if (game.GetKey(PixelEngine.Key.Right).Down) level.cameraX++;

            game.Clear(PixelEngine.Pixel.Empty);
            level.Draw(game);
        }

        public PlayState(Game game, string startingLevel) : base(game) {
            level = new World(startingLevel);
        }
    }
}
