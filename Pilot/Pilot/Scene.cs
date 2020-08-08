namespace Pilot {
    /// <summary>
    /// A state of the game.
    /// </summary>
    class Scene {
        protected Game game;

        public virtual void Update(float elapsed) { }

        protected Scene(Game game) {
            this.game = game;
        }
    }
}
