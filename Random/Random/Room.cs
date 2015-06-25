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
using Microsoft.Xna.Framework.Net;
namespace Randomz
{
    class Room
    {
        public enum Doors {
            Left,
            Up,
            Right,
            Down
        }
        public int[] doors;
        Generation generation = new Generation();
        List<Tile> tiles = new List<Tile>();
        List<Enemy> enemies = new List<Enemy>();
        public List<Projectile> blubaBall = new List<Projectile>();
        Random rnd = new Random();
        ContentManager Content;
        Game1 game;
        Vector2 roomPosition;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition, int[] doors)
        {
            this.game = game;
            this.Content = Content;
            this.roomPosition = roomPosition;
            this.doors = doors;

                generation.Generate(Content, tiles, "dunmap1");
                #region CreateDoorOrWall
                if (doors[0] > 0)
                    tiles.Add(new Tile(generation.doorLeft, new Vector2(50, 300), 1));
                else
                    tiles.Add(new Tile(generation.wallLeft, new Vector2(50, 300), 3));

                if (doors[1] > 0)
                    tiles.Add(new Tile(generation.doorUp, new Vector2(450, 50), 1));
                else
                    tiles.Add(new Tile(generation.wallUp, new Vector2(450, 50), 3));

                if (doors[2] > 0)
                    tiles.Add(new Tile(generation.doorRight, new Vector2(850, 300), 1));
                else
                    tiles.Add(new Tile(generation.wallRight, new Vector2(850, 300), 3));

                if (doors[3] > 0)
                    tiles.Add(new Tile(generation.doorDown, new Vector2(450, 550), 1));
                else
                    tiles.Add(new Tile(generation.wallDown, new Vector2(450, 550), 3));
                #endregion

                for (int i = 0; i < tiles.Count; i++)
			{
                if (rnd.Next(-5,5) == 2 && tiles[i].type == 1)
                tiles.Add(new Tile(Content.Load<Texture2D>("bush"), tiles[i].position, 1));
			}
            for(int i = 0; i < spawn.Count; i++)
            {
                for (int j = 0; j < spawn[i].Item2; j++)
                    enemies.Add(CreateMob(spawn[i].Item1));
            }
        }

        public void Update(GameTime gameTime,Player player)
        {
            if (player.position.X < 0) 
            {
                ExistOrCreate(Doors.Left);
                player.position.X = 50 * 18;
            }

            if (player.position.X > (50 * 18))
            {
                ExistOrCreate(Doors.Right);
                player.position.X = 0;
            }

            if (player.position.Y < -20) 
            {
                ExistOrCreate(Doors.Down);
                player.position.Y = 50 * 11;
            }

            if (player.position.Y > (50 * 11)) 
            {
                ExistOrCreate(Doors.Up);
                player.position.Y = -20;
            }
            foreach (Projectile p in blubaBall)
            {
                p.Update();
            }
            foreach (Tile t in tiles)
            {
                t.Update();
            }
            foreach (Enemy e in enemies)
                e.Update(tiles, gameTime,this);
            player.Update(gameTime, tiles, enemies, Content);
        }

        public void Draw(SpriteBatch spriteBatch,Player player)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            for (int i = 0; i < blubaBall.Count; i++)
            {
                if (blubaBall[i].hitBox.Intersects(player.hitBox))
                {
                    player.health--;
                    blubaBall.RemoveAt(i);
                    player.isHurt = true;
                }
            }
            foreach (Projectile p in blubaBall)
            {
                p.Draw(spriteBatch);
            }
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
        }

        private Enemy CreateMob(String mob)
        {
            switch(mob)
            {
                case "bat":
                    return new Bat(Content, rnd.Next(), new Vector2(rnd.Next(100,700), rnd.Next(100, 450)));
                case "bluba":
                    return new Bluba(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)),Content.Load<Texture2D>("blubaball"));
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
                int[] doors = {0};
                switch (side)
                {
                    case Doors.Left:
                        doors = new int[] { rnd.Next(0, 2), rnd.Next(0, 2), 1, rnd.Next(0, 2) };
                        break;
                    case Doors.Up:
                        doors = new int[] { rnd.Next(0, 2), 1, rnd.Next(0, 2), rnd.Next(0, 2) };
                        break;
                    case Doors.Right:
                        doors = new int[] { 1, rnd.Next(0, 2), rnd.Next(0, 2), rnd.Next(0, 2) };
                        break;
                    case Doors.Down:
                        doors = new int[] { rnd.Next(0, 2), rnd.Next(0, 2), rnd.Next(0, 2), 1 };
                        break;

                }
                game.CreateRoom(nextRoom, doors);
            }

            game.SetCurrentRoom(nextRoom);
        }
    }
}
