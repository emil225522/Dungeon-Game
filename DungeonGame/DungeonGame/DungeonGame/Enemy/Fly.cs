using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Fly : Enemy
    {
        float angle;
        float angleDirection;
        bool goingLeft;
        Vector2 circelingPlace;
        public Fly(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "fly", 150, 2, true), seed, 1.5F, 15, 1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            if (rnd.Next(2) == 1)
                goingLeft = true;
        }

        public override void Update(List<Tile> tiles, GameTime gameTime, Room room, Player player)
        {
            base.Update(tiles, gameTime, room, player);
                float XDistance = position.X - player.position.X - 40;
                float YDistance = position.Y - player.position.Y - 40;
                //sets the velocity to that with the right angle thanks to this function
                circelingPlace.X -= 1 * (float)Math.Cos(Math.Atan2(YDistance, XDistance));
                circelingPlace.Y -= 1 * (float)Math.Sin(Math.Atan2(YDistance, XDistance));
            if (goingLeft)
                angle -= angleDirection;
            else
                angle += angleDirection;
            position = new Vector2((float)(Math.Cos(angle)) * 60 + circelingPlace.X, (float)(Math.Sin(angle)) * 60 + circelingPlace.Y);

            if (!IsColliding(tiles))
            {
                if (direction == Direction.Down)
                    position.Y += speed;
                else if (direction == Direction.Left)
                    position.X -= speed;
                else if (direction == Direction.Right)
                    position.X += speed;
                else if (direction == Direction.Up)
                    position.Y -= speed;
            }
           
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            animation.Draw(spriteBatch, position, color);
        }

    }
}
