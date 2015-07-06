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
        int jumptimer;
        float ypos;
        float yvel;
        bool isjumping;
          public Slime(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "slimeEnemy", 100, 2, true), seed, 1.5F, 50)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }
          public override void Update(List<Tile> tiles, GameTime gameTime, Room room)
          {
              base.Update(tiles, gameTime, room);
              jumptimer++;
              if (!isjumping && jumptimer > 4)
              {
                  jumptimer = 0;
                  isjumping = true;
                  ypos = position.Y;
                  yvel--;
              }
              if (isjumping)
              {
                  if (position.Y < ypos)
                      velocity.Y += 1.2f;
                  else
                      isjumping = false;
              }
          }
          public override void Draw(SpriteBatch spriteBatch)
          {
              base.Draw(spriteBatch);
          }
    }
}
