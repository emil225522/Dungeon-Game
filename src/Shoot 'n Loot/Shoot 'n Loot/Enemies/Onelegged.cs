using Microsoft.Xna.Framework;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Enemies
{
    class Onelegged : Enemy
    {
        const int mw = 60, mh = 25, bw = 60, bh = 150;
        public override Rectangle MapCollider { get { return new Rectangle((int)(Position.X - mw / 2), (int)(Position.Y + Size.Y / 1.35f - mh), mw, mh); } }
        public override Rectangle BulletCollider { get { return new Rectangle((int)(Position.X - bw / 2), (int)(Position.Y + Size.Y / 1.35f -bh), bw, bh); } }


        public Onelegged(Vector2 position)
            : base(position, TextureManager.oneleggedWalk, TextureManager.oneleggedAttack, TextureManager.deadOneLeg)
        {
            SetGameplayVars(2, 1, .7f, 100);
            SetAnimVars(new Point(100, 150), 4, 9f / 60, 4, 6f / 60);
        }

        public override void Update()
        {
            if (attacking)
            {
                Attacking();
            }
            else if (DistanceSquared(SceneManager.gameScene.player.Center) < range * range)
            {
                attacking = true;
            }
            else if (DistanceSquared(SceneManager.gameScene.player.Center) < 300000)
            {
                MoveTowardsPlayer(4 * Speed);
            }
            else
            {
                MoveTowardsPlayer(Speed);
            }

            base.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(TextureManager.font, (DistanceSquared(SceneManager.gameScene.player.Center) - (range * range)).ToString(), Position, Color.Black);
            //spriteBatch.Draw(TextureManager.house, MapCollider, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
