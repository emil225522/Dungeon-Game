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
    class Drop : GameObject
    {
        public sbyte typeOfDrop;
        public Drop(Animation animation, Vector2 position, sbyte typeOfDrop)
            : base (position,animation,1)
        {
            this.typeOfDrop = typeOfDrop;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
