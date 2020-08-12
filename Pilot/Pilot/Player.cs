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

            if (game.GetKey(PixelEngine.Key.Up).Down)
                if (OverlappingGoblin())
                    Attach(GetOverlappingGoblin());

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
            attached = other;
            y = other.y;
        }

        bool OverlappingGoblin() {
            foreach (Actor actor in World.Instance.actors)
                if (x + width > actor.x && x < actor.x + actor.width)
                    return true;
            return false;
        }

        Goblin GetOverlappingGoblin() {
            foreach (Actor actor in World.Instance.actors)
                if (x > actor.x && x < actor.x + actor.width)
                    return (Goblin)actor;
            return null;
        }

        public Player(Game game, int x) {
            this.game = game;
            this.x = x;
            animation = new AnimatedSprite("data/animations/flame", 30);
            animation.Play("walk_right");
            drawables.Add(animation);

            y = game.ScreenHeight - animation.GetHeight();
        }
    }
}
