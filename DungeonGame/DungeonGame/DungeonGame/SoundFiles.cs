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
    class SoundFiles
    {
        public static SoundEffect swordSlash;
        public static SoundEffect shootFireBall;
        public static SoundEffect shootArrow;

        public static SoundEffect winSound;
        public static SoundEffect gameOverSound;
        public static SoundEffect portalSound;

        public static SoundEffect bossDeath;

        public SoundFiles(ContentManager content)
        {
            swordSlash = content.Load<SoundEffect>("swordSlash");
            shootFireBall = content.Load<SoundEffect>("shootFireBall");
            shootArrow = content.Load<SoundEffect>("shootArrow");

            winSound = content.Load<SoundEffect>("winSound");
            gameOverSound = content.Load<SoundEffect>("gameOverSound");
            portalSound = content.Load<SoundEffect>("portalSound");

            bossDeath = content.Load<SoundEffect>("bossDeath");
        }
    }
}
