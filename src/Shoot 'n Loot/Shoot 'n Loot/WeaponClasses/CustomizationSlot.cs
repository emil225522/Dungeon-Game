using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shoot__n_Loot.Scenes;
using Shoot__n_Loot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot.WeaponClasses
{
    class CustomizationSlot
    {
        Rectangle position; // should be relative to camera
        public WeaponPart.PartType Type { get; private set; }
        List<Button> buttons;
        string partInfo;
        Texture2D thumbnail;

        private Rectangle WorldPosition { get { return new Rectangle(position.X + (int)Camera.Position.X, position.Y + (int)Camera.Position.Y, position.Width, position.Height); } }

        public CustomizationSlot(WeaponPart.PartType type, Rectangle position)
        {
            this.position = position;
            this.Type = type;
            buttons = new List<Button>();
            partInfo = "no info :(";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part">should be raw from weapon.partOfType, ie can be null</param>
        public void Update(Item part)
        {
            if (part != null)
            {
                foreach (Button b in buttons) b.Update();
                //on click create buttons, remove on another click or part removal
                thumbnail = part.Properties.Texture;
                partInfo = part.Properties.WeaponPart.GetInfoText();
                if (Input.AreaIsClicked(WorldPosition))
                {
                    if (buttons.Count == 0)
                    {
                        buttons.Add(new Button("Remove", WorldPosition, RemoveItem));
                        buttons[0].Area = new Rectangle(WorldPosition.X, WorldPosition.Y + 100, WorldPosition.Width, WorldPosition.Height);
                    }
                    else
                    {
                        buttons.Clear();
                    }

                }
                else if (buttons.Count == 1)
                {
                    buttons[0].Area = new Rectangle((int)WorldPosition.X, (int)WorldPosition.Y + 100, buttons[0].Area.Width, buttons[0].Area.Height);
                    if (Input.LeftClickWasJustPressed())
                        buttons.Clear();
                }
            }
            else
            {
                thumbnail = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.inventorySlot, WorldPosition, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.0000004f);
            spriteBatch.DrawString(TextureManager.font, Type.ToString(), new Vector2(WorldPosition.X, WorldPosition.Y - 25), Color.Black);
            if (thumbnail != null) spriteBatch.Draw(thumbnail, WorldPosition, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.00000035f);
            //if mouseover and no buttons draw part text
            if (Input.AreaIsHoveredOver(WorldPosition) && buttons.Count == 0)
            {
                const float padding = 15;
                Vector2 size = TextureManager.font.MeasureString(partInfo);
                spriteBatch.Draw(TextureManager.inventorySlot, new Rectangle(Input.newMs.X + (int)Camera.TotalOffset.X, Input.newMs.Y + (int)Camera.TotalOffset.Y, (int)(size.X + padding * 2), (int)(size.Y + padding * 2)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.00000025f);
                spriteBatch.DrawString(TextureManager.font, partInfo, new Vector2(Input.newMs.X + padding, Input.newMs.Y + padding) + Camera.TotalOffset, Color.Black);
            }
            foreach (Button b in buttons) b.Draw(spriteBatch);
        }

        void RemoveItem()
        {
            SceneManager.gameScene.player.inventory.Add(SceneManager.gameScene.player.weapon.RemovePart(Type));
        }
    }
}
