using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class CannonBoss : Enemy
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
        public CannonBoss(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "towerboss", 150, 1, true), seed, 6, 400, 1, false, true)
        {
            circelingPlace = new Vector2(rnd.Next(50, 800), rnd.Next(50, 600));
            angleDirection = (float)rnd.Next(200, 500) / 10000;
            normalColor = new Color(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
            points.Add(new Vector2(550, 400));
            points.Add(new Vector2(550, 0));
        }

        public override void Update(GameTime gameTime, Room room)
        {
            timer++;
            base.Update(gameTime, room);

            Vector2 direction = new Vector2(points[index].X, points[index].Y) - Position;
            direction.Normalize();

            Position += direction * speed;

            timer++;
            if (timer > 120)
            {
                timer = 0;
                room.gameObjectsToAdd.Add(new Projectile(new Animation(Game1.content, "cannonball", 150, 1, false), new Vector2(Position.X, Position.Y + animation.frameHeight / 2), new Vector2(-4, 0), 2,1.4f));
            }
            for (int i = 0; i < points.Count; i++)
            {
                if ((points[index] - Position).Length() < speed)
                    index++;
                if (index > points.Count - 1)
                    index = 0;
            }
            if (hp < 1)
                isDead = true;
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
