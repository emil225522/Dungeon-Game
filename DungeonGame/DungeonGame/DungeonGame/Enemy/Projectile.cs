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

        public Projectile(Animation animation, Vector2 position, Vector2 velocity, sbyte type)
            : base (position, animation, 0)
        {
            Velocity = velocity;
            this.type = type;
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
            if (HitBox.Intersects(room.player.HitBox))
            {
                room.player.isHurt = true;
                //room.player.Velocity = Velocity;
                //Room.gameObjectsToRemove.Add(this);
                isDead = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X + Animation.frameWidth/2,Position.Y + Animation.frameHeight/2), Color.White, rotation);
        }
    }
}
