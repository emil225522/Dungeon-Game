using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    class Room
    {
        public enum Doors 
        {
            Left,
            Up,
            Right,
            Down
        }
        public int[] doors;
        int test;
        public int numOfEnemies;
        public int level = 1;
        Generation generation = new Generation();
        public List<GameObject> gameObjectsToAdd = new List<GameObject>();
        public List<GameObject> gameObjects = new List<GameObject>();
        List<Enemy> enemiesList = new List<Enemy>();
        public List<Tile> tiles = new List<Tile>();
        public List<Drop> drops = new List<Drop>();
        Random random = new Random();
        Color color = Color.White;
        public enum TypeOfRoom
        {
            Boss = 0,
            Normal = 1,
            Bonus = 2,
            Empty = 3,
            Puzzle = 4

        }
        public TypeOfRoom typeOfRoom;
        public bool isDark;
        bool hasDroppedKey;
        Random rnd = new Random();
        ContentManager Content;
        public Player player;
        Game1 game;
        public Vector2 roomPosition;
        float rng = 0.1f;
        int BONUS_LOWER = 14;
        int BONUS_UPPER = 16;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition, int[] doors, sbyte fromRoom, int level)
        {
            this.game = game;
            this.Content = Content;
            this.roomPosition = roomPosition;
            this.doors = doors;
            this.level = level;
            numOfEnemies = spawn[0].Item2;
            if (roomPosition != Vector2.Zero)
            {
                int rand = rnd.Next(20);


               float num = (float) (Math.Pow(1.08, Game1.normalRow) * Math.Pow(0.9969, Game1.bonus) * (0.043 * rand));
               if (num < 1)
               {
                   Game1.normalRow++;
                   typeOfRoom = TypeOfRoom.Normal;
               }
               else if (num > 1 && roomPosition.Length() < 5)
               {
                   Game1.bonus += 5;
                   Game1.normalRow = 0;
                   typeOfRoom = TypeOfRoom.Bonus;
               }
               else
               {
                   if (rnd.Next(1, 5) == 2)
                       typeOfRoom = TypeOfRoom.Boss;
               }

               Console.WriteLine(num);
            }
            if (typeOfRoom == TypeOfRoom.Bonus)
                color = Color.Yellow;
            if (typeOfRoom == TypeOfRoom.Puzzle)
                color = new Color(228, 0, 228);

            generation.Generate(Content, tiles, "map",(int)typeOfRoom);
            #region CreateDoorOrWall
            if (doors[0] == 1)
            {
                tiles.Add(new Tile(generation.doorLeft, new Vector2(50, 300), 2));
                if (fromRoom != 2)
                    tiles.Add(new LockedDoor(new Vector2(50, 300), Content, 5));
            }
            else
                tiles.Add(new Tile(generation.wallLeft, new Vector2(50, 300), 3));

            if (doors[1] == 1)
            {
                tiles.Add(new Tile(generation.doorUp, new Vector2(450, 50), 2));
                if (fromRoom != 1)
                    tiles.Add(new LockedDoor(new Vector2(450, 50), Content, 6));
            }
            else
                tiles.Add(new Tile(generation.wallUp, new Vector2(450, 50), 3));

            if (doors[2] == 1)
            {
                tiles.Add(new Tile(generation.doorRight, new Vector2(850, 300), 2));
                if (fromRoom != 0)
                    tiles.Add(new LockedDoor(new Vector2(850, 300), Content, 7));
            }
            else
                tiles.Add(new Tile(generation.wallRight, new Vector2(850, 300), 3));

            if (doors[3] == 1)
            {
                tiles.Add(new Tile(generation.doorDown, new Vector2(450, 550), 2));
                if (fromRoom != 3)
                    tiles.Add(new LockedDoor(new Vector2(450, 550), Content, 8));
            }
            else
                tiles.Add(new Tile(generation.wallDown, new Vector2(450, 550), 3));
            #endregion


            for (int i = 0; i < tiles.Count; i++)
            {
                //removed random number value
                if (rnd.Next(-20, 20) == 5 && tiles[i].type == 1)
                {
                    Vector2 tempPos = new Vector2();
                    tempPos = tiles[i].position;
                    tiles.RemoveAt(i);
                    //tiles.Add(new Tile(Content.Load<Texture2D>("hole"), tempPos, 3));

                }
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                if (rnd.Next(-5, 5) == 2 && tiles[i].type == 1)
                    tiles.Add(new Tile(Content.Load<Texture2D>("crack"), tiles[i].position, 1));
            }
            if (typeOfRoom == TypeOfRoom.Normal)
            {
                for (int i = 0; i < spawn.Count; i++)
                {
                    for (int j = 0; j < spawn[i].Item2; j++)
                        gameObjects.Add(CreateMob(spawn[i].Item1));
                }
            }
            if (typeOfRoom == TypeOfRoom.Bonus)
            {
               
                    int randomNumber = rnd.Next(2);
                    if (randomNumber == 1)
                    {
                        for (int i = 0; i < random.Next(1, 5); i++)
                        {
                            gameObjects.Add(new Drop(new Animation(Content, "hearth", 0, 1, false), new Vector2(rnd.Next(200, 400), rnd.Next(200, 400)), 1));
                        }
                    }
                    else
                    {
                        gameObjects.Add(new Drop(new Animation(Content, "hearthPlusOne", 0, 1, false), new Vector2(rnd.Next(200, 400), rnd.Next(200, 400)), 5));
                    }
                 if (random.Next(3) == 0)
                     gameObjects.Add(new Drop(new Animation(Content, "bowPower", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)),11));
                 else if (random.Next(3) == 1)
                     gameObjects.Add(new Drop(new Animation(Content, "fireBallPower", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)), 12));
                else
                     gameObjects.Add(new Drop(new Animation(Content, "cube", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)), 13));
            }
            if (typeOfRoom == TypeOfRoom.Boss)
            { 
                if (level == 1)
                    gameObjects.Add(new SlimeBoss(Content, rnd.Next(), new Vector2(475,300)));
                else if (level == 2)
                    gameObjects.Add(new Snake(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450))));
                else if (level == 3)
                    gameObjects.Add(new FrogBoss(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450))));
                else if (level == 4)
                    gameObjects.Add(new SlimeBoss(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450))));

                color = Color.Red;
            }
            if (typeOfRoom == TypeOfRoom.Puzzle)
            {
                tiles.Add(new PuzzleBlock(Content.Load<Texture2D>("cube"), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),2,1));

                tiles.Add(new PuzzleBlock(Content.Load<Texture2D>("cube"), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)), 2,1));
                tiles.Add(new PuzzleBlock(Content.Load<Texture2D>("cube"), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)), 2, 1));
                tiles.Add(new PuzzleBlock(Content.Load<Texture2D>("cube"), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)), 2, 1));
            }
        }

        public void Update(GameTime gameTime,Player player)
        {
            this.player = player;
            #region doorfunction
            if (player.Position.X < 0) 
            {
                ExistOrCreate(Doors.Left);
                player.Position = new Vector2(50 * 18,player.Position.Y);
            }

            if (player.Position.X > (50 * 18))
            {
                ExistOrCreate(Doors.Right);
                player.Position = new Vector2(0,player.Position.Y);
            }

            if (player.Position.Y < -20) 
            {
                ExistOrCreate(Doors.Down);
                player.Position = new Vector2(player.Position.X,50 * 11);
            }

            if (player.Position.Y > (50 * 11)) 
            {
                ExistOrCreate(Doors.Up);
                player.Position = new Vector2(player.Position.X,-20);
            }
            #endregion
            foreach(GameObject go in gameObjects.Where(item => item is Enemy))
            {
                enemiesList.Add((Enemy)go);
                if (go.isDead)
                {
                    numOfEnemies--;
                    gameObjectsToAdd.Add(new Ghost(new Animation(Content, "ghost", 100, 2, true, new Vector2(go.Animation.frameWidth,go.Animation.frameHeight)), go.Position, go.Animation.frameHeight));
                    if (ObjectIs<Snake>(go))
                        gameObjectsToAdd.Add(new Stair(new Animation(Content,"stair",0,1,false), new Vector2(450, 300), 1));
                }
            }
            if (numOfEnemies == 0 && !hasDroppedKey)
            {
                hasDroppedKey = true;
                gameObjects.Add(new Drop(new Animation(Content, "key", 0, 1, false), new Vector2(500, 500), 2));
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].isDeleted)
                    tiles.RemoveAt(i);
            }
            foreach (GameObject go in gameObjects)
            {
                if (go != null && go.HitBox.Intersects(player.HitBox) && ObjectIs<Stair>(go))
                {
                    if (level < 4)
                        game.UpLevel(level);
                    else
                    {
                        game.Win();
                    }
                }
            }
            if (player.hp <= 0)
                game.GameOver();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].isDead)
                {
                    gameObjects.RemoveAt(i);
                }
            }
            foreach (GameObject g in gameObjects)
                g.Update(gameTime,this);
            foreach (GameObject go in gameObjectsToAdd)
                gameObjects.Add(go);
            gameObjectsToAdd.Clear();
            foreach (Tile t in tiles)
            {
                t.Update(gameTime,player);
            }
            player.Update(gameTime, tiles, Content,this,gameObjects);
        }

        public void Draw(SpriteBatch spriteBatch,Player player,GameTime gameTime)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch,color);
            }

            foreach (Drop d in drops)
            {
                d.Draw(spriteBatch);
            }
            foreach (GameObject g in gameObjects)
                g.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }

        private Enemy CreateMob(String mob)
        {
            switch(mob)
            {
                case "bat":
                    return new Bat(Content, rnd.Next(), new Vector2(rnd.Next(100,700), rnd.Next(100, 450)));
                case "bluba":
                    return new Bluba(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "blubatower":
                    return new BlubaTower(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "slime":
                    return new Slime(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "fly":
                    return new Fly(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "swordenemy":
                    return new SwordEnemy(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "snake":
                    return new Snake(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                default:
                    return null;
            }
        }
        public bool ObjectIs <T>(object v)
        {
            if (v is T)
                return true;
            else
                return false;
        }
        private void ExistOrCreate(Doors side)
        {
            
            Vector2 nextRoom = new Vector2(roomPosition.X, roomPosition.Y);
            switch (side)
            {
                case Doors.Down:
                    nextRoom.Y--;
                    break;
                case Doors.Up:
                    nextRoom.Y++;
                    break;
                case Doors.Left:
                    nextRoom.X--;
                    break;
                case Doors.Right:
                    nextRoom.X++;
                    break;
            }

            if (!game.RoomExists(nextRoom))
            {
                int[] doors = { 0 };
                sbyte fromRoom = 0;
                sbyte randomFactor;
                if (roomPosition.Length() < 5)
                    randomFactor = 2;
                else
                    randomFactor = 6;

                switch (side)
                {
                    case Doors.Left:
                        fromRoom = 0;
                        doors = new int[] { rnd.Next(0, randomFactor), rnd.Next(0, randomFactor), 1, rnd.Next(0, randomFactor) };
                        break;
                    case Doors.Up:
                        fromRoom = 1;
                        doors = new int[] { rnd.Next(0, randomFactor), 1, rnd.Next(0,randomFactor), rnd.Next(0, randomFactor) };
                        break;
                    case Doors.Right:
                        fromRoom = 2;
                        doors = new int[] { 1, rnd.Next(0, randomFactor), rnd.Next(0, randomFactor), rnd.Next(0, randomFactor) };
                        break;
                    case Doors.Down:
                        fromRoom = 3;
                        doors = new int[] { rnd.Next(0, randomFactor), rnd.Next(0, randomFactor), rnd.Next(0, randomFactor), 1 };
                        break;

                }
                game.CreateRoom(nextRoom, doors, fromRoom,level);
            }
            game.SetCurrentRoom(nextRoom);
        }
    }
}
