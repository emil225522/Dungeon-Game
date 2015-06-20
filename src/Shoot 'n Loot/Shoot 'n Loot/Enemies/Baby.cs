using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Enemies
{
    class Baby : Enemy
    {
        const float sightRadius = 600;
        const float nukeRadius = 100;
        const float jumpDist = 250;
        const int nukeDamage = 30;

        byte attacks;
        bool nuking;
        bool jumping;


        public Baby(Vector2 position)
            : base(position, TextureManager.babyWalk, TextureManager.babyAttack, null)
        {
            SetGameplayVars(1, 1, 3, 50);
            SetAnimVars(new Point(75, 75), 4, 4f / 60, 4, 15f / 60);
            Sprite.Frame = 0;
        }

        protected override void OnDestroy()
        {
            if (!nuking)
            {
                Health = 1;
                nuking = true;
                SceneManager.CurrentScene.AddObject(this);
                base.OnDestroy();
            }
        }

        public override void Update()
        {
            if (DistanceSquared(SceneManager.gameScene.player.Center) < range * range && !nuking)
            {
                nuking = true;
                jumping = false;
                Sprite.SetTexture(TextureManager.babyNuke[(int)VelDirection], 6, new Point(75, 75));
                Sprite.Frame = 0;
                Sprite.AnimationSpeed = 15f / 60;
            }
            else if (DistanceSquared(SceneManager.gameScene.player.Center) < sightRadius * sightRadius && !nuking)
            {
                MoveTowardsPlayer(Speed);
                if (Math.Abs(DistanceSquared(SceneManager.gameScene.player.Center) - jumpDist * jumpDist) < 30 && !jumping)
                {
                    jumping = true;
                    walkingAnims = TextureManager.babyAttack;
                    Sprite.Frame = 0;
                    Speed *= 2;
                }
            }

            if (jumping && Sprite.EndOfAnim)
            {
                Sprite.AnimationSpeed /= 2;
                walkingAnims = TextureManager.babyWalk;
                Sprite.Frame = 0;
                jumping = false;
            }

            if (nuking && Sprite.EndOfAnim)
            {
                foreach (GameObject g in SceneManager.gameScene.objects.Where(item => item.DistanceSquared(Center) < nukeRadius * nukeRadius + 100))
                {
                    g.Health -= nukeDamage;
                }
                Sprite.AnimationSpeed = 0;
                SceneManager.CurrentScene.RemoveObject(this);
            }

            if (!nuking) base.Update(); //shouldnt animate more, the rest doesnt matter either
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.DrawString(TextureManager.font, jumping.ToString() + "\n" + Math.Sqrt(DistanceSquared(SceneManager.gameScene.player.Center)), Center, Color.Black);
        }
    }
}