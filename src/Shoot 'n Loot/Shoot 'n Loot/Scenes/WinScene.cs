using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shoot__n_Loot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Scenes
{
    class WinScene : Scene
    {
        Button b;
        Button bt;

        const string text =
            "You won!";

        public WinScene()
        {
            b = new Button("", new Rectangle(-64, 32, 120, 72), TextureManager.menuButton, TextureManager.menuButtonL, null);
            bt = new Button("", new Rectangle(-64, 96, 120, 72), TextureManager.exitButton, TextureManager.exitButtonL, null);
            base.Initialize();
        }

        public override void OnResume()
        {
            Camera.Position = Vector2.Zero;
        }

        public override void Update()
        {
            if (Input.KeyWasJustPressed(Keys.Escape)) SceneManager.CurrentScene = SceneManager.gameScene;
            Camera.Follow(Vector2.Zero);
            if (b.IsClicked) { SceneManager.CurrentScene = SceneManager.mainMenuScene; }
            if (bt.IsClicked) { /* Exit the Game */ Game1.exit = true; }
            b.Update();
            bt.Update();
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.font, text, -TextureManager.font.MeasureString(text) / 2, Microsoft.Xna.Framework.Color.Black);
            b.Draw(spriteBatch);
            bt.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
