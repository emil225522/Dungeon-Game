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
    class Drop
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle hitBox;

        public Drop(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)position.X,(int)position.Y, texture.Width / 2,texture.Height / 2);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,hitBox,Color.White);
        }
    }
}
