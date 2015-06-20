using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.UI
{
    class Bar
    {
        Texture2D foreground, background;
        Rectangle screenSpace;
        Color fullColor, emptyColor;
        int minWidth;

        Rectangle RealSpace { get { return new Rectangle(screenSpace.X + (int)Camera.TotalOffset.X, screenSpace.Y + (int)Camera.TotalOffset.Y, screenSpace.Width, screenSpace.Height); } }

        public Bar(Texture2D foreground, Texture2D background, Rectangle screenSpace, int minWidth, Color fullColor, Color emptyColor)
        {
            this.foreground = foreground;
            this.background = background;
            this.screenSpace = screenSpace;
            this.minWidth = minWidth;
            this.fullColor = fullColor;
            this.emptyColor = emptyColor;
        }

        private Color BackgroundColor(float percent)
        {
            return Color.Lerp(emptyColor, fullColor, percent);
        }

        private Rectangle BackgroundRectangle(float percent)
        {
            return new Rectangle(RealSpace.X + minWidth, RealSpace.Y + RealSpace.Height / 3, (int)((RealSpace.Width - minWidth) * percent), RealSpace.Height / 3);
        }

        public void Draw(SpriteBatch batch, float percent)
        {
            batch.Draw(background, BackgroundRectangle(percent), null, BackgroundColor(percent), 0, Vector2.Zero, SpriteEffects.None, .001f);
            batch.Draw(foreground, RealSpace, Color.White);
        }
    }
}
