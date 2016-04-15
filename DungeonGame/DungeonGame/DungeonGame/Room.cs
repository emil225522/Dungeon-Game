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
        public int[] doors;
        public int numOfEnemies;
        public int level = 1;

        public List<GameObject> gameObjectsToAdd = new List<GameObject>();
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<Tile> tiles = new List<Tile>();
        public List<Drop> drops = new List<Drop>();
        Color color = Color.White;

        public RoomConstants.TypeOfRoom typeOfRoom;
        public Player player;
        public bool isDark;
        bool hasDroppedKey;

        Generation generation = new Generation();
        Random rnd = new Random();
        ContentManager Content;
        Game1 game;

        public Vector2 roomPosition;
        float rng = 0.1f;
        int BONUS_LOWER = 14;
        int BONUS_UPPER = 16;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition, int[] doors, int level)
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


                float num = (float)(Math.Pow(1.08, Game1.normalRow) * Math.Pow(0.9969, Game1.bonus) * (0.043 * rand));
                if (num < 1 && roomPosition.Length() < 5)
                {
                    Game1.normalRow++;
                    typeOfRoom = RoomConstants.TypeOfRoom.Normal;
                }
                else if (num > 1 && roomPosition.Length() < 5)
                {
                    Game1.bonus += 5;
                    Game1.normalRow = 0;
                    typeOfRoom = RoomConstants.TypeOfRoom.Bonus;
                }
                else
                {
                    int chance = 10;
                    chance -= (int)roomPosition.Length();
                    if (rnd.Next(chance) == 2)
                        typeOfRoom = RoomConstants.TypeOfRoom.Boss;
                    else
                    {
                        if (roomPosition.Length() > 10)
                            typeOfRoom = RoomConstants.TypeOfRoom.Boss;
                    }
                }

                Console.WriteLine(num);
            }
            if (typeOfRoom == RoomConstants.TypeOfRoom.Bonus)
                color = Color.Yellow;
            if (typeOfRoom == RoomConstants.TypeOfRoom.Puzzle)
                color = new Color(228, 0, 228);
            if (typeOfRoom == RoomConstants.TypeOfRoom.Boss)
                color = Color.Red; ;

            generation.Generate(Content, tiles, "map");
            for (int i = 0; i < doors.Length; i++)
            {
                int door = doors[i];
                if (door > 0)
                {
                    tiles.Add(new Archway(RoomConstants.DOOR_POSITIONS[i], Content, 2, (sbyte)i));
                    if (door == 1)
                        tiles.Add(new LockedDoor(RoomConstants.DOOR_POSITIONS[i], Content, 5, (sbyte)i));
                }
                else
                {
                    tiles.Add(new Wall(RoomConstants.DOOR_POSITIONS[i], Content, 3, (sbyte)i));
                }
            }

            for (int i = 0; i < tiles.Count; i++)
            {
                if (rnd.Next(-5, 5) == 2 && tiles[i].type == 1)
                    tiles.Add(new Tile(Content.Load<Texture2D>("crack"), tiles[i].position, 1, 0));
            }
            if (typeOfRoom == RoomConstants.TypeOfRoom.Normal)
            {
                for (int i = 0; i < spawn.Count; i++)
                {
                    for (int j = 0; j < spawn[i].Item2; j++)
                        gameObjects.Add(CreateMob(spawn[i].Item1));
                }
            }
            if (typeOfRoom == RoomConstants.TypeOfRoom.Bonus)
            {

                int randomNumber = rnd.Next(2);
                if (randomNumber == 1)
                {
                    for (int i = 0; i < rnd.Next(1, 5); i++)
                    {
                        gameObjects.Add(new Drop(new Animation(Content, "hearth", 0, 1, false), new Vector2(rnd.Next(200, 400), rnd.Next(200, 400)), 1));
                    }
                }
                else
                {
                    gameObjects.Add(new Drop(new Animation(Content, "hearthPlusOne", 0, 1, false), new Vector2(rnd.Next(200, 400), rnd.Next(200, 400)), 5));
                }
                randomNumber = rnd.Next(3);
                if (randomNumber == 0)
                    gameObjects.Add(new Drop(new Animation(Content, "bowPower", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)), 11));
                else if (randomNumber == 1)
                    gameObjects.Add(new Drop(new Animation(Content, "fireBallPower", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)), 12));
                gameObjects.Add(new Drop(new Animation(Content, "key", 0, 1, false), new Vector2(rnd.Next(200, 650), rnd.Next(200, 400)), 2));

            }

            if (typeOfRoom == RoomConstants.TypeOfRoom.Boss)
            {
                if (level == 1)
                    gameObjects.Add(new SlimeBoss(Content, rnd.Next(), new Vector2(475, 300),level));
                else if (level == 2)
                    gameObjects.Add(new Snake(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level));
                else if (level == 3)
                    gameObjects.Add(new FrogBoss(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level));
                else if (level == 4)
                    gameObjects.Add(new SlimeBoss(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level));
            }
        }

        public void Update(GameTime gameTime,Player player)
        {
            this.player = player;
            #region doorfunction
            if (player.Position.X < 0) 
            {
                ExistOrCreate(RoomConstants.Direction.Left);
                CleanUpProjectiles();
                player.Position = new Vector2(50 * 18,player.Position.Y);
            }

            if (player.Position.X > (50 * 18))
            {
                ExistOrCreate(RoomConstants.Direction.Right);
                CleanUpProjectiles();
                player.Position = new Vector2(0,player.Position.Y);
            }

            if (player.Position.Y < -20) 
            {
                ExistOrCreate(RoomConstants.Direction.Up);
                CleanUpProjectiles();
                player.Position = new Vector2(player.Position.X,50 * 11);
            }

            if (player.Position.Y > (50 * 11)) 
            {
                ExistOrCreate(RoomConstants.Direction.Down);
                CleanUpProjectiles();
                player.Position = new Vector2(player.Position.X,-20);
            }
            #endregion
            foreach(GameObject go in gameObjects.Where(item => item is Enemy))
            {
                Enemy enemy;
                enemy = (Enemy)go;
                if (go.isDead)
                {
                    numOfEnemies--;
                    gameObjectsToAdd.Add(new Ghost(new Animation(Content, "ghost", 100, 2, true, new Vector2(go.Animation.frameWidth,go.Animation.frameHeight)), go.Position, go.Animation.frameHeight));
                    if (enemy.isBoss)
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

            foreach(LockedDoor tile in tiles.Where(item => item is LockedDoor))
            {
                tile.isDeleted = doors[tile.direction] == RoomConstants.DOOR_OPEN;
            }

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

                if (ObjectIs<LockedDoor>(t))
                    doors[t.direction] = t.isDeleted ? RoomConstants.DOOR_OPEN : RoomConstants.DOOR_CLOSED;
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
                    return new Bat(Content, rnd.Next(), new Vector2(rnd.Next(100,700), rnd.Next(100, 450)),level);
                case "bluba":
                    return new Bluba(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "blubatower":
                    return new BlubaTower(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "slime":
                    return new Slime(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "fly":
                    return new Fly(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "swordenemy":
                    return new SwordEnemy(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "snake":
                    return new Snake(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
                case "swordFlower":
                    return new SwordFlower(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),level);
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
        private void ExistOrCreate(RoomConstants.Direction side)
        {
            int door = (int)side + 2;
            if (door > 3)
                door = door - 4;

            Vector2 nextRoom = new Vector2(roomPosition.X, roomPosition.Y);
            switch (side)
            {
                case RoomConstants.Direction.Down:
                    nextRoom.Y--;
                    break;
                case RoomConstants.Direction.Up:
                    nextRoom.Y++;
                    break;
                case RoomConstants.Direction.Left:
                    nextRoom.X--;
                    break;
                case RoomConstants.Direction.Right:
                    nextRoom.X++;
                    break;
            }

            if (!game.RoomExists(nextRoom))
            {
                game.CreateRoom(nextRoom, new int[] { rnd.Next(2), rnd.Next(2), rnd.Next(2), rnd.Next(2) }, door, level);
            }
            else
            {
                Room room = game.GetRoom(nextRoom);

                room.doors[door] = RoomConstants.DOOR_OPEN;
                game.SetRoom(nextRoom, room);
            }
            game.SetCurrentRoom(nextRoom);
        }

        public void CleanUpProjectiles()
        {
            foreach (GameObject go in gameObjects.Where(item => item is Projectile))
            {
                go.isDead = true;
            }
        }
    }
}
