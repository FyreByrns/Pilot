using System.IO;
using System.Collections.Generic;

using Sprite = PixelEngine.Sprite;
using Point = PixelEngine.Point;

namespace Pilot {
    class AnimatedSprite : IDrawable, IUpdateable {
        Sprite spritesheet;
        int frameWidth;
        string currentAnimation;
        Dictionary<string, Animation> animations;
        float timer;

        public int GetWidth()
            => frameWidth;
        public int GetHeight()
            => spritesheet.Height;

        public void Play(string name) {
            currentAnimation = name;
            timer = 0;
        }

        public void Update(float elapsed) {
            Animation current = animations[currentAnimation];

            timer += elapsed;
            if (timer > current.frameDuration) {
                timer = 0;
                current.AdvanceFrame();
            }
        }

        public void Draw(Game target, int x, int y) {
            Animation current = animations[currentAnimation];
            target.DrawPartialSprite(new Point(x, y), spritesheet, new Point(frameWidth * current.frames[current.currentFrame], 0), frameWidth, spritesheet.Height);
        }

        public AnimatedSprite(string path, int frameWidth) {
            spritesheet = Sprite.Load($"{path}.png");
            animations = new Dictionary<string, Animation>();
            this.frameWidth = frameWidth;

            foreach (string s in File.ReadAllLines($"{path}.txt")) {
                string[] entry = s.Split(' ');
                string sname = entry[0];
                string sduration = entry[1];
                string[] sframes = entry[2].Split(',');

                float fduration = float.Parse(sduration);
                int[] iframes = new int[sframes.Length];
                for (int i = 0; i < sframes.Length; i++)
                    iframes[i] = int.Parse(sframes[i]);

                animations[sname] = new Animation(fduration, iframes);
            }
        }

        class Animation {
            public float frameDuration;
            public int currentFrame;
            public int[] frames;

            public void AdvanceFrame() {
                currentFrame++;
                if (currentFrame >= frames.Length)
                    currentFrame = 0;
            }

            public Animation(float frameDuration, int[] frames) {
                this.frameDuration = frameDuration;
                this.frames = frames;
            }
        }
    }
}
