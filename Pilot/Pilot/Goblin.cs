using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class Goblin : Actor {
        int destination;
        int lastDir;

        public override void Update(float elapsed) {
            base.Update(elapsed);


        }

        public Goblin(Game game, float x) {
            this.x = x;
            drawables.Add(new AnimatedSprite("data/animations/possessed", 22));
            y = game.ScreenHeight - drawables[0].GetHeight();

            PlayAnimation("idle_right");
        }
    }
}
