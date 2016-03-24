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
        public Vector2 Position { get; set;}
        const float FRICTION = 0.68f;
        public Vector2 Velocity { get; set;}
        
        KeyboardState oldKs = Keyboard.GetState();
        GamePadState oldgps = GamePad.GetState(PlayerIndex.One);
        public Rectangle attackRect;
        public Rectangle HitBox { get { return new Rectangle((int)Position.X  +5, (int)Position.Y + 25, 40, 42); } }

        public float hp { get; set;}
        public float maxHealth{ get; set;}
        public float speed { get; set;}
        public float currentSpeed { get; set; }
        private sbyte isHurtTimer;
        private sbyte timer;

        public bool isHurt;
        public bool isAttacking;
        public bool currentRoomDark;

        #region hasWeaponBools
        bool hasSword = true;
        bool hasSpell = true;
        bool hasBow;
        #endregion

        public WeaponState weaponState = WeaponState.Sword;

        private int counter;
        public int roomlevel;
        public int xp;
        public int xpNeeded;
        public int mana = 200;
        public int level;
        public int numberOfKeys { get; set;}
        public int numberOfBombs { get; set;}

        private Texture2D manabarTex;
        private Animation attackRight;
        private Animation attackDown;
        private Animation attackLeft;
        private Animation attackUp;
        private Animation animationLeft;
        private Animation animationRight;
        private Animation animationUp;
        private Animation animationDown;
        public Animation currentAnimation { get; set;}

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
            speed = 1.2f;
            currentSpeed = speed;
            direction = Direction.Down;
            maxHealth = 5;
            #region LoadContent
            {
                StreamReader streamReader = new StreamReader("gameinfo.txt");
                level = int.Parse(streamReader.ReadLine());
                xp = int.Parse(streamReader.ReadLine());
                numberOfKeys = int.Parse(streamReader.ReadLine());
                hp = int.Parse(streamReader.ReadLine());
                maxHealth = int.Parse(streamReader.ReadLine());
                xpNeeded = int.Parse(streamReader.ReadLine());
                numberOfBombs = int.Parse(streamReader.ReadLine());
                streamReader.Close();
                weaponState = WeaponState.Sword;
                tex = Content.Load<Texture2D>("dark");
                hp = 5;

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
                        writer.Write(hp);
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
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
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
            //GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            if ((gps.IsButtonUp(Buttons.DPadLeft) && gps.IsButtonUp(Buttons.DPadRight) && gps.IsButtonUp(Buttons.DPadDown)
                && gps.IsButtonUp(Buttons.DPadUp) && ks.IsKeyUp(Keys.Left) && ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Down) && ks.IsKeyUp(Keys.Up) && !isAttacking))
            {
                    //makes sure the animation part where the player stops is when he is standing
                    if (currentAnimation.asset == "player/runRight")
                        currentAnimation.currentFrame = 2;
                    else
                        currentAnimation.currentFrame = 3;
            }
            else
                currentAnimation.looping = true;
            
            #region walkInput
            if (!isAttacking)
            {
                if (gps.IsButtonDown(Buttons.DPadLeft) || ks.IsKeyDown(Keys.Left))
                {

                    Velocity -= new Vector2(currentSpeed,0);
                    direction = Direction.Left;
                }
                if (gps.IsButtonDown(Buttons.DPadRight) || ks.IsKeyDown(Keys.Right))
                {
                    Velocity += new Vector2(currentSpeed,0);
                    direction = Direction.Right;
                }
                if (gps.IsButtonDown(Buttons.DPadUp) || ks.IsKeyDown(Keys.Up))
                {
                    Velocity -= new Vector2(0,currentSpeed);
                    direction = Direction.Up;
                }
                if (gps.IsButtonDown(Buttons.DPadDown) || ks.IsKeyDown(Keys.Down))
                {
                    Velocity += new Vector2(0,currentSpeed);
                    direction = Direction.Down;
                }

                if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Up) || gps.IsButtonDown(Buttons.DPadRight) && gps.IsButtonDown(Buttons.DPadUp))
                {
                    currentSpeed = speed / 1.5f;
                }
                else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Up) || gps.IsButtonDown(Buttons.DPadLeft) && gps.IsButtonDown(Buttons.DPadUp))
                {
                    currentSpeed = speed / 1.5f;
                }
                else if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Down) || gps.IsButtonDown(Buttons.DPadRight) && gps.IsButtonDown(Buttons.DPadDown))
                {
                    currentSpeed = speed / 1.5f;
                }
                else if (ks.IsKeyDown(Keys.Left) && ks.IsKeyDown(Keys.Down) || gps.IsButtonDown(Buttons.DPadLeft) && gps.IsButtonDown(Buttons.DPadDown))
                {
                    currentSpeed = speed/ 1.5f;
                }
                else
                    currentSpeed = speed;
            }
            #endregion
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
            if (ks.IsKeyDown(Keys.Q) && oldKs.IsKeyUp(Keys.Q))
            {
                if (weaponState == WeaponState.Sword)
                {
                    weaponState = WeaponState.Bow;
                }
                else if (weaponState == WeaponState.Bow)
                {
                    weaponState = WeaponState.FireSpell;
                }
                else if (weaponState == WeaponState.FireSpell)
                    weaponState = WeaponState.Sword;
            }
            if (weaponState == WeaponState.Bow && ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space))
            {
                if (hasBow)
                {
                    Vector2 arrowVel = new Vector2();
                    string asset = "";
                    switch (direction)
                    {
                        case Direction.Left:
                            arrowVel = new Vector2(-10, 0);
                            asset = "Left";
                            break;
                        case Direction.Right:
                            arrowVel = new Vector2(10, 0);
                            asset = "Right";
                            break;
                        case Direction.Up:
                            arrowVel = new Vector2(0, -10);
                            asset = "Up";
                            break;
                        case Direction.Down:
                            arrowVel = new Vector2(0, 10);
                            asset = "Down";
                            break;
                    }
                    room.gameObjectsToAdd.Add(new Arrow(new Animation(Content, "arrow" + asset, 150, 1, false), Position, arrowVel, 2));
                }
            }
            if (weaponState == WeaponState.FireSpell && ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space) && mana > 0)
            {
                if (hasSpell)
                {
                    Vector2 spellVel = new Vector2();
                    mana -= 20;
                    string asset = "";
                    switch (direction)
                    {
                        case Direction.Left:
                            spellVel = new Vector2(-7, 0);
                            asset = "";
                            break;
                        case Direction.Right:
                            spellVel = new Vector2(7, 0);
                            asset = "";
                            break;
                        case Direction.Up:
                            spellVel = new Vector2(0, -7);
                            asset = "";
                            break;
                        case Direction.Down:
                            spellVel = new Vector2(0, 7);
                            asset = "";
                            break;
                    }
                    room.gameObjectsToAdd.Add(new Arrow(new Animation(Content, "fireball" + asset, 150, 1, false), Position, spellVel, 1));
                }
            }
            if (weaponState == WeaponState.Sword && hasSword)
            {
                if (gps.IsButtonDown(Buttons.RightTrigger) && oldgps.IsButtonUp(Buttons.RightTrigger) && !isAttacking
                    || (ks.IsKeyDown(Keys.Space) && oldKs.IsKeyUp(Keys.Space)) && !isAttacking)
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
                    foreach (GameObject go in gameObjects.Where(item => item is Enemy))
                    {
                        Enemy enemy = (Enemy)go;
                        if (attackRect.Intersects(go.HitBox) && enemy.isHurt == false)
                        {
                            if (enemy.state == 0)
                            {
                                enemy.hp -= 20;
                                enemy.isHurt = true;
                            }
                            if (enemy.canBeKnocked)
                            {
                                if (direction == Direction.Right)
                                    enemy.Velocity = new Vector2(40, 0);
                                else if (direction == Direction.Left)
                                    enemy.Velocity = new Vector2(-40, 0);
                                else if (direction == Direction.Up)
                                    enemy.Velocity = new Vector2(0, -40);
                                else if (direction == Direction.Down)
                                    enemy.Velocity = new Vector2(0, 40);
                            }
                            if (enemy.hp < 1)
                            {
                                int random = rnd.Next(10);
                                if (random == 1)
                                    room.gameObjectsToAdd.Add(new Drop(new Animation(Content, "hearth", 0, 1, false), enemy.Position, 1));
                                if (random == 2)
                                    room.gameObjectsToAdd.Add(new Drop(new Animation(Content, "key", 0, 1, false), enemy.Position, 2));
                                if (random == 3)
                                    room.gameObjectsToAdd.Add(new Drop(new Animation(Content, "bombDrop", 0, 1, false), enemy.Position, 3));
                                if (random > 3)
                                    room.gameObjectsToAdd.Add(new Drop(new Animation(Content, "manaBottle", 0, 1, false), enemy.Position,4));
                                go.isDead = true;
                                enemy.isDead = true;
                                xp += rnd.Next(20, 40);
                            }
                        }
                    }
                }
            }
            foreach(GameObject go in gameObjects.Where(item => item is Drop))
            {
                Drop drop = (Drop)go;
                if (drop.HitBox.Intersects(HitBox))
                {
                    if (drop.typeOfDrop == 1 && hp < maxHealth)
                        hp++;
                    else if (drop.typeOfDrop == 2)
                        numberOfKeys++;
                    else if (drop.typeOfDrop == 3)
                        numberOfBombs += rnd.Next(1, 4);
                    else if (drop.typeOfDrop == 4)
                    {
                        if (mana < 200)
                        mana += 60;
                        if (mana > 200)
                            mana = 200;
                    }
                    else if (drop.typeOfDrop == 11)
                    {
                        hasBow = true;
                    }

                    else if (drop.typeOfDrop == 12)
                    {
                        hasSpell = true;
                    }

                    else if (drop.typeOfDrop == 13)
                    {
                        hasSword = true;
                    }
                    go.isDead = true;
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
            if (!isHurt && !EnemiesIsHurt(gameObjects))
            {
                foreach (GameObject go in gameObjects.Where(item => item is Enemy))
                    if (HitBox.Intersects(go.HitBox))
                    {
                        isHurt = true;
                        float XDistance = (go.Position.X  + go.Animation.frameHeight/2) - (Position.X + currentAnimation.frameWidth/2);
                        float YDistance = (go.Position.Y + go.Animation.frameHeight/2) - (Position.Y + currentAnimation.frameHeight/2);
                        Velocity = new Vector2(20 *- (float)Math.Cos(Math.Atan2(YDistance, XDistance)), 20 * -(float)Math.Sin(Math.Atan2(YDistance, XDistance)));
                        if (hp > 0)
                        {
                            hp--;
                        }
                    }
            }
            if (isHurt == true)
                isHurtTimer++;
            if (isHurtTimer > 80)
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
            oldgps = gps;

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
        public bool ButtonDown(string button)
        {
            if (!Game1.isUsingGamePad)
            {
                if (button == "left")
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        return true;
                }
            }
            return false;
        }
    }
}
