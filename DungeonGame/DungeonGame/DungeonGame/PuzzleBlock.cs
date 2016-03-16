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
    class PuzzleBlock : Tile
    {
        public sbyte typeOfDrop;
        public PuzzleBlock(Texture2D texture, Vector2 position, sbyte type, sbyte typeOfColor)
            : base(Game1.content.Load<Texture2D>("cube"),position,11)
        {
            this.typeOfDrop = typeOfDrop;
        }

    }
}
