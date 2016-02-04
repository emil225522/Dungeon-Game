using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class BatBoss : Enemy
    {
        float angle;
        List<Vector2> points = new List<Vector2>();
        float angleDirection;
        int timer;
        int index;

        Color normalColor;
        Vector2 circelingPlace;
        bool playerInRange;
        public BatBoss(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "batBoss", 150, 3, true), seed, 1.5F, 300, 1, false, true)
        {
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
            direction = Direction.Left;
            //Velocity = new Vector2(-2, 0);
            points.Add(new Vector2(200, 200));
            points.Add(new Vector2(300, 500));
            points.Add(new Vector2(400, 100));
            points.Add(new Vector2(400, 100));
            points.Add(new Vector2(200, 200));
            points.Add(new Vector2(300, 500));
            points.Add(new Vector2(400, 100));
            points.Add(new Vector2(400, 100));
           
        }

        public override void Update(GameTime gameTime, Room room)
        {
            timer++;
            base.Update(gameTime, room);
            //float XDistance = (Position.X + Animation.frameWidth / 2) - points[0].X;
            //float YDistance = (Position.Y + Animation.frameHeight / 2) - points[0].Y;
            ////sets the velocity to that with the right angle thanks to this function
            //Position += new Vector2((float)Math.Cos(Math.Atan2(YDistance, XDistance)));
            //Position += new Vector2 ((float)Math.Sin(Math.Atan2(YDistance, XDistance)));
            //poisonVelocity = new Vector2(22,22);

           
            Vector2 direction = new Vector2(points[index].X, points[index].Y) - Position;
            direction.Normalize();

            Position += direction * speed;

            for (int i = 0; i < points.Count; i++)
            {
                if ((points[index] - Position).Length() < 2)
                    index++;
                if (index > points.Count - 1)
                    index = 0;
            }
            if (timer > 200)
            {
                timer = 0;
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
