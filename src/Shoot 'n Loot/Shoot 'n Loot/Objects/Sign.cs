using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class Sign : GameObject
    {
        Texture2D big; //the actual sign face
        bool visible;

        public Sign(Vector2 position, Texture2D big)
        {
            Sprite = new Sprite(TextureManager.sign, position, new Vector2(48));
            this.big = big;
        }

        public override void Update()
        {
            visible = DistanceSquared(SceneManager.gameScene.player.Center) < 2500;
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible) spriteBatch.Draw(big, Camera.Center, null, Color.White, 0, new Vector2(big.Width, big.Height) / 2, 1, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }
    }
}
