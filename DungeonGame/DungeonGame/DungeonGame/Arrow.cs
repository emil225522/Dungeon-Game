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
    class Arrow : GameObject
    {
        float rotation;
        bool spinningLeft;

        public Arrow(Animation animation, Vector2 position, Vector2 velocity, sbyte type)
            : base(position, animation, 0)
        {
            Velocity = velocity;
            spinningLeft = Convert.ToBoolean(rnd.Next(2));
            //spinningDirection = rnd;
            this.type = type;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            Position += Velocity;
            if (type == 1)
            {
                if (spinningLeft)
                    rotation += 0.2f;
                else
                    rotation -= 0.2f;
            }
             foreach(GameObject go in room.gameObjects.Where(item => item is Enemy))
            {
                Enemy enemy = (Enemy)go;
                if (go.HitBox.Intersects(HitBox))
                {
                    enemy.hp -= 20;
                    if (enemy.hp < 1)
                    {
                        go.isDead = true;
                        enemy.isDead = true;
                    }
                    isDead = true;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, new Vector2(Position.X + Animation.frameWidth / 2, Position.Y + Animation.frameHeight / 2), Color.White, rotation);
        }
    }
}
