using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class Player : Actor {
        Game game;
        public Actor attached;
        public AnimatedSprite animation;

        float speed = 50f;
        int lastDir = 0;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            int direction = 0;
            if (game.GetKey(PixelEngine.Key.Left).Down) direction--;
            if (game.GetKey(PixelEngine.Key.Right).Down) direction++;
            x += direction * speed * elapsed;

            if (direction == 0) {
                if (lastDir >= 0)
                    PlayAnimation("idle_right");
                else if (lastDir < 0)
                    PlayAnimation("idle_left");
            }
            else {
                if (direction > 0)
                    PlayAnimation("idle_right");
                else
                if (direction < 0)
                    PlayAnimation("idle_left");

                lastDir = direction;
            }
        }

        public void Attach(Actor other) {

        }

        public Player(Game game, int x) {
            this.game = game;
            this.x = x;
            animation = new AnimatedSprite("data/animations/flame", 30);
            animation.Play("walk_right");
            drawables.Add(animation);

            y = 200 - animation.GetHeight();
        }
    }
}
