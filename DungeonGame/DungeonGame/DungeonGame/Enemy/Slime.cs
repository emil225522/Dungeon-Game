using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
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
          public override void Update(GameTime gameTime, Room room)
          {
              base.Update(gameTime, room);
              jumptimer++;
              if (!isjumping)
              {
                  if (rnd.Next(100) == 5)
                  {
                      isjumping = true;
                      ypos = Position.Y;
                      yvel = -7;
                  }
                  else
                  {
                      yvel = 0; 
                  }
              }
              if (jumptimer > 1)
              {
                  Position += new Vector2(0,yvel);
              }
              if (isjumping)
              {
                  if (Position.Y < ypos)
                      yvel+= 0.5f;
                  else
                      isjumping = false;
              }
              if (isjumping == false)
              {

                  if (!IsColliding(room.tiles))
                  {
                      if (direction == Direction.Down)
                          Position += new Vector2(0, speed);
                      else if (direction == Direction.Left)
                          Position -= new Vector2(speed, 0);
                      else if (direction == Direction.Right)
                          Position += new Vector2(speed, 0);
                      else if (direction == Direction.Up)
                          Position -= new Vector2(0, speed);
                  }
                  else
                  {
                      if (direction == Direction.Down)
                      {
                          Position -= new Vector2(0, speed * 4);
                          direction = Direction.Up;
                      }
                      else if (direction == Direction.Left)
                      {
                          Position -= new Vector2(speed * 4, 0);
                          direction = Direction.Right;
                      }
                      else if (direction == Direction.Right)
                      {
                          Position += new Vector2(speed * 4, 0);
                          direction = Direction.Left;
                      }
                      else if (direction == Direction.Up)
                      {
                          Position += new Vector2(0, speed * 4);
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
