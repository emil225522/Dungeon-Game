using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame 
{
    class Bat : Enemy 
    {
        public Bat(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "bat", 100, 2, true), seed, 1.5F, 50,1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }

        public override void Update(List<Tile> tiles, GameTime gameTime, Room room,Player player)
        {
            base.Update(tiles, gameTime, room,player);
            if (!IsColliding(tiles)) {
                if (direction == Direction.Down)
                    position.Y += speed;
                else if (direction == Direction.Left)
                    position.X -= speed;
                else if (direction == Direction.Right)
                    position.X += speed;
                else if (direction == Direction.Up)
                    position.Y -= speed;
            }
            else 
            {
                if (direction == Direction.Down) {
                    position.Y -= speed * 4;
                    direction = Direction.Up;
                } else if (direction == Direction.Left) {
                    position.X += speed * 4;
                    direction = Direction.Right;
                } else if (direction == Direction.Right) {
                    position.X -= speed * 4;
                    direction = Direction.Left;
                } else if (direction == Direction.Up) {
                    position.Y += speed * 4;
                    direction = Direction.Down;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.Black;
            animation.Draw(spriteBatch, position, color);
        }

    }
}
