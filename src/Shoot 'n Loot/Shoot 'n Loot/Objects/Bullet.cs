using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Bullet : GameObject
    {
        const float speed = 15;
        const float w = 6, h = 3;

        float traveledDist;

        Vector2 velocity;

        public BulletProperties Properties { get; private set; }
        
        public Bullet(float angle, Vector2 position, BulletProperties properties)
        {
            this.Properties = properties;
            velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Properties.Speed;
            Sprite = new Sprite(TextureManager.bullet, position, new Vector2(w, h), angle, null);
            Sprite.LayerDepth = 0;
        }

        public override void Update()
        {
            Position += velocity;
            traveledDist += velocity.Length();

            //List<GameObject> objects = new List<GameObject>();
            //foreach (Enemy e in Game1.enemies) objects.Add((GameObject)e);

            foreach(GameObject g in SceneManager.gameScene.objects)
            {
                if (g.ObstructsBullets)
                {
                    if (MapCollider.Intersects(g.BulletCollider))
                    {
                        g.Health -= Properties.Damage * (traveledDist / Properties.MaxRange);
                        this.Dead = true;
                    }
                }
            }
            foreach(Tile t in CloseSolidTiles)
            {
                if (t.Properties.ObstructsBullets)
                {
                    if (MapCollider.Intersects(t.Hitbox))
                    {
                        this.Dead = true;
                    }
                }
            }
            if (!Camera.AreaIsVisible(MapCollider)) Dead = true;
        }
    }
}
