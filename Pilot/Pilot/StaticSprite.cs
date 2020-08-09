using Sprite = PixelEngine.Sprite;
using Point = PixelEngine.Point;

namespace Pilot {
    class StaticSprite : IDrawable {
        Sprite sprite;

        public int GetWidth()
            => sprite.Width;
        public int GetHeight()
            => sprite.Height;

        public void Draw(Game target, int x, int y) {
            target.DrawSprite(new Point(x, y), sprite);
        }

        public StaticSprite(string path) {
            sprite = Sprite.Load(path);
        }
    }
}
