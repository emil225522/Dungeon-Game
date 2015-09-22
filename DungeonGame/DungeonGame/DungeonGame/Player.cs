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

namespace DungeonGame
{
    class Player
    {
        #region Variables
        public Vector2 Position {get; set;}
        const float FRICTION = 0.68f;
        public Vector2 Velocity {get; set;}

        KeyboardState oldKs = Keyboard.GetState();
        Rectangle attackRect;
        public Rectangle HitBox { get { return new Rectangle((int)Position.X + 10, (int)Position.Y + 55, 37, 15); } }

        public float health {get; set;}
        public float maxHealth{get; set;}
        public float speed {get; set;} 
        private sbyte isHurtTimer ;
        private sbyte timer = 0;

        public bool isHurt;
        public bool isAttacking;
        public bool currentRoomDark;

        private int counter;
        public int xp;
        public int xpNeeded;
        public int level;
        public int numberOfKeys {get; set;}
        public int numberOfBombs {get; set;}

        private Animation attackRight;
        private Animation attackDown;
        private Animation attackLeft;
        private Animation attackUp;
        private Animation animationLeft;
        private Animation animationRight;
        private Animation animationUp;
        private Animation animationDown;
        public Animation currentAnimation {get; set;}

        Texture2D tex;

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
        #endregion
        public Player(Vector2 position, ContentManager Content)
        {
            this.Position = position;
            speed = 1.4f;
            direction = Direction.Down;
            maxHealth = 5;
            #region LoadContent
            {
                StreamReader streamReader = new StreamReader("gameinfo.txt");
                level = int.Parse(streamReader.ReadLine());
                xp = int.Parse(streamReader.ReadLine());
                numberOfKeys = int.Parse(streamReader.ReadLine());
                health = int.Parse(streamReader.ReadLine());
                maxHealth = int.Parse(streamReader.ReadLine());
                xpNeeded = int.Parse(streamReader.ReadLine());
                numberOfBombs = int.Parse(streamReader.ReadLine());
                streamReader.Close();

                tex = Content.Load<Texture2D>("dark");

                animationLeft = new Animation(Content, "player/runLeft", 110, 6, true);
                animationRight = new Animation(Content, "player/runRight", 110, 6, true);
                animationUp = new Animation(Content, "player/runUp", 110, 8, true);
                animationDown = new Animation(Content, "player/runDown", 110, 7, true);
                currentAnimation = new Animation(Content, "player/runDown", 110, 7, true);

                attackRight = new Animation(Content, "player/attackright", 50, 5, false);
                attackDown = new Animation(Content, "player/attackDown", 50, 6, false);
                attackLeft = new Animation(Content, "player/attackleft", 50, 5, false);
                attackUp = new Animation(Content, "player/attackUp", 50, 5, false);
            }
            #endregion

        }
        public void Update(GameTime gameTime, List<Tile> tiles,ContentManager Content, Room room, List<GameObject> gameObjects)
        {
            #region AutoSave
            {
                saveTick++;
                if (saveTick > 200)
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
            }
            #endregion
           
            Velocity = Velocity * FRICTION;
            if (Math.Abs(Velocity.X) < 0.1f)
                Velocity = new Vector2(0,Velocity.Y);
            if (Math.Abs(Velocity.Y) < 0.1f)
                Velocity = new Vector2(Velocity.X, 0);

            if (xp >= xpNeeded)
            {
                level++;
                xpNeeded *= (int)1.4f;
                xp = 0;
            }
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Enter) && oldKs.IsKeyUp(Keys.Enter) && numberOfBombs > 0)
            {
                numberOfBombs--;
                Vector2 bombPosition = new Vector2();
                if (direction == Direction.Down)
                {
                    bombPosition = new Vector2(Position.X - 8, Position.Y + 60);
                }
                if (direction == Direction.Left)
                {
                    bombPosition = new Vector2(Position.X - 50, Position.Y + 15);
                }
                if (direction == Direction.Right)
                {
                    bombPosition = new Vector2(Position.X + 20, Position.Y + 20);
                }
                if (direction == Direction.Up)
                {
                    bombPosition = new Vector2(Position.X - 8, Position.Y - 30);
                }
                gameObjects.Add(new Bomb(new Animation(Content, "bomb", 400, 4, false), bombPosition,Content));
            }
            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Up) && ks.IsKeyUp(Keys.Down) && (!isAttacking))
            {
                //makes sure the animation part where the player stops is when he is standing
                if (currentAnimation.asset == "player/runRight")
                    currentAnimation.currentFrame = 2;
                else
                    currentAnimation.currentFrame = 3;
            }
            else
                currentAnimation.looping = true;

            if (!isAttacking)
            {
                if (ks.IsKeyDown(Keys.Left))
                {

                    Velocity -= new Vector2(speed,0);
                    direction = Direction.Left;
                }
                if (ks.IsKeyDown(Keys.Right))
                {
                    Velocity += new Vector2(speed,0);

                    direction = Direction.Right;
                }
                if (ks.IsKeyDown(Keys.Up))
                {

                    Velocity -= new Vector2(0,speed);
                    direction = Direction.Up;
                }

                if (ks.IsKeyDown(Keys.Down))
                {

                    Velocity += new Vector2(0,speed);
                    direction = Direction.Down;
                }
            }
            Random rnd = new Random();
            if (!isAttacking)
            {
                if (direction == Direction.Down)
                    currentAnimation = animationDown;
                else if (direction == Direction.Up)
                    currentAnimation = animationUp;
                else if (direction == Direction.Left)
                    currentAnimation = animationLeft;
                else if (direction == Direction.Right)
                    currentAnimation = animationRight;
            }
            if (ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space) && !isAttacking)
            {
                if (direction == Direction.Right)
                {
                    currentAnimation = attackRight;
                }
                else if (direction == Direction.Left)
                {
                    currentAnimation = attackLeft;
                }
                else if (direction == Direction.Down)
                {
                    currentAnimation = attackDown;
                }
                else if (direction == Direction.Up)
                {
                    currentAnimation = attackUp;
                }
                currentAnimation.currentFrame = 0;
                isAttacking = true;
                switch (direction)
                {
                    case Direction.Left:
                        attackRect = new Rectangle((int)Position.X - 30, (int)Position.Y + 20, 50, 65);
                        break;
                    case Direction.Right:
                        attackRect = new Rectangle((int)Position.X + 10, (int)Position.Y + 20, 50, 65);
                        break;
                    case Direction.Up:
                        attackRect = new Rectangle((int)Position.X - 15, (int)Position.Y - 5, 75, 50);
                        break;
                    case Direction.Down:
                        attackRect = new Rectangle((int)Position.X - 15, (int)Position.Y + 50, 70, 50);
                        break;
                }
                foreach(GameObject go in gameObjects.Where(item => item is Enemy))
                {
                    Enemy enemy = (Enemy)go;
                    if (attackRect.Intersects(go.HitBox) && enemy.isHurt == false)
                    {
                        if (enemy.state == 0)
                        {
                            enemy.hp -= 20;
                            enemy.isHurt = true;
                        }
                            if (enemy.type == 1)
                        {
                            if (direction == Direction.Right)
                                enemy.velocity.X = 30;
                            else if (direction == Direction.Left)
                                enemy.velocity.X = -30;
                            else if (direction == Direction.Up)
                                enemy.velocity.Y = -30;
                            else if (direction == Direction.Down)
                                enemy.velocity.Y = 30;
                        }
                        if (enemy.hp < 1)
                        {
                            int random = rnd.Next(10);
                            if (random == 1)
                                room.drops.Add(new Drop(Content.Load<Texture2D>("hearth"), enemy.position, 1));
                            if (random == 2)
                                room.drops.Add(new Drop(Content.Load<Texture2D>("key"), enemy.position, 2));
                            if (random == 3)
                                room.drops.Add(new Drop(Content.Load<Texture2D>("bombDrop"), enemy.position, 3));
                            go.isDead = true;
                            xp += rnd.Next(20, 40);
                        }
                    }
                }
            }
            for (int i = 0; i < room.drops.Count; i++)
            {
                if (room.drops[i].HitBox.Intersects(HitBox))
                {
                    if (room.drops[i].type == 1 && health < maxHealth)
                        health++;
                    else if (room.drops[i].type == 2)
                        numberOfKeys++;
                    else if (room.drops[i].type == 3)
                        numberOfBombs += rnd.Next(1, 4);
                    room.drops.RemoveAt(i);
                }
            }
            if (isAttacking)
            {
                Velocity = new Vector2();
                counter++;
                if (counter > 18)
                {
                    counter = 0;
                    attackRect = new Rectangle(0, 0, 0, 0);
                    isAttacking = false;
                    if (direction == Direction.Down)
                        currentAnimation = new Animation(Content, "player/runDown", 110, 7, false);
                    else if (direction == Direction.Left)
                        currentAnimation = new Animation(Content, "player/runLeft", 110, 6, false);
                    else if (direction == Direction.Right)
                        currentAnimation = new Animation(Content, "player/runRight", 110, 6, false);
                    else if (direction == Direction.Up)
                        currentAnimation = new Animation(Content, "player/runUp", 110, 8, false);
                }
            }
            if (IsColliding(gameObjects) && isHurt == false && !EnemiesIsHurt(gameObjects))
            {
                isHurt = true;
                health--;
            }
            if (isHurt == true)
                isHurtTimer++;
            if (isHurtTimer > 45)
            {
                isHurtTimer = 0;
                isHurt = false;
            }
            Position += Velocity;
            #region Collision
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].type > 2)
                {
                    if (Velocity.X > 0)
                    {
                        if (new Rectangle((int)Position.X - 2, (int)Position.Y + (int)Velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(0,Velocity.Y);
                            Velocity = new Vector2(Velocity.X,0);
                        }
                    }
                    else if (Velocity.X < 0)
                    {
                        if (new Rectangle((int)Position.X + 2, (int)Position.Y + (int)Velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(0,Velocity.Y);
                            Velocity = new Vector2(Velocity.X,0);
                        }
                    }
                    else
                    {
                        if (new Rectangle((int)Position.X, (int)Position.Y + (int)Velocity.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(0,Velocity.Y);
                            Velocity = new Vector2(Velocity.X,0);
                        }
                    }

                    if (Velocity.Y > 0)
                    {
                        if (new Rectangle((int)Position.X + (int)Velocity.X, (int)Position.Y + 55 - 2, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(Velocity.X,0);
                            Velocity = new Vector2(0,Velocity.Y);
                        }
                    }
                    else if (Velocity.Y < 0)
                    {
                        if (new Rectangle((int)Position.X + (int)Velocity.X, (int)Position.Y + 55 + 2, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(Velocity.X,0);
                            Velocity = new Vector2(0,Velocity.Y);
                        }
                    }
                    else
                    {
                        if (new Rectangle((int)Position.X + (int)Velocity.X, (int)Position.Y + 55, 37, 15).Intersects(tiles[i].hitBox))
                        {
                            Position -= new Vector2(Velocity.X,0);
                            Velocity = new Vector2(0,Velocity.Y);
                        }
                    }


                }
            }
            #endregion
            oldKs = ks;

            currentAnimation.PlayAnim(gameTime);
            if (room.isDark)
                currentRoomDark = true;
            else
                currentRoomDark = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentRoomDark)
                spriteBatch.Draw(tex, new Rectangle((int)Position.X - 1870, (int)Position.Y - 1950,4000,4000), Color.Black * 0.19f);
            Color color;
            if (isHurt)
            {
                timer++;
                if (timer > 8)
                {
                    timer = 0;
                    color = Color.White;
                }
                else
                {
                    color = Color.Red;
                }
            }
            else
            {
                color = Color.White;
            }

            if (direction == Direction.Left)
            {
                if (isAttacking)
                    currentAnimation.Draw(spriteBatch, new Vector2(Position.X - 30, Position.Y), color);
                else
                    currentAnimation.Draw(spriteBatch, new Vector2(Position.X - 15, Position.Y), color);
            }
            else
                currentAnimation.Draw(spriteBatch, new Vector2(Position.X - 15, Position.Y), color);
          
        }

        public bool IsColliding(List<GameObject> gameObjects)
        {
            foreach (GameObject go in gameObjects.Where(item => item is Enemy))
                if (HitBox.Intersects(go.HitBox))
                {
                    return true;
                }
            return false;
        }
        public bool EnemiesIsHurt(List<GameObject> gameObjects)
        {
            foreach (GameObject go in gameObjects.Where(item => item is Enemy))
            {
                Enemy enemy = (Enemy)go;
                if (enemy.isHurt)
                    return true;
            }
            return false;
        }
    }
}
