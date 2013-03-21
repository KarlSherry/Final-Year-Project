using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class GameOverScreen : BaseScreen
    {
        float screenRunTime = 0;
        float maxScreenTime = 10;

        string gameOver = "GAME OVER";

        public GameOverScreen() { }

        public override void Update(GameTime gameTime)
        {
                base.Update(gameTime);

                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (screenRunTime > maxScreenTime)
                {                    
                    Game1.GameState = "Main Menu Screen";
                    screenRunTime = 0;
                    MenuSystem.menus.gameScreen.isRunning = false;
                }

                screenRunTime += timeDelta;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
                spriteBatch.DrawString(menuFont, gameOver,
                    new Vector2((Game1.Instance().GraphicsDevice.Viewport.Width / 2) - (menuFont.MeasureString(gameOver).X / 2),
                        Game1.Instance().GraphicsDevice.Viewport.Height / 2), color);
            spriteBatch.End();
        }
    }
}
