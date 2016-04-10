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

namespace DungeonGame
{
    class Projectile : GameObject
    {
        float rotation;
        sbyte type;
        float size;
        int health = 100;
        bool canBeHurt;

        public Projectile(Animation animation, Vector2 position, Vector2 velocity, sbyte type,float size)
            : base (position, animation, 0)
        {
            Velocity = velocity;
            this.type = type;
            this.size = size;
            if (type == 4)
                Velocity *= rnd.Next(1, 5);
 
        }
        public override void Update(GameTime gameTime, Room room)
        {
            Position += Velocity;
            rotation -= 0.2f;
            if (HitBox.Intersects(room.player.attackRect) && type == 2)
            {
                type = 3;
                  Velocity = new Vector2(Velocity.X * -1, Velocity.Y);
            }
            if (HitBox.Intersects(room.player.HitBox))
                room.player.Velocity = Velocity*5;
            foreach (GameObject go in room.gameObjects.Where(item => item is CannonBoss))
            {
                CannonBoss boss = (CannonBoss)go;
                if (HitBox.Intersects(boss.HitBox) && type == 3)
                {
                    isDead = true;
                    boss.hp -= 50;
                    if (boss.hp < 1)
                    {
                        boss.isDead = true;
                        go.isDead = true;
                    }
                }
            }
            if (type == 4)
            {
                Velocity *= 0.99f;
                rotation *= 0.99f;
                if (room.player.attackRect.Intersects(HitBox))
                {
                    health -= 25;
                }
                if (health < 1)
                    isDead = true;
            }
            if (HitBox.Intersects(room.player.HitBox))
            {
                room.player.isHurt = true;
                room.player.hp--;
                isDead = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X + Animation.frameWidth/2,Position.Y + Animation.frameHeight/2), Color.White, rotation,size);
        }

    }
}
