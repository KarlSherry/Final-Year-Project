using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class BaseScreen
    {
        public KeyboardState keyState;
        public KeyboardState prevKeyState;

        public GamePadState gamePadState;
        public GamePadState prevPadState;

        public int menuItem = 0;
        public bool isSelected = false;

        public SpriteFont menuFont;
        public Color color = Color.White;

        float vibrationAmount = 0.0f;

        public List<string> menuButtons = new List<string>();

        public BaseScreen() { }

        public virtual void LoadContent(ContentManager Content) 
        {
            menuFont = Content.Load<SpriteFont>("menuFont");
        }//

        public virtual void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            
            if (CheckKeyboardInput(Keys.Up) || (gamePadState.ThumbSticks.Left.Y > 0.5f && !(prevPadState.ThumbSticks.Left.Y > 0.5f)))
            {
                if (menuItem > 0) menuItem--;
            }
            if (CheckKeyboardInput(Keys.Down) || (gamePadState.ThumbSticks.Left.Y < -0.5f && !(prevPadState.ThumbSticks.Left.Y < -0.5f)))
            {
                if (menuItem < menuButtons.Count - 1) menuItem++;
            }
            if (CheckKeyboardInput(Keys.Enter) || (gamePadState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A == ButtonState.Released))
            {
                isSelected = true;
            }
            else isSelected = false;

            prevKeyState = keyState;
            prevPadState = gamePadState;
        }

        public bool CheckKeyboardInput(Keys key)
        {
            return (keyState.IsKeyDown(key) && !prevKeyState.IsKeyDown(key));
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
