using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class FrogBoss : Enemy
    {
        float angle;
        List<Vector2> points = new List<Vector2>();
        float angleDirection;
        bool isSpawning;
        int timer;
        int index;
        int chargeTimer;
        sbyte numBatsSpawned;
        float rotation;
        public override Rectangle HitBox { get { return new Rectangle((int)Position.X - Animation.frameWidth / 2, (int)Position.Y - Animation.frameHeight / 2, Animation.frameWidth, Animation.frameHeight); } }

        Color normalColor;
        Vector2 circelingPlace;
        bool playerInRange;
        public FrogBoss(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "Frog", 150,1, true), seed, 6, 400, 1,false, true)
        {
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
            points.Add(new Vector2(100, 200));
            points.Add(new Vector2(750, 200));
            Texture = animation.animation;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            timer++;
            base.Update(gameTime, room);
            Vector2 ballVelocity = new Vector2();
            //calculate the distance between the two objects
            float XDistance = Position.X - room.player.Position.X;
            float YDistance = Position.Y - room.player.Position.Y;
            //sets the velocity to that with the right angle thanks to this function
            ballVelocity.X -= (float)Math.Cos(Math.Atan2(YDistance, XDistance));
            ballVelocity.Y -= (float)Math.Sin(Math.Atan2(YDistance, XDistance));
            rotation = (float)Math.Atan2(ballVelocity.Y, ballVelocity.X);
            if (rnd.Next(200) == 50)
            room.gameObjectsToAdd.Add(new Egg(Game1.content,rnd.Next(),Position,ballVelocity));
            //Vector2 direction = new Vector2(points[index].X, points[index].Y) - Position;
            //direction.Normalize();

            //Position += direction * speed;

            //for (int i = 0; i < points.Count; i++)
            //{
            //    if ((points[index] - Position).Length() < 50)
            //        index++;
            //    if (index > points.Count - 1)
            //        index = 0;
            //}
            chargeTimer++;
            if (hp < 150)
            {
                points.Clear();
                chargeTimer = 0;
                Vector2 pos = room.player.Position;
                points.Add(pos);
                points.Add(new Vector2(100, 500));
                points.Add(pos);
                points.Add(new Vector2(750, 500));
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;

            animation.Draw(spriteBatch, new Vector2(Position.X,Position.Y), color, rotation);
        }
    }
}
