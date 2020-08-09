using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class PositionableDrawable : IDrawable {
        public float x, y;
        public IDrawable drawable;

        public int GetWidth()
            => drawable.GetWidth();
        public int GetHeight()
            => drawable.GetHeight();

        public void Draw(Game target, int x, int y) {
            x += (int)this.x;
            y += (int)this.y;

            drawable.Draw(target, x, y);
        }

        public PositionableDrawable(IDrawable drawable, float x, float y) {
            this.drawable = drawable;
            this.x = x;
            this.y = y;
        }
    }
}
