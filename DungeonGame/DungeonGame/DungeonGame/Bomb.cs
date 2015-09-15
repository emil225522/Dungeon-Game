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
    class Bomb : GameObject
    {
        ContentManager content;
        public Bomb(Animation animation,Vector2 position, ContentManager Content)
            : base (position,animation,1)
        {
            content = Content;
        }
        public override void Update(GameTime gameTime, Room room)
        {
            base.Update(gameTime, room);
            if (Animation.currentFrame == Animation.numOffFrames - 1)
            {
                room.gameObjectsToAdd.Add(new Explosion(new Vector2(Position.X - 65, Position.Y - 65), new Animation(content, "explosion", 130, 4, false)));
                isDead = true;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
