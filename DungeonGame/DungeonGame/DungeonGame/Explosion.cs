using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    class Explosion
    {
        public Vector2 Position { get; set; }
        public Animation Animation { get; set; }
        public Rectangle HitBox { get { return new Rectangle((int)Position.X, (int)Position.Y, Animation.frameWidth, Animation.frameHeight); }}

        public Explosion(Vector2 position, Animation animation)
        {
            this.Position = position;
            this.Animation = animation;
        }
        public void Update(GameTime gameTime)
        {
            Animation.PlayAnim(gameTime);
           
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position, Color.White);
        }
    }
}
