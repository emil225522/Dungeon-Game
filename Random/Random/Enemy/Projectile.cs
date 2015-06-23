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
        //public Animation animation; may be used later on
        public Texture2D texture;
        float rotation;

        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
        }
        public void Update()
        {
            position += velocity;
            rotation-= 0.2f;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation,new Vector2(texture.Width/2,texture.Height/2), 1.0f, SpriteEffects.None, 0f);
        }
    }
}
