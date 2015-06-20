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

namespace Shoot__n_Loot
{
    class Music
    {
        public Song CurrentTrack { get; set; }

        public Music (Song track)
        {
            this.CurrentTrack = track;
        }

        public void Initialize()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(CurrentTrack);
        }
    }
}
