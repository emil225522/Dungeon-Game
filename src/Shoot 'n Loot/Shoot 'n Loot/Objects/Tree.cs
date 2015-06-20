using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.Objects
{
    class Tree : GameObject
    {
        public enum TreeType { Oak, Fir }
        public Tree(Vector2 position, TreeType type)
        {
            if (type == TreeType.Oak)
            {
                Sprite = new Sprite(TextureManager.oakTree, position, new Vector2(194, 288));
            }
            else
            {
                Sprite = new Sprite(TextureManager.firTree, position, new Vector2(194, 288));
            }
        }
    }
}
