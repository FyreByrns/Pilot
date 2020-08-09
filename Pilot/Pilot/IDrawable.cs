namespace Pilot {
    /// <summary>
    /// Something which can be drawn.
    /// Requires width and height so it can be determined whether or not it is onscreen.
    /// </summary>
    interface IDrawable {
        int GetWidth();
        int GetHeight();
        void Draw(Game target, int x, int y);
    }
}
