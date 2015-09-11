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
    class Camera
    {
        public Matrix transform;
        Viewport view;
        Player player;
        public Camera(Viewport newView, Player player)
        {
            view = newView;
            this.player = player;
        }
        public void Update(GameTime gameTime)
        {
            //transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
            //    Matrix.CreateTranslation(new Vector3(-player.position.X + view.Width / 2, -player.position.Y + view.Height / 2, 0));
        }
    }
}