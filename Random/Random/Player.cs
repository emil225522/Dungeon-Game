using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Randomz
{
    class Player
    {
        public Vector2 position;
        const float FRICTION = 0.68f;
        public Vector2 velocity;
        public Animation animation;
        public float speed;
        KeyboardState oldKs = Keyboard.GetState();
        public bool isAttacking;
        int counter;
        public Rectangle attackRect;
        public Rectangle hitBox;

        public float health;
        public bool isHurt;
        public sbyte isHurtTimer;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };
        Direction direction;
        public sbyte state = 1;
        public Player(Vector2 position, ContentManager Content)
        {
            this.position = position;
            speed = 1.4f;
            direction = Direction.Down;
            health = 7;
            animation = new Animation(Content, "linkRight", 150, 2, true);
        }
        public void Update(GameTime gameTime, List<Tile> tiles, List<Enemy> enemies,ContentManager Content, List<Drop> drops)
        {
            animation.PlayAnim(gameTime);
            velocity = velocity*FRICTION;
            if (Math.Abs(velocity.X) < 0.2f)
                velocity.X = 0;
            if (Math.Abs(velocity.Y) < 0.2f)
                velocity.Y = 0;

           
            KeyboardState ks = Keyboard.GetState();
            hitBox = new Rectangle((int)position.X + 10, (int)position.Y + 55, 37, 15);

            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Up) && ks.IsKeyUp(Keys.Down))
                animation.looping = false;


            if (ks.IsKeyDown(Keys.Left))
            {
                if (oldKs.IsKeyUp(Keys.Left))
                    animation = new Animation(Content, "linkLeft", 150, 2, true);
                velocity.X -= speed;
                direction = Direction.Left;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                velocity.X += speed;
                if (oldKs.IsKeyUp(Keys.Right))
                    animation = new Animation(Content, "linkRight", 150, 2, true);
                direction = Direction.Right;
            }
            if (ks.IsKeyDown(Keys.Up))
            {
                if (oldKs.IsKeyUp(Keys.Up))
                        animation = new Animation(Content, "linkup11", 150, 2, true);
                velocity.Y -= speed;
                direction = Direction.Up;
            }
            
            if (ks.IsKeyDown(Keys.Down))
            {
                if (oldKs.IsKeyUp(Keys.Down))
                    animation = new Animation(Content, "linkDown", 150, 2, true);
                velocity.Y += speed;
                direction = Direction.Down;
            }
            Random rnd = new Random();

            if (ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space))
            {
                if (direction == Direction.Right)
                animation = new Animation(Content,"attackRight",30,3,false);
                else if (direction == Direction.Left)
                    animation = new Animation(Content, "attackLeft", 30, 3, false);
                else if (direction == Direction.Down)
                    animation = new Animation(Content, "attackDown", 30, 3, false);
                else if (direction == Direction.Up)
                    animation = new Animation(Content, "attackUp", 30, 3, false);
                isAttacking = true;
                switch (direction)
                {
                    case Direction.Left:
                        attackRect = new Rectangle((int)position.X - 30, (int)position.Y + 5, 50, 60);
                        break;
                    case Direction.Right:
                        attackRect = new Rectangle((int)position.X + 30, (int)position.Y + 5, 50, 60);
                        break;
                    case Direction.Up:
                        attackRect = new Rectangle((int)position.X +5, (int)position.Y - 10, 75, 50);
                        break;
                    case Direction.Down:
                        attackRect = new Rectangle((int)position.X -20, (int)position.Y + 50, 50, 50);
                        break;
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (attackRect.Intersects(enemies[i].hitBox) && enemies[i].isHurt == false)
                    {
                        enemies[i].hp -= 20;
                        enemies[i].isHurt = true;
                        if (direction == Direction.Right)
                            enemies[i].velocity.X = 150;
                        else if (direction == Direction.Left)
                            enemies[i].velocity.X = -150;
                        else if (direction == Direction.Up)
                            enemies[i].velocity.Y = -150;
                        else if (direction == Direction.Down)
                            enemies[i].velocity.Y = 150;
                        if (enemies[i].hp < 1)
                        {
                            drops.Add(new Drop(Content.Load<Texture2D>("hearth"), enemies[i].position));
                            enemies.RemoveAt(i);
                        }
                    }
                }
            }
            for (int i = 0; i < drops.Count; i++)
            {
                if (drops[i].hitBox.Intersects(hitBox))
                {
                    health++;
                    drops.RemoveAt(i);
                }
            }
            if (isAttacking)
            {
                velocity = new Vector2();
                counter++;
                if (counter > 15)
                {
                    counter = 0;
                    attackRect = new Rectangle(0, 0, 0, 0);
                    isAttacking = false;
                    if (direction == Direction.Down)
                        animation = new Animation(Content, "linkDown", 50, 2, false);
                    else if (direction == Direction.Left)
                        animation = new Animation(Content, "linkLeft", 50, 2, false);
                    else if (direction == Direction.Right)
                        animation = new Animation(Content, "linkRight", 50, 2, false);
                    else if (direction == Direction.Up)
                        animation = new Animation(Content, "linkup11", 50, 2, false);
                }
            }
            if (IsColliding(enemies)&& isHurt == false)
            {
                isHurt = true;
                health--;
            }
            if (isHurt == true)
                isHurtTimer++;
            if (isHurtTimer > 120)
            {
                isHurtTimer = 0;
                isHurt = false;
            }
            position += velocity;
            #region Collision
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].type > 2)
                {
                    if (tiles[i].position.X < position.X + 500)
                    {
                        if (tiles[i].position.X > position.X - 500)
                        {
                            if (tiles[i].position.Y < position.Y + 400)
                            {
                                if (position.Y - tiles[i].position.Y < 500)
                                {
                                    if (velocity.X > 0)
                                    {
                                        if (new Rectangle((int)position.X - 2, (int)position.Y + (int)velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }
                                    else if (velocity.X < 0)
                                    {
                                        if (new Rectangle((int)position.X + 2, (int)position.Y + (int)velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (new Rectangle((int)position.X, (int)position.Y + (int)velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }

                                    if (velocity.Y > 0)
                                    {
                                        if (new Rectangle((int)position.X + (int)velocity.X, (int)position.Y + 55 -2, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.X -= velocity.X;
                                            velocity.X = 0;
                                        }
                                    }
                                    else if (velocity.Y < 0)
                                    {
                                        if (new Rectangle((int)position.X + (int)velocity.X, (int)position.Y + 55 + 2, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.X -= velocity.X;
                                            velocity.X = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (new Rectangle((int)position.X + (int)velocity.X, (int)position.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.X -= velocity.X;
                                            velocity.X = 0;
                                        }
                                    }


                                }
                            }
                        }
                    }
                }
            }
            #endregion
            oldKs = ks;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            if (animation.asset == "attackLeft")
            animation.Draw(spriteBatch,new Vector2(position.X - 30,position.Y),color);

            else if (animation.asset == "attackDown")
                animation.Draw(spriteBatch, new Vector2(position.X - 40, position.Y + 30), color);
            else if (animation.asset == "attackUp")
                animation.Draw(spriteBatch, new Vector2(position.X - 30, position.Y - 5), color);
            else
                animation.Draw(spriteBatch, new Vector2(position.X, position.Y), color);
           
        }
        public bool IsColliding(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (hitBox.Intersects(enemies[i].hitBox))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
