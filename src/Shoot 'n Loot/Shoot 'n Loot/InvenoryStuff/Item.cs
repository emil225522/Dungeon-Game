using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.InvenoryStuff;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Item : GameObject
    {
        public ItemProperties Properties { get; private set; }

        public Item(ItemProperties properties, Vector2 position)
        {
            this.Sprite = new Sprite(properties.Texture, position, new Vector2(30));
            this.Properties = properties;
        }

        public override void Update()
        {
            if (SceneManager.gameScene.player.MapCollider.Intersects(MapCollider) && Input.KeyWasJustPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                if (SceneManager.gameScene.player.inventory.Fits(this))
                {
                    SceneManager.gameScene.player.inventory.Add(this);
                    SceneManager.gameScene.RemoveObject(this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">width and height will be multiplied by the size automatically</param>
        /// <param name="spriteBatch"></param>
        public void DrawInInventory(Rectangle position, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite.Texture, new Rectangle(position.X, position.Y, position.Width * Properties.Width, position.Height * Properties.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000003f);
        }
    }
}
