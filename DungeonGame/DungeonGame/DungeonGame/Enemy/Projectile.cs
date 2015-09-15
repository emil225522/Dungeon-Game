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
    class Projectile : GameObject
    {
        float rotation;

        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity)
            : base (position,texture,1)
        {
        }
        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            rotation-= 0.2f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, rotation,new Vector2(Texture.Width/2,Texture.Height/2), 1.0f, SpriteEffects.None, 0f);
        }
    }
}
