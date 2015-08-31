﻿using System;
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
        public int numberOfKeys;
        public int numberOfBombs;
        public Animation animationLeft;
        public Animation animationRight;
        public Animation animationUp;
        public Animation animationDown;
        public float speed;
        KeyboardState oldKs = Keyboard.GetState();
        public bool isAttacking;
        private int counter;
        public Rectangle attackRect;
        public Rectangle hitBox;
        private bool animationIsLooping;

        public float health;
        public float maxHealth;
        public bool isHurt;
        private sbyte isHurtTimer;

        public int xp;
        public int xpNeeded;
        public int level;

        public Animation attackRight;
        public Animation attackDown; 
        public Animation attackLeft;
        public Animation attackUp;

        int saveTick;

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
            maxHealth = 5;
            StreamReader streamReader = new StreamReader("gameinfo.txt");
            level = int.Parse(streamReader.ReadLine());
            xp = int.Parse(streamReader.ReadLine());
            numberOfKeys = int.Parse(streamReader.ReadLine());
            health = int.Parse(streamReader.ReadLine());
            maxHealth = int.Parse(streamReader.ReadLine());
            xpNeeded = int.Parse(streamReader.ReadLine());
            numberOfBombs = int.Parse(streamReader.ReadLine());
            streamReader.Close();
            animationLeft = new Animation(Content, "player/runLeft", 110, 6, true);
            animationRight = new Animation(Content, "player/runRight", 110, 6, true);

            animationUp = new Animation(Content, "player/runUp", 110, 8, true);

            animationDown = new Animation(Content, "player/runDown", 110, 7, true);

            attackRight = new Animation(Content, "player/attackright", 50, 5, false);
            attackDown = new Animation(Content, "player/attackDown", 50, 6, false);
            attackLeft = new Animation(Content, "player/attackleft", 50, 5, false);
            attackUp = new Animation(Content, "player/attackUp", 50, 5, false);

        }
        public void Update(GameTime gameTime, List<Tile> tiles, List<Enemy> enemies,ContentManager Content, List<Drop> drops, List<Bomb> bombs)
        {
           
            saveTick++;
            if (saveTick > 100)
            {
                saveTick = 0;
                using (StreamWriter writer =
            new StreamWriter("gameinfo.txt"))
                {
                    writer.Write(level);
                    writer.WriteLine();
                    writer.Write(xp);
                    writer.WriteLine();
                    writer.Write(numberOfKeys);
                    writer.WriteLine();
                    writer.Write(health);
                    writer.WriteLine();
                    writer.Write(maxHealth);
                    writer.WriteLine();
                    writer.Write(xpNeeded);
                    writer.WriteLine();
                    writer.Write(numberOfBombs);
                }
            }

   
            velocity = velocity*FRICTION;
            if (Math.Abs(velocity.X) < 0.1f)
                velocity.X = 0;
            if (Math.Abs(velocity.Y) < 0.1f)
                velocity.Y = 0;

            if (xp >= xpNeeded)
            {
                level++;
                xpNeeded *= (int)1.4f;
                xp = 0;
            }
            KeyboardState ks = Keyboard.GetState();
            hitBox = new Rectangle((int)position.X + 10, (int)position.Y + 55, 37, 15);
            if (ks.IsKeyDown(Keys.Enter) && oldKs.IsKeyUp(Keys.Enter) && numberOfBombs > 0)
            {
                numberOfBombs--;
                Vector2 bombPosition = new Vector2();
                if (direction == Direction.Down)
                {
                    bombPosition = new Vector2(position.X - 8,position.Y + 60);
                }
                if (direction == Direction.Left)
                {
                    bombPosition = new Vector2(position.X - 50,position.Y + 15);
                }
                if (direction == Direction.Right)
                {
                    bombPosition = new Vector2(position.X + 20,position.Y + 20);
                }
                if (direction == Direction.Up)
                {
                    bombPosition = new Vector2(position.X - 8,position.Y - 30);
                }
                    bombs.Add(new Bomb(new Animation(Content, "bomb", 500, 4, false), bombPosition));
            }
            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Up) && ks.IsKeyUp(Keys.Down) && (!isAttacking))
            {
                animationIsLooping = false;
                animationDown.currentFrame = 0;
                animationUp.currentFrame = 0;
                animationLeft.currentFrame = 0;
                animationRight.currentFrame = 0;
            }
            else
                animationIsLooping = true;

            animationDown.looping = animationIsLooping;
            animationUp.looping = animationIsLooping;
            animationRight.looping = animationIsLooping;
            animationLeft.looping = animationIsLooping;

            if (!isAttacking)
            {
                if (ks.IsKeyDown(Keys.Left))
                {

                    velocity.X -= speed;
                    direction = Direction.Left;
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    velocity.X += speed;

                    direction = Direction.Right;
                }
                if (ks.IsKeyDown(Keys.Up))
                {

                    velocity.Y -= speed;
                    direction = Direction.Up;
                }

                if (ks.IsKeyDown(Keys.Down))
                {

                    velocity.Y += speed;
                    direction = Direction.Down;
                }
            }
            Random rnd = new Random();
            
            if (ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space) && !isAttacking)
            {
                if (direction == Direction.Right)
                {
                    animationRight = attackRight;
                    animationRight.currentFrame = 0;
                }
                else if (direction == Direction.Left)
                {
                    animationLeft = attackLeft;
                    animationLeft.currentFrame = 0;
                }
                else if (direction == Direction.Down)
                {
                    animationDown = attackDown;
                    animationDown.currentFrame = 0;
                }

                else if (direction == Direction.Up)
                {
                    animationUp = attackUp;
                    animationUp.currentFrame = 0;
                }
                isAttacking = true;
                switch (direction)
                {
                    case Direction.Left:
                        attackRect = new Rectangle((int)position.X - 30, (int)position.Y + 20, 50, 65);
                        break;
                    case Direction.Right:
                        attackRect = new Rectangle((int)position.X + 10, (int)position.Y + 20, 50, 65);
                        break;
                    case Direction.Up:
                        attackRect = new Rectangle((int)position.X -15, (int)position.Y - 5, 75, 50);
                        break;
                    case Direction.Down:
                        attackRect = new Rectangle((int)position.X -15, (int)position.Y + 50, 70, 50);
                        break;
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (attackRect.Intersects(enemies[i].hitBox) && enemies[i].isHurt == false)
                    {
                        enemies[i].hp -= 20;
                        enemies[i].isHurt = true;
                        if (enemies[i].type == 1)
                        {
                            if (direction == Direction.Right)
                                enemies[i].velocity.X = 30;
                            else if (direction == Direction.Left)
                                enemies[i].velocity.X = -30;
                            else if (direction == Direction.Up)
                                enemies[i].velocity.Y = -30;
                            else if (direction == Direction.Down)
                                enemies[i].velocity.Y = 30;
                        }
                        if (enemies[i].hp < 1)
                        {
                            int random = rnd.Next(10);
                            if (random == 1)
                                drops.Add(new Drop(Content.Load<Texture2D>("hearth"), enemies[i].position, 1));
                            if (random == 2)
                                drops.Add(new Drop(Content.Load<Texture2D>("key"), enemies[i].position, 2));
                            if (random == 3)
                                drops.Add(new Drop(Content.Load<Texture2D>("bombDrop"), enemies[i].position, 3));
                            enemies[i].isdead = true;
                            xp += rnd.Next(20, 40);
                        }
                    }
                }
            }
            for (int i = 0; i < drops.Count; i++)
            {
                if (drops[i].hitBox.Intersects(hitBox))
                {
                    if (drops[i].type == 1 && health < maxHealth)
                        health++;
                    else if (drops[i].type == 2)
                        numberOfKeys++;
                    else if (drops[i].type == 3)
                        numberOfBombs+= rnd.Next(1,4);
                    drops.RemoveAt(i);
                }
            }
            if (isAttacking)
            {
                velocity = new Vector2();
                counter++;
                if (counter > 18)
                {
                    counter = 0;
                    attackRect = new Rectangle(0, 0, 0, 0);
                    isAttacking = false;
                    if (direction == Direction.Down)
                        animationDown = new Animation(Content, "player/runDown", 110, 7, false);
                    else if (direction == Direction.Left)
                        animationLeft = new Animation(Content, "player/runLeft", 110, 6, false);
                    else if (direction == Direction.Right)
                        animationRight = new Animation(Content, "player/runRight", 110, 6, false);
                    else if (direction == Direction.Up)
                        animationUp = new Animation(Content, "player/runUp", 110, 8, false);
                }
            }
            if (IsColliding(enemies)&& isHurt == false && !EnemiesIsHurt(enemies))
            {
                isHurt = true;
                health--;
            }
            if (isHurt == true)
                isHurtTimer++;
            if (isHurtTimer > 100)
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

            switch (direction)
            {
                case Direction.Down:
                    animationDown.PlayAnim(gameTime);
                    break;
                case Direction.Left:
                    animationLeft.PlayAnim(gameTime);
                    break;
                case Direction.Right:
                    animationRight.PlayAnim(gameTime);
                    break;
                case Direction.Up:
                    animationUp.PlayAnim(gameTime);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;

            if (direction == Direction.Left)
            {
                if (isAttacking)
                    animationLeft.Draw(spriteBatch, new Vector2(position.X - 30,position.Y), color);
                else
                    animationLeft.Draw(spriteBatch, new Vector2(position.X - 15, position.Y), color);
            }
            if (direction == Direction.Right)
            {
                if (isAttacking)
                    animationRight.Draw(spriteBatch, new Vector2(position.X - 15, position.Y), color);
                else
                    animationRight.Draw(spriteBatch, new Vector2(position.X - 15, position.Y), color);
            }
                if (direction == Direction.Up)
                    animationUp.Draw(spriteBatch, new Vector2(position.X - 15, position.Y), color);
            if (direction == Direction.Down)
                animationDown.Draw(spriteBatch, new Vector2(position.X - 15, position.Y), color);
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
        public bool EnemiesIsHurt(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
			{
			    if (enemies[i].isHurt)
                    return true;
			}
            return false;
        }
    }
}
