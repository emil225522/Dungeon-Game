using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class Landmine : GameObject
    {
        const float range = 150;

        public Landmine(Vector2 position)
        {
            Sprite = new Sprite(TextureManager.landmine, position, new Vector2(38, 18), 2, new Point(38, 18), 1 / 60f);
        }

        public override void Update()
        {
            foreach (GameObject g in SceneManager.CurrentScene.objects.Where(item => item is Enemy))
            {
                if (DistanceSquared(g.Center) < range * range)
                {
                    //TODO: spawn new explosion
                    SceneManager.CurrentScene.AddObject(new Explosion(Center));
                    SceneManager.CurrentScene.RemoveObject(this);
                    g.Health -= 10;
                }
            }
        }
    }
}
