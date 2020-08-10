using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class World : IUpdateable {
        public float cameraX = 0;
        /// <summary>
        /// How wide the level is. When the player exceeds this, the next level is entered.
        /// </summary>
        float width;
        /// <summary>
        /// The name of the next level.
        /// </summary>
        string nextWorld;
        /// <summary>
        /// AI wandering the level.
        /// </summary>
        List<Actor> aimless;
        /// <summary>
        /// Background graphics.
        /// </summary>
        List<PositionableDrawable> decorations;

        public bool CollidingWithFloor(Actor actor) {
            return actor.y + actor.height > 300;
        }

        public void Update(float elapsed) {

        }

        public void Draw(Game target) {
            foreach (PositionableDrawable drawable in decorations) {
                // TODO: Add check to see if the drawable is onscreen.
                target.PixelMode = PixelEngine.Pixel.Mode.Alpha;
                drawable.Draw(target, -(int)cameraX, 0);
                target.PixelMode = PixelEngine.Pixel.Mode.Normal;
            }
        }

        #region     Loading
        void LoadGeneral(string input) {
            if (input.StartsWith("width:"))
                width = float.Parse(input.Split(':')[1]);
            else if (input.StartsWith("next:"))
                nextWorld = input.Split(':')[1];
        }

        void LoadActor(string input) {
            Console.WriteLine(input);
        }

        void LoadDecoration(string input) {
            Console.WriteLine(input);
            string[] bits = input.Split(' ');
            decorations.Add(new PositionableDrawable(
                new StaticSprite(
                    $"data/decorations/{bits[0]}.png"),
                    float.Parse(bits[1]),
                    float.Parse(bits[2])
                    )
                );
        }
        #endregion  Loading

        public World(string name) {
            aimless = new List<Actor>();
            decorations = new List<PositionableDrawable>();

            foreach (string s in File.ReadAllLines($"data/levels/{name}.txt")) {
                char type = s[0];
                string line = s.TrimStart(type);

                switch (type) {
                    case 'g':
                        LoadGeneral(line);
                        break;
                    case 'a':
                        LoadActor(line);
                        break;
                    case 'd':
                        LoadDecoration(line);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
