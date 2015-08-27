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
    class Bomb
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle hitBox;
        int bombTick;
        public sbyte type;

        public Bomb(Texture2D texture,Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public void Update()
        {
            bombTick++;
            if (bombTick > 10)
            {

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
