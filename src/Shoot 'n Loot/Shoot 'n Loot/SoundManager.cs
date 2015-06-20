using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Shoot__n_Loot
{
    class SoundManager
    {
        public static SoundEffect
            playerShoot,
            playerHurt,
            playerWalkGrass,
            playerWalkFloor,
            playerWalkDirt,
            playerWalkPath,
            playerWalkBeach,
            playerWalkBridge,
            playerDie,

            inventory,
            weaponEquip,
            itemUse,

            zombie1,
            zombie2,
            zombie3,
            zombieHurt,

            door,
            water;

        public static void Load(ContentManager content)
        {
            playerShoot = content.Load<SoundEffect>("Sounds/playerShoot");
            playerHurt = content.Load<SoundEffect>("Sounds/playerHurt");
            playerWalkGrass = content.Load<SoundEffect>("Sounds/playerWalkGrass");
            playerWalkFloor = content.Load<SoundEffect>("Sounds/playerWalkFloor");
            playerWalkDirt = content.Load<SoundEffect>("Sounds/playerWalkDirt");
            playerWalkPath = content.Load<SoundEffect>("Sounds/playerWalkPath");
            playerWalkBeach = content.Load<SoundEffect>("Sounds/playerWalkBeach");
            playerWalkBridge = content.Load<SoundEffect>("Sounds/playerWalkBridge");
            playerDie = content.Load<SoundEffect>("Sounds/playerDie");

            inventory = content.Load<SoundEffect>("Sounds/inventory");
            weaponEquip = content.Load<SoundEffect>("Sounds/weaponEquip");
            itemUse = content.Load<SoundEffect>("Sounds/itemUse");

            zombie1 = content.Load<SoundEffect>("Sounds/zombie1");
            zombie2 = content.Load<SoundEffect>("Sounds/zombie2");
            zombie3 = content.Load<SoundEffect>("Sounds/zombie3");
            zombieHurt = content.Load<SoundEffect>("Sounds/zombieHurt");

            door = content.Load<SoundEffect>("Sounds/door");
            water = content.Load<SoundEffect>("Sounds/water");
        }


            

    }
}
