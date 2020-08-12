using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class PlayState : Scene {
        World level;
        Player player;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            player.Update(elapsed);
            level.Update(elapsed);

            level.cameraX = game.Lerp(level.cameraX, player.x - 100, elapsed * 2);

            game.Clear(PixelEngine.Pixel.Presets.Lavender);
            level.Draw(game);

            game.PixelMode = PixelEngine.Pixel.Mode.Alpha;
            player.Draw(game, level.cameraX, 0);
            game.PixelMode = PixelEngine.Pixel.Mode.Normal;
        }

        public PlayState(Game game, string startingLevel) : base(game) {
            level = new World(startingLevel);

            player = new Player(game, 10);
        }
    }
}
