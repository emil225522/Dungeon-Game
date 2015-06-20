using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Scenes;
using Shoot__n_Loot.UI;
using Shoot__n_Loot.WeaponClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.InvenoryStuff
{
    class ItemSlot
    {
        const int BUTTON_W = 100, BUTTON_H = 0;

        public Item Item { get; private set; }
        public byte StackSize { get; private set; }

        public bool ShowingOptions { get; set; }

        Rectangle infoPos;

        Inventory parent;

        int x, y;

        bool hovering;

        public float Weight 
        {
            get
            {
                if (Item != null) return Item.Properties.Weight * StackSize;
                else return 0;
            }
        }

        List<Button> buttons;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">the inventory that contains this slot.</param>
        /// <param name="x">where in the container this slot resides</param>
        /// <param name="y">where in the container this slot resides</param>
        public ItemSlot(Inventory parent, int x, int y)
        {
            this.parent = parent;
            this.x = x;
            this.y = y;
            StackSize = 0;
        }

        /// <summary>
        /// if this slot contains this item or null one will be added to the stack. if it contains something else it will be overwritten.
        /// </summary>
        /// <param name="i">the item to be added.</param>
        public void Add(Item item)
        {
            if (Item == null)
            {
                StackSize = 1;
            }
            else if (Item.Properties != item.Properties)
            {
                StackSize = 1;
            }
            else StackSize++;

            Item = item;
        }

        public bool CanContain(Item i)
        {
            Debug.Write("checking if itemSlot " + this.x + ", " + this.y + " can contain " + i.Properties.InfoText);
            //first check if this slot already contains that item
            if (parent.Weight + i.Properties.Weight > parent.MaxWeight)
            {
                Debug.WriteLine("this inventory is too heavy");
                return false;
            }
            if (Item != null)
            {
                if (Item.Properties != i.Properties) { Debug.WriteLine("slot " + x + ", " + y + " contains a " + Item.Properties.InfoText + ". Stacksize = " + StackSize); return false; }
                else if (StackSize < Item.Properties.MaxStack) return true; //if its the same and the stack is not maxed, the item fits
                else return false;
            }
            //otherwise see if no other item is obstructing the slot
            foreach (ItemSlot s in parent.Slots)
            {
                for (int x = this.x; x < i.Properties.Width + this.x; x++)
                {
                    for (int y = this.y; y < i.Properties.Height + this.y; y++)
                    {
                        if (s.ExtendsTo(x, y)) { Debug.WriteLine("slot " + x + ", " + y + " is obstructed by " + s.Item.Properties.InfoText); return false; }
                    }
                }
            }
            //then see if the item would obstruct another slot if placed here
            for (int x = 0; x < i.Properties.Width; x++ )
            {
                for (int y = 0; y < i.Properties.Height; y++)
                {
                    if (x + this.x >= parent.Width || y + this.y >= parent.Height || parent.Slots[x + this.x, y + this.y].Item != null) { Debug.WriteLine("adding to slot " + x + ", " + y + " would obstruct other slot"); return false; }
                }
            }
            //if none of the above is true, the item fits
            return true;
        }

        /// <summary>
        /// checks if the item in this slot also obstructs the slot at the specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool ExtendsTo(int x, int y)
        {
            if (Item == null) return false;
            bool b =
                x <= this.x + Item.Properties.Width - 1 
                    &&
                y <= this.y + Item.Properties.Height - 1 
                    && 
                y >= this.y 
                    && 
                x >= this.x;
            Debug.WriteLine("slot at " + this.x + ", " + this.y + (b ? " does not" : "" ) + " obstructs the slot at " + x + ", " + y);
            return b;
        }

        /// <summary>
        /// removes the specified amount of items from the stack.
        /// </summary>
        /// <param name="num"></param>
        public void Remove(byte num)
        {
            for (int i = 0; i < num; i++)
            {
                StackSize--;

                if (StackSize == 0)
                {
                    Debug.WriteLine("no items in this slot");
                    Item = null;
                    break;
                }
                
            }
        }

        /// <summary>
        /// creates the buttons that appear when this item is selected, setting the position to the right spot
        /// </summary>
        private void SetButtons(int xOffset, int yOffset, Inventory container, bool isPlayerInventory)
        {
            if (buttons == null) buttons = new List<Button>();
            else buttons.Clear();
            Rectangle baseRect =  new Rectangle(container.PositionForItem(xOffset, yOffset).X, container.PositionForItem(xOffset, yOffset).Y, BUTTON_W, BUTTON_H);

            if (isPlayerInventory) AddButton(buttons, new Button("drop", baseRect, DropItem));

            if (StackSize > 1 && isPlayerInventory) AddButton(buttons, new Button("drop all", baseRect, DropAll));

            if (Item.Properties.IsConsumable) AddButton(buttons, new Button("use", baseRect, Consume));

            if (Item.Properties.IsWeaponPart)
            {
                if (ArrayOverlaps(SceneManager.gameScene.player.weapon.CompatitbleAmmoTypes(Item.Properties.WeaponPart.Type), Item.Properties.WeaponPart.AcceptableAmmo))
                    AddButton(buttons, new Button("use in weapon", baseRect, UseInWeapon));
                else AddButton(buttons, new Button("This part is not compatible with your weapon", baseRect, Color.Red));
            }

            if (Item.Properties.IsAmmo && isPlayerInventory)
            {
                if (SceneManager.gameScene.player.weapon.CompatitbleAmmoTypes(null).Contains(Item.Properties.AmmoType))
                {
                    if (SceneManager.gameScene.player.weapon.currentAmmoType == Item.Properties.AmmoType) AddButton(buttons, new Button("You're using this ammo already", baseRect, Color.LimeGreen));
                    else AddButton(buttons, new Button("Use this ammo", baseRect, UseAsAmmo));
                }
                else AddButton(buttons, new Button("Your weapon can't use this ammo", baseRect, Color.Red));
            }

            if (Item.Properties.IsMeleeWeapon && isPlayerInventory)
            {
                if (Item.Properties.MeleeWeaponProperties != SceneManager.gameScene.player.MeleeWeapon) AddButton(buttons, new Button("Use as melee weapon", baseRect, UseMeleeWeapon));
                else AddButton(buttons, new Button("You're using this weapon already", baseRect, Color.LimeGreen));
            }

            if (Item.Properties == Items.properties[7]) //this is a can of beans
            {
                if (parent.Contains(Items.properties[16]) && isPlayerInventory) //2x4"
                {
                    AddButton(buttons, new Button("Attach to 2x4\"", baseRect, AttachCanToWood));
                }
            }
        }


        /// <summary>
        /// check if two arrays share any elements
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        bool ArrayOverlaps(Weapon.AmmoType[] a1, Weapon.AmmoType[] a2)
        {
            foreach (Weapon.AmmoType a in a1)
            {
                if (a2.Contains(a)) return true;
            }
            return false;
        }

        /// <summary>
        /// adds the specified button to the specified list, setting the position to align under the other buttons
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="b"></param>
        private void AddButton(List<Button> buttons, Button b)
        {
            if (buttons.Count > 0) b.Area = new Rectangle(b.Area.X, buttons[buttons.Count - 1].Area.Bottom, b.Area.Width, b.Area.Height);
            buttons.Add(b);
        }

        public void Update(bool isPlayerInventory)
        {
            if (Item != null && (ShowingOptions || hovering))
            {
                infoPos = parent.PositionForItem(x, y);
                infoPos.Width = (int)TextureManager.font.MeasureString(Item.Properties.InfoText).X + Button.PADDING_X * 2;
                infoPos.X -= infoPos.Width + 10;
                infoPos.Height = (int)TextureManager.font.MeasureString(Item.Properties.InfoText).Y + Button.PADDING_Y * 2;

                if (ShowingOptions)
                {
                    SetButtons(x, y, parent, isPlayerInventory);
                    foreach (Button b in buttons) b.Update();
                }

            }
            else ShowingOptions = false;

            if (Input.AreaIsClicked(parent.PositionForItem(x, y)) && Input.LeftClickWasJustPressed())
            {
                parent.HideAllItemMenus();
                ShowingOptions = !ShowingOptions;
            }
            hovering = Input.AreaIsHoveredOver(parent.PositionForItem(x, y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.inventorySlot, parent.PositionForItem(x, y), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000004f);
            if (Item != null)
            {
                Item.DrawInInventory(parent.PositionForItem(x, y), spriteBatch);
                spriteBatch.DrawString(TextureManager.font, StackSize.ToString(), new Vector2(parent.PositionForItem(x, y).X, parent.PositionForItem(x, y).Y), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0000002f);

                if (ShowingOptions)
                {
                    if (buttons != null) foreach (Button b in buttons) b.Draw(spriteBatch);
                }
                Debug.WriteLine(hovering + ", " + ShowingOptions);
                if (hovering || ShowingOptions)
                {
                    spriteBatch.Draw(TextureManager.inventorySlot, infoPos, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000001f);
                    spriteBatch.DrawString(TextureManager.font, Item.Properties.InfoText, new Vector2(infoPos.X + Button.PADDING_X, infoPos.Y + Button.PADDING_Y), Color.Black);
                }
            }
        }

        //--------------- THESE ARE USED AS DELEGATES FOR BUTTON ONCLICKS -------------
        void DropItem() 
        {
            SceneManager.gameScene.AddObject(new Item(Item.Properties, SceneManager.gameScene.player.Position));
            Remove(1);
        }

        void AttachCanToWood()
        {
            parent.Remove(Item, 1);
            parent.Remove(Items.properties[16], 1);
            parent.Add(new Item(Items.properties[18], Vector2.Zero));
        }

        void DropAll()
        {
            while (StackSize > 0) DropItem();
        }

        void Consume()
        {
            SoundManager.itemUse.Play();
            Item.Properties.onConsume(SceneManager.gameScene.player);
            Remove(1);
        }

        void UseInWeapon()
        {
            SoundManager.itemUse.Play();
            Item i = SceneManager.gameScene.player.weapon.AddPart(Item);
            Remove(1);
            if (i != null) SceneManager.gameScene.player.inventory.Add(i);
        }

        void UseAsAmmo()
        {
            SoundManager.itemUse.Play();
            SceneManager.gameScene.player.weapon.currentAmmoType = Item.Properties.AmmoType;
        }

        void UseMeleeWeapon()
        {
            SceneManager.gameScene.player.MeleeWeapon = Item.Properties.MeleeWeaponProperties;
        }
    }
}
