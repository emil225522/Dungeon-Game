using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class Boat : GameObject
    {
        const byte REQUIRED_FUEL = 3;
        byte fuel;
        int time;

        public Boat(Vector2 position)
        {
            Sprite = new Sprite(TextureManager.boat, position, new Vector2(200, 100), 2, new Point(200, 100), 0);
        }

        public override void Update()
        {
            foreach (Item item in SceneManager.gameScene.objects.Where(item => item is Item)) //yo dawg, i heard you like items..
            {
                if (item.Properties.InfoText.Contains("Fuel"))
                {
                    SceneManager.gameScene.RemoveObject(item);
                    fuel++;
                    Debug.WriteLine("boat got fuel, level: " + fuel);
                }
            }

            if (Velocity.Length() > 0)
            {
                Camera.Follow(Center);
                time++;
                if (time > 120) SceneManager.CurrentScene = SceneManager.winScene;
                SceneManager.gameScene.player.Position = Position + new Vector2(0, -50);
            }


            if (fuel >= REQUIRED_FUEL && MapCollider.Intersects(SceneManager.gameScene.player.MapCollider))
            {
                //end the game
                Velocity = new Vector2(10, 5);
                //SceneManager.gameScene.RemoveObject(SceneManager.gameScene.player);
                Sprite.Frame = 0;
                SceneManager.CurrentScene.RemoveObject(SceneManager.gameScene.player);
                SceneManager.gameScene.player.Sprite.AnimationSpeed = 0;
                SceneManager.gameScene.player.inventoryVisible = false;
            }

            Position += Velocity;


            base.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (Velocity.LengthSquared() > 0)
            {
                SceneManager.gameScene.player.Sprite.LayerDepth = .1f;
                SceneManager.gameScene.player.Draw(spriteBatch, true);
            }
            Sprite.LayerDepth = 0;
            base.Draw(spriteBatch, true);
        }
    }
}
