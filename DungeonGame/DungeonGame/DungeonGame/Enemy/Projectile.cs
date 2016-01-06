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

namespace DungeonGame
{
    class Projectile : GameObject
    {
        float rotation;

        public Projectile(Animation animation, Vector2 position, Vector2 velocity)
            : base (position, animation, 0)
        {
            Velocity = velocity;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            Position += Velocity;
            rotation -= 0.2f;
            if (HitBox.Intersects(room.player.HitBox))
            {
                room.player.isHurt = true;
                //Room.gameObjectsToRemove.Add(this);
                isDead = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch, Position, Color.White, rotation);
        }
    }
}
