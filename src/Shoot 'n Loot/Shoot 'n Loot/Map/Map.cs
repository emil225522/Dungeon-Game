using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Objects;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Map
    {
        public const byte width = 12, height = 12, maxZombies = 4; //number of chunks. width * Tile.size * chunk.size should equal the width of the map texture, same for height.

        public static Chunk[,] chunks { get; set; }

        public static List<Vector2> signPositions;

        public static void Initialize()
        {
            chunks = new Chunk[width, height];

            Color[,] mapData = loadTexture(TextureManager.map);
            Color[,] propData = loadTexture(TextureManager.propData);
            Color[,] spawnData = loadTexture(TextureManager.spawnData);

            signPositions = new List<Vector2>();

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    chunks[x, y] = new Chunk(Chunk.sizePx * new Vector2(x, y), subChunk(mapData, x * Chunk.size, y * Chunk.size, Chunk.size, Chunk.size), subChunk(propData, x * Chunk.size, y * Chunk.size, Chunk.size, Chunk.size), spawnData[x, y]);
                }
            }

            for (int t = 0; t < TextureManager.signs.Length; t++)
            {
                if (signPositions.Count == 0) break;
                int i = Game1.random.Next(signPositions.Count);
                SceneManager.gameScene.AddObject(new Sign(signPositions[i], TextureManager.signs[t]));
                signPositions.RemoveAt(i);
                Debug.WriteLine("adding sign with texture " + t + " at position " + i);
            }
        }

        public static bool TileAtPosIsWalkable(Vector2 position)
        {
            int x = (int)(position.X / Tile.size);
            int y = (int)(position.Y / Tile.size);
            int chunkX = x / Chunk.size;
            int chunkY = y / Chunk.size;
            try
            {
                return chunks[chunkX, chunkY].Tiles[x % Chunk.size, y % Chunk.size].Properties.IsWalkable;
            }
            catch { return false; }
        }

        static Color[,] subChunk(Color[,] source, int x, int y, int w, int h)
        {
            Color[,] c = new Color[w, h];
            for (int ix = 0; ix < w; ix++)
            {
                for (int iy = 0; iy < h; iy++)
                {
                    c[ix, iy] = source[ix + x, iy + y];
                }
            }
            return c;
        }

        static Color[,] loadTexture(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            Color[,] data = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    data[x, y] = colors1D[x + y * texture.Width];
                }
            }
            return data;
        }

        public static List<Chunk> VisibleChunks
        {
            get
            {
                List<Chunk> c = new List<Chunk>();
                foreach(Chunk ch in chunks) if(Camera.AreaIsVisible(ch.Position, Chunk.sizePx, Chunk.sizePx)) c.Add(ch);
                return c;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(Chunk c in VisibleChunks)
            {
                c.Draw(spriteBatch);
            }
        }
    }
}
