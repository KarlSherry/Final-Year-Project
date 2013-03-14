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
        Texture2D playerHealthBar;
        Texture2D playerShieldBar;
        Texture2D playerCapacityBar;
        Vector3 playerPosition;
        int health;
        int shield;
        int capacity;

        public void LoadContent() 
        {
            gameFont = Game1.Instance().Content.Load<SpriteFont>("Verdana");
            playerHealthBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            playerShieldBar = Game1.Instance().Content.Load<Texture2D>("Bar");
            playerCapacityBar = Game1.Instance().Content.Load<Texture2D>("Bar");
        }

        public void Update(Player player, GameTime gameTime)
        {
            health = (int)MathHelper.Clamp(health, 0, 100);
            shield = (int)MathHelper.Clamp(shield, 0, 100);
            capacity = (int)MathHelper.Clamp(capacity, 0, 15);

            health = (int)player._health;
            shield = (int)player._shield;
            capacity = (int)player.capacity;
            playerPosition = player._pos;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.Instance().spriteBatch.DrawString(gameFont, shield + "%", new Vector2(215, 10), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, health + "%", new Vector2(215, 30), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Stored: " + capacity, new Vector2(115, 50), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Weapon: " + Game1.Instance().Player.weaponName, new Vector2(10, 70), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Player Position: (" + playerPosition.X + "," + playerPosition.Y + "," + playerPosition.Z + ")", new Vector2(10, 90), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Current Round: " + Game1.Instance().currentRound, new Vector2(10, 110), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Time: " + Game1.Instance().timer.Elapsed.Minutes + ":" + Game1.Instance().timer.Elapsed.Seconds, new Vector2(10, 130), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "Docked: " + Game1.Instance().Player.Docked, new Vector2(10, 150), Color.White);
            Game1.Instance().spriteBatch.DrawString(gameFont, "State: " + Game1.Instance().Player.playerState, new Vector2(10, 170), Color.White);

            // Draws the Player's shield bar
            Game1.Instance().spriteBatch.Draw(playerShieldBar, new Rectangle(10, 10, 200, 15), new Rectangle(0, 45, playerShieldBar.Width, 44), Color.Gray);
            Game1.Instance().spriteBatch.Draw(playerShieldBar, new Rectangle(10, 10, (int)(200 * ((double)shield / 100)), 15), new Rectangle(0, 45, playerShieldBar.Width, 44), Color.Blue); // Full Healthbar
            Game1.Instance().spriteBatch.Draw(playerShieldBar, new Rectangle(10, 10, 200, 15), new Rectangle(0, 0, playerShieldBar.Width, 44), Color.White); // Healthbar border

            // Draws the Player's health bar
            Game1.Instance().spriteBatch.Draw(playerHealthBar, new Rectangle(10, 30, 200, 15), new Rectangle(0, 45, playerHealthBar.Width, 44), Color.Gray); // Empty Healthbar
            Game1.Instance().spriteBatch.Draw(playerHealthBar, new Rectangle(10, 30, (int)(200 * ((double)health / 100)), 15), new Rectangle(0, 45, playerHealthBar.Width, 44), Color.Red); // Full Healthbar
            Game1.Instance().spriteBatch.Draw(playerHealthBar, new Rectangle(10, 30, 200, 15), new Rectangle(0, 0, playerHealthBar.Width, 44), Color.White); // Healthbar border
            
            // Draws the Player's capacity bar
            Game1.Instance().spriteBatch.Draw(playerCapacityBar, new Rectangle(10, 50, 100, 15), new Rectangle(0, 45, playerCapacityBar.Width, 44), Color.Gray); // Empty Healthbar
            Game1.Instance().spriteBatch.Draw(playerCapacityBar, new Rectangle(10, 50, (int)(100 * ((double)capacity / 15)), 15), new Rectangle(0, 45, playerCapacityBar.Width, 44), Color.Orange); // Full Healthbar
            Game1.Instance().spriteBatch.Draw(playerCapacityBar, new Rectangle(10, 50, 100, 15), new Rectangle(0, 0, playerCapacityBar.Width, 44), Color.White); // Healthbar border
        }
    }
}
