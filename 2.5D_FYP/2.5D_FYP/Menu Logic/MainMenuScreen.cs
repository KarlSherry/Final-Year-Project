using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace _2._5D_FYP
{
    class MainMenuScreen : BaseScreen
    {
        Texture2D menuBackground;
        SoundEffect gameMusic;

        public MainMenuScreen() 
        {
            menuItem = 0;
            menuButtons.Add("Start Game");
            menuButtons.Add("Highscores");
            menuButtons.Add("Controls");
            menuButtons.Add("Quit Game");
            menuBackground = Game1.Instance().Content.Load<Texture2D>("menuBackground1");
            //gameMusic = Game1.Instance().Content.Load<SoundEffect>("SoundEffects//Ifyouwantto-starlightremix");                

            //gameMusic.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (menuItem == 0 && isSelected == true)
            {
                Game1.GameState = "Game Screen";
            }
            if (menuItem == 1 && isSelected == true) Game1.GameState = "Highscore Screen";
            if (menuItem == 2 && isSelected == true) Game1.GameState = "Control Screen";
            if (menuItem == menuButtons.Count - 1 && isSelected == true) Game1.Instance().Exit();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int padding = 3;
            int menuItemPosX = Game1.Instance().GraphicsDevice.Viewport.Width / 2;
            int menuItemPosY = Game1.Instance().GraphicsDevice.Viewport.Height / 2;

            spriteBatch.Draw(menuBackground, new Vector2(0, 0), Color.White);

            for (int i = 0; i < menuButtons.Count; i++)
            {
                color = (i == menuItem) ? Color.Yellow : Color.White;

                spriteBatch.DrawString(menuFont, menuButtons[i],
                    new Vector2(menuItemPosX - (menuFont.MeasureString(menuButtons[i]).X /2),
                        menuItemPosY - (menuFont.LineSpacing * (menuButtons.Count /2)) + ((menuFont.LineSpacing + padding) * i)),
                        color);
            }
        }

    }
}

