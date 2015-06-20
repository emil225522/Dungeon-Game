using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shoot__n_Loot.InvenoryStuff;
using Shoot__n_Loot.Scenes;
using Shoot__n_Loot.UI;
using Shoot__n_Loot.WeaponClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Weapon
    {
        const float baseDamage = 1;
        const float baseBulletDamage = 1;
        const float baseBulletSpeed = 10;
        const int baseRange = 300;
        const byte baseShootTime =  15;
        const byte baseReloadTime = 60;
        const byte baseMagSize = 10;

        public enum AmmoType { None = 0, Light = 5, Medium = 8, Heavy = 10, Nails  = 20}

        public AmmoType currentAmmoType;

        public int Parts { get { return parts.Count; } }
                
        private BulletProperties BulletProperties
        {
            get
            {
                return new BulletProperties(bulletDamage * (int)currentAmmoType / 10f, bulletSpeed, range);
            }
        }

        //private byte bulletPenetration;
        private byte shootTime;
        private byte reloadTime;
        private byte magSize;
        private float bulletSpeed;
        private float bulletDamage;
        private int range;

        public bool IsAuto { get; private set; }

        private byte reloadTimer;
        private byte shootTimer;
        
        public byte Ammo { get; private set; }

        private List<Item> parts;

        private List<CustomizationSlot> partSlots;



        public Weapon()
        {
            parts = new List<Item>();
            reloadTimer = 0;
            shootTimer = 0;
            CalculateValues();
            partSlots = new List<CustomizationSlot>();
            MakeSlots();
        }

        public void ShootingUpdate(Inventory bulletContainer)
        {
            if (reloadTimer > 0) reloadTimer++;
            if (reloadTimer >= reloadTime)
            {
                reloadTimer = 0;
                Reload(bulletContainer);
            }

            if (shootTimer > 0) shootTimer++;
            if (shootTimer >= shootTime) shootTimer = 0;
        }

        #region Part Related Stuff
        /// <summary>
        /// returns the replaced part if one existed, otherwise null
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public Item AddPart(Item part)
        {
            Item oldPart = RemovePart(part.Properties.WeaponPart.Type);
            parts.Add(part);

            CalculateValues();
            return oldPart;
        }

        public bool ContainsType(WeaponPart.PartType t)
        {
            foreach (Item p in parts) if (p.Properties.WeaponPart.Type == t) return true;
            return false;
        }

        public Item PartOfType(WeaponPart.PartType t)
        {
            foreach (Item p in parts) if (p.Properties.WeaponPart.Type == t) return p;
            return null;
        }

        public void RemovePart(Item p)
        {
            parts.Remove(p);
            CalculateValues();
        }


        /// <summary>
        /// tries to remove the part of the specified type. Returns null if no part was found, otherwise returns the removed part.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Item RemovePart(WeaponPart.PartType type)
        {
            foreach (Item p in parts)
            {
                if (p.Properties.WeaponPart.Type == type)
                {
                    parts.Remove(p);
                    CalculateValues();
                    return p;
                }
            }
            return null;
        }

        public AmmoType[] CompatitbleAmmoTypes(WeaponPart.PartType? type)
        {
                List<AmmoType> types = new List<AmmoType>();
                foreach (AmmoType t in AmmoType.GetValues(typeof(AmmoType))) types.Add(t);

                foreach (Item i in parts)
                {
                    if (i.Properties.WeaponPart.Type == type) continue;
                    for (int j = types.Count - 1; j >= 0; j--)
                    {
                        if (!i.Properties.WeaponPart.AcceptableAmmo.Contains(types[j])) types.RemoveAt(j);
                    }
                }

                return types.ToArray();
        }
        #endregion

        #region ammo stuff
        private void Reload(Inventory bulletContainer)
        {
            //ammo = maxAmmo etc
            if (CompatitbleAmmoTypes(null).Contains(currentAmmoType))
            {
                Ammo = magSize;
                Ammo = GetAmmoFromInventory(bulletContainer, magSize);
            }
            else
            {
                currentAmmoType = AmmoType.None;
                Debug.WriteLine("removing bad ammo");
            }
        }

        public void StartReload(Inventory bulletContainer)
        {
            if (ContainsType(WeaponPart.PartType.Base))
            {
                reloadTimer = 1;
                DropAllAmmo();
            }
        }

        private void DropAllAmmo()
        {
            for (int i = 0; i < Ammo; i++)
            {
                SceneManager.gameScene.AddObject(Items.GetAmmo(currentAmmoType, SceneManager.gameScene.player.Position));
            }
            Ammo = 0;
        }

        /// <summary>
        /// removes the specified amount of ammo and returns the amount removed.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>how much was found</returns>
        byte GetAmmoFromInventory(Inventory i, byte amount)
        {
            byte found = 0;
            foreach (ItemSlot s in i.Slots)
            {
                if (s.Item != null)
                {
                    if (s.Item.Properties.IsAmmo)
                    {
                        if (s.Item.Properties.AmmoType == currentAmmoType)
                        {
                            if (s.StackSize >= amount - found)
                            {
                                s.Remove(amount);
                                return amount;
                            }
                            else
                            {
                                found += s.StackSize;
                                s.Remove(s.StackSize);
                            }
                        }
                    }
                }
            }
            return found;
        }
        
        /// <summary>
        /// checks if the weapon can shoot and if so adds a bullet
        /// </summary>
        /// <returns></returns>
        public void TryShoot(Vector2 position, float angle, Scene scene)
        {
            //TODO: check if necessary parts are present
            if (shootTimer == 0 && Ammo > 0)
            {
                scene.AddObject(new Bullet(angle, position, BulletProperties));
                SoundManager.playerShoot.Play();
                Ammo--;
                shootTimer = 1;
            }
        }
        #endregion

        /// <summary>
        /// draws the different parts similarly to an inventory
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Array types = WeaponPart.PartType.GetValues(typeof(WeaponPart.PartType));
            Vector2 center = Camera.Position - new Vector2(0, 300);
            const int padding = 30, itemSize = 50;
            float wPerI = (padding + itemSize);
            float totalW = types.Length * wPerI;
            for (int i = 0; i < types.Length; i++)
            {
                Rectangle position = new Rectangle((int)(center.X - totalW / 2 + i * wPerI), (int)(center.Y + itemSize / 2), itemSize, itemSize);
                spriteBatch.Draw(TextureManager.inventorySlot, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000004f);
                spriteBatch.DrawString(TextureManager.font, types.GetValue(i).ToString(), new Vector2(position.X, position.Y - 25), Color.Black);
                Item p = PartOfType((WeaponPart.PartType)types.GetValue(i));
                Debug.WriteLine(p == null);
                if (p != null) p.DrawInInventory(position, spriteBatch);
            }
        }

        #region customization ui related

        Button useWeapon;

        const int HUD_W = 400, HUD_H = 200, HUD_Y = 20;

        private void MakeSlots()
        {
            Array types = WeaponPart.PartType.GetValues(typeof(WeaponPart.PartType));
            Vector2 center = Camera.Position - new Vector2(0, 300);
            const int padding = 30, itemSize = 50;
            float wPerI = (padding + itemSize);
            float totalW = types.Length * wPerI;
            for (int i = 0; i < types.Length; i++)
            {
                Rectangle position = new Rectangle((int)(center.X - totalW / 2 + i * wPerI), (int)(center.Y + itemSize / 2), itemSize, itemSize);
                partSlots.Add(new CustomizationSlot((WeaponPart.PartType)types.GetValue(i), position));
            }
        }

        private void CalculateValues()
        {
            float shootTimeMod = 1;
            float reloadTimeMod = 1;
            sbyte magSizeMod = 0;
            float bulletSpeedMod = 1;
            float bulletDamageMod = 1;
            bool auto = false;
            float rangeMod = 1;

            foreach (Item p in parts)
            {
                shootTimeMod += p.Properties.WeaponPart.ShootSpeedMod;
                reloadTimeMod += p.Properties.WeaponPart.ReloadSpeedMod;
                magSizeMod += p.Properties.WeaponPart.MagSizeMod;
                bulletSpeedMod += p.Properties.WeaponPart.BulletSpeedMod;
                bulletDamageMod += p.Properties.WeaponPart.DamageMod;
                rangeMod += p.Properties.WeaponPart.RangeMod;
                if (p.Properties.WeaponPart.MakesAuto) auto = true;
            }

            shootTime = (byte)(baseShootTime * shootTimeMod);
            reloadTime = (byte)(baseReloadTime * reloadTimeMod);
            magSize = (byte)(baseMagSize + magSizeMod);
            bulletSpeed = baseBulletSpeed * bulletSpeedMod;
            bulletDamage = baseBulletDamage * bulletDamageMod;
            range = (int)(baseRange * rangeMod);
            IsAuto = auto;
        }

        public void CustomizingUpdate()
        {
            foreach (CustomizationSlot s in partSlots) s.Update(PartOfType(s.Type));
            if (SceneManager.gameScene.player.UsingMelee)
            {
                useWeapon = new Button("Use weapon", new Rectangle((int)Camera.Position.X - HUD_H / 2 + 20, (int)Camera.TotalOffset.Y + HUD_Y + HUD_H - 20, 0, 0), StopUsingMelee);
                useWeapon.Update();
            }
            else useWeapon = null;
        }

        public void DrawCustomization(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.inventorySlot, new Rectangle((int)Camera.Position.X - HUD_W / 2, (int)Camera.TotalOffset.Y + HUD_Y, HUD_W, HUD_H), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000005f);
            foreach (CustomizationSlot s in partSlots) s.Draw(spriteBatch);
            if (useWeapon != null) useWeapon.Draw(spriteBatch);
            spriteBatch.DrawString(TextureManager.font, "Using Ammo: " + currentAmmoType.ToString(), Camera.Center - new Vector2(100, 200), Color.Black);
            if (!ContainsType(WeaponPart.PartType.Base)) spriteBatch.DrawString(TextureManager.font, "You need a base for your weapon!", Camera.Center - new Vector2(175, 220), Color.Red);
        }

        void StopUsingMelee()
        {
            SceneManager.gameScene.player.MeleeWeapon = null;
        }

        #endregion
    }
}
