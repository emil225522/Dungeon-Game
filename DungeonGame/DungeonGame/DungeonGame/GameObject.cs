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
    class GameObject
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Velocity { get; set; }
        public Animation Animation { get; set; }
        public Rectangle hitBox;
        public Random rnd = new Random();
        public virtual Rectangle HitBox { get { return new Rectangle((int)Position.X, (int)Position.Y, Animation.frameWidth, Animation.frameHeight); } }
        public sbyte type { get; set; }
        public bool isDead;

        public GameObject(Vector2 postion, Texture2D texture, sbyte type) 
        {
            Position = postion;
            Texture = texture;
            this.type = type;
        }

        public GameObject(Vector2 postion, Animation animation, sbyte type)
        {
            Position = postion;
            Animation = animation;
            this.type = type;
        }

        public virtual void Update(GameTime gameTime, Room room)
        {
                Animation.PlayAnim(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position, Color.White);
        }
        public virtual void TakeDamage(int damage)
        {

        }



    }
}
