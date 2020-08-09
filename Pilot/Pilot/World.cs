using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class World {
        public bool CollidingWithFloor(Actor actor) {
            return actor.y + actor.height > 300;
        }
    }
}
