using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;


namespace _2._5D_FYP
{
    public class NewScoreScreen : BaseScreen
    {
        float screenRunTime = 0;
        float maxScreenTime = 10;

        long playerScore;

        string congrats = "Congratulations! You've made the top 10!";

        public NewScoreScreen(long score) 
        {
            playerScore = score;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerScore > MenuSystem.menus.highscoreScreen.highScores[9].score)
            {
                MenuSystem.menus.highscoreScreen.highScores[9].score = playerScore;
                SortScores(MenuSystem.menus.highscoreScreen.highScores);
                WriteTextFile();
                playerScore = 0;
            }

            if (screenRunTime > maxScreenTime)
            {
                MenuSystem.menus.highscoreScreen = new HighscoreScreen();
                MenuSystem.menus.highscoreScreen.LoadContent(Game1.Instance().Content);
                Game1.GameState = "Highscore Screen";
                MenuSystem.menus.gameScreen.isRunning = false;
                screenRunTime = 0;
            }

            screenRunTime += timeDelta;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(menuFont, congrats,
                new Vector2((Game1.Instance().GraphicsDevice.Viewport.Width / 2) - (menuFont.MeasureString(congrats).X / 2),
                    Game1.Instance().GraphicsDevice.Viewport.Height / 2), color);
            spriteBatch.End();
        }

        private void WriteTextFile()
        {
            string line;
            using (StreamWriter file = new StreamWriter(@"C:\Users\Imaginote\Desktop\Karl_Sherry_FYP\2.5D_FYP\2.5D_FYP\2.5D_FYPContent\highScores.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    char[] delimiters = new char[] { '\t' };

                    line = MenuSystem.menus.highscoreScreen.highScores[i].initials + delimiters[0] +
                    MenuSystem.menus.highscoreScreen.highScores[i].score + delimiters[0] +
                    MenuSystem.menus.highscoreScreen.highScores[i].scoreDate + delimiters[0] +
                    MenuSystem.menus.highscoreScreen.highScores[i].scoreTime;
                    file.WriteLine(line);
                }
                file.Close();
            }
        }

        private void SortScores(_2._5D_FYP.HighscoreScreen.Score[] scores)
        {
            int i, min, max;
            _2._5D_FYP.HighscoreScreen.Score temp;

            min = 0;
            max = scores.Length - 1;

            for (i = max; i > min; i--)
            {
                if (scores[i].score > scores[i - 1].score)
                {
                    temp = scores[i - 1];
                    scores[i - 1] = scores[i];
                    scores[i] = temp;
                }
            }
        }
    }
}
