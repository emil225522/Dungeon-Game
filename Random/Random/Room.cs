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
        int[] doors;
        Generation generation = new Generation();
        List<Tile> tiles = new List<Tile>();
        List<Enemy> enemies = new List<Enemy>();
        List<Projectile> blubbaball = new List<Projectile>();
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
            //if (roomPosition == new Vector2(0,0)) {
            //    generation.Generate(Content, tiles, "dunmap2");
            //} else
                generation.Generate(Content, tiles, "dunmap1");
                if (doors[0] > 0)
                    tiles.Add(new Tile(generation.doorLeft, new Vector2(0, 250), 1));
                else
                    tiles.Add(new Tile(generation.wallLeft, new Vector2(0, 250), 3));

                if (doors[1] > 0)
                    tiles.Add(new Tile(generation.doorUp, new Vector2(400, 0), 1));
                else
                    tiles.Add(new Tile(generation.wallUp, new Vector2(400, 0), 3));

                if (doors[2] > 0)
                    tiles.Add(new Tile(generation.doorRight, new Vector2(800, 250), 1));
                else
                    tiles.Add(new Tile(generation.wallRight, new Vector2(800, 250), 3));

                if (doors[3] > 0)
                    tiles.Add(new Tile(generation.doorDown, new Vector2(400, 500), 1));
                else
                    tiles.Add(new Tile(generation.wallDown, new Vector2(400, 500), 3));

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
            if (player.position.X < -50) {
                ExistOrCreate(Doors.Left);
                player.position.X = 50 * 18;
            }

            if (player.position.X > (50 * 18)) {
                ExistOrCreate(Doors.Right);
                player.position.X = -50;
            }

            if (player.position.Y < -50) {
                ExistOrCreate(Doors.Down);
                player.position.Y = 50 * 11;
            }

            if (player.position.Y > (50 * 11)) {
                ExistOrCreate(Doors.Up);
                player.position.Y = -50;
            }
            foreach (Projectile p in blubbaball) {
                p.Update();
            }
            foreach (Tile t in tiles)
            {
                t.Update();
            }
            foreach (Enemy e in enemies)
                e.Update(tiles, gameTime);
            player.Update(gameTime, tiles, enemies, Content);
        }

        public void Draw(SpriteBatch spriteBatch,Player player)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            foreach (Projectile p in blubbaball)
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
                    return new Bluba(Content, rnd.Next(), new Vector2(rnd.Next(100, 700), rnd.Next(100, 450)));
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
                switch(side)
                {
                    case Doors.Left:
                game.CreateRoom(nextRoom, new int[] {rnd.Next(0,2),rnd.Next(0,2),1,rnd.Next(0,2) });
                break;
                    case Doors.Up:
                game.CreateRoom(nextRoom, new int[] { rnd.Next(0, 2), 1, rnd.Next(0, 2), rnd.Next(0,2) });
                break;
                    case Doors.Right:
                game.CreateRoom(nextRoom, new int[] { 1, rnd.Next(0, 2), rnd.Next(0,2), rnd.Next(0, 2) });
                break;
                    case Doors.Down:
                game.CreateRoom(nextRoom, new int[] { rnd.Next(0, 2), rnd.Next(0,2), rnd.Next(0,2), 1 });
                break;
            }
            }

            game.SetCurrentRoom(nextRoom);
        }
    }
}
