using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class TextureManager
    {
        public static Texture2D
            playerLeft,
            playerRight,
            playerUp,
            playerDown,

            explosion,

            oakTree,
            firTree,

            map,
            spawnData,
            tiles,
            propData,

            enemy1,
            enemy2,
            enemy3,

            bullet,
            house,
            lightHouse,
            inventorySlot,
            landmine,
            mineThumb,

            medicineItem,
            beans,
            bandage,
            fuelCan,
            chest,

            twoByFour,
            machete,
            sledgeHammer,
            
            boat,
            sign,

            lightAmmo,
            mediumAmmo,
            heavyAmmo,
            nails,

            gunBarrel,
            gunMechs,
            gunScrap,
            rifleBarrel,
            rifleHandle,
            rifleScrap,

            hpBar,
            hpRed,
            healthBar,
            hungerBar,
            
            pixel,
            
            backButton,
            backButtonL,
            exitButton,
            exitButtonL,
            infoButton,
            infoButtonL,
            menuButton,
            menuButtonL,
            playButton,
            playButtonL,
            resumeButton,
            resumeButtonL,
            
            menuBackground,
            gameOverBackground,
            
            deadLady,
            deadOneLeg,
            deadFisherman;

        public static Texture2D[]
            fishermanWalk,
            fishermanAttack,

            oneleggedWalk,
            oneleggedAttack,

            fatLadyWalk,
            fatLadyAttack,

            babyWalk,
            babyAttack,
            babyNuke,

            playerWalkNoWeapon,
            playerWalkGun,

            playerWalkMelee, //machete, standard
            playerAttack, //machete, standard

            playerWalkWood,
            playerAttackWood,
            
            playerWalkSledgeHammer,
            playerAttackSledgeHammer,
            signs;

        public static SpriteFont font;

        public static void Load(ContentManager content)
        {
            playerWalkNoWeapon = LoadWalkSprites("player/walk/noWeapon", content);
            playerWalkGun = LoadWalkSprites("player/walk/gun", content);
            playerWalkMelee = LoadWalkSprites("player/walk/melee", content);
            playerAttack = LoadWalkSprites("player/attack/melee", content);

            playerWalkSledgeHammer = LoadWalkSprites("player/walk/sledgehammer", content);
            playerAttackSledgeHammer = LoadWalkSprites("player/attack/sledgehammer", content);

            playerWalkWood = LoadWalkSprites("player/walk/wood", content);
            playerAttackWood = LoadWalkSprites("player/attack/wood", content);

            oakTree = content.Load<Texture2D>("objects/oak");
            firTree = content.Load<Texture2D>("objects/fir");

            try
            {
                font = content.Load<SpriteFont>("font");
            }
            catch
            {
                font = content.Load<SpriteFont>("font_default");
            }
            

            tiles = content.Load<Texture2D>("tiles");
            map = content.Load<Texture2D>("map/map_wotrees");
            spawnData = content.Load<Texture2D>("map/spawnData");
            propData = content.Load<Texture2D>("map/propData");

            explosion = content.Load<Texture2D>("explosion");

            enemy1 = content.Load<Texture2D>("enemies/enemy1");
            enemy2 = content.Load<Texture2D>("enemies/enemy2");
            enemy3 = content.Load<Texture2D>("enemies/enemy3");

            fishermanWalk = LoadWalkSprites("enemies/fisherman/walk", content);
            fishermanAttack = LoadWalkSprites("enemies/fisherman/attack", content);

            oneleggedWalk = LoadWalkSprites("enemies/onelegged/walk", content);
            oneleggedAttack = LoadWalkSprites("enemies/onelegged/attack", content);

            fatLadyWalk = LoadWalkSprites("enemies/fatLady/walk", content);
            fatLadyAttack = LoadWalkSprites("enemies/fatLady/attack", content);

            pixel = content.Load<Texture2D>("pixel");

            babyWalk = LoadWalkSprites("enemies/baby/walk", content);
            babyAttack = LoadWalkSprites("enemies/baby/attack", content);
            babyNuke = LoadWalkSprites("enemies/baby/nuke", content);

            bullet = content.Load<Texture2D>("bullet");
            house = content.Load<Texture2D>("houses/house");
            lightHouse = content.Load<Texture2D>("houses/lightHouse");
            inventorySlot = content.Load<Texture2D>("inventorySlot");
            landmine = content.Load<Texture2D>("items/landmine");
            mineThumb = content.Load<Texture2D>("items/mineThumb");
            chest = content.Load<Texture2D>("objects/chest");

            medicineItem = content.Load<Texture2D>("items/pill");
            beans = content.Load<Texture2D>("items/beans");
            bandage = content.Load<Texture2D>("items/bandage");
            fuelCan = content.Load<Texture2D>("items/fuelCan");

            twoByFour = content.Load<Texture2D>("items/twoByFour");
            machete = content.Load<Texture2D>("items/machete");
            sledgeHammer = content.Load<Texture2D>("items/sledgeHammer");

            boat = content.Load<Texture2D>("boat");
            sign = content.Load<Texture2D>("objects/sign");

            lightAmmo = mediumAmmo = heavyAmmo = content.Load<Texture2D>("items/ammo"); //should be different when we get sprites
            nails = content.Load<Texture2D>("items/nails");

            gunBarrel = content.Load<Texture2D>("items/gunParts/gunBarrel");
            gunMechs = content.Load<Texture2D>("items/gunParts/gunMechs");
            gunScrap = content.Load<Texture2D>("items/gunParts/gun scrap");
            rifleBarrel = content.Load<Texture2D>("items/gunParts/Rifle barrel");
            rifleHandle = content.Load<Texture2D>("items/gunParts/Rifle handle");
            rifleScrap = content.Load<Texture2D>("items/gunParts/Rifle scrap");

            hpBar = content.Load<Texture2D>("hpBar");
            hpRed = content.Load<Texture2D>("RedHP");

            healthBar = content.Load<Texture2D>("healthBar");
            hungerBar = content.Load<Texture2D>("bar_hunger");

            backButton = content.Load<Texture2D>("Buttons/back_button");
            backButtonL = content.Load<Texture2D>("Buttons/back_button_light");
            exitButton = content.Load<Texture2D>("Buttons/exit_button");
            exitButtonL = content.Load<Texture2D>("Buttons/exit_button_light");
            infoButton = content.Load<Texture2D>("Buttons/info_button");
            infoButtonL = content.Load<Texture2D>("Buttons/info_button_light");
            menuButton = content.Load<Texture2D>("Buttons/menu_button");
            menuButtonL = content.Load<Texture2D>("Buttons/menu_button_light");
            playButton = content.Load<Texture2D>("Buttons/play_button");
            playButtonL = content.Load<Texture2D>("Buttons/play_button_light");
            resumeButton = content.Load<Texture2D>("Buttons/resume_button");
            resumeButtonL = content.Load<Texture2D>("Buttons/resume_button_light");

            menuBackground = content.Load<Texture2D>("backgrounds/menu background");
            gameOverBackground = content.Load<Texture2D>("backgrounds/game over");

            deadLady = content.Load<Texture2D>("enemies/fatLady/dead");
            deadOneLeg = content.Load<Texture2D>("enemies/onelegged/dead");
            deadFisherman = content.Load<Texture2D>("enemies/fisherman/dead");

            signs = new Texture2D[8];
            for (int i = 0; i < signs.Length; i++)
            {
                signs[i] = content.Load<Texture2D>("signs/" + i);
            }
        }

        private static Texture2D[] LoadWalkSprites(string path, ContentManager content)
        {
            Texture2D[] t = new Texture2D[4];
            try { t[0] = content.Load<Texture2D>(path + "/up"); }
            catch { t[0] = content.Load<Texture2D>("player/walk/gun/up"); }
            t[1] = content.Load<Texture2D>(path + "/down");
            t[2] = content.Load<Texture2D>(path + "/left");
            t[3] = content.Load<Texture2D>(path + "/right");
            return t;
        }
    }
}
