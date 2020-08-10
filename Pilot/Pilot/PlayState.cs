using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class PlayState : Scene {
        World level;
        Actor player;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            if (game.GetKey(PixelEngine.Key.Left).Down) player.x--;
            if (game.GetKey(PixelEngine.Key.Right).Down) player.x++;
            player.Update(elapsed);

            level.cameraX = game.Lerp(level.cameraX, player.x - 20, elapsed);

            game.Clear(PixelEngine.Pixel.Presets.Beige);
            level.Draw(game);


            game.PixelMode = PixelEngine.Pixel.Mode.Alpha;
            player.Draw(game, level.cameraX, 0);
            game.PixelMode = PixelEngine.Pixel.Mode.Normal;
        }

        public PlayState(Game game, string startingLevel) : base(game) {
            level = new World(startingLevel);

            player = new Actor {
                x = 10,
                y = 300 - 40,
                width = 22,
                height = 40
            };
            player.drawables.Add(new AnimatedSprite("data/animations/possessed", 22));
            ((AnimatedSprite)player.drawables[0]).Play("run");
        }
    }
}
