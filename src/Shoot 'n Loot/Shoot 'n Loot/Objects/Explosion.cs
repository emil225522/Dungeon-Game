using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class Explosion : GameObject
    {
        public Explosion(Vector2 position)
        {
            Sprite = new Sprite(TextureManager.explosion, position, new Vector2(100), 6, new Point(100, 100), 10 / 60f);
            Sprite.Frame = 1;
        }

        public override void Update()
        {
            if (Sprite.EndOfAnim) SceneManager.CurrentScene.RemoveObject(this);
        }
    }
}
