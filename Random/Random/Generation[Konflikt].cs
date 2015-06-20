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

namespace Randomz
{
    public class Generation
    {
        public void Generate(ContentManager Content,List<Tile> tiles)
        {
            Texture2D grassTexture = Content.Load<Texture2D>("grass");
            Texture2D waterTexture = Content.Load<Texture2D>("water");
            Texture2D blockTexture = Content.Load<Texture2D>("block");
            Random rnd = new Random();

            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader("map1.txt"))
                while (!reader.EndOfStream) lines.Add(reader.ReadLine());
            for (int i = 0; i < lines.Count; i++)
            {
                String line = lines[i];
                lines[i] = line.Replace(",", "");
            }
            for (int y = 0; y < lines.Count; y++)
            {
                String line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    //lägg till blocks med position x, y.
                    if (line[x].Equals('1'))
                        tiles.Add(new Tile(grassTexture, new Vector2(x * 50, y * 50),1));
                    if (line[x].Equals('2'))
                        tiles.Add(new Tile(waterTexture, new Vector2(x * 50, y * 50),3));
                    if (line[x].Equals('3'))
                        tiles.Add(new Tile(blockTexture, new Vector2(x * 50, y * 50), 3));
                }
            }
            for (int i = 0; i < tiles.Count; i++)
            {

                if (tiles[i].type == 1 && rnd.Next(10) == 2)
                    tiles.Add(new Tile(Content.Load<Texture2D>("bush"), tiles[i].position, 4));
                else if (rnd.Next(500) == 3 && tiles[i].type == 1)
                    tiles.Add(new Tile(Content.Load<Texture2D>("tree"), tiles[i].position, 4));
            }

        }
    }
}

