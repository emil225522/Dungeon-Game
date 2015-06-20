using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Enemies
{
    class Fisherman : Enemy
    {
        public override Rectangle MapCollider { get { return new Rectangle(base.MapCollider.X + (int)(base.MapCollider.Width * .4f), base.MapCollider.Y + (int)(base.MapCollider.Height * .75f), (int)(base.MapCollider.Width / 5), (int)(base.MapCollider.Height * .25f)); } }
        public override Rectangle BulletCollider { get { return new Rectangle(base.BulletCollider.X + (int)(base.MapCollider.Width / 3f), base.MapCollider.Y, base.MapCollider.Width / 3, base.MapCollider.Height); } }

        public Fisherman(Vector2 position) 
            : base(position, TextureManager.fishermanWalk, TextureManager.fishermanAttack, TextureManager.deadFisherman)
        {
            SetGameplayVars(2.5f, 8, 1.2f, 90);
            SetAnimVars(new Point(200, 100), 4, 9f / 60, 5, 3f / 60);
        }

        public override void Update()
        {
            if (attacking)
            {
                Attacking();
                return;
            }
            else if (DistanceSquared(SceneManager.gameScene.player.Center) < Math.Pow(range, 2))
            {
                Velocity = Vector2.Zero;
                attacking = true;
            }
            else if (DistanceSquared(SceneManager.gameScene.player.Center) < 250000)
            {
                MoveTowardsPlayer(Speed);
            }

            base.Update();
        }
    }
}
