using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    class Explosion : GameObject
    {
        public Explosion(Vector2 position, Animation animation)
            : base (position,animation,1)
        {
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            if (Animation.currentFrame == Animation.numOffFrames - 1)
                isDead = true;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
