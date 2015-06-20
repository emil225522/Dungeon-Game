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
        public Texture2D texture;
        public float speed;
        KeyboardState oldKs = Keyboard.GetState();
        public bool test;
        public int bushesShanked;
        public bool isAttacking;
        int counter;
        public Rectangle attackRect;
        public Rectangle hitBox;
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };
        Direction direction;
        public sbyte state = 1;
        public Player(Vector2 position,Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
            speed = 1.4f;
            direction = Direction.Down;
        }
        public void Update(GameTime gameTime, List<Tile> tiles, Game1 game1,ContentManager Content)
        {
            velocity = velocity*FRICTION;
            if (Math.Abs(velocity.X) < 0.2f)
                velocity.X = 0;
            if (Math.Abs(velocity.Y) < 0.2f)
                velocity.Y = 0;
            if (direction == Direction.Down)
                texture = Content.Load<Texture2D>("playerDown");
            else if (direction == Direction.Up)
                texture = Content.Load<Texture2D>("playerUp");
            else if (direction == Direction.Left)
                texture = Content.Load<Texture2D>("playerSide");
            else if (direction == Direction.Right)
                texture = Content.Load<Texture2D>("playerSide");
            KeyboardState ks = Keyboard.GetState();
            hitBox = new Rectangle((int)position.X, (int)position.Y - 50, 100,  100);

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
            Random rnd = new Random();
            for (int i = 0; i < tiles.Count; i++)
            {
                if (ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space))
                {
                    isAttacking = true;
                    switch (direction)
                    {
                        case Direction.Left:
                            attackRect = new Rectangle((int)position.X - 30, (int)position.Y + 25, 50, 10);
                            break;
                        case Direction.Right:
                            attackRect = new Rectangle((int)position.X + 30, (int)position.Y + 25, 50, 10);
                            break;
                        case Direction.Up:
                            attackRect = new Rectangle((int)position.X + 12, (int)position.Y - 25, 10, 50);
                            break;
                        case Direction.Down:
                            attackRect = new Rectangle((int)position.X + 12, (int)position.Y + 30, 10, 50);
                            break;
                    }
                    if (attackRect.Intersects(tiles[i].hitBox) && tiles[i].type == 4)
                    {
                        tiles.RemoveAt(i);
                        bushesShanked++;
                    }
                }
            }
            if (isAttacking)
            {
                counter++;
                if (counter > 5)
                {
                    counter = 0;
                    attackRect = new Rectangle(0, 0, 0, 0);
                    isAttacking = false;
                }
            }
            if (ks.IsKeyDown(Keys.Enter) && oldKs.IsKeyUp(Keys.Enter))
            {
                for (int i = 0; i < tiles.Count; i++)
                {

                    if (tiles[i].type == 1 && rnd.Next(10) == 2)
                    tiles.Add(new Tile(Content.Load<Texture2D>("bush"), tiles[i].position, 4));
                    else if (rnd.Next(500) == 3 && tiles[i].type == 1)
                        tiles.Add(new Tile(Content.Load<Texture2D>("tree"), tiles[i].position, 4));
                }
            }
            position += velocity;
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
                                        if (new Rectangle((int)position.X - 3, (int)position.Y + (int)velocity.Y + 25, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }
                                    else if (velocity.X < 0)
                                    {
                                        if (new Rectangle((int)position.X + 3, (int)position.Y + (int)velocity.Y + 25, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (new Rectangle((int)position.X, (int)position.Y + (int)velocity.Y + 25, 37, 15).Intersects(tiles[i].hitBox))
                                        {
                                            position.Y -= velocity.Y;
                                            velocity.Y = 0;
                                        }
                                    }
                                    
                                    if (new Rectangle((int)position.X + (int)velocity.X, (int)position.Y + 25, 37, 15).Intersects(tiles[i].hitBox))
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
            oldKs = ks;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, attackRect, Color.Black);
            if (direction == Direction.Left)
                spriteBatch.Draw(texture,position,null,Color.White,0.0f,Vector2.Zero,1,SpriteEffects.FlipHorizontally,0);
            else
                spriteBatch.Draw(texture, position, Color.White);
        }   
    }
}
