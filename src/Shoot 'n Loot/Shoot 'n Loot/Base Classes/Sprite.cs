//Created by Johannes Larsson 2015-01-07

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Sprite
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get { return size; } set { size = value; scale = new Vector2(size.X / FrameSize.X, size.Y / FrameSize.Y); } }
        public Vector2 Origin { get; set; }
        public float AnimationSpeed { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
        public float LayerDepth { get; set; }
        public byte Frames { get; set; }
        public byte Frame { get; set; }
        public Point FrameSize { get; set; }
        public Rectangle Area { get { return new Rectangle((int)Position.X - (int)(Origin.X * scale.X), (int)Position.Y - (int)(Origin.Y * scale.Y), (int)Size.X, (int)Size.Y); } }
        public Texture2D Texture { get; private set; }
        public float Alpha { get; set; }
        public bool EndOfAnim { get { return Frame == Frames - 1 && frameCounter > 1 - AnimationSpeed * 1.05f; } }

        public Vector2 Scale { get { return scale; } }

        Vector2 size;
        Vector2 scale;
        Rectangle sourceRectangle { get { return new Rectangle(FrameSize.X * Frame, 0, FrameSize.X, FrameSize.Y); } }
        float frameCounter;

        /// <summary>
        /// creates a non-animated sprite.
        /// </summary>
        /// <param name="texture">the spritesheet to be used.</param>
        /// <param name="position"></param>
        /// <param name="size">the size in pixels to be drawn on screen</param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 size) : this(texture, position, size, 1, null, 0) { }

        /// <summary>
        /// creates an animated sprite.
        /// </summary>
        /// <param name="texture">the spritesheet to be used.</param>
        /// <param name="position"></param>
        /// <param name="size">the size in pixels to be drawn on screen</param>
        /// <param name="frames">the number of frames on the spritesheet. should be placed in a horizontal line.</param>
        /// <param name="frameSize">the size of one frame on the spritesheet, in pixels.</param>
        /// <param name="animSpeed">decides how often to switch frame. n / 60 will switch n times per second.</param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 size, byte frames, Point? frameSize, float animSpeed) : this(texture, position, size, frames, frameSize, animSpeed, Color.White, 0, null, SpriteEffects.None, .5f) { }

        /// <summary>
        /// creates a rotated sprite.
        /// </summary>
        /// <param name="texture">the spritesheet to be used.</param>
        /// <param name="position"></param>
        /// <param name="size">the size in pixels to be drawn on screen</param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 size, float rotation, Vector2? origin) : this(texture, position, size, 1, null, 0, rotation, origin) { }

        /// <summary>
        /// creates a rotated and animated sprite.
        /// </summary>
        /// <param name="texture">the spritesheet to be used.</param>
        /// <param name="position"></param>
        /// <param name="size">the size in pixels to be drawn on screen</param>
        /// <param name="frames">the number of frames on the spritesheet. should be placed in a horizontal line.</param>
        /// <param name="frameSize">the size of one frame on the spritesheet, in pixels.</param>
        /// <param name="animSpeed">decides how often to switch frame. n / 60 will switch n times per second.</param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 size, byte frames, Point? frameSize, float animSpeed, float rotation, Vector2? origin) : this(texture, position, size, frames, frameSize, animSpeed, Color.White, rotation, origin, SpriteEffects.None, .5f) { }

        /// <summary>
        /// makes a sprite with every value set.
        /// </summary>
        /// <param name="texture">the spritesheet to be used.</param>
        /// <param name="position"></param>
        /// <param name="size">the size in pixels to be drawn on screen</param>
        /// <param name="origin"></param>
        /// <param name="animSpeed">decides how often to switch frame. n / 60 will switch n times per second.</param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        /// <param name="frames">the number of frames on the spritesheet. should be placed in a horizontal line.</param>
        /// <param name="frameSize">the size of one frame on the spritesheet, in pixels.</param>
        public Sprite(Texture2D texture, Vector2 position, Vector2 size, byte frames, Point? frameSize, float animSpeed, Color color, float rotation, Vector2? origin, SpriteEffects effects, float layerDepth)
        {
            this.Texture = texture;
            this.Position = position;

            if(frameSize != null) this.FrameSize = (Point)frameSize;
            else FrameSize = new Point(texture.Width, texture.Height);

            this.Size = size;

            if (origin != null) this.Origin = (Vector2)origin;
            else Origin = new Vector2(FrameSize.X, FrameSize.Y) / 2;

            this.AnimationSpeed = animSpeed;
            this.Color = color;
            this.Rotation = rotation;
            this.SpriteEffects = effects;
            this.LayerDepth = layerDepth;
            this.Frames = frames;
            this.Frame = 0;
            this.Alpha = 1;
        }

        /// <summary>
        /// sets a new texture but keeps all other settings
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="frames"></param>
        public void SetTexture(Texture2D texture, byte frames, Point frameSize)
        {
            this.Texture = texture;
            this.Frames = frames;
            this.FrameSize = frameSize;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(Frames >= 1)
            {
                frameCounter += AnimationSpeed;
                if(frameCounter >= 1)
                {
                    frameCounter = 0;
                    Frame++;
                    if (Frame >= Frames) Frame = 0;
                }
            }
            
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color * Alpha, Rotation, Origin, scale, SpriteEffects, LayerDepth);
        }
    }
}
