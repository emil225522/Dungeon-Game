using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.WeaponClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.InvenoryStuff
{
    class ItemProperties
    {
        bool isAmmo;

        public string InfoText { get; private set; }
        public byte Width { get; private set; }
        public byte Height { get; private set; }
        public byte MaxStack { get; private set; }
        public float Weight { get; private set; }
        public Texture2D Texture { get; private set; }

        public bool IsConsumable { get { return onConsume != null; } }
        public bool IsWeaponPart { get { return WeaponPart != null; } }
        public bool IsAmmo { get { return isAmmo; } }
        public bool IsMeleeWeapon { get { return MeleeWeaponProperties != null; } }

        public WeaponPart WeaponPart { get; private set; }
        public Weapon.AmmoType AmmoType { get; private set; }
        public MeleeWeaponProperties MeleeWeaponProperties { get; private set; }
        public Action<Player> onConsume;


        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText) : this(width, height, weight, texture, maxStack, infoText, null, null, null, null) { }

        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText, Weapon.AmmoType ammoType) : this(width, height, weight, texture, maxStack, infoText, null, null, ammoType, null) { }

        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText, Action<Player> onConsume) : this(width, height, weight, texture, maxStack, infoText, onConsume, null, null, null) { }

        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText, WeaponPart part) : this(width, height, weight, texture, maxStack, infoText, null, part, null, null) { }

        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText, MeleeWeaponProperties meleeWeaponProperties) : this(width, height, weight, texture, maxStack, infoText, null, null, null, meleeWeaponProperties) { }

        public ItemProperties(byte width, byte height, float weight, Texture2D texture, byte maxStack, string infoText, Action<Player> onConsume, WeaponPart weaponPart, Weapon.AmmoType? ammoType, MeleeWeaponProperties meleeWeaponProperties)
        {
            this.Width = width;
            this.Height = height;
            this.Weight = weight;
            this.onConsume = onConsume;
            this.WeaponPart = weaponPart;
            this.MaxStack = maxStack;
            this.Texture = texture;
            this.InfoText = infoText;
            this.MeleeWeaponProperties = meleeWeaponProperties;
            if (ammoType != null)
            {
                isAmmo = true;
                AmmoType = (Weapon.AmmoType)ammoType;
            }
            else isAmmo = false;
        }
    }
}
