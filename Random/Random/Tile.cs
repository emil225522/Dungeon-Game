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

namespace Randomz
{
    public class Tile
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle hitBox;
        public bool isDeleted;
        public sbyte type;
        public bool isSelected;
        public Tile(Texture2D texture, Vector2 position, sbyte type)
        {
            this.texture = texture;
            this.position = position;
            this.type = type;
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
        public virtual void Update(GameTime gameTime,Player player)
        {
            hitBox = new Rectangle((int)position.X,(int)position.Y,texture.Width,texture.Height);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}