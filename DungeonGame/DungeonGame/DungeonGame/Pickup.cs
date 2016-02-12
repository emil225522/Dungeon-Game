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
    class Pickup : GameObject
    {
        public sbyte PickupType;
        public Pickup(Animation animation, Vector2 position, sbyte type)
            : base(position, animation, 1)
        {
            this.PickupType = type;
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
