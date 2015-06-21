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

        public Room(int numberOfEnemies,ContentManager Content)
        {
            this.numberOfEnemies = numberOfEnemies;
            generation.Generate(Content,tiles);
            this.Content = Content;
            for (int i = 0; i < numberOfEnemies; i++)
            {
                enemies.Add(new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(300, 200), rnd.Next(), new Animation(Content, "bat", 100, 2, true)));
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
            //player.Update(gameTime, tiles, enemies, Content);
        }
        public void Draw(SpriteBatch spriteBatch,Player player)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            
        }

    }
}
