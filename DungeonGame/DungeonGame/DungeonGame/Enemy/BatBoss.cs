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
        bool isSpawning;
        int timer;
        int index;
        int chargeTimer;
        sbyte numBatsSpawned;

        Color normalColor;
        Vector2 circelingPlace;
        bool playerInRange;
        public BatBoss(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "batBoss",150, 3, true), seed,6,300, 1, false, true)
        {
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
            points.Add(new Vector2(100, 100));
            points.Add(new Vector2(100, 500));
            points.Add(new Vector2(750, 500));
            points.Add(new Vector2(750, 100));
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
            if ((timer % 40) == 0 && numBatsSpawned < 20)
            {
                numBatsSpawned++;
                room.gameObjectsToAdd.Add(new Bat(Game1.content, rnd.Next(), new Vector2(Position.X + Animation.frameWidth / 2, Position.Y + Animation.frameHeight / 2)));
            }
            Vector2 direction = new Vector2(points[index].X, points[index].Y) - Position;
            direction.Normalize();

            Position += direction * speed;

            for (int i = 0; i < points.Count; i++)
            {
                if ((points[index] - Position).Length() < speed)
                    index++;
                if (index > points.Count - 1)
                    index = 0;
            }
            chargeTimer++;
            if (hp < 150 && chargeTimer > 120)
            {
                points.Clear();
                chargeTimer = 0;
                Vector2 pos = room.player.Position;
                points.Add(pos);
                points.Add(new Vector2(100, 500));
                points.Add(new Vector2(750, 500));
                points.Add(pos);
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
