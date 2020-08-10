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

        public void PlayAnimation(string name) {
            foreach (IDrawable drawable in drawables)
                if (drawable is AnimatedSprite animatedSprite)
                    animatedSprite.Play(name);
        }

        public virtual void Update(float elapsed) {
            foreach (AnimatedSprite animatedSprite in drawables)
                animatedSprite.Update(elapsed);
        }

        public virtual void Draw(Game target, float offsetX, float offsetY) {
            foreach (IDrawable drawable in drawables)
                drawable.Draw(target, (int)(x + graphicOffsetX - offsetX), (int)(y + graphicOffsetY - offsetY));
        }
    }
}
