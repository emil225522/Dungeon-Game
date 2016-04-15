using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    class Enemy : GameObject
    {

        public override Rectangle HitBox { get { return new Rectangle((int)Position.X, (int)Position.Y, Animation.frameWidth, Animation.frameHeight); } }

        public int hp;
        public bool isHurt;
        protected sbyte isHurtTimer;
        public int walktimer;
        public sbyte state;
        public bool canBeKnocked;
        public bool isBoss;

        protected Animation animation;
        protected float speed;
        protected Array values = Enum.GetValues(typeof(RoomConstants.Direction));
        protected RoomConstants.Direction direction;

        public Enemy(Vector2 position, Animation animation, int seed, float speed, int hp,sbyte type, bool canBeKnocked, bool isBoss,int level)
            : base (position,animation,1)
        {
            this.Position = position;
            this.animation = animation;
            direction = RoomConstants.Direction.Left;
            rnd = new Random(seed);
            this.speed = speed;
            this.hp = hp;
            this.canBeKnocked = canBeKnocked;
            this.type = type;
            this.isBoss = isBoss;
            hp *= level;
        }
        public Enemy(Vector2 position, Animation animation, int seed, float speed, int hp, sbyte type, bool canBeKnocked, bool isBoss,Vector2 velocity,int level)
            : base(position, animation, 1)
        {
            this.Position = position;
            this.animation = animation;
            direction = RoomConstants.Direction.Left;
            rnd = new Random(seed);
            this.speed = speed;
            this.hp = hp;
            this.canBeKnocked = canBeKnocked;
            this.type = type;
            this.isBoss = isBoss;
            Velocity = velocity;
            hp *= level;
        }

        public override void Update(GameTime gameTime, Room room)
        {
            animation.PlayAnim(gameTime);
            Position += Velocity;
            if (hp < 1)
                isDead = true;
            if (isHurt == true)
                isHurtTimer++;
            if (type != 9)
             Velocity *= 0.4f;
             if (!IsCollidingMovingX(room.tiles))
             {
                 Position += new Vector2(Velocity.X,0);
             }
             else
                 Position -= new Vector2(Velocity.X*2, 0);
            if (!IsCollidingMovingY(room.tiles)) 
            {
                Position += new Vector2(0,Velocity.Y);
            }
            else
                Position -= new Vector2(0, Velocity.Y*2);

            if (isHurtTimer > 15) 
            {
                isHurtTimer = 0;
                isHurt = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            animation.Draw(spriteBatch,Position, color);
            
        }

        public bool IsColliding(List<Tile> tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (HitBox.Intersects(tiles[i].hitBox) && tiles[i].type > 1)
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
                return new Rectangle((int)Position.X + (int)Velocity.X, (int)Position.Y, animation.frameWidth, animation.frameHeight);
            else
                return new Rectangle((int)Position.X, (int)Position.Y + (int)Velocity.Y, animation.frameWidth, animation.frameHeight);
        }
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            hp -= damage;
        }
        virtual public void OnHit()
        {

        }
    }
}
