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

        Generation generation = new Generation();
        List<Tile> tiles = new List<Tile>();
        List<Enemy> enemies = new List<Enemy>();
        List<Projectile> blubbaball = new List<Projectile>();
        Random rnd = new Random();
        ContentManager Content;
        Game1 game;
        Vector2 roomPosition;

        public Room(Game1 game, ContentManager Content, List<Tuple<String, int>> spawn, Vector2 roomPosition)
        {
            this.game = game;
            this.Content = Content;
            this.roomPosition = roomPosition;

            if (roomPosition == new Vector2(0,0)) {
                generation.Generate(Content, tiles, "dunmap2");
            } else
                generation.Generate(Content, tiles, "dunmap1");

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

            if (player.position.Y > 50 * 11) {
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

        private void ExistOrCreate(Doors side){
            Vector2 nextRoom = new Vector2(roomPosition.X, roomPosition.Y);
            switch (side) {
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
                game.CreateRoom(nextRoom);
            game.SetCurrentRoom(nextRoom);
        }
    }
}
