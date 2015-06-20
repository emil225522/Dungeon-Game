using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Camera
    {
        public static Matrix Transform { get { return Matrix.Identity* Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0)) * Matrix.CreateScale(Scale); } }
        public static float Scale { get; set; }
        public static Vector2 Position { get; set; }
        public static Vector2 Origin { get; set; }
        public static float FollowSpeed { get; set; }
        public static Vector2 Center { get { return Position; } }

        public static Vector2 TotalOffset { get { return Position - Origin; } }

        public static void Follow(Vector2 target)
        {
            Vector2 d = (target - Position) * FollowSpeed;
            Position += new Vector2((int)d.X, (int)d.Y);
        }

        public static bool AreaIsVisible(Vector2 position, Vector2 size)
        {
            return AreaIsVisible(position.X, position.Y, size.X, size.Y);
        }

        public static bool AreaIsVisible(float x, float y, float w, float h)
        {
            return x < Position.X + Origin.X * Scale && x + w > Position.X - Origin.X * Scale && y < Position.Y + Origin.Y * Scale && y + h > Position.Y - Origin.Y * Scale;
        }

        public static bool AreaIsVisible(Vector2 position, float w, float h)
        {
            return AreaIsVisible(position.X, position.Y, w, h);
        }

        public static bool AreaIsVisible(Rectangle area)
        {
            return AreaIsVisible(area.X, area.Y, area.Width, area.Height);
        }
    }
}
