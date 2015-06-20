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
        Player player;
        Camera camera;
        SpriteFont font1;
        public FrameCounter _frameCounter = new FrameCounter();
        List<Tile> addTiles = new List<Tile>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            player = new Player(new Vector2(500,600),Content.Load<Texture2D>("playerDown"));
            font1 = Content.Load<SpriteFont>("font1");
            camera = new Camera(GraphicsDevice.Viewport, player);

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
            List<Tile> addTiles = new List<Tile>();

            foreach (Tile t in tiles)
                t.Update();
            player.Update(gameTime,tiles,this,Content);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (player.hitBox.Intersects(tiles[i].hitBox))
                {
                    player.test = true;
                }
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                 BlendState.AlphaBlend,
                 null, null, null, null,
                 camera.transform);

            
            foreach (Tile t in tiles)
            {
                    if (t.position.X < player.position.X + 500)
                    {
                        if (t.position.X > player.position.X - 500)
                        {
                            if (t.position.Y < player.position.Y + 400)
                            {
                                if (player.position.Y - t.position.Y < 500)
                                {
                                    t.Draw(spriteBatch);
                                }
                            }
                    }
                }
            }
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            spriteBatch.DrawString(font1, fps, new Vector2(player.position.X - 380,player.position.Y - 230), Color.Black);
            spriteBatch.DrawString(font1, "Bushes Shanked: " + player.bushesShanked, new Vector2(player.position.X - 380, player.position.Y - 200), Color.Black);
            player.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
