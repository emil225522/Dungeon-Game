using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class SwordEnemy : Enemy
    {
        int timer;
        public SwordEnemy(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "swordEnemy", 100, 1, true), seed, 1.5F, 75, 1)
        {
            direction = (Direction)values.GetValue(rnd.Next(values.Length));
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            float XDistance = position.X - room.player.Position.X - 40;
            float YDistance = position.Y - room.player.Position.Y - 40;
            //sets the velocity to that with the right angle thanks to this function
            velocity.X -= 0.5f * (float)Math.Cos(Math.Atan2(YDistance, XDistance));
            velocity.Y -= 0.5f * (float)Math.Sin(Math.Atan2(YDistance, XDistance));
            timer++;
            if (timer > 10)
            {
                timer = 0;
                velocity += new Vector2(rnd.Next(-2, 2), rnd.Next(-2, 2));
            }
                //this makes it so that the player can only attack the soldier from above 
            if (position.Y < room.player.Position.Y + 50)
                state = 1;
            else
                state = 0;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            animation.Draw(spriteBatch, position, color);
        }

    }
}
