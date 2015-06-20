using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Input
    {
        public static KeyboardState newKs, oldKs;
        public static MouseState newMs, oldMs;

        public static Vector2 MousePosition { get { return new Vector2(newMs.X, newMs.Y) + Camera.Position - Camera.Origin; } }

        public static Vector2 DeltaPos { get { return new Vector2(oldMs.X, oldMs.Y) - new Vector2(newMs.X, newMs.Y); } }

        public static void Initialize()
        {
            newKs = oldKs = Keyboard.GetState();
            newMs = oldMs = Mouse.GetState();
        }

        public static void Update()
        {
            oldKs = newKs;
            oldMs = newMs;
            newKs = Keyboard.GetState();
            newMs = Mouse.GetState();
        }

        public static bool KeyWasJustPressed(Keys key)
        {
            return newKs.IsKeyDown(key) && oldKs.IsKeyUp(key);
        }

        public static bool LeftClickWasJustPressed()
        {
            return newMs.LeftButton == ButtonState.Pressed && oldMs.LeftButton == ButtonState.Released;
        }


        /// <summary>
        /// relative to the world position.
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public static bool AreaIsClicked(Rectangle area)
        {
            return AreaIsHoveredOver(area) && LeftClickWasJustPressed();
        }

        public static bool AreaIsHoveredOver(Rectangle area)
        {
            return new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1).Intersects(area);
        }
    }
}
