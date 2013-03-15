﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class HighscoreScreen : BaseScreen
    {
        public HighscoreScreen() { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                Game1.GameState = "Main Menu Screen";
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(menuFont, "Highscore Screen", new Vector2(10, 10), color);
        }
    }
}