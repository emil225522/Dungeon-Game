using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Randomz
{
    class Bluba : Enemy
    {
        Texture2D balltexture;
        public Bluba(ContentManager Content, int seed, Vector2 position,Texture2D balltexture)
            : base(position, new Animation(Content, "shootingEnemyUp", 100, 2, true), seed, 1.5F, 50)
        {
            this.balltexture = balltexture;
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }

        public override void Update(List<Tile> tiles, GameTime gameTime, Room room)
        {
            base.Update(tiles, gameTime,room);
            velocity *= 0.3f;
            if (!IsCollidingMovingX(tiles))
            {
                position.X += velocity.X;
            }
            if (!IsCollidingMovingY(tiles))
            {
                position.Y += velocity.Y;
            }

            if (isHurtTimer > 30)
            {
                isHurtTimer = 0;
                isHurt = false;
            }
            walktimer++;
            if (walktimer > rnd.Next(200, 400) && !IsColliding(tiles))
            {
                Vector2 ballVelocity = new Vector2();
                if (direction == Direction.Down)
                    ballVelocity = new Vector2(0, 4);
                else if (direction == Direction.Left)
                    ballVelocity = new Vector2(-4, 0);
                else if (direction == Direction.Right)
                    ballVelocity = new Vector2(4, 0);
                else if (direction == Direction.Up)
                    ballVelocity = new Vector2(0, -4);

                room.blubaBall.Add(new Projectile(balltexture, position, ballVelocity));
                walktimer = 0;
                direction = (Direction)values.GetValue(rnd.Next(values.Length));
            }

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
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            animation.Draw(spriteBatch, position, color);
        }

    }
}
