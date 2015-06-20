using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Randomz
{
    class Enemy
    {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;
        public Animation animation;
        public Rectangle hitBox;
        public Random rnd;
        public int hp = 50;
        public bool isHurt;
        public sbyte isHurtTimer;

        public int walktimer;
        public float speed;
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }
        Array values = Enum.GetValues(typeof(Direction));
        private Direction direction;

        public Enemy(Texture2D texture, Vector2 position, int seed, Animation animation)
        {
            this.texture = texture;
            this.position = position;
            this.animation = animation;
            direction = Direction.Left;
            rnd = new Random(seed);
            speed = 1.5f;

        }
        public void Update(List<Tile> tiles, GameTime gameTime)
        {
            animation.PlayAnim(gameTime);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (isHurt == true)
                isHurtTimer++;

            velocity *= 0.3f;
            if (IsCollidingMovingX(tiles) == false)
            {
                position.X += velocity.X;
            }
            if (IsCollidingMovingY(tiles) == false)
            {
                position.Y += velocity.Y;
            }

            if (isHurtTimer > 30)
            {
                isHurtTimer = 0;
                isHurt = false;
            }
            walktimer++;
            if (walktimer > rnd.Next(50, 200) && (IsColliding(tiles) == false))
            {
                walktimer = 0;
                direction = (Direction)values.GetValue(rnd.Next(values.Length));
            }

            if (IsColliding(tiles) == false)
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
            else
            {
                if (direction == Direction.Down)
                {
                    position.Y -= speed * 4;
                    direction = Direction.Up;
                }
                else if (direction == Direction.Left)
                {
                    position.X += speed * 4;
                    direction = Direction.Right;
                }
                else if (direction == Direction.Right)
                {
                    position.X -= speed * 4;
                    direction = Direction.Left;
                }
                else if (direction == Direction.Up)
                {
                    position.Y += speed * 4;
                    direction = Direction.Down;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.Black;
            animation.Draw(spriteBatch,position, color);
            
        }
        public bool IsColliding(List<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (hitBox.Intersects(tiles[i].hitBox) && tiles[i].type > 1)
                {
                    return true;
                }
            }
                return false;
        }
        public bool IsCollidingMovingX(List<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (new Rectangle((int)position.X + (int)velocity.X,(int)position.Y,texture.Width,texture.Height).Intersects(tiles[i].hitBox) && tiles[i].type > 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsCollidingMovingY(List<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (new Rectangle((int)position.X, (int)position.Y + (int)velocity.Y, texture.Width, texture.Height).Intersects(tiles[i].hitBox) && tiles[i].type > 1)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
