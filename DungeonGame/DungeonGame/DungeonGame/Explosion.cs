﻿using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonGame
{
    class Explosion : GameObject
    {
       public override Rectangle HitBox { get { return new Rectangle((int)Position.X, (int)Position.Y, Animation.frameWidth, Animation.frameHeight); } }
        public Explosion(Vector2 position, Animation animation)
            : base (position,animation,1)
        {
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            if (Animation.currentFrame == Animation.numOffFrames -1)
                isDead = true;
            
            foreach (GameObject go in room.gameObjects.Where(item => item is Enemy))
            {
                Enemy gos = (Enemy)go;
                if (HitBox.Intersects(go.HitBox) && !gos.isHurt)
                {
                    gos.hp -= 30;
                    gos.isHurt = true;
                    if (gos.hp < 0)
                        go.isDead = true;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
