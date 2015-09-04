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
        public Rectangle OwnHitBox { get { return new Rectangle((int)position.X, (int)position.Y-2, 63, 58); } }
        public int test;
        public float rotation;
        public sbyte Type { get; set; }

        public LockedDoor(Vector2 position,ContentManager Content,sbyte type)
            :base (Content.Load<Texture2D>("bat"),new Vector2(500),3)
        {
            texture = Content.Load<Texture2D>("LockedDoorRight");
            this.position = position;
            Type = type;
        }
        internal override void Update(GameTime gameTime, Player player)
        {

            if (player.hitBox.Intersects(OwnHitBox))
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
            if (type == 0)
                rotation = 1;
            else if (type == 1)
                rotation = 2;
            else if (type == 2)
                rotation = 3;
            else if (type == 3)
                rotation = 4;
                spriteBatch.Draw(texture,position,new Rectangle((int)position.X,(int)position.Y,50,50),Color.White,0,new Vector2(),1,SpriteEffects.FlipHorizontally,1);
        }
    }
}
