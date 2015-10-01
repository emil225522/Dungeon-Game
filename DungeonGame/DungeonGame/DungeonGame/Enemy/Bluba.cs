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
            : base(position, new Animation(Content, "shootingEnemyUp", 100, 2, true), seed,2, 50,1)
        {
            balltexture = Content.Load<Texture2D>("blubaball");
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime,room);
            if (!isdead)
            {
                Velocity *= 0.3f;
                if (!IsCollidingMovingX(room.tiles))
                {
                    Position += new Vector2(Velocity.X,0);
                }
                if (!IsCollidingMovingY(room.tiles))
                {
                    Position += new Vector2(0,Velocity.Y);
                }

                if (isHurtTimer > 30)
                {
                    isHurtTimer = 0;
                    isHurt = false;
                }
                timeBetweenAttack++;
                if (timeBetweenAttack > rnd.Next(100, 150))
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
                            ballVelocity = new Vector2(0, 8);
                        else if (direction == Direction.Left)
                            ballVelocity = new Vector2(-8, 0);
                        else if (direction == Direction.Right)
                            ballVelocity = new Vector2(8, 0);
                        else if (direction == Direction.Up)
                            ballVelocity = new Vector2(0, -8);

                        room.gameObjectsToAdd.Add(new Projectile(balltexture, Position, ballVelocity));
                    }
                }
                walktimer++;
                if (walktimer > rnd.Next(2000, 4000) && !IsColliding(room.tiles))
                {
                    walktimer = 0;
                    direction = (Direction)values.GetValue(rnd.Next(values.Length));
                }

                if (!isAttacking)
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
                            Position += new Vector2(speed * 4, 0);
                            direction = Direction.Right;
                        }
                        else if (direction == Direction.Right)
                        {
                            Position -= new Vector2(speed * 4, 0);
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
            animation.Draw(spriteBatch, Position, color, rotation);
        }

    }
}
