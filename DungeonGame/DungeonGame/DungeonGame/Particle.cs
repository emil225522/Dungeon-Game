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
        Vector2 originalPos;
        public Particle(Animation animation, Vector2 position, Vector2 velocity)
            : base(position, animation,1)
        {
            Velocity = velocity;
            originalPos = position;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            Velocity += new Vector2(0,0.2f);
            if (Position.Y - originalPos.Y > 50)
                isDead = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }
    }
}
