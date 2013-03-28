using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class ControlScreen : BaseScreen
    {
        public ControlScreen() { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyState = Keyboard.GetState();

            if (Game1.Instance().menuSystem.gameScreen.isPaused && (keyState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.B == ButtonState.Pressed))
            {
                Game1.GameState = "Pause Menu Screen";
            }

            if (!Game1.Instance().menuSystem.gameScreen.isPaused && (keyState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.B == ButtonState.Pressed))
            {
                Game1.GameState = "Main Menu Screen";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
                spriteBatch.DrawString(menuFont, "Control Screen", new Vector2(10, 10), color);
            spriteBatch.End();
        }
    }
}
