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
        List<Tile> tiles = new List<Tile>();
        List<Room> rooms = new List<Room>();
        Texture2D blackBarTex;
        Texture2D hearthTex;
        Texture2D batTex;
        Player player;
        Camera camera;
        SpriteFont font1;
        public FrameCounter _frameCounter = new FrameCounter();
        Random rnd = new Random();
        List<Enemy> enemies = new List<Enemy>();
        Animation animation;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 50*14;
            graphics.PreferredBackBufferWidth = 50 * 17;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            Generation generation = new Generation();
            generation.Generate(Content,tiles);
            player = new Player(new Vector2(100,300),Content);
            blackBarTex = Content.Load<Texture2D>("blackBar");
            batTex = Content.Load<Texture2D>("enemy");
            animation = new Animation(Content, "attacks", 50, 6, true);
            hearthTex = Content.Load<Texture2D>("hearth");
            rooms.Add(new Room(10, Content, null));
           
            font1 = Content.Load<SpriteFont>("font1");

            camera = new Camera(GraphicsDevice.Viewport, player);
            EnterRoom();
            camera.transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(000, 150, 0));

            // TODO: use this.Content to load your game content here
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
            // TODO: Add your update logic here
            foreach (Room r in rooms)
            {
                r.Update(gameTime, player);
            }
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

            foreach (Room r in rooms)
            {
                r.Draw(spriteBatch, player);
            }
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animation.Draw(spriteBatch,new Vector2(200,200),Color.White);
            _frameCounter.Update(deltaTime);
            spriteBatch.Draw(blackBarTex, new Vector2(0,-150), Color.White);

            var fps = string.Format("FPS: {0} {1}", _frameCounter.AverageFramesPerSecond, player.velocity);
            for (int i = 0; i < player.health; i++)
            {
                spriteBatch.Draw(hearthTex, new Vector2(200 * i / 5 + 500, -100), Color.White);
            }

            spriteBatch.DrawString(font1, fps, new Vector2(1,-150), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public void EnterRoom()
        {
            enemies.Clear();
            player.health = 3;

           

        }
    }
}
