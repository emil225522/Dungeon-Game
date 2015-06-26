using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Randomz
{
    class Slime : Enemy
    {
          public Slime(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "slimeEnemy", 100, 2, true), seed, 1.5F, 50)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }
          public override void Draw(SpriteBatch spriteBatch)
          {
              base.Draw(spriteBatch);
          }
    }
}
