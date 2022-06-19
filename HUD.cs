using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO; 

namespace Mononoke
{
    class HUD
    {
        //uint UnitPerStamina = 1;
        //uint UnitPerHp = 1;
        Player Player;
        Camera2D Camera;
        Texture2D FoodIcon;
        Texture2D WealthIcon;
        Texture2D PetrichorIcon;
        Texture2D DateBox;

        Vector2 spacing = new Vector2(60, 0);

        public HUD( Player player, Camera2D camera, GraphicsDeviceManager graphics )
        {
            LoadIcons( graphics );
            Player = player;
            Camera = camera;
        }

        public void SetStaminaMax( uint stamina)
        {
            //StaminaBar.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //StaminaBar.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
            //StaminaBarBackground.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //StaminaBarBackground.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
        }
        public void SetStamina( uint stamina )
        {
            //StaminaImage.fillAmount = ( UnitPerStamina * stamina ) / ( StaminaBar.rect.width );
        }
        public void SetHealthMax( uint stamina)
        {
            //HealthBar.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, UnitPerStamina * stamina );
            //HealthBar.anchoredPosition = new Vector2( -900 + ( StaminaBar.rect.width / 2 ), StaminaBar.anchoredPosition.y );
        }
        public void SetHealth( uint stamina )
        {
            //HealthImage.fillAmount = ( UnitPerStamina * stamina ) / ( StaminaBar.rect.width );
        }
        public void Draw( SpriteBatch spriteBatch, GraphicsDeviceManager graphics )
        {
            Vector2 origin = -Camera.Position + new Vector2( 10, 10);

            Vector2 DateboxPos = origin;

            Vector2 foodIconPos = DateboxPos + new Vector2( 64, 0) + spacing;
            Vector2 foodTextPos = foodIconPos + new Vector2(48, 0);

            Vector2 wealthIconPos = foodTextPos + spacing;
            Vector2 wealthTextPos = wealthIconPos + new Vector2( 48, 0 );

            Vector2 oreIconPos = wealthTextPos + spacing;
            Vector2 oreTextPos = oreIconPos + new Vector2(48, 0);

            spriteBatch.Draw(DateBox, DateboxPos, Color.White);
            spriteBatch.DrawString(Mononoke.Font, "22/10", DateboxPos, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Mononoke.Font, "1114", DateboxPos + new Vector2(0, 16), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            spriteBatch.Draw( FoodIcon, foodIconPos, Color.White );
            spriteBatch.DrawString( Mononoke.Font, Player.Food.ToString(), foodTextPos, Color.White );

            spriteBatch.Draw( WealthIcon, wealthIconPos, Color.White);
            spriteBatch.DrawString(Mononoke.Font, Player.Wealth.ToString(), wealthTextPos, Color.White);

            spriteBatch.Draw( PetrichorIcon, oreIconPos, Color.White);
            spriteBatch.DrawString(Mononoke.Font, Player.Food.ToString(), oreTextPos, Color.White);

            //spriteBatch.DrawString(Mononoke.Font, Player.Stability.ToString(), -Camera.Position + new Vector2(40, 10), Color.White);
        }
        private void LoadIcons( GraphicsDeviceManager graphics )
        {
            string str = "data/textures/icons/food.png";
            if (!File.Exists(str))
            {
                throw new Exception("Icon does not exist at this path " + str);
            }
            FoodIcon = Texture2D.FromFile(graphics.GraphicsDevice, str);
            str = "data/textures/icons/wealth.png";
            if (!File.Exists(str))
            {
                throw new Exception("Icon does not exist at this path " + str);
            }
            WealthIcon = Texture2D.FromFile(graphics.GraphicsDevice, str);
            str = "data/textures/icons/ore1.png";
            if (!File.Exists(str))
            {
                throw new Exception("Icon does not exist at this path " + str);
            }
            PetrichorIcon = Texture2D.FromFile(graphics.GraphicsDevice, str);
            str = "data/textures/gui/date.png";
            if (!File.Exists(str))
            {
                throw new Exception("Icon does not exist at this path " + str);
            }
            DateBox = Texture2D.FromFile(graphics.GraphicsDevice, str);
        }
    }
}
