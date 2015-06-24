using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Randomz
{
    public class Animation
    {
        public Texture2D animation;
        Rectangle sourceRect;
        public Vector2 position;
        public string asset;

        public float elapsed;
        public float frameTime;
        public int numOffFrames;
        public int currentFrame;
        public int frameWidth;
        public int frameHeight;
        public bool looping;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Animation(ContentManager Content, string asset, float frameSpeed, int numOffFrames, bool looping)
        {
            this.frameTime = frameSpeed;
            this.numOffFrames = numOffFrames;
            this.looping = looping;
            this.asset = asset;
            this.animation = Content.Load<Texture2D>(asset);
            frameWidth = (animation.Width / numOffFrames);
            frameHeight = (animation.Height);
        }
        public void PlayAnim(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);

            if (elapsed >= frameTime)
            {
                if (currentFrame >= numOffFrames - 1)
                {
                    if (looping)
                        currentFrame = 0;
                }
                else
                {
                    currentFrame++;
                }
                elapsed = 0;
            }
        }
        public void Draw(SpriteBatch spriteBatch,Vector2 position, Color color, SpriteEffects spriteffects)
        {
            spriteBatch.Draw(animation, position, sourceRect, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(animation, position, sourceRect, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }
    }
}
