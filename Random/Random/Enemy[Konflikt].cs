using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Randomz
{
    class Enemy
    {
        public Vector2 position;
        public Vector2 velocity;
        public Texture2D texture;
        public Rectangle hitBox;
        public int hp = 100;
        public float rotation;
        private sbyte hurtTimer;
        public bool isHurt;
        public bool hasDied;

        public Enemy(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }
        public void Update(Player player)
        {
            velocity *= 0.5f;
            position += velocity;
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            float XDistance = position.X - player.position.X;
            float YDistance = position.Y - player.position.Y;
            if (isHurt)
                hurtTimer++;
            if (hurtTimer > 20)
            {
                isHurt = false;
                hurtTimer = 0;
            }
            if (hp < 1)
                hasDied = true;
            //Calculate the required rotation by doing a two-variable arc-tan
            rotation = (float)Math.Atan2(YDistance, XDistance);

            //Move square towards mouse by closing the gap 3 pixels per update
            if (Math.Abs(XDistance) > 50)
                velocity.X -= (float)(3 * Math.Cos(rotation));
            if (Math.Abs(YDistance) > 50)
                velocity.Y -= (float)(3 * Math.Sin(rotation));          
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            if (isHurt)
                color = Color.Red;
            else
                color = Color.Green;
            spriteBatch.Draw(texture,position,color);
        }

    }
}
