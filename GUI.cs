using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO; 

namespace Mononoke
{
    class GUI
    {
        //uint UnitPerStamina = 1;
        //uint UnitPerHp = 1;
        Camera2D Camera;
        Texture2D FoodIcon;
        Texture2D WealthIcon;
        Texture2D PetrichorIcon;
        Texture2D LintIcon;
        Texture2D MunitionsIcon;
        Texture2D AlloyIcon;
        Texture2D DateBox;

        Color col;

        Vector2 spacing = new Vector2(60, 0);
        public GUI( Camera2D camera, GraphicsDeviceManager graphics, List<Apprentice> apprentices )
        {
            col = new Color(161, 200, 255);
            LoadIcons( graphics );
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
            //mApprenticeListView.Draw(spriteBatch, graphics);
            /*
            Vector2 origin = -Camera.Position;// + new Vector2( 0, 0);

            Vector2 DateboxPos = origin;
            Vector2 DayMonthPos = origin + new Vector2 ( 2, 2);
            Vector2 YearPos = DateboxPos + new Vector2(52, 0);

            Vector2 foodIconPos = DateboxPos + new Vector2( 64, 8) + spacing;
            Vector2 foodTextPos = foodIconPos + new Vector2(48, 0);

            Vector2 wealthIconPos = foodTextPos + spacing;
            Vector2 wealthTextPos = wealthIconPos + new Vector2( 48, 0 );

            Vector2 pIconPos = wealthTextPos + spacing;
            Vector2 pTextPos = pIconPos + new Vector2(48, 0);

            Vector2 lIconPos = pTextPos + spacing;
            Vector2 lTextPos = lIconPos + new Vector2(48, 0);

            Vector2 AlloyIconPos = lTextPos + spacing;
            Vector2 AlloyTextPos = AlloyIconPos + new Vector2(48, 0);

            //Vector2 WeaponsIconPos = AlloyTextPos + spacing;
            //Vector2 WeaponsTextPos = WeaponsIconPos + new Vector2(48, 0);

            spriteBatch.Draw(TextureAssetManager.GetSimpleSquare(), origin, null, col, 0f, Vector2.Zero, new Vector2(Mononoke.RENDER_WIDTH, 48), SpriteEffects.None, 0f);

            //spriteBatch.Draw(DateBox, DateboxPos, Color.White);
            //spriteBatch.DrawString(Mononoke.Font, IntegerToDay(Player.Time) + " " + IntegerToMonth( Player.Time ) , DayMonthPos, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(Mononoke.Font, IntegerToYear( Player.Time ) , YearPos, Color.White, 0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0);

            //spriteBatch.Draw( FoodIcon, foodIconPos, Color.White );
            //spriteBatch.DrawString( Mononoke.Font, Player.Food.ToString(), foodTextPos, Color.White );

            //spriteBatch.Draw( WealthIcon, wealthIconPos, Color.White);
            //spriteBatch.DrawString(Mononoke.Font, Player.Wealth.ToString(), wealthTextPos, Color.White);

            //spriteBatch.Draw( PetrichorIcon, pIconPos, Color.White);
            //spriteBatch.DrawString(Mononoke.Font, Player.Petrichor.ToString(), pTextPos, Color.White);

            //spriteBatch.Draw(LintIcon, lIconPos, Color.White);
            //spriteBatch.DrawString(Mononoke.Font, Player.Lint.ToString(), lTextPos, Color.White);

            //spriteBatch.Draw(AlloyIcon, AlloyIconPos, Color.White);
            //spriteBatch.DrawString(Mononoke.Font, Player.Alloys.ToString(), AlloyTextPos, Color.White);

            ////spriteBatch.Draw(baseTextureMap[tileBaseTextures[pos]], pos * MapHolder.PIXELS_PER_TILE, null, Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            ////spriteBatch.Draw(PetrichorIcon, lIconPos, Color.White);
            ////spriteBatch.DrawString(Mononoke.Font, Player.Weapons.ToString(), lTextPos, Color.White);

            //spriteBatch.DrawString(Mononoke.Font, Player.Stability.ToString(), AlloyTextPos + new Vector2(48, 0), Color.White);
            */
        }
        private void LoadIcons( GraphicsDeviceManager graphics )
        {
            FoodIcon = TextureAssetManager.GetIconByName( "food");
            WealthIcon = TextureAssetManager.GetIconByName("wealth");
            PetrichorIcon = TextureAssetManager.GetIconByName("petrichor");
            LintIcon = TextureAssetManager.GetIconByName("lint");
            AlloyIcon = TextureAssetManager.GetIconByName("enamel");
            MunitionsIcon = TextureAssetManager.GetIconByName("ammo");
            DateBox = TextureAssetManager.GetIconByName("food");

            string str = "data/textures/gui/date.png";
            if (!File.Exists(str))
            {
                throw new Exception("Icon does not exist at this path " + str);
            }
            DateBox = Texture2D.FromFile(graphics.GraphicsDevice, str);
        }
        // 0 = 01/01/1114
        // t = days since 01/01/1114
        // 360 days in a year
        // 30 days in a month
        private string IntegerToYear(int t)
        {
            return ((t / 360) + 1114).ToString() ;
        }
        private string IntegerToMonth( int t )
        { 
            return (( t % 360 ) / 30 ).ToString();
        }
        private string IntegerToDay(int t)
        {
            return ((t % 360) % 30).ToString();
        }
    }
}
