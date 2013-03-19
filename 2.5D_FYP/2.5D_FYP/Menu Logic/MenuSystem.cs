﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _2._5D_FYP
{
    public class MenuSystem
    {
        private string state;

        public static MenuSystem menus;

        MainMenuScreen mainMenuScreen;
        PauseMenuScreen pauseMenuScreen;
        public GameScreen gameScreen;
        public HighscoreScreen highscoreScreen;
        ControlScreen controlScreen;
        GameOverScreen gameOverScreen;
        public NewScoreScreen newScoreScreen;

        List<BaseScreen> screenList = new List<BaseScreen>();

        public MenuSystem() 
        {
            menus = this;

            mainMenuScreen = new MainMenuScreen();
            screenList.Add(mainMenuScreen);
            pauseMenuScreen = new PauseMenuScreen();
            screenList.Add(pauseMenuScreen);
            gameScreen = new GameScreen();
            screenList.Add(gameScreen);
            highscoreScreen = new HighscoreScreen();
            screenList.Add(highscoreScreen);
            controlScreen = new ControlScreen();
            screenList.Add(controlScreen);
            gameOverScreen = new GameOverScreen();
            screenList.Add(gameOverScreen);
            newScoreScreen = new NewScoreScreen(0);
            screenList.Add(newScoreScreen);
        }

        public void LoadContent(ContentManager Content)
        {
            for (int i = 0; i < screenList.Count; i++)
            {
                screenList[i].LoadContent(Content);
            }
        }

        public void Update(GameTime gameTime) 
        {
            state = getGameState();

            switch (state)
            {
                case "Main Menu Screen":
                    mainMenuScreen.Update(gameTime);
                    break;
                case "Pause Menu Screen":
                    pauseMenuScreen.Update(gameTime);
                    break;
                case "Game Screen":
                    gameScreen.Update(gameTime);
                    break;
                case "Highscore Screen":
                    highscoreScreen.Update(gameTime);
                    break;
                case "Control Screen":
                    controlScreen.Update(gameTime);
                    break;
                case "Game Over Screen":
                    gameOverScreen.Update(gameTime);
                    break;
                case "New Score Screen":
                    newScoreScreen.Update(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case "Main Menu Screen":
                    mainMenuScreen.Draw(spriteBatch);
                    break;
                case "Pause Menu Screen":
                    pauseMenuScreen.Draw(spriteBatch);
                    gameScreen.Draw(spriteBatch, gameTime);
                    break;
                case "Game Screen":
                    gameScreen.Draw(spriteBatch, gameTime);
                    break;
                case "Highscore Screen":
                    highscoreScreen.Draw(spriteBatch);
                    break;
                case "Control Screen":
                    controlScreen.Draw(spriteBatch);
                    break;
                case "Game Over Screen":
                    gameOverScreen.Draw(spriteBatch);
                    break;
                case "New Score Screen":
                    newScoreScreen.Draw(spriteBatch);
                    break;
            }
        }

        private string getGameState() 
        {
            return Game1.GameState;
        }
    }
}
