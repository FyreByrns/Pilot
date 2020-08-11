using PixelEngine;

namespace Pilot {
    /// <summary>
    /// A state of the game.
    /// </summary>
    class Scene : IUpdateable {
        protected Game game;

        public virtual void Update(float elapsed) { }

        protected Scene(Game game) {
            this.game = game;
        }
        
        public bool IsButtonPressed(float mx, float my, Point location, Sprite button) {
            return mx > location.X && my > location.Y && mx < location.X + button.Width && my < location.Y + button.Height;
        }
    }
}
