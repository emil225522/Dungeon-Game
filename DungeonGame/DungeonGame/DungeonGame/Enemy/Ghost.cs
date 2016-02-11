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
    class Ghost : GameObject
    {
        int deathtimer;
        int size;
        public Ghost(Animation animation, Vector2 position,int size)
            : base (position,animation,1)
        {
            this.size = size;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            deathtimer++;
            if (deathtimer > 10)
                isDead = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Animation.Draw(spriteBatch, Position, Color.White, size);
        }
    }
}
