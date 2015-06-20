using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class WeaponPart
    {
        public enum PartType { Base, Barrel, Mag }

        public PartType Type { get; private set; }

        public float DamageMod { get; private set; }
        public float BulletSpeedMod { get; private set; }
        public float ShootSpeedMod { get; private set; }
        public float ReloadSpeedMod { get; private set; }
        public float RangeMod { get; private set; }
        public sbyte MagSizeMod { get; private set; }
        public bool MakesAuto { get; private set; }
        public Weapon.AmmoType[] AcceptableAmmo { get; private set; }
        //maybe armor penetration?

        public WeaponPart(PartType type, float damageMod, float speedMod, sbyte magSizeMod, bool makesAuto, float reloadSpeedMod, float rangeMod, Weapon.AmmoType[] acceptableAmmo)
        {
            this.Type = type;
            this.DamageMod = damageMod;
            this.BulletSpeedMod = speedMod;
            this.MagSizeMod = magSizeMod;
            this.MakesAuto = makesAuto;
            this.ReloadSpeedMod = reloadSpeedMod;
            this.AcceptableAmmo = acceptableAmmo;
            this.BulletSpeedMod = BulletSpeedMod;
        }

        public string GetInfoText()
        {
            string s = "";

            s += ConstructMessage(DamageMod, "Damage");
            s += ConstructMessage(BulletSpeedMod, "Bullet Speed");
            s += ConstructMessage(ShootSpeedMod, "Fire Rate");
            s += ConstructMessage(ReloadSpeedMod, "Reload Speed");
            s += ConstructMessage(MagSizeMod, "Ammo");
            if (MakesAuto) s += "Makes gun full auto\n";
            s += "Compatible ammo:\n";
            foreach (Weapon.AmmoType t in AcceptableAmmo) s += "  " + t + "\n";

            return s;
        }

        private string ConstructMessage(float modifier, string name)
        {
            if (modifier == 0) return "";
            string sign = "";
            if (modifier > 0) sign = "+";
            else if (modifier < 0) sign = "-";
            return name + " " + sign + (int)(modifier * 100) + "%\n";
        }

        private string ConstructMessage(int modifier, string name)
        {
            if (modifier == 0) return "";
            string sign = "";
            if (modifier > 0) sign = "+";
            else if (modifier < 0) sign = "-";
            return name + " " + sign + modifier + "\n";
        }
    }
}
