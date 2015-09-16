using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        Generation generation = new Generation();
        public List<GameObject> gameObjectsToAdd = new List<GameObject>();
        public List<GameObject> gameObjects = new List<GameObject>();
        public List<Tile> tiles = new List<Tile>();
        public List<Drop> drops = new List<Drop>();
       // public List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();
        Color color = Color.White;
        public bool isDark;
        Random rnd = new Random();
        ContentManager Content;
        public Player player;
        Game1 game;
        public Vector2 roomPosition;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition, int[] doors, sbyte fromRoom)
        {
            this.game = game;
            this.Content = Content;
            this.roomPosition = roomPosition;
            this.doors = doors;
            int randomValue = random.Next(1, 5);
            if (randomValue == 1)
                color = Color.Red;
            else if (randomValue == 2)
                color = Color.Turquoise;
            else if (randomValue == 3)
                color = Color.Yellow;
            else if (randomValue == 4)
                if (rnd.Next(1,2) == 1)
                    isDark = true;
                color = new Color(random.Next(60, 255), random.Next(60, 255), random.Next(60, 255));
            generation.Generate(Content, tiles, "map");
            #region CreateDoorOrWall
            if (doors[0] == 1)
            {
                tiles.Add(new Tile(generation.doorLeft, new Vector2(50, 300), 2));
                if (fromRoom != 2)
                tiles.Add(new LockedDoor(new Vector2(50, 300), Content,5));
            }
            else
                tiles.Add(new Tile(generation.wallLeft, new Vector2(50, 300), 3));

            if (doors[1] == 1)
            {
                tiles.Add(new Tile(generation.doorUp, new Vector2(450, 50), 2));
                if (fromRoom != 1)
                tiles.Add(new LockedDoor(new Vector2(450, 50), Content,6));
            }
            else
                tiles.Add(new Tile(generation.wallUp, new Vector2(450, 50), 3));

            if (doors[2] == 1)
            {
                tiles.Add(new Tile(generation.doorRight, new Vector2(850, 300), 2));
                if (fromRoom != 0)
                tiles.Add(new LockedDoor(new Vector2(850, 300), Content,7));
            }
            else
                tiles.Add(new Tile(generation.wallRight, new Vector2(850, 300), 3));

            if (doors[3] == 1)
            {
                tiles.Add(new Tile(generation.doorDown, new Vector2(450, 550), 2));
                if (fromRoom != 3)
                tiles.Add(new LockedDoor(new Vector2(450, 550), Content,8));
            }
            else
                tiles.Add(new Tile(generation.wallDown, new Vector2(450, 550), 3));
            #endregion

            for (int i = 0; i < tiles.Count; i++)
            {
                if (rnd.Next(-5, 5) == 2 && tiles[i].type == 1)
                    tiles.Add(new Tile(Content.Load<Texture2D>("crack"), tiles[i].position, 1));
            }
            for (int i = 0; i < spawn.Count; i++)
            {
                for (int j = 0; j < spawn[i].Item2; j++)
                    gameObjects.Add(CreateMob(spawn[i].Item1));
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
            foreach(GameObject go in gameObjects.Where(item => item.GetType().Name == "Slime"))
            {
                if (go.isDead)
                {
                    gameObjects.Add(new Ghost(new Animation(Content, "ghost", 100, 2, true), go.Position));
                    gameObjects.Remove(go);
                }
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].isDeleted)
                    tiles.RemoveAt(i);
            }
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].isDead)
                    gameObjects.RemoveAt(i);
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
                case "blubaTower":
                    return new BlubaTower(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "slime":
                    return new Slime(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "fly":
                    return new Fly(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                case "swordEnemy":
                    return new SwordEnemy(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
                default:
                    return null;
            }
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
                sbyte i = 3;

                switch (side)
                {
                    case Doors.Left:
                        fromRoom = 0;
                        doors = new int[] { rnd.Next(0, i), rnd.Next(0, i), 1, rnd.Next(0, i) };
                        break;
                    case Doors.Up:
                        fromRoom = 1;
                        doors = new int[] { rnd.Next(0, i), 1, rnd.Next(0,i), rnd.Next(0, i) };
                        break;
                    case Doors.Right:
                        fromRoom = 2;
                        doors = new int[] { 1, rnd.Next(0, i), rnd.Next(0, i), rnd.Next(0, i) };
                        break;
                    case Doors.Down:
                        fromRoom = 3;
                        doors = new int[] { rnd.Next(0, i), rnd.Next(0, i), rnd.Next(0, i), 1 };
                        break;

                }
                game.CreateRoom(nextRoom, doors, fromRoom);
            }
            game.SetCurrentRoom(nextRoom);
        }
    }
}
