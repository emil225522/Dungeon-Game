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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();
        List<Tuple<String, int>> spawn = new List<Tuple<String, int>>();
        List<Projectile> blubbaball = new List<Projectile>();
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

            player = new Player(new Vector2(100, 300), Content);

            spawn.Add(new Tuple<string,int>("bluba", 3));

            CreateRoom(new Vector2(0, 0),new int[] {1,1,1,0});
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
           
            foreach (Projectile p in blubbaball)
            {
                p.Update();
            }

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

            currentRoom.Draw(spriteBatch,player);
            spriteBatch.Draw(blackBarTex, new Vector2(50,-100), Color.White);
            spriteBatch.DrawString(font1, "Keys: " + player.numberOfKeys, new Vector2(750, -100), Color.White);
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

        public void CreateRoom(Vector2 position, int[] doors)
        {
            spawn.Add(new Tuple<string, int>("bat", 10));

            if (rooms.ContainsKey(new Vector2(position.X + 1, position.Y)))
            {
                Room roomRight;
                if (rooms.TryGetValue(new Vector2(position.X + 1, position.Y), out roomRight))
                    Console.Write("");

                if (roomRight.doors[0] == 1)
                    doors[2] = 1;
                else
                    doors[2] = 0;

            }

            if (rooms.ContainsKey(new Vector2(position.X - 1, position.Y)))
            {
                Room roomLeft;
                if (rooms.TryGetValue(new Vector2(position.X - 1, position.Y), out roomLeft))
                    Console.Write("");

                if (roomLeft.doors[2] == 1)
                    doors[0] = 1;
                else
                    doors[0] = 0;

            }

            if (rooms.ContainsKey(new Vector2(position.X, position.Y + 1)))
            {
                Room roomDown;
                if (rooms.TryGetValue(new Vector2(position.X, position.Y + 1), out roomDown))
                    Console.Write("");

                if (roomDown.doors[1] == 1)
                    doors[3] = 1;
                else
                    doors[3] = 0;

            }
            if (rooms.ContainsKey(new Vector2(position.X, position.Y - 1)))
            {
                Room roomUp;
                if (rooms.TryGetValue(new Vector2(position.X, position.Y - 1), out roomUp))
                    Console.Write("");

                if (roomUp.doors[3] == 1)
                    doors[1] = 1;
                else
                    doors[1] = 0;

            }
            rooms.Add(position, new Room(this, Content, spawn, position, doors));
            spawn.Clear();
        }

    }
}
