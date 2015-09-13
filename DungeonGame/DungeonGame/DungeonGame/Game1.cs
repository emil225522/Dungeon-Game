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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();
        List<Tuple<String, int>> spawn = new List<Tuple<String, int>>();
        Room currentRoom;
        Texture2D blackBarTex;
        Texture2D hearthTex;
        Player player;
        Camera camera;
        SpriteFont font1;
        public FrameCounter _frameCounter = new FrameCounter();
        Random rnd = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 50*14;
            graphics.PreferredBackBufferWidth = 50*17;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            blackBarTex = Content.Load<Texture2D>("blackBar");
            hearthTex = Content.Load<Texture2D>("hearth");
            font1 = Content.Load<SpriteFont>("font1");

            player = new Player(new Vector2(200, 300), Content);

            spawn.Add(new Tuple<string,int>("slime", 3));

            CreateRoom(new Vector2(0, 0),new int[] {1,1,1,0},3);
            currentRoom = rooms[new Vector2(0, 0)];

            camera = new Camera(GraphicsDevice.Viewport, player);

            camera.transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-50,100, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            currentRoom.Update(gameTime, player);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                   BlendState.AlphaBlend,
                   null, null, null, null,
                   camera.transform);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0} {1}", _frameCounter.AverageFramesPerSecond, player.velocity);

            currentRoom.Draw(spriteBatch,player,gameTime);
            spriteBatch.Draw(blackBarTex, new Vector2(50,-100), Color.White);
            spriteBatch.DrawString(font1, "Keys: " + player.numberOfKeys, new Vector2(750, -100), Color.White);
            spriteBatch.DrawString(font1, "Bombs: " + player.numberOfBombs, new Vector2(750, 10), Color.White);
            spriteBatch.DrawString(font1, "Xp " + player.xp, new Vector2(750, -60), Color.White);
            spriteBatch.DrawString(font1, "Level " + player.level, new Vector2(750, -20), Color.White);
            for (int i = 0; i < player.health; i++)
                spriteBatch.Draw(hearthTex, new Vector2(200 * i / 5 + 50, -50), Color.White);

            spriteBatch.DrawString(font1, fps, new Vector2(51,-100), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public bool RoomExists(Vector2 pos) 
        {
            return rooms.ContainsKey(pos);
        }

        public void SetCurrentRoom(Vector2 position)
        {
            currentRoom = rooms[position];
        }

        public int[] CheckDoor(Vector2 roomPosition, int doorFrom, int doorToo, int[] doors)
        {
            if (rooms.ContainsKey(roomPosition))
            {
                Room roomNextdoor = rooms[roomPosition];

                if (roomNextdoor.doors[doorFrom] == 1)
                    doors[doorToo] = 1;
                else
                    doors[doorToo] = 0;
                return doors;
            }
            return doors;
        }

        public void CreateRoom(Vector2 position, int[] doors, sbyte fromRoom)
        {

            if (Math.Abs((int)position.X + (int)position.Y) < 6 && position.X != 0)
                spawn.Add(new Tuple<string, int>("swordEnemy", Math.Abs((int)position.X + (int)position.Y)* 10));
            else if (position.X == 0)
                spawn.Add(new Tuple<string, int>("blubaTower", Math.Abs((int)position.X + (int)position.Y)));
            else if (position.X == 0)
                spawn.Add(new Tuple<string, int>("bat", 2 * Math.Abs((int)position.X + (int)position.Y)));
            else
                spawn.Add(new Tuple<string, int>("bluba", Math.Abs((int)position.X + (int)position.Y)));
            //create a door where needded
            doors = CheckDoor(new Vector2(position.X + 1, position.Y), 0, 2, doors);
            doors = CheckDoor(new Vector2(position.X - 1, position.Y), 2, 0, doors);
            doors = CheckDoor(new Vector2(position.X, position.Y + 1), 1, 3, doors);
            doors = CheckDoor(new Vector2(position.X, position.Y - 1), 3, 1, doors);

            rooms.Add(position, new Room(this, Content, spawn, position, doors, fromRoom));
            spawn.Clear();
        }

    }
}
