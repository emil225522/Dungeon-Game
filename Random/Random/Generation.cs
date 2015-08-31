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

        public Texture2D wallDown;
        public Texture2D wallUp;
        public Texture2D wallRight; 
        public Texture2D wallLeft;


        public Texture2D doorDown;
        public Texture2D doorUp;
        public Texture2D doorRight;
        public Texture2D doorLeft;
        public Texture2D groundTile1;

        Texture2D cornerDown; 
        Texture2D cornerUp;
        Texture2D cornerRight; 
        Texture2D cornerLeft; 

        public void Generate(ContentManager Content,List<Tile> tiles, string mapName)
        {
            #region textures
            wallDown = Content.Load<Texture2D>("walls/wall1");
            wallUp = Content.Load<Texture2D>("walls/wall4");
            wallRight = Content.Load<Texture2D>("walls/wall2");
            wallLeft = Content.Load<Texture2D>("walls/wall3");

            cornerDown = Content.Load<Texture2D>("walls/corner4");
            cornerUp = Content.Load<Texture2D>("walls/corner1");
            cornerRight = Content.Load<Texture2D>("walls/corner3");
            cornerLeft = Content.Load<Texture2D>("walls/corner2");

            doorDown = Content.Load<Texture2D>("doors/doorDown");
            doorUp = Content.Load<Texture2D>("doors/doorUp");
            doorRight = Content.Load<Texture2D>("doors/doorRight");
            doorLeft = Content.Load<Texture2D>("doors/doorLeft");

            groundTile1 = Content.Load<Texture2D>("floor1");
            #endregion
            Random rnd = new Random();

            int[,] map;
            string mapData = mapName + ".txt";
            int width = 0;
            int height = File.ReadLines(mapData).Count();

            StreamReader sReader = new StreamReader(mapData);
            string line = sReader.ReadLine();
            string[] tileNo;
            tileNo = line.Split(',');

            width = tileNo.Count();

            map = new int[height, width];

            sReader = new StreamReader(mapData);

            for (int y = 0; y < height; y++)
            {
                line = sReader.ReadLine();
                tileNo = line.Split(',');
                width = tileNo.Count();
                for (int x = 0; x < width; x++)
                {
                    try
                    {
                        if (tileNo[x] != "" || tileNo[x] != " ")
                        {
                            #region AddTiles
                            if (tileNo[x] == ("11"))
                                tiles.Add(new Tile(groundTile1, new Vector2(x * 50, y * 50), 1));

                            else if (tileNo[x] == ("4"))
                                tiles.Add(new Tile(wallDown, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("3"))
                                tiles.Add(new Tile(wallUp, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("8"))
                                tiles.Add(new Tile(wallRight, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("7"))
                                tiles.Add(new Tile(wallLeft, new Vector2(x * 50, y * 50), 3));


                            else if (tileNo[x] == ("10"))
                                tiles.Add(new Tile(doorLeft, new Vector2(x * 50, y * 50), 2));
                            else if (tileNo[x] == ("13"))
                                tiles.Add(new Tile(doorUp, new Vector2(x * 50, y * 50), 2));
                            else if (tileNo[x] == ("14"))
                                tiles.Add(new Tile(doorDown, new Vector2(x * 50, y * 50), 2));
                            else if (tileNo[x] == ("9"))
                                tiles.Add(new Tile(doorRight, new Vector2(x * 50, y * 50), 2));


                            else if (tileNo[x] == ("1"))
                                tiles.Add(new Tile(cornerUp, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("2"))
                                tiles.Add(new Tile(cornerLeft, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("6"))
                                tiles.Add(new Tile(cornerRight, new Vector2(x * 50, y * 50), 3));
                            else if (tileNo[x] == ("5"))
                                tiles.Add(new Tile(cornerDown, new Vector2(x * 50, y * 50), 3));
                            #endregion
                        }
                    }
                    catch
                    {
                        Console.WriteLine("error that i cant fix");
                    }
                }
            }
            sReader.Close();
        }
    }
}

