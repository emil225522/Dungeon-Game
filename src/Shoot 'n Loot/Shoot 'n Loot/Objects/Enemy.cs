using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Shoot__n_Loot.Scenes;
using Shoot__n_Loot.WeaponClasses;
using System.Diagnostics;
using Shoot__n_Loot.Objects;

namespace Shoot__n_Loot
{
    internal class Enemy : GameObject
    {

        new public const string TYPE = "Enemy";
        public override string Type { get { return TYPE; } }

        public enum EnemyType { Fisherman = 1, enemy2 = 2, enemy3 = 3 };

        public int Damage { get; set; }
        public float Speed { get; set; }
        public EnemyType enemyType { get; set; }

        protected float range;
        protected bool attacking;

        byte walkFrames, attackFrames;
        float walkAnimSpeed, attackAnimSpeed;
        Point frameSize;

        protected Texture2D[] walkingAnims, attackAnims;
        protected Texture2D deadTexture;

        int hitTimer;

        Direction direction;

        public Enemy(Vector2 position, Texture2D[] walkingAnims, Texture2D[] attackAnims, Texture2D deadTexture)
        {
            this.walkingAnims = walkingAnims;
            this.attackAnims = attackAnims;
            ObstructsBullets = true;
            Sprite = new Sprite(walkingAnims[0], position, new Vector2(200, 100), 4, new Point(200, 100), 0); //maybe an overload for different sizes etc
            CanDie = true;
            this.deadTexture = deadTexture;
            //Sprite.Origin = Size / 2;
        }


        /// <summary>
        /// set all the vars one might need for an enemy
        /// </summary>
        /// <param name="maxHealth"></param>
        /// <param name="damage"></param>
        /// <param name="speed"></param>
        /// <param name="range"></param>
        protected void SetGameplayVars(float maxHealth, int damage, float speed, float range)
        {
            this.MaxHealth = maxHealth;
            this.Damage = damage;
            this.Speed = speed;
            this.range = range;
        }

        protected void SetAnimVars(Point frameSize, byte walkFrames, float walkAnimSpeed, byte attackFrames, float attackAnimSpeed)
        {
            this.frameSize = frameSize;
            this.walkFrames = walkFrames;
            this.walkAnimSpeed = walkAnimSpeed;
            this.attackFrames = attackFrames;
            this.attackAnimSpeed = attackAnimSpeed;
            Sprite.Origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
        }

        protected override void OnTakeDamage(float amount)
        {
            hitTimer = 20;
        }

        /*public Enemy(Vector2 position, EnemyType enemytype)
        {
            this.enemyType = enemytype;
            
            Sprite = new Sprite(TextureManager.fishermanWalk[0], position, new Vector2(200, 100), 4, new Point(200, 100), 0); //TODO: should be type specific when we get sprites

            switch (enemytype)
            {
                case EnemyType.Fisherman:
                    this.MaxHealth = 3; 
                    this.Damage = 8; 
                    this.Speed = 1.2f;
                    this.range = 90;
                    break;
                case EnemyType.enemy2:
                    this.MaxHealth = 4; 
                    this.Damage = 2; 
                    this.Speed = 0.8f;
                    this.range = 90;
                    break;
                case EnemyType.enemy3:
                    this.MaxHealth = 6; 
                    this.Damage = 12; 
                    this.Speed = 2.4f;
                    this.range = 90;
                    break;
            }

            CanDie = true; //testing purposes, remove and use as example if you want
            ObstructsBullets = true;
        }*/

        protected void MoveTowardsPlayer(float speed)
        {
            Vector2 d = -1 * (Position - SceneManager.gameScene.player.Center);
            d.Normalize();
            Velocity = d * speed;
            Move(true);
        }

