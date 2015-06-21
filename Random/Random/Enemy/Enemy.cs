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

        protected enum Direction {
            Left,
            Right,
            Up,
            Down
        }

        public Vector2 position;
        public Vector2 velocity;
        public Rectangle hitBox;
        public int hp;
        public bool isHurt;
        public sbyte isHurtTimer;
        public int walktimer;

        protected Animation animation;
        protected Random rnd;
        protected float speed;
        protected Array values = Enum.GetValues(typeof(Direction));
        protected Direction direction;

        public Enemy(Vector2 position, Animation animation, int seed, float speed, int hp)
        {
            this.position = position;
            this.animation = animation;
            direction = Direction.Left;
            rnd = new Random(seed);
            this.speed = speed;
            this.hp = hp;
        }

        public virtual void Update(List<Tile> tiles, GameTime gameTime)
        {
            animation.PlayAnim(gameTime);
            hitBox = new Rectangle((int)position.X, (int)position.Y, animation.frameWidth, animation.frameHeight);
            if (isHurt == true)
                isHurtTimer++;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
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
                if (GetNextFrameHitBox(true).Intersects(tiles[i].hitBox) && tiles[i].type > 1)
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
                if (GetNextFrameHitBox(false).Intersects(tiles[i].hitBox) && tiles[i].type > 1)
                {
                    return true;
                }
            }
            return false;
        }

        private Rectangle GetNextFrameHitBox(bool isX) 
        {
            if(isX)
                return new Rectangle((int)position.X + (int)velocity.X, (int)position.Y, animation.frameWidth, animation.frameHeight);
            else
                return new Rectangle((int)position.X, (int)position.Y + (int)velocity.Y, animation.frameWidth, animation.frameHeight);
        }
    }
}
