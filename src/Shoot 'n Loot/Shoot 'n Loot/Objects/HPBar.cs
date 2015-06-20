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
using Shoot__n_Loot.Scenes;

namespace Shoot__n_Loot
{
    class HPBar : GameObject
    {
        public Vector2 Position { get; set; }
        public int Health { get; set; }

        public HPBar(Vector2 position, int health)
        {
            this.Position = position;
            this.Health = health;
        }

        //percentage = currentHP / maxHp
        public void Draw(SpriteBatch spriteBatch, float percentage)
        {
            spriteBatch.Draw(TextureManager.hpRed, new Vector2(26, 2) + Camera.TotalOffset, new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(percentage * 100), TextureManager.hpRed.Height), Color.White);
            spriteBatch.Draw(TextureManager.hpBar, Camera.TotalOffset, Color.White);
        }
    }
}