        public override void Update()
        {
            Animate();

            if (DistanceSquared(SceneManager.gameScene.player.Center) > GameScene.MAXSPAWNDIST * 2 * GameScene.MAXSPAWNDIST)
            {
                SceneManager.gameScene.RemoveObject(this);
            }

            /*if (enemyType == EnemyType.Fisherman)
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
                    Vector2 d = SceneManager.gameScene.player.Position - Position;
                    d.Normalize();
                    Velocity = d * 3;

                    Move(true);
                }
            }

            if (enemyType == EnemyType.enemy2)
            {
                if(attacking)
                {
                    Attacking();
                    return;
                }
                else if (DistanceSquared(SceneManager.gameScene.player.Center) < Math.Pow(range, 2))
                {
                    Velocity = Vector2.Zero;
                    attacking = true;
                }
                else if (DistanceSquared(SceneManager.gameScene.player.Center) < 80000)
                {
                    Move(true);
                    Vector2 d = SceneManager.gameScene.player.Position - Position;
                    d.Normalize();
                    Velocity = d * 4;
                }
                else if (DistanceSquared(SceneManager.gameScene.player.Center) < 1000000)
                {
                    Move(true);
                    attacking = false;
                    Vector2 d = SceneManager.gameScene.player.Position - Position;
                    d.Normalize();
                    Velocity = d;
                }
            }

            if (enemyType == EnemyType.enemy3)
            {

            }*/

            foreach (GameObject g in SceneManager.CurrentScene.objects)
            {
                if (g.Type == Enemy.TYPE && g != this)
                {
                    if (g.DistanceSquared(Position) < 3000)
                    {
                        Vector2 v = g.Position - Position;
                        if (v == Vector2.Zero) v = new Vector2((float)Game1.random.NextDouble() - .5f, (float)Game1.random.NextDouble()- .5f);
                        v.Normalize();
                        Position += v * -1;
                        //Debug.WriteLine("moving zombie, distance = " + (g.Position - Position).Length());
                    }
                }
            }
        }

        /// <summary>
        /// checks if the animation is over and if so removes damage hp from the player if they are in range
        /// </summary>
        protected void Attacking()
        {
            if (Sprite.EndOfAnim)
            {
                attacking = false;

                if (DistanceSquared(SceneManager.gameScene.player.Center) <= range * range)
                {
                    SoundManager.playerHurt.Play();
                    SceneManager.gameScene.player.Health -= Damage;
                    SceneManager.gameScene.player.bleeding += .001f; // maybe this should be different for different zombies
                }
            }
        }

        protected void Animate()
        {
            if (!attacking)
            {
                if (Velocity.Length() > .1f)
                {
                    /*Sprite.AnimationSpeed = walkAnimSpeed;
                    if (Math.Abs(Velocity.X) > Math.Abs(Velocity.Y))
                    {
                        //left and right movement
                        if (Velocity.X > 0) direction = Direction.Right;
                        else if (Velocity.X < 0) direction = Direction.Left;
                    }
                    else
                    {
                        if (Velocity.Y > 0) direction = Direction.Down;
                        else if (Velocity.Y < 0) direction = Direction.Up;
                    }*/
                    Sprite.AnimationSpeed = walkAnimSpeed;
                    direction = VelDirection;
                    Sprite.SetTexture(walkingAnims[(int)direction], walkFrames, frameSize);
                }
                else
                {
                    Sprite.AnimationSpeed = 0;
                    Sprite.Frame = 0;
                }
            }
            else
            {
                Sprite.SetTexture(attackAnims[(int)direction], attackFrames, frameSize);
                Sprite.AnimationSpeed = attackAnimSpeed;
            }
        }

        protected override void OnDestroy()
        {
            //create particles, spawn dropped items etc
            //SceneManager.gameScene.AddObject(new Enemy(new Vector2(400), EnemyType.Fisherman));
            //SceneManager.CurrentScene.AddObject(new ItemContainer(Position));
            SoundManager.zombieHurt.Play();
            if (deadTexture != null) SceneManager.CurrentScene.AddObject(new DeadEnemy(deadTexture, Position));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            hitTimer--;
            if (hitTimer > 0) Sprite.Color = Color.Pink;
            else Sprite.Color = Color.White;
            base.Draw(spriteBatch);
        }
    }
}
