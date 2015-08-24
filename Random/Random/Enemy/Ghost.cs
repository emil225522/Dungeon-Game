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
    class Ghost
    {
        Animation animation;
        Vector2 position;
        int deathtimer;
        public bool isdead;
        public Ghost(Animation animation, Vector2 position)
        {
            this.position = position;
            this.animation = animation;

        }
        public void Update(GameTime gameTime)
        {
            animation.PlayAnim(gameTime);
            deathtimer++;
            if (deathtimer > 10)
            {
                isdead = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch,position,Color.White);
        }
    }
}
