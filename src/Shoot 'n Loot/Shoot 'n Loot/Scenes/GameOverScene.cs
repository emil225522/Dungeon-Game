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
    class GameOverScene : Scene
    {
        Button b;
        Button bt;

        public GameOverScene()
        {
            b = new Button("", new Rectangle(-64, -64, 120, 72), TextureManager.menuButton, TextureManager.menuButtonL, null);
            bt = new Button("", new Rectangle(-64, 0, 120, 72), TextureManager.exitButton, TextureManager.exitButtonL, null);
            base.Initialize();
        }

        public override void Update()
        {
            if (Input.KeyWasJustPressed(Keys.Enter)) SceneManager.CurrentScene = SceneManager.gameScene;
            Camera.Follow(Vector2.Zero);
            if (b.IsClicked) { SceneManager.CurrentScene = SceneManager.mainMenuScene; }
            if (bt.IsClicked) { /* Exit the Game */ Game1.exit = true; }
            b.Update();
            bt.Update();
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            b.Draw(spriteBatch);
            bt.Draw(spriteBatch);
            //spriteBatch.DrawString(TextureManager.font, "GAME OVER", - TextureManager.font.MeasureString("GAME OVER") / 2, Color.Black);
            spriteBatch.Draw(TextureManager.gameOverBackground, new Rectangle(-Game1.ScreenSize.X / 2, -Game1.ScreenSize.Y / 2, Game1.ScreenSize.X, Game1.ScreenSize.Y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, .001f);
            base.Draw(spriteBatch);
        }
    }
}
