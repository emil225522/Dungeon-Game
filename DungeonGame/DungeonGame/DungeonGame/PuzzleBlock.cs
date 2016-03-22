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

namespace DungeonGame
{
    class PuzzleBlock : Tile
    {
        public sbyte typeOfDrop;
        public bool isDown;
        public Rectangle PushBlockHitBox;
        public PuzzleBlock(Texture2D texture, Vector2 position, sbyte type, sbyte typeOfColor)
            : base(Game1.content.Load<Texture2D>("cube"),position,type)
        {
        
        }
        internal override void Update(GameTime gameTime, Player player)
        {
            PushBlockHitBox = new Rectangle((int)position.X, (int)position.Y-5, texture.Width, texture.Height);
            if (player.HitBox.Intersects(PushBlockHitBox))
                isDown = true;
            else
                isDown = false;
            base.Update(gameTime, player);
        }
        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (isDown)
                color = new Color(13, 37, 13, 37);
 	        base.Draw(spriteBatch, color);

            }

    }
}
