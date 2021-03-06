﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Pilot {
    class Editor : Scene {
        enum EditorState {
            Decorations,
            Actors,
            Rain
        }

        World editing;
        EditorState state;
        int lastMX, lastMY;
        string currentDecoration;

        public override void Update(float elapsed) {
            base.Update(elapsed);

            if (editing == null) { // Load level if none is being edited
                BringConsoleToFront();
                Console.Write("Enter the name of the level to load:\n> ");
                editing = new World(Console.ReadLine());

                lastMX = game.MouseX;
                lastMY = game.MouseY;
            }
            else { // Draw the current level, edit it, etc
                game.Clear(PixelEngine.Pixel.Presets.Black);
                editing.Draw(game);

                int mx = game.MouseX;
                int my = game.MouseY;

                if (game.GetMouse(PixelEngine.Mouse.Left).Down) {
                    PositionableDrawable decoration = GetSelectedDecoration((int)(mx + editing.cameraX), my, lastMX, lastMY);
                    Actor actor = GetSelectedActor((int)(mx + editing.cameraX), lastMX);

                    switch (state) {
                        case EditorState.Decorations:
                            if (decoration != null) { // If there's a decoration under the mouse
                                decoration.x = mx - decoration.GetWidth() / 2 + editing.cameraX;
                                decoration.y = my - decoration.GetHeight() / 2;

                                if (game.GetMouse(PixelEngine.Mouse.Right).Down)
                                    editing.decorations.Remove(decoration);
                            }
                            else if (!string.IsNullOrEmpty(currentDecoration))
                                editing.decorations.Add(new PositionableDrawable(new StaticSprite($"data/decorations/{currentDecoration}.png"), editing.cameraX + mx - 4, my - 4));
                            break;

                        case EditorState.Actors:
                            if (my > game.ScreenHeight - 100) { // If the mouse is in NPC area
                                game.PixelMode = PixelEngine.Pixel.Mode.Alpha;
                                game.FillRect(new PixelEngine.Point(0, 300 - 100), new PixelEngine.Point(game.ScreenWidth, 300), new PixelEngine.Pixel(255, 255, 255, 10));
                                game.PixelMode = PixelEngine.Pixel.Mode.Normal;

                                if (actor != null) {
                                    actor.x = mx - actor.width / 2 + editing.cameraX;

                                    if (game.GetMouse(PixelEngine.Mouse.Right).Down)
                                        editing.actors.Remove(actor);
                                }
                                else {
                                    Actor player;
                                    player = new Actor {
                                        x = mx - 4 + editing.cameraX,
                                        y = game.ScreenHeight - 40,
                                        width = 22,
                                        height = 40
                                    };
                                    player.drawables.Add(new AnimatedSprite("data/animations/possessed", 22));
                                    ((AnimatedSprite)player.drawables[0]).Play("run_left");
                                    editing.actors.Add(player);
                                }
                            }
                            break;

                        case EditorState.Rain:
                            float camMX = mx + editing.cameraX;
                            // Is the mouse on a rain region?
                            World.RainRegion region = null;
                            foreach (var r in editing.rainRegions) {
                                if (camMX > r.startX && camMX < r.endX) {
                                    region = r;
                                    break;
                                }
                            }

                            if (region != null) {
                                // If so, edit it.
                                if (game.GetKey(PixelEngine.Key.J).Down)
                                    region.startX--;
                                if (game.GetKey(PixelEngine.Key.K).Down)
                                    region.startX++;

                                if (game.GetKey(PixelEngine.Key.Left).Down)
                                    region.endX--;
                                if (game.GetKey(PixelEngine.Key.Right).Down)
                                    region.endX++;

                                if (game.GetMouse(PixelEngine.Mouse.Right).Down)
                                    editing.rainRegions.Remove(region);
                            }
                            else
                                // Otherwise, make a new one.
                                editing.rainRegions.Add(new World.RainRegion(mx + editing.cameraX - 20, mx + editing.cameraX + 20));
                            break;
                    }
                }

                if (game.GetMouse(PixelEngine.Mouse.Middle).Down) {
                    int difference = mx - lastMX;
                    editing.cameraX -= difference;
                }
                lastMX = mx;
                lastMY = my;

                if (game.GetKey(PixelEngine.Key.Control).Down) { // Check for hotkeys
                    game.DrawRect(PixelEngine.Point.Origin, new PixelEngine.Point(game.ScreenWidth - 1, game.ScreenHeight - 1), PixelEngine.Pixel.Presets.DarkRed);

                    if (game.GetKey(PixelEngine.Key.D).Pressed) { // Load a decoration
                        BringConsoleToFront();
                        retry:
                        Console.Write("Enter the name of the decoration to load:\n> ");
                        currentDecoration = Console.ReadLine();
                        if (!System.IO.File.Exists($"data/decorations/{currentDecoration}.png")) goto retry;
                    }

                    if (game.GetKey(PixelEngine.Key.S).Pressed)
                        editing.SaveAll();

                    if (game.GetKey(PixelEngine.Key.O).Pressed) {
                        BringConsoleToFront();
                        Console.Write("Discard unsaved changes? y/n\n>");
                        if (!Console.ReadLine().ToLower().StartsWith("n"))
                            editing.SaveAll();

                        editing = null;
                    }
                }
                else {
                    if (game.GetKey(PixelEngine.Key.D).Pressed)
                        state = EditorState.Decorations;
                    if (game.GetKey(PixelEngine.Key.A).Pressed)
                        state = EditorState.Actors;
                    if (game.GetKey(PixelEngine.Key.R).Pressed)
                        state = EditorState.Rain;
                }
            }
        }

        PositionableDrawable GetSelectedDecoration(int selectX, int selectY, int lastSelectX, int lastSelectY) {
            foreach (PositionableDrawable drawable in editing.decorations)
                if (selectX > drawable.x && selectX < drawable.x + drawable.GetWidth() && selectY > drawable.y && selectY < drawable.y + drawable.GetHeight() ||
                    lastSelectX > drawable.x && lastSelectX < drawable.x + drawable.GetWidth() && selectY > drawable.y && selectY < drawable.y + drawable.GetHeight())
                    return drawable;
            return null;
        }

        Actor GetSelectedActor(int selectX, int lastSelectX) {
            foreach (Actor actor in editing.actors)
                if (selectX > actor.x && selectX < actor.x + actor.width || lastSelectX > actor.x && lastSelectX < actor.x + actor.width)
                    return actor;
            return null;
        }

        #region     Bring Console To Front
        void BringConsoleToFront() =>
            SetForegroundWindow(GetConsoleWindow());

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();
        #endregion  Bring Console To Front

        public Editor(Game game) : base(game) {
        }
    }
}
