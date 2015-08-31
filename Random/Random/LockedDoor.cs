using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Randomz
{
    class LockedDoor : Tile
    {
        public Texture2D tex;
        public Animation animation;
        public int test;

        public LockedDoor(Texture2D texture, Vector2 position,ContentManager Content)
            :base (Content.Load<Texture2D>("bat"),new Vector2(500),3)
        {
            this.position = position;
        }
        public override void Update(GameTime gameTime, Player player)
        {
            base.Update(gameTime,player);
            if (player.hitBox.Intersects(hitBox))
                test++;
            if (test > 20 && player.numberOfKeys > 0)
            {
                isDeleted = true;
                player.numberOfKeys--;
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, position, Color.White);
            spriteBatch.Draw(tex,position,Color.White);
        }
    }
}
