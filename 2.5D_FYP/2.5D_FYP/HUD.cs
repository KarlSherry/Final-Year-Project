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
        SpriteFont gameFont;

        Player player;
        ForceField forceField;
        Station station;

        Texture2D playerHealthBar;
        Texture2D playerShieldBar;
        Texture2D playerCapacityBar;
        Texture2D forceFieldHealthBar;
        Texture2D stationHealthBar;

        Vector3 playerPosition;

        float playerShield;
        float playerHealth;
        float playerCapacity;
        float forceFieldHealth;
        float stationHealth;

        string playerScore;

        public void LoadContent() 
        {
            gameFont = Game1.Instance().Content.Load<SpriteFont>("Verdana");

            player = Game1.Instance().Player;
            forceField = Game1.Instance().ForceField;
            station = Game1.Instance().Station;

            playerShieldBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            playerHealthBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            playerCapacityBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            forceFieldHealthBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            stationHealthBar = Game1.Instance().Content.Load<Texture2D>("Bar");
        }

        public void Update(GameTime gameTime)
        {
            playerShield = (float)MathHelper.Clamp(playerShield, 0, 100);
            playerHealth = (float)MathHelper.Clamp(playerHealth, 0, 100);
            playerCapacity = (float)MathHelper.Clamp(playerCapacity, 0, 15);
            forceFieldHealth = (float)MathHelper.Clamp(forceFieldHealth, 0, 100);
            stationHealth = (float)MathHelper.Clamp(stationHealth, 0, 100);
            
            playerShield = (float)player._shield;
            playerHealth = (float)player._health;
            playerCapacity = (float)player.capacity;
            forceFieldHealth = (float)forceField._health;
            stationHealth = (float)station._health;

            playerPosition = player._pos;

            playerScore = player.playerScore.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenWidth = Game1.Instance().GraphicsDevice.Viewport.Width;
            int screenHeight = Game1.Instance().GraphicsDevice.Viewport.Height;
            int textureWidth = forceFieldHealthBar.Width;
            int distanceToEdge = 10;

            spriteBatch.DrawString(gameFont, playerShield + "%", new Vector2(215, 10), Color.White);
            spriteBatch.DrawString(gameFont, playerHealth + "%", new Vector2(215, 30), Color.White);
            spriteBatch.DrawString(gameFont, "Stored: " + playerCapacity, new Vector2(115, 50), Color.White);
            spriteBatch.DrawString(gameFont, "Weapon: " + Game1.Instance().Player.weaponName, new Vector2(distanceToEdge, 70), Color.White);
            spriteBatch.DrawString(gameFont, "Player Position: (" + playerPosition.X + "," + playerPosition.Y + "," + playerPosition.Z + ")", new Vector2(distanceToEdge, 90), Color.White);
            spriteBatch.DrawString(gameFont, "Current Round: " + Game1.getCurrentRound(), new Vector2(distanceToEdge, 110), Color.White);
            spriteBatch.DrawString(gameFont, "Time: " + Game1.Instance().timer.Elapsed.Hours + ":" + Game1.Instance().timer.Elapsed.Minutes + ":" + Game1.Instance().timer.Elapsed.Seconds, new Vector2(distanceToEdge, 130), Color.White);
            spriteBatch.DrawString(gameFont, "Docked: " + Game1.Instance().Player.Docked, new Vector2(distanceToEdge, 150), Color.White);
            spriteBatch.DrawString(gameFont, "State: " + Game1.Instance().Player.playerState, new Vector2(distanceToEdge, 170), Color.White);

            spriteBatch.DrawString(gameFont, "Score: " + Game1.Instance().Player.playerScore,
                new Vector2((Game1.Instance().GraphicsDevice.Viewport.Width / 2) - gameFont.MeasureString(playerScore).X/2, 10), Color.White);

            // Draws the Player's shield bar
            spriteBatch.Draw(playerShieldBar, new Rectangle(distanceToEdge, 10, 200, 15), new Rectangle(0, 45, playerShieldBar.Width, 44), Color.Gray);
            spriteBatch.Draw(playerShieldBar, new Rectangle(distanceToEdge, 10, (int)(200 * ((double)playerShield / 100)), 15), new Rectangle(0, 45, playerShieldBar.Width, 44), Color.CornflowerBlue); // Full Healthbar
            spriteBatch.Draw(playerShieldBar, new Rectangle(distanceToEdge, 10, 200, 15), new Rectangle(0, 0, playerShieldBar.Width, 44), Color.White); // Healthbar border

            // Draws the Player's health bar
            spriteBatch.Draw(playerHealthBar, new Rectangle(distanceToEdge, 30, 200, 15), new Rectangle(0, 45, playerHealthBar.Width, 44), Color.Gray); // Empty Healthbar
            spriteBatch.Draw(playerHealthBar, new Rectangle(distanceToEdge, 30, (int)(200 * ((double)playerHealth / 100)), 15), new Rectangle(0, 45, playerHealthBar.Width, 44), Color.Red); // Full Healthbar
            spriteBatch.Draw(playerHealthBar, new Rectangle(distanceToEdge, 30, 200, 15), new Rectangle(0, 0, playerHealthBar.Width, 44), Color.White); // Healthbar border
            
            // Draws the Player's capacity bar
            spriteBatch.Draw(playerCapacityBar, new Rectangle(distanceToEdge, 50, 100, 15), new Rectangle(0, 45, playerCapacityBar.Width, 44), Color.Gray); // Empty Healthbar
            spriteBatch.Draw(playerCapacityBar, new Rectangle(distanceToEdge, 50, (int)(100 * ((double)playerCapacity / 15)), 15), new Rectangle(0, 45, playerCapacityBar.Width, 44), Color.Orange); // Full Healthbar
            spriteBatch.Draw(playerCapacityBar, new Rectangle(distanceToEdge, 50, 100, 15), new Rectangle(0, 0, playerCapacityBar.Width, 44), Color.White); // Healthbar border

            // Draws the ForceField's health bar
            spriteBatch.Draw(forceFieldHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 10, (textureWidth - 67), 15), new Rectangle(0, 45, forceFieldHealthBar.Width, 44), Color.Gray); // Empty Healthbar
            spriteBatch.Draw(forceFieldHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 10, (int)((textureWidth - 67) * ((double)forceFieldHealth / 100)), 15), new Rectangle(0, 45, forceFieldHealthBar.Width, 44), Color.CornflowerBlue); // Full Healthbar
            spriteBatch.Draw(forceFieldHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 10, (textureWidth - 67), 15), new Rectangle(0, 0, forceFieldHealthBar.Width, 44), Color.White); // Healthbar border

            // Draws the Station's health bar
            spriteBatch.Draw(stationHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 30, (textureWidth - 67), 15), new Rectangle(0, 45, stationHealthBar.Width, 44), Color.Gray); // Empty Healthbar
            spriteBatch.Draw(stationHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 30, (int)((textureWidth - 67) * ((double)stationHealth / 100)), 15), new Rectangle(0, 45, stationHealthBar.Width, 44), Color.Red); // Full Healthbar
            spriteBatch.Draw(stationHealthBar, new Rectangle((screenWidth - (textureWidth - 67) - distanceToEdge), 30, (textureWidth - 67), 15), new Rectangle(0, 0, stationHealthBar.Width, 44), Color.White); // Healthbar border
        }
    }
}
