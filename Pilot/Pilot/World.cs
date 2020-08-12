using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class World : IUpdateable {
        public string name;
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
        public List<Actor> actors;
        /// <summary>
        /// Background graphics.
        /// </summary>
        public List<PositionableDrawable> decorations;
        /// <summary>
        /// Regions of rain in the level.
        /// </summary>
        public List<RainRegion> rainRegions;

        Dictionary<PositionableDrawable, string> names; // HACK

        /// <summary>
        /// Returns whether an actors middle is within a region of rain.
        /// </summary>
        public bool WithinRain(Actor actor) {
            foreach (RainRegion region in rainRegions)
                if (actor.x + actor.width / 2 > region.startX && actor.x + actor.width / 2 < region.endX)
                    return true;
            return false;
        }

        public void Update(float elapsed) {
            foreach (Actor actor in actors)
                actor.Update(elapsed);
        }

        public void Draw(Game target) {
            target.PixelMode = PixelEngine.Pixel.Mode.Alpha;
            foreach (PositionableDrawable drawable in decorations) {
                // TODO: Add check to see if the drawable is onscreen.
                if (drawable.x + drawable.GetWidth() > cameraX) {
                    drawable.Draw(target, -(int)cameraX, 0);
                }
            }
            foreach (Actor actor in actors)
                actor.Draw(target, (int)cameraX, 0);

            foreach (RainRegion region in rainRegions) {
                if (region.startX > cameraX && region.startX + region.endX < target.ScreenWidth)
                    target.FillRect(new PixelEngine.Point((int)(region.startX - cameraX), 0), new PixelEngine.Point((int)region.endX, target.ScreenHeight), new PixelEngine.Pixel(0, 10, 200, 10));
            }
            target.PixelMode = PixelEngine.Pixel.Mode.Normal;
        }

        #region     Saving / Loading
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
            string[] pieces = input.Split(' ');

            PositionableDrawable drawable = new PositionableDrawable(
                new StaticSprite(
                    $"data/decorations/{pieces[0]}.png"),
                    float.Parse(pieces[1]),
                    float.Parse(pieces[2])
                    );

            decorations.Add(drawable);
            names[drawable] = pieces[0]; // HACK
        }

        void LoadRain(string input) {
            string[] pieces = input.Split(' ');
            rainRegions.Add(new RainRegion(float.Parse(pieces[0]), float.Parse(pieces[1])));
        }

        /// <summary>
        /// Save the level to the levels folder.
        /// </summary>
        public void SaveAll() {
            List<string> toSave = new List<string>();

            // Save width & next level
            toSave.Add($"width:{width}");
            toSave.Add($"next:{nextWorld}");

            // Save all decorations
            foreach (PositionableDrawable staticSprite in decorations)
                toSave.Add($"d{names[staticSprite] /* HACK */} {staticSprite.x} {staticSprite.y}");

            // Save all npcs
            foreach (Actor actor in actors)
                toSave.Add($"agoblin {actor.x}"); // Originally I intended to have more actors than goblins, but all of them will be goblins because time.

            // Save all rain
            foreach (RainRegion region in rainRegions)
                toSave.Add($"r{region.startX} {region.endX}");

            File.WriteAllLines($"data/levels/{name}.txt", toSave.ToArray());
        }
        #endregion  Saving / Loading

        public World(string name) {
            this.name = name;
            actors = new List<Actor>();
            decorations = new List<PositionableDrawable>();
            names = new Dictionary<PositionableDrawable, string>(); // HACK

            if (File.Exists($"data/levels/{name}.txt"))
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
                        case 'r':
                            LoadRain(line);
                            break;
                        default:
                            break;
                    }
                }
            else Console.WriteLine($"New level: {name}");
        }

        public class RainRegion {
            public float startX, endX;

            public RainRegion(float startX, float endX) {
                this.startX = startX;
                this.endX = endX;
            }
        }
    }
}
