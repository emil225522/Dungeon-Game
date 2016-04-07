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
    class Particle : GameObject
    {
        public sbyte typeOfDrop;
        public Particle(Texture2D texture, Vector2 position)
            : base(position, texture,1)
        {

        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            Velocity = new Vector2(0,4);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
