using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class LightHouse : GameObject
    {
        public LightHouse(Vector2 position)
        {
            Sprite = new Sprite(TextureManager.lightHouse, position, new Vector2(200, 528));
        }
    }
}
