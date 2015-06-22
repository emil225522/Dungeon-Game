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
using Microsoft.Xna.Framework.Net;

namespace Randomz
{
    class Projectile
    {
        public Vector2 velocity;
        public Vector2 position;
        public Animation animation;

        public Projectile(Animation animation, Vector2 position, Vector2 velocity)
        {
            this.animation = animation;
            this.position = position;
            this.velocity = velocity;
        }
        public void Update()
        {
            position += velocity;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, Color.White);
        }
    }
}
