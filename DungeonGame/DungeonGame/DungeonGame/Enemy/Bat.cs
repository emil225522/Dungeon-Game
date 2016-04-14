using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame 
{
    class Bat : Enemy 
    {
        public Bat(ContentManager Content, int seed, Vector2 position)
            : base(position, new Animation(Content, "bat", 100, 2, true), seed, 1.5F, 50,1,true,false)
        {
            direction = (RoomConstants.Direction) values.GetValue(rnd.Next(values.Length));
            Texture = Content.Load<Texture2D>("dark");
        }

        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            Velocity *= 0.7f;
            if (!IsColliding(room.tiles)) {
                if (direction == RoomConstants.Direction.Down)
                    Position += new Vector2(0,speed);
                else if (direction == RoomConstants.Direction.Left)
                    Position -= new Vector2(speed,0);
                else if (direction == RoomConstants.Direction.Right)
                    Position += new Vector2(speed,0);
                else if (direction == RoomConstants.Direction.Up)
                    Position -= new Vector2(0,speed);
            }
            else 
            {
                if (direction == RoomConstants.Direction.Down) {
                    Position -= new Vector2(0, speed*4);
                    direction = RoomConstants.Direction.Up;
                } else if (direction == RoomConstants.Direction.Left) {
                    Position += new Vector2(speed*4, 0);
                    direction = RoomConstants.Direction.Right;
                } else if (direction == RoomConstants.Direction.Right) {
                    Position -= new Vector2(speed*4, 0);
                    direction = RoomConstants.Direction.Left;
                } else if (direction == RoomConstants.Direction.Up) {
                    Position += new Vector2(0, speed*4);
                    direction = RoomConstants.Direction.Down;
                }
            }
            if (rnd.Next(40) == 20)
                direction = (RoomConstants.Direction)values.GetValue(rnd.Next(values.Length));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.Black;
            animation.Draw(spriteBatch, Position, color);
        }

    }
}
