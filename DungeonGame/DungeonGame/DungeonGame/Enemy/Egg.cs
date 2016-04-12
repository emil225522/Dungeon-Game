using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Egg : Enemy
    {
        float rotation;
        int timer;
        public Egg(ContentManager Content, int seed, Vector2 position,Vector2 velocity)
            : base(position, new Animation(Content, "egg", 0, 1, false), seed, 1.5F, 90,9,false, false,velocity)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
            Velocity = velocity;
            Velocity *= rnd.Next(1, 5);
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            timer++;
            if (timer > 200)
            {
                for (int i = 0; i < rnd.Next(10,20 ); i++)
                {
                    float angle = i + 1 * (float)Math.PI / 5;
                    Vector2 givenVelocity = 2 * new Vector2((float)Math.Cos(angle) * rnd.Next(1, 3), (float)Math.Sin(angle) * rnd.Next(1, 3));
                    room.gameObjectsToAdd.Add(new Projectile(new Animation(Game1.content, "frogBall", 0, 1, false), Position, givenVelocity, 1, 1));
                }
                isDead = true;
            }
            rotation+= 0.2f;
            Velocity *= 0.99f;
            rotation *= 0.99f;
            if (room.player.attackRect.Intersects(HitBox) && !isHurt)
            {
                hp -= 25;
                Velocity = new Vector2();
                isHurt = true;
            }
            if (hp < 1)
                isDead = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X + Animation.frameWidth / 2, Position.Y + Animation.frameHeight / 2), Color.White, rotation);
        }

    }
}
