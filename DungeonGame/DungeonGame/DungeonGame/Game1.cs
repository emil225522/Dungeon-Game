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
        KeyboardState oldKs = Keyboard.GetState();
        static public int bonus;
        static public int normalRow;
        int menuSelectedOption;
        Dictionary<Vector2, Room> rooms = new Dictionary<Vector2, Room>();
        List<Tuple<String, int>> spawn = new List<Tuple<String, int>>();
        public static bool isUsingGamePad;
        Room currentRoom;
        bool menuIsOpen;
        enum GameState
        {
            Start,
            Play,
            Pause,
            GameOver,
            Win
        }
        GameState gameState;
        Texture2D blackBarTex;
        Texture2D hearthTex;
        Texture2D yellowHighlight;
        Texture2D manaBarTex;
        Player player;
        Camera camera;
        SpriteFont font1;
        public FrameCounter _frameCounter = new FrameCounter();
        Random rnd = new Random();
        Texture2D mapTexture;
        Vector2 testposition;
        public static ContentManager content;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 50*14;
            graphics.PreferredBackBufferWidth = 50*17;
            content = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.Start;
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
            manaBarTex = content.Load<Texture2D>("manaBar");
            font1 = Content.Load<SpriteFont>("font1");
            mapTexture = Content.Load<Texture2D>("towerUnder");
            yellowHighlight = Content.Load<Texture2D>("yellowHighlight");
            player = new Player(new Vector2(200, 300), Content);
            testposition = new Vector2();
            spawn.Add(new Tuple<string,int>("fly", 2));

            CreateRoom(new Vector2(0, 0),new int[] {1,1,1,0},3,1);
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
            if (gameState == GameState.Play && ks.IsKeyDown(Keys.P) && oldKs.IsKeyUp(Keys.P))
                gameState = GameState.Pause;
            else if (gameState == GameState.Pause && ks.IsKeyDown(Keys.P) && oldKs.IsKeyUp(Keys.P))
                gameState = GameState.Play;
            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            if (gameState == GameState.Play)
            {
                if (ks.IsKeyDown(Keys.Z))
                    menuIsOpen = true;
                else
                    menuIsOpen = false;
                currentRoom.Update(gameTime, player);
            }
            else if (gameState == GameState.GameOver)
            {
                if (ks.IsKeyDown(Keys.Enter) || ks.IsKeyDown(Keys.Space))
                    gameState = GameState.Play;
            }
            else if (gameState == GameState.Win)
            {
                if (ks.IsKeyDown(Keys.Enter))
                    GameOver();
            }
            else if (gameState == GameState.Start)
            {
                if (ks.IsKeyDown(Keys.Down) && oldKs.IsKeyUp(Keys.Down))
                    menuSelectedOption++;
                if (ks.IsKeyDown(Keys.Up) && oldKs.IsKeyUp(Keys.Up))
                    menuSelectedOption--;

                if (menuSelectedOption < 0)
                    menuSelectedOption = 3;
                if (menuSelectedOption > 3)
                    menuSelectedOption = 0;


                if (ks.IsKeyDown(Keys.Enter))
                {
                    if (menuSelectedOption == 0)
                        gameState = GameState.Play;
                    else if (menuSelectedOption == 3)
                        Environment.Exit(1);
                }

            }
            camera.Update(gameTime);
            oldKs = ks;
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
                if (gameState == GameState.Play)
                {
                    var fps = string.Format("FPS: {0} {1} {2}", _frameCounter.AverageFramesPerSecond, currentRoom.roomPosition, currentRoom.roomPosition.Length());

                    currentRoom.Draw(spriteBatch, player, gameTime);
                    spriteBatch.Draw(blackBarTex, new Vector2(50, -100), Color.White);
                    spriteBatch.DrawString(font1, "Keys: " + player.numberOfKeys, new Vector2(600, -100), Color.White);
                    spriteBatch.DrawString(font1, "Bombs: " + player.numberOfBombs, new Vector2(600, -60), Color.White);
                    spriteBatch.DrawString(font1, "Level " + currentRoom.level, new Vector2(600, -20), Color.White);
                    spriteBatch.Draw(manaBarTex, new Rectangle(60, 0, player.mana, 25), Color.White);
                    spriteBatch.Draw(Content.Load<Texture2D>("equipBar"), new Vector2(400, -50), Color.White);
                    if (player.hasBow)
                    spriteBatch.Draw(Content.Load<Texture2D>("bowPower"), new Vector2(435, -49), Color.White);
                    if (player.hasSpell)
                    spriteBatch.Draw(Content.Load<Texture2D>("FireBallPower"), new Vector2(472, -49), Color.White);
                    if (player.hasSword)
                        spriteBatch.Draw(Content.Load<Texture2D>("swordPower"), new Vector2(401, -49), Color.White);
                    if (player.weaponState == WeaponState.Bow)
                        spriteBatch.Draw(yellowHighlight, new Vector2(435, -50), Color.White);
                    else if (player.weaponState == WeaponState.FireSpell)
                        spriteBatch.Draw(yellowHighlight, new Vector2(470, -50), Color.White);
                    else if(player.weaponState == WeaponState.Sword)
                        spriteBatch.Draw(yellowHighlight, new Vector2(400, -50), Color.White);
                    if (menuIsOpen)
                    {
                        foreach (KeyValuePair<Vector2, Room> room in rooms)
                        {

                            if (currentRoom == room.Value)
                                spriteBatch.Draw(mapTexture, new Rectangle((int)room.Key.X * 17 + 200, (int)room.Key.Y * 11 + 200, 15, 10), Color.BlueViolet);
                            else
                                spriteBatch.Draw(mapTexture, new Rectangle((int)room.Key.X * 17 + 200, (int)room.Key.Y * 11 + 200, 15, 10), Color.LightGreen);
                        }
                    }
                    for (int i = 0; i < player.hp; i++)
                        spriteBatch.Draw(hearthTex, new Vector2(200 * i / 5 + 60, -50), Color.White);

                    spriteBatch.DrawString(font1, fps, new Vector2(51, -100), Color.White);
                }
                else if (gameState == GameState.GameOver)
                {
                    spriteBatch.DrawString(font1, "GameOver!", new Vector2(400, 200), Color.White);
                }
                else if (gameState == GameState.Start)
                {
                    #region menuText
                    spriteBatch.DrawString(font1, "Binding of Zelda", new Vector2(300, 200), Color.BlueViolet);
                    if (menuSelectedOption == 0)
                    spriteBatch.DrawString(font1, "Play", new Vector2(360, 250), Color.Yellow);
                    else
                        spriteBatch.DrawString(font1, "Play", new Vector2(360, 250), Color.White);
                    if (menuSelectedOption == 1)
                    spriteBatch.DrawString(font1, "Options", new Vector2(360, 300), Color.Yellow);
                    else
                        spriteBatch.DrawString(font1, "Options", new Vector2(360, 300), Color.White);
                    if (menuSelectedOption == 2)
                    spriteBatch.DrawString(font1, "Help", new Vector2(360, 350), Color.Yellow);
                    else
                        spriteBatch.DrawString(font1, "Help", new Vector2(360, 350), Color.White);
                    if (menuSelectedOption == 3)
                    spriteBatch.DrawString(font1, "Quit", new Vector2(360, 400), Color.Yellow);
                    else
                        spriteBatch.DrawString(font1, "Quit", new Vector2(360, 400), Color.White);
                    #endregion
                }
                else if (gameState == GameState.Win)
                {
                    spriteBatch.DrawString(font1, "You are winner!", new Vector2(400, 200), Color.White);
                }
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
        public void UpLevel(int level)
        {
            rooms.Clear();
            CreateRoom(new Vector2(0, 0), new int[] { 1, 1, 1, 0 }, 3,level);
            currentRoom = rooms[new Vector2(0, 0)];
        }
        public void Win()
        {
            gameState = GameState.Win;
        }
        public void GameOver()
        {
            player.hp = player.maxHealth;
            player.Position = new Vector2(300, 300);
            player.Velocity = new Vector2();
            gameState = GameState.GameOver;
            player.mana = 200;
            player.isHurt = false;
            player.hasBow = false;
            player.hasSpell = false;
            rooms.Clear();
            CreateRoom(new Vector2(0, 0), new int[] { 1, 1, 1, 0 }, 3,1);
            currentRoom = rooms[new Vector2(0, 0)];
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

        public void CreateRoom(Vector2 position, int[] doors, sbyte fromRoom,int level)
        {
            if (position.Length() < 2f)
                spawn.Add(new Tuple<string, int>("bat", (int)(2 *position.Length())));
            else if (position.Length() >= 2 && position.Length() < 3)
                spawn.Add(new Tuple<string, int>("slime", (int)(position.Length())));
            else if (position.Length() >= 3 && position.Length() < 4)
                spawn.Add(new Tuple<string, int>("blubatower", (int)(position.Length())));
            else if (position.Length() >= 4 && position.Length() < 5)
                spawn.Add(new Tuple<string, int>("swordenemy", (int)(2 *position.Length())));
            else
                if (rnd.Next(2) == 1)
                spawn.Add(new Tuple<string, int>("fly", (int)(2 *position.Length())));
                else
                    spawn.Add(new Tuple<string, int>("bluba", (int)(2 * position.Length())));
            //create a door where needded
            doors = CheckDoor(new Vector2(position.X + 1, position.Y), 0, 2, doors);
            doors = CheckDoor(new Vector2(position.X - 1, position.Y), 2, 0, doors);
            doors = CheckDoor(new Vector2(position.X, position.Y + 1), 1, 3, doors);
            doors = CheckDoor(new Vector2(position.X, position.Y - 1), 3, 1, doors);

            rooms.Add(position, new Room(this, Content, spawn, position, doors, fromRoom,level));
            spawn.Clear();
        }

    }
}
