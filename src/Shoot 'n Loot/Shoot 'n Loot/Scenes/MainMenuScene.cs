using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shoot__n_Loot.Scenes;
using Shoot__n_Loot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class MainMenuScene : Scene
    {
        List<Button> b;

        public MainMenuScene()
        {
            b = new List<Button>();
            b.Add(new Button("", new Rectangle(-64, 32, 120, 72), TextureManager.playButton, TextureManager.playButtonL, null));
            b.Add(new Button("", new Rectangle(-64, 96, 120, 72), TextureManager.infoButton, TextureManager.infoButtonL, null));
            b.Add(new Button("", new Rectangle(-64, 160, 120, 72), TextureManager.exitButton, TextureManager.exitButtonL, null));

            base.Initialize();
        }

        public override void Update()
        {
            Camera.Position = Vector2.Zero;

            if (Input.newKs.IsKeyDown(Keys.Enter)) SceneManager.CurrentScene = SceneManager.gameScene;
            else if (Input.KeyWasJustPressed(Keys.Escape)) SceneManager.CurrentScene = SceneManager.aboutScene;
            
            if (b[0].IsClicked) 
            { 
                SceneManager.gameScene = new GameScene();
                Map.Initialize();
                SceneManager.CurrentScene = SceneManager.gameScene; 
            }
            if (b[1].IsClicked) { SceneManager.CurrentScene = SceneManager.aboutScene; }
            if (b[2].IsClicked) { /* Exit the Game */ Game1.exit = true; }

            foreach (Button bu in b) bu.Update();

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.menuBackground, new Vector2(-TextureManager.menuBackground.Width / 2, -TextureManager.menuBackground.Height / 2), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, .01f);
            for (int i = 0; i < b.Count; i++)
            {
                b[i].Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
