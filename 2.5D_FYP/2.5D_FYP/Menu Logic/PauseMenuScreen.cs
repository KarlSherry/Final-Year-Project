using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace _2._5D_FYP
{
    class PauseMenuScreen : BaseScreen
    {
        public PauseMenuScreen()
        {
            menuItem = 0;
            menuButtons.Add("Resume Game");
            menuButtons.Add("Controls");
            menuButtons.Add("Back to Main Menu");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (menuItem == 0 && isSelected == true)
            {
                menuItem = 0;
                Game1.Instance().menuSystem.gameScreen.isPaused = false;
                Game1.GameState = "Game Screen";
            }
            if (menuItem == 1 && isSelected == true)
            { 
                menuItem = 0;
                Game1.GameState = "Control Screen";
            }
            if (menuItem == menuButtons.Count - 1 && isSelected == true)
            {
                menuItem = 0;
                Game1.GameState = "Main Menu Screen";
                Game1.Instance().menuSystem.gameScreen.isRunning = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int padding = 3;
            int menuItemPosX = Game1.Instance().GraphicsDevice.Viewport.Width / 2;
            int menuItemPosY = Game1.Instance().GraphicsDevice.Viewport.Height / 2;

            spriteBatch.Begin();
            for (int i = 0; i < menuButtons.Count; i++)
            {
                color = (i == menuItem) ? Color.Yellow : Color.White;

                spriteBatch.DrawString(menuFont, menuButtons[i],
                    new Vector2(menuItemPosX - (menuFont.MeasureString(menuButtons[i]).X / 2),
                        menuItemPosY - (menuFont.LineSpacing * (menuButtons.Count / 2)) + ((menuFont.LineSpacing + padding) * i)),
                        color);
            }
            spriteBatch.End();
        }

    }
}

