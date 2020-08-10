using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class PlayState : Scene {
        World level;
        Actor player;
        float playerSpeed = 50f;
        int lastDir = 0;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            int direction = 0;
            if (game.GetKey(PixelEngine.Key.Left).Down) direction--;
            if (game.GetKey(PixelEngine.Key.Right).Down) direction++;
            player.x += direction * playerSpeed * elapsed;

            if (direction == 0) {
                if (lastDir >= 0)
                    player.PlayAnimation("idle_right");
                else if (lastDir < 0)
                    player.PlayAnimation("idle_left");
            }
            else {
                if (direction > 0)
                    player.PlayAnimation("run_right");
                else
                if (direction < 0)
                    player.PlayAnimation("run_left");

                lastDir = direction;
            }

            player.Update(elapsed);

            level.cameraX = game.Lerp(level.cameraX, player.x - 100, elapsed * 2);

            game.Clear(PixelEngine.Pixel.Presets.Lavender);
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
            ((AnimatedSprite)player.drawables[0]).Play("run_left");
        }
    }
}
