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
            : base(position, new Animation(Content, "slimeEnemy", 100, 2, true), seed, 1.5F, 50,1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }
          public override void Update(List<Tile> tiles, GameTime gameTime, Room room,Player player)
          {
              base.Update(tiles, gameTime, room,player);
              jumptimer++;
              if (!isjumping)
              {
                  if (rnd.Next(100) == 5)
                  {
                      isjumping = true;
                      ypos = position.Y;
                      yvel = -7;
                  }
                  else
                  {
                      yvel = 0; 
                  }
              }
              if (jumptimer > 1)
              {
                  position.Y += yvel;
              }
              if (isjumping)
              {
                  if (position.Y < ypos)
                      yvel+= 0.5f;
                  else
                      isjumping = false;
              }
              if (isjumping == false)
              {

                  if (!IsColliding(tiles))
                  {
                      if (direction == Direction.Down)
                          position.Y += speed;
                      else if (direction == Direction.Left)
                          position.X -= speed;
                      else if (direction == Direction.Right)
                          position.X += speed;
                      else if (direction == Direction.Up)
                          position.Y -= speed;
                  }
                  else
                  {
                      if (direction == Direction.Down)
                      {
                          position.Y -= speed * 4;
                          direction = Direction.Up;
                      }
                      else if (direction == Direction.Left)
                      {
                          position.X += speed * 4;
                          direction = Direction.Right;
                      }
                      else if (direction == Direction.Right)
                      {
                          position.X -= speed * 4;
                          direction = Direction.Left;
                      }
                      else if (direction == Direction.Up)
                      {
                          position.Y += speed * 4;
                          direction = Direction.Down;
                      }
                  }
              }


          }
          public override void Draw(SpriteBatch spriteBatch)
          {
              base.Draw(spriteBatch);
          }
    }
}
