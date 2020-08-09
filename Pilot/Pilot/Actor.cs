using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class Actor : IUpdateable {
        public float x, y;
        public float width, height;
        public List<IDrawable> drawables = new List<IDrawable>();
        public float graphicOffsetX, graphicOffsetY;

        public virtual void Update(float elapsed) {

        }

        public virtual void Draw(Game target) {
            foreach (IDrawable drawable in drawables)
                drawable.Draw(target, (int)(x + graphicOffsetX), (int)(y + graphicOffsetY));
        }
    }
}
