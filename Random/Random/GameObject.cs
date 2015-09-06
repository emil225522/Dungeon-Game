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
    class GameObject
    {
        public Vector2 Positon { get; set; }
        public Texture2D Texture { get; set; }
        public Animation Animation { get; set; }
        public sbyte type {get; set;}


        public GameObject(Vector2 postion, Texture2D texture, sbyte type) 
        {
            Positon = postion;
            Texture = texture;
            this.type = type;
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Positon, Color.White);
        }



    }
}
