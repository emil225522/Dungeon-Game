using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonGame
{
    class SwordFlower : Enemy
    {
        int jumptimer;
        float ypos;
        float yvel;
        bool isjumping;
        float spearRotation;
        Texture2D spearTexture;
        public SwordFlower(ContentManager Content, int seed, Vector2 position, int level)
            : base(position, new Animation(Content, "SwordFlower", 0, 1, false), seed, 1.5F, 50, 1, false, false,level)
        {
            direction = (RoomConstants.Direction)values.GetValue(rnd.Next(values.Length));
            spearTexture = Content.Load<Texture2D>("spear");
            hp *= level;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
        
            //Vector2 target = new Vector2();
            //float XDistance = (Position.X + animation.frameWidth /2) - room.player.Position.X;
            //float YDistance = (Position.Y + animation.frameHeight /2) - room.player.Position.Y;
            ////sets the velocity to that with the right angle thanks to this function
            //target.X -=5* (float)Math.Cos(Math.Atan2(YDistance, XDistance));
            //target.Y -=5* (float)Math.Sin(Math.Atan2(YDistance, XDistance));
            //spearRotation = (float)Math.Atan2(target.Y, target.X);
            spearRotation += 0.1f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(spearTexture,new Vector2(Position.X + animation.frameWidth / 2, Position.Y + animation.frameHeight / 2),null,Color.White,spearRotation - 0.2f,new Vector2(115,5),1,SpriteEffects.None,0);
            //spriteBatch.Draw(Game1.content.Load<Texture2D>("dark"), new Rectangle((int)(Position.X + animation.frameWidth / 2), (int)(Position.Y + animation.frameHeight / 2),50,50), Color.Black);
        }
    }
}
