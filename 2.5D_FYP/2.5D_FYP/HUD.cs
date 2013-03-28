using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace _2._5D_FYP
{
    public class HUD
    {
        SpriteFont currentFont;
        SpriteFont smallFont;
        SpriteFont mediumFont;
        SpriteFont largeFont;

        int fontHeight;

        Player player = Game1.Instance().Player;
        ForceField forceField = Game1.Instance().ForceField;
        Station station = Game1.Instance().Station;

        float playerShield;
        float playerHealth;
        float playerCapacity;
        float forceFieldHealth;
        float stationHealth;

        string playerScore;

        int screenWidth = Game1.Instance().GraphicsDevice.Viewport.Width;
        int screenHeight = Game1.Instance().GraphicsDevice.Viewport.Height;
        int distanceToEdge = 20;        
        
        int HudLineA = 630;

        string[] playerHudLines = new string[7];

        public void LoadContent()
        {
            smallFont = Game1.Instance().Content.Load<SpriteFont>("small");
            smallFont.LineSpacing = 39;
            mediumFont = Game1.Instance().Content.Load<SpriteFont>("medium");
            mediumFont.LineSpacing = 39;
            largeFont = Game1.Instance().Content.Load<SpriteFont>("large");

            currentFont = smallFont;
        }

        public void Update(GameTime gameTime, bool enabled)
        {
            if (enabled)
            {
                playerShield = (float)player._shield;
                playerHealth = (float)player._health;
                playerCapacity = (float)player.capacity;
                forceFieldHealth = (float)forceField._health;
                stationHealth = (float)station._health;

                playerShield = (float)MathHelper.Clamp(playerShield, 0, 200);
                playerHealth = (float)MathHelper.Clamp(playerHealth, 0, 100);
                playerCapacity = (float)MathHelper.Clamp(playerCapacity, 0, 20);
                forceFieldHealth = (float)MathHelper.Clamp(forceFieldHealth, 0, 100);
                stationHealth = (float)MathHelper.Clamp(stationHealth, 0, 100);

                playerScore = player.playerScore.ToString();
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool enabled)
        {
            if (enabled)
            {
                Color underLayColor = new Color(50, 50, 50, 0);
                Rectangle playerHudUnderlay = new Rectangle();
                playerHudUnderlay.Width = 310;
                playerHudUnderlay.Height = 170;
                playerHudUnderlay.X = 10;
                playerHudUnderlay.Y = HudLineA - 10;

                Texture2D dummyTexture = new Texture2D(Game1.Instance().GraphicsDevice, 1, 1);
                dummyTexture.SetData(new Color[] { underLayColor });

                spriteBatch.Draw(dummyTexture, playerHudUnderlay, underLayColor);

                playerHudLines[0] = "Shield:    ";
                playerHudLines[1] = "Health:    ";
                playerHudLines[2] = "Cargo Bay: ";
                playerHudLines[3] = "Weapon: " + player.weaponName;
                playerHudLines[4] = "Time:   ";// +Game1.Instance().timer.Elapsed.Hours + ":" + Game1.Instance().timer.Elapsed.Minutes + ":" + Game1.Instance().timer.Elapsed.Seconds;
                playerHudLines[5] = "Docked: " + player.Docking;
                playerHudLines[6] = "State:  " + player.playerState;

                for (int i = 0; i < playerHudLines.Length; i++)
                {
                    if (i <= 2)
                        currentFont = smallFont;
                    else
                        currentFont = mediumFont;

                    fontHeight = (int)currentFont.MeasureString("Default").Y / 2 + 2;
                    spriteBatch.DrawString(currentFont, playerHudLines[i],new Vector2(distanceToEdge,(HudLineA + (fontHeight * i))),Color.White);
                    //DisplayText(spriteBatch, currentFont, playerHudLines[i], distanceToEdge, HudLineA + (fontHeight * i));
                }

                //DisplayText(spriteBatch, largeFont, "Round " + Game1.getCurrentRound(), (int)((screenWidth / 2) - largeFont.MeasureString("Round " + Game1.getCurrentRound()).X / 2), 10);
                //DisplayText(spriteBatch, mediumFont, "Score: " + playerScore, (int)((screenWidth / 2) - mediumFont.MeasureString("Score: " + playerScore).X / 2), 40);


                CreateBar(spriteBatch, 110, HudLineA, 200, 15, playerShield, 100, Color.CornflowerBlue);
                CreateBar(spriteBatch, 110, HudLineA + 20, 200, 15, playerHealth, 100, Color.Red);
                CreateBar(spriteBatch, 111, HudLineA + 40, 100, 15, playerCapacity, 20, Color.Orange);
                CreateBar(spriteBatch, 10, 10, 400, 30, forceFieldHealth, 100, Color.CornflowerBlue);
                CreateBar(spriteBatch, screenWidth - 410, 10, 400, 30, stationHealth, 100, Color.Red);
            }            
      
        }
        void DisplayText(SpriteBatch spriteBatch, SpriteFont font, string value, int xPos, int yPos) 
        {
            spriteBatch.DrawString(font, value, new Vector2(xPos, yPos), Color.White);
        }

        public void CreateBar(SpriteBatch spriteBatch, int xPos, int yPos, int recWidth, int recHeight, float health, int maxHealth, Color color)
        {
            Texture2D bar = Game1.Instance().Content.Load<Texture2D>("Bar");
            spriteBatch.Draw(bar, new Rectangle(xPos, yPos, recWidth, recHeight), new Rectangle(0, 45, bar.Width, 44), Color.Gray); // Empty Healthbar
            spriteBatch.Draw(bar, new Rectangle(xPos, yPos, (int)(recWidth * ((double)health / maxHealth)), recHeight), new Rectangle(0, 45, bar.Width, 44), color); // Full Healthbar
            spriteBatch.Draw(bar, new Rectangle(xPos, yPos, recWidth, recHeight), new Rectangle(0, 0, bar.Width, 44), Color.White); // Healthbar border
        }
    }    
}
