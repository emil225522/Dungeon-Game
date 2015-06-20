using Microsoft.Xna.Framework;
using Shoot__n_Loot.InvenoryStuff;
using Shoot__n_Loot.Objects;
using Shoot__n_Loot.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.WeaponClasses
{
    static class Items
    {
        static ItemProperties
            ligthAmmo = new ItemProperties(1, 1, .02f, TextureManager.lightAmmo, 20, "Light Ammo", Weapon.AmmoType.Light),
            mediumAmmo = new ItemProperties(1, 1, .05f, TextureManager.lightAmmo, 20, "Medium Ammo", Weapon.AmmoType.Medium),
            heavyAmmo = new ItemProperties(1, 1, .1f, TextureManager.lightAmmo, 20, "Heavy Ammo", Weapon.AmmoType.Heavy),
            nailAmmo = new ItemProperties(1, 1, .07f, TextureManager.nails, 20, "Nails\nVery deadly", Weapon.AmmoType.Nails);

        public static Item RandomItem(Vector2 position)
        {
            while(true) 
            {
                int item = Game1.random.Next(properties.Length);
                if (Game1.random.NextDouble() < rarity[item]) 
                {
                    Item i = new Item(properties[item], position);
                    i.Position = position;
                    return i;
                }
            }
        }

        public static ItemProperties[] properties = new ItemProperties[]
        {
            //============== AMMO =====================
            ligthAmmo,
            mediumAmmo,
            heavyAmmo,
            nailAmmo,

            //============ MISC ITEMS ============================
            new ItemProperties(1, 1, 3, TextureManager.enemy1, 1, "Pink blob\nVery rare,\nVery useless"),
            new ItemProperties(2, 1, .1f, TextureManager.medicineItem, 10, "Heal 10 points", Health),
            new ItemProperties(1, 1, .1f, TextureManager.bandage, 10, "Bandage\nReduce bleeding", Bandage),
            new ItemProperties(1, 1, .2f, TextureManager.beans, 5, "Can of Beans\nRestore 10 hunger points", Hunger),
            new ItemProperties(1, 1, 1, TextureManager.mineThumb, 1, "Landmine\nExplodes on zombie contact\nUse to place where you stand", Landmine),
            new ItemProperties(2, 2, 1, TextureManager.fuelCan, 1, "Fuel Can\nDrop on a vehicle to fuel it"),

            //============= WEAPON PARTS ===============================
            new ItemProperties(1, 1, .45f, TextureManager.gunMechs, 1, "Automatic thing", new WeaponPart(WeaponPart.PartType.Base, 1, 1, 30, false, 1, 1, new Weapon.AmmoType[] { Weapon.AmmoType.Light, Weapon.AmmoType.Medium } )),
            new ItemProperties(1, 1, .4f, TextureManager.gunBarrel, 1, "Shitty gun barrel", new WeaponPart(WeaponPart.PartType.Barrel, -.1f, 0, 0, false, 10, 10, new Weapon.AmmoType[] { Weapon.AmmoType.Light } )),
            new ItemProperties(1, 1, .5f, TextureManager.gunScrap, 1, "Old gun scraps", new WeaponPart(WeaponPart.PartType.Mag, 0, 0, 10, true, -.5f, 0, new Weapon.AmmoType[] { Weapon.AmmoType.Medium, Weapon.AmmoType.Light } )),
            new ItemProperties(1, 1, .35f, TextureManager.rifleScrap, 1, "Heavy gun base", new WeaponPart(WeaponPart.PartType.Base, 1, 3, 10, false, 2, 10, new Weapon.AmmoType[] { Weapon.AmmoType.Heavy } )),
            new ItemProperties(1, 2, .5f, TextureManager.rifleBarrel, 1, "Rifle Barrel", new WeaponPart(WeaponPart.PartType.Barrel, -.03f, 0, 0, false, .1f, 1, new Weapon.AmmoType[] { Weapon.AmmoType.Medium, Weapon.AmmoType.Heavy } )),
            new ItemProperties(1, 1, 1, TextureManager.rifleHandle, 1, "Rifle Mechanics\nTurn your weapon into an automatic killing machine", new WeaponPart(WeaponPart.PartType.Base, 1, .5f, 0, true, -.5f, 0, new Weapon.AmmoType[] { Weapon.AmmoType.Light, Weapon.AmmoType.Medium, Weapon.AmmoType.Heavy } )),
            
            //============== MELEE WEAPONS ========================
            new ItemProperties(1, 2, .5f, TextureManager.twoByFour, 1, "2x4\" piece of wood\nCrushes sculls", new MeleeWeaponProperties(1, 15, TextureManager.playerWalkWood, TextureManager.playerAttackWood)),
            new ItemProperties(3, 1, 1, TextureManager.machete, 1, "Machete\nSharpest edge on the island", new MeleeWeaponProperties(3, 10)),
            new ItemProperties(2, 1, 1, TextureManager.sledgeHammer, 1, "Sledge Hammer\nCrushes sculls even harder", new MeleeWeaponProperties(3, 20, TextureManager.playerWalkSledgeHammer, TextureManager.playerAttackSledgeHammer))
        } ;

        public static float[] rarity = new float[] 
        {
            1,
            1,
            1,
            1,

            .05f,
            .8f,
            .7f,
            .8f,
            .3f,
            .25f,

            .35f,
            .35f,
            .35f,
            .15f,
            .15f,
            .15f,

            .9f,
            .3f,
            .01f
        };

        public static Item GetAmmo(Weapon.AmmoType type, Vector2 position)
        {
            switch (type)
            {
                case Weapon.AmmoType.Light :
                    return new Item(ligthAmmo, position);
                case Weapon.AmmoType.Medium:
                    return new Item(mediumAmmo, position);
                case Weapon.AmmoType.Heavy:
                    return new Item(heavyAmmo, position);
                case Weapon.AmmoType.Nails:
                    return new Item(nailAmmo, position);
                default: goto case Weapon.AmmoType.Light; // keep all the bases covered ( ͡° ͜ʖ ͡°)
            }
        }

        //======================== DELEGATES FOR ITEM USAGE =======================

        static void Health(Player p)
        {
            p.Health += 10;
        }

        static void Hunger(Player p)
        {
            p.Hunger -= 10;
            if (p.Hunger < 0) p.Hunger = 0;
        }

        static void Bandage(Player p)
        {
            p.bleeding -= .5f;
            if (p.bleeding < 0) p.bleeding = 0;
        }

        static void Landmine(Player p)
        {
            SceneManager.CurrentScene.AddObject(new Landmine(p.Center));
        }
    }
}
