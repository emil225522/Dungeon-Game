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
        int numberOfEnemies;
        Generation generation = new Generation();
        List<Tile> tiles = new List<Tile>();
        List<Enemy> enemies = new List<Enemy>();
        Random rnd = new Random();
        ContentManager Content;

        public Room(int numberOfEnemies, ContentManager Content, Dictionary<String, int>[] spawn)
        {
            this.numberOfEnemies = numberOfEnemies;
            generation.Generate(Content,tiles);
            this.Content = Content;

            for (int i = 0; i < numberOfEnemies; i++)
            {
                enemies.Add(createMob("bat"));
            }
        }

        public void Update(GameTime gameTime,Player player)
        {
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
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
        }

        private Enemy createMob(String mob)
        {
            switch(mob)
            {
                case "bat":
                    return new Bat(Content, rnd.Next(), new Vector2(rnd.Next(100,700), rnd.Next(100, 500)));
                default:
                    return null;
            }
        }
    }
}
