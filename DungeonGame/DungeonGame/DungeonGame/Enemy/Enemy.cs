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

namespace DungeonGame
{
    class Enemy : GameObject
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
        public bool isdead;
        public int hp;
        public bool isHurt;
        public sbyte isHurtTimer;
        public int walktimer;
        public sbyte type;
        public sbyte state;

        protected Animation animation;
        protected Random rnd;
        protected float speed;
        protected Array values = Enum.GetValues(typeof(Direction));
        protected Direction direction;

        public Enemy(Vector2 position, Animation animation, int seed, float speed, int hp,sbyte type)
            : base (position,animation,1)
        {
            this.position = position;
            this.animation = animation;
            direction = Direction.Left;
            rnd = new Random(seed);
            this.speed = speed;
            this.hp = hp;
            this.type = type;
        }

        public override void Update(List<Tile> tiles, GameTime gameTime, Room room, Player player)
        {
            animation.PlayAnim(gameTime);
            if (hp < 1)
                isdead = true;
            hitBox = new Rectangle((int)position.X, (int)position.Y, animation.frameWidth, animation.frameHeight);
            if (isHurt == true)
                isHurtTimer++;
             velocity *= 0.7f;
             if (!IsCollidingMovingX(tiles))
             {
                 position.X += velocity.X;
             }
            if (!IsCollidingMovingY(tiles)) 
            {
                position.Y += velocity.Y;
            }

            if (isHurtTimer > 15) 
            {
                isHurtTimer = 0;
                isHurt = false;
            }
            walktimer++;
            if (walktimer > rnd.Next(50, 200) && !IsColliding(tiles)) {
                walktimer = 0;
                direction = (Direction)values.GetValue(rnd.Next(values.Length));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
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
