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
        Vector2 circelingPlace;
        public Fly(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "fly", 100, 2, true), seed, 1.5F, 50, 1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
            circelingPlace = new Vector2(rnd.Next(50, 500), rnd.Next(50, 500));
            angleDirection = (float)rnd.NextDouble();
        }

        public override void Update(List<Tile> tiles, GameTime gameTime, Room room, Player player)
        {
            base.Update(tiles, gameTime, room, player);
                angle -= angleDirection;
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
