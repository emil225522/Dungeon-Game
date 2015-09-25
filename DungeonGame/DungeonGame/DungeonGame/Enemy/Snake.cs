using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Snake : Enemy
    {
        float angle;
        float angleDirection;
        bool goingLeft;
        Color normalColor;
        Vector2 circelingPlace;
        public Snake(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "fly", 150, 2, true), seed, 1.5F, 15, 1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            if (rnd.Next(2) == 1)
                goingLeft = true;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            float XDistance = Position.X - room.player.Position.X - 40;
            float YDistance = Position.Y - room.player.Position.Y - 40;
            //sets the velocity to that with the right angle thanks to this function
            circelingPlace.X -= 1 * (float)Math.Cos(Math.Atan2(YDistance, XDistance));
            circelingPlace.Y -= 1 * (float)Math.Sin(Math.Atan2(YDistance, XDistance));
            if (goingLeft)
                angle -= angleDirection;
            else
                angle += angleDirection;
            Position = new Vector2((float)(Math.Cos(angle)) * 60 + circelingPlace.X, (float)(Math.Sin(angle)) * 60 + circelingPlace.Y);

            if (!IsColliding(room.tiles))
            {
                if (direction == Direction.Down)
                    Position += new Vector2(0, speed);
                else if (direction == Direction.Left)
                    Position -= new Vector2(speed, 0);
                else if (direction == Direction.Right)
                    Position += new Vector2(speed, 0);
                else if (direction == Direction.Up)
                    Position -= new Vector2(0, speed);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = normalColor;
            animation.Draw(spriteBatch, Position, color);
        }

    }
}
