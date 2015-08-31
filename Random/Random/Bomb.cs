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
        public Animation animation;
        public Vector2 position;
        public bool willExplode;
        int bombTick;

        public Bomb(Animation animation,Vector2 position)
        {
            this.position = position;
            this.animation = animation;
        }
        public void Update(GameTime gameTime)
        {
            bombTick++;
            animation.PlayAnim(gameTime);
            if (bombTick > 120)
            {
                willExplode = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch,position,Color.White);
        }
    }
}
