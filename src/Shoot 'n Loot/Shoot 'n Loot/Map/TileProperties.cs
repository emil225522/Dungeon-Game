using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    /// <summary>
    /// manages everything related to tiles thats not intsance specific, to make it easier to copy them
    /// </summary>
    class TileProperties
    {
        public byte TextureIndex {get; private set; }
        public byte Frames { get; private set; }
        public byte FramesPerFrame { get; private set; }
        public bool IsWalkable { get; private set; }
        public bool ObstructsBullets { get; private set; }

        public bool IsAnimated { get { return Frames > 1; } }

        public TileProperties(byte textureIndex, bool walkable, bool obstructsBullets, byte frames, byte animSpeed)
        {
            this.Frames = frames;
            this.FramesPerFrame = animSpeed;
            this.TextureIndex = textureIndex;
            this.IsWalkable = walkable;
            this.ObstructsBullets = obstructsBullets;
        }

        public TileProperties(byte textureIndex, bool walkable)
            : this(textureIndex, walkable, !walkable, 1, 0)
        { }

        public TileProperties(byte textureIndex, bool walkable, bool obstructsBullets)
            : this(textureIndex, walkable, obstructsBullets, 1, 0)
        { }

        public void Animate(ref byte frame, ref byte counter)
        {
            if (FramesPerFrame == 0) return;
            counter += 1;
            if(counter >= FramesPerFrame)
            {
                counter = 0;
                frame++;
                if(frame >= Frames)
                {
                    frame = 0;
                }
            }
        }
    }
}
