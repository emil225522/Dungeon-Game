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
        public Rectangle OwnHitBox { get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); } }
        public int test;

        public LockedDoor(Vector2 position,ContentManager Content)
            :base (Content.Load<Texture2D>("bat"),new Vector2(500),3)
        {
            texture = Content.Load<Texture2D>("LockedDoorRight");
            this.position = position;
        }
        internal override void Update(GameTime gameTime, Player player)
        {

            if (player.hitBox.Intersects(hitBox))
                test++;
            else test = 0;
            if (test > 20 && player.numberOfKeys > 0)
            {
                isDeleted = true;
                player.numberOfKeys--;
            }
            base.Update(gameTime, player);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,position,Color.White);
        }
    }
}
