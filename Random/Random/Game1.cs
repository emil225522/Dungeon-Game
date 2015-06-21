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
        List<Room> rooms = new List<Room>();
        Room currentRoom;
        Texture2D blackBarTex;
        Texture2D hearthTex;
        Player player;
        Camera camera;
        SpriteFont font1;
        public FrameCounter _frameCounter = new FrameCounter();
        Random rnd = new Random();
        Animation animation;

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
            animation = new Animation(Content, "attacks", 50, 6, true);

            player = new Player(new Vector2(100, 300), Content);

            List<Tuple<String, int>> spawn = new List<Tuple<String, int>>();
            spawn.Add(new Tuple<string,int>("bat", 10));

            rooms.Add(new Room(Content, spawn));
            currentRoom = rooms[0];

            camera = new Camera(GraphicsDevice.Viewport, player);
            EnterRoom();
            camera.transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(000, 150, 0));
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

            animation.PlayAnim(gameTime);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0} {1}", _frameCounter.AverageFramesPerSecond, player.velocity);

            currentRoom.Draw(spriteBatch,player);
            animation.Draw(spriteBatch,new Vector2(200,200),Color.White);
            spriteBatch.Draw(blackBarTex, new Vector2(0,-150), Color.White);

            for (int i = 0; i < player.health; i++)
                spriteBatch.Draw(hearthTex, new Vector2(200 * i / 5 + 500, -100), Color.White);

            spriteBatch.DrawString(font1, fps, new Vector2(1,-150), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void EnterRoom()
        {
            player.health = 7;
        }
    }
}
