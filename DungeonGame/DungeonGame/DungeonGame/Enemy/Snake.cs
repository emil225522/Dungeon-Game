using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class Snake : Enemy
    {
        float angle;
        float angleDirection;
        int timer;
        Vector2 poisonVelocity;

        Color normalColor;
        Vector2 circelingPlace;

        public Snake(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "snake", 150, 1, true), seed, 1.5F, 300, 1,false,true)
        {
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
        }
        
        public override void Update(GameTime gameTime, Room room)
        {
            timer++;
            base.Update(gameTime, room);

            float XDistance = (Position.X + Animation.frameWidth / 2) - room.player.Position.X;
            float YDistance = (Position.Y + Animation.frameHeight / 2) - room.player.Position.Y;
            //sets the velocity to that with the right angle thanks to this function
            poisonVelocity.X = -(float)Math.Cos(Math.Atan2(YDistance, XDistance));
            poisonVelocity.Y = -(float)Math.Sin(Math.Atan2(YDistance, XDistance));
            Vector2 combVec = new Vector2(XDistance,YDistance);
           
            if (timer > 200)
            {

                timer = 0;
                //angle += 0.01f;
                for (int i = 0; i < 100; i++)
                {

                    timer = 0;
                    angle += (float)Math.PI/5;
                    //angle *= i * 10;
       
                    room.gameObjectsToAdd.Add(new Poison(new Animation(Game1.content, "poision", 150, 1, false)
                  , new Vector2((Position.X + Animation.frameWidth / 2) + (float)Math.Cos(angle) * 30 + rnd.Next(-40, 40),
                      (Position.Y + Animation.frameHeight / 2) + (float)Math.Sin(angle) * 30 + + rnd.Next(-40, 40)), poisonVelocity * combVec.Length() / 60));
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = normalColor;
            animation.Draw(spriteBatch, Position, color);
        }
    }
}
