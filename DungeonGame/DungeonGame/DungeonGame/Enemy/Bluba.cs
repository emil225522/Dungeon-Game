using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Bluba : Enemy
    {
        Texture2D balltexture;
        int timeBetweenAttack;
        bool isAttacking;
        int attackingTimer;
        public Bluba(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "shootingEnemyUp", 100, 2, true), seed, 1.5F, 50,1)
        {
            balltexture = Content.Load<Texture2D>("blubaball");
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime,room);
            if (!isdead)
            {
                velocity *= 0.3f;
                if (!IsCollidingMovingX(room.tiles))
                {
                    position.X += velocity.X;
                }
                if (!IsCollidingMovingY(room.tiles))
                {
                    position.Y += velocity.Y;
                }

                if (isHurtTimer > 30)
                {
                    isHurtTimer = 0;
                    isHurt = false;
                }
                timeBetweenAttack++;
                if (timeBetweenAttack > rnd.Next(40, 70))
                {
                    timeBetweenAttack = 0;
                    isAttacking = true;
                }

                if (isAttacking)
                {
                    attackingTimer++;
                    if (attackingTimer > 60)
                    {
                        attackingTimer = 0;
                        isAttacking = false;
                        Vector2 ballVelocity = new Vector2();
                        if (direction == Direction.Down)
                            ballVelocity = new Vector2(0, 4);
                        else if (direction == Direction.Left)
                            ballVelocity = new Vector2(-4, 0);
                        else if (direction == Direction.Right)
                            ballVelocity = new Vector2(4, 0);
                        else if (direction == Direction.Up)
                            ballVelocity = new Vector2(0, -4);

                        room.gameObjects.Add(new Projectile(balltexture, position, ballVelocity));
                    }
                }
                walktimer++;
                if (walktimer > rnd.Next(200, 400) && !IsColliding(room.tiles))
                {
                    walktimer = 0;
                    direction = (Direction)values.GetValue(rnd.Next(values.Length));
                }

                if (!isAttacking)
                {
                    if (!IsColliding(room.tiles))
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
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            float rotation = 0f;
            switch (direction)
            {
                case Direction.Down:
                    rotation = (float)Math.PI;
                    break;
                case Direction.Left:
                    rotation = 3*(float)Math.PI / 2;
                    break;
                case Direction.Right:
                    rotation = (float)Math.PI / 2;
                    break;
                case Direction.Up:
                    rotation = 0;
                    break;
            }
            animation.Draw(spriteBatch, position, color, rotation);
        }

    }
}
