using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    internal class Wall : Tile
    {
        public Wall(Vector2 position, ContentManager Content, sbyte type, sbyte direction) 
            : base(Content.Load<Texture2D>("walls/wall2"), position, type, direction)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Color color)
        {
            float offsetY = 0;
            SpriteEffects eff = SpriteEffects.None;
            if (direction == 0 || direction == 3)
                eff = SpriteEffects.FlipHorizontally;

            if (direction == 1 || direction == 3)
            {
                rotation = -(float)Math.PI / 2;
                offsetY = 50;
            }

            spriteBatch.Draw(texture, new Vector2(position.X, position.Y + offsetY), null, color, rotation, new Vector2(), 1, eff, 1);
        }
    }
}