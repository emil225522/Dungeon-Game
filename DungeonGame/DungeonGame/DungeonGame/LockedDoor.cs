using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class LockedDoor : Tile
    {
        //public Animation animation;
        public Rectangle OwnHitBox { get { return new Rectangle((int)position.X, (int)position.Y-4, 63, 60); } }
        public int test;


        public LockedDoor(Vector2 position, ContentManager Content, sbyte type, sbyte direction)
            : base(Content.Load<Texture2D>("LockedDoorRight"), position, type, direction)
        {
        }

        internal override void Update(GameTime gameTime, Player player)
        {

            if (player.HitBox.Intersects(OwnHitBox))
                test++;
            else
                test = 0;

            if (test > 20 && player.numberOfKeys > 0)
            {
                isDeleted = true;
                player.numberOfKeys--;
            }
            base.Update(gameTime, player);
        }

        public override void Draw(SpriteBatch spriteBatch,Color color)
        {
            float offsetY = 0;
            SpriteEffects eff = SpriteEffects.None;
            if (direction == 0 || direction == 3)
                eff = SpriteEffects.FlipHorizontally;
            
            if (direction == 1 || direction == 3) {
                rotation = -(float)Math.PI / 2;
                offsetY = 50;
            }

            spriteBatch.Draw(texture, new Vector2(position.X, position.Y + offsetY), null, color,rotation, new Vector2(), 1, eff, 1);
        }
    }
}
