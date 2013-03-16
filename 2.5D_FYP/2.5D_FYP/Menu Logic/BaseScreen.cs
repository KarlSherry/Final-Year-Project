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

        public int menuItem = 0;
        public bool isSelected = false;

        public SpriteFont menuFont;
        public Color color = Color.White;

        public List<string> menuButtons = new List<string>();

        public BaseScreen() { }

        public virtual void LoadContent(ContentManager Content) 
        {
            menuFont = Content.Load<SpriteFont>("menuFont");
        }

        public virtual void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (CheckKeyboardInput(Keys.Up))
            {
                if (menuItem > 0) menuItem--;                    
            }
            if (CheckKeyboardInput(Keys.Down))
            {
                if (menuItem < menuButtons.Count - 1) menuItem++;                    
            }
            if (CheckKeyboardInput(Keys.Enter))
            {
                isSelected = true;
            }
            else isSelected = false;

            prevKeyState = keyState;
        }

        public bool CheckKeyboardInput(Keys key)
        {
            return (keyState.IsKeyDown(key) && !prevKeyState.IsKeyDown(key));
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
