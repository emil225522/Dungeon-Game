using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class SlimeBoss : Enemy
    {
        float angle;
        List<Vector2> points = new List<Vector2>();
        float angleDirection;
        Texture2D shadowTexture;
        bool isSpawning;
        int timer;
        int index;
        int chargeTimer;
        sbyte numBatsSpawned;
        float rotation;
        int jumptimer;
        float ypos;
        Vector2 originalPosition;
        float yvel;
        bool isjumping;
        bool isMassiveJumping;
        public override Rectangle HitBox { get { return new Rectangle((int)Position.X - Animation.frameWidth / 2, (int)Position.Y - Animation.frameHeight / 2, Animation.frameWidth, Animation.frameHeight); } }

        Color normalColor;
        Vector2 circelingPlace;
        bool playerInRange;
        public SlimeBoss(ContentManager Content, int seed, Vector2 position, int level)
            : base(position, new Animation(Content, "slimeBoss", 150, 2, true), seed, 6, 400, 1, false, true, level)
        {
            shadowTexture = Content.Load<Texture2D>("shadow");
            direction = (RoomConstants.Direction)values.GetValue(rnd.Next(values.Length));
            ypos = Position.Y;
            originalPosition = Position;
            Position = new Vector2(originalPosition.X + 30, Position.Y);
            hp *= level;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            jumptimer++;
            if (!isjumping && !isMassiveJumping)
            {
                if (rnd.Next(100) == 5)
                {
                    isjumping = true;
                    ypos = Position.Y;
                    Position = new Vector2(originalPosition.X + 30, Position.Y);
                    yvel = -10;
                }
                else
                {
                    yvel = 0;
                }
            }
            if (rnd.Next(50) == 5)
                direction = (RoomConstants.Direction)values.GetValue(rnd.Next(values.Length));
            if (jumptimer > 1)
            {
                Position += new Vector2(0, yvel);
            }
            if (isjumping)
            {
                if (Position.Y < originalPosition.Y)
                    yvel += 0.5f;
                else
                    isjumping = false;
            }
            if (isMassiveJumping)
            {
                Vector2 directionTo = room.player.Position - new Vector2(originalPosition.X + 40,originalPosition.Y- 50);
                directionTo.Normalize();
                Position = new Vector2(originalPosition.X+ 30,Position.Y);
                if (Position.Y > originalPosition.Y)
                {
                    yvel = 0;
                    isMassiveJumping = false;
                }
                else
                    yvel+= +0.5f;
                originalPosition += directionTo * 2;
            }
            //else if (!isjumping)


        }
        public override void OnHit()
        {
            yvel = -20;
            isMassiveJumping = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.White;
            spriteBatch.Draw(shadowTexture,originalPosition,Color.White);
            animation.Draw(spriteBatch, new Vector2((int)Position.X, (int)Position.Y), color, rotation);
        }
    }
}
