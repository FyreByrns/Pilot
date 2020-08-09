using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot {
    class World : IUpdateable {
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

        public bool CollidingWithFloor(Actor actor) {
            return actor.y + actor.height > 300;
        }

        public void Update(float elapsed) {

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
        }
        #endregion  Loading

        public World(string name) {
            foreach (string s in File.ReadAllLines($"data/levels/{name}.txt")) {
                char type = s[0];
                s.TrimStart(type);

                switch (type) {
                    case 'g':
                        LoadGeneral(s);
                        break;
                    case 'a':
                        LoadActor(s);
                        break;
                    case 'd':
                        LoadDecoration(s);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
