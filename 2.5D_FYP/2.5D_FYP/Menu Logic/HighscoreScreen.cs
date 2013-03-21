using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace _2._5D_FYP
{
    public class HighscoreScreen : BaseScreen
    {
        Texture2D screenBackground;

        public struct Score
        {
            public string initials;
            public long score;
            public string scoreDate;
            public string scoreTime;
        }

        public Score[] highScores = new Score[10];
        string[] scoreLine = new string[10];

        public HighscoreScreen() 
        {
            screenBackground = Game1.Instance().Content.Load<Texture2D>("menuBackground1");
            ReadTextFile();
            for (int i = 0; i < 10; i++)
            {
                scoreLine[i] = (i + 1) + ".  " + highScores[i].initials + '\t' + highScores[i].score + '\t' + highScores[i].scoreDate + '\t' + highScores[i].scoreTime;
            }

            Console.WriteLine(highScores[9].score + " last score");
        }

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
            int padding = 3;
            int linePosX = 150;
            int linePosY = 150;

            spriteBatch.Draw(screenBackground, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(menuFont, "High Scores", new Vector2(500, 50), Color.White);

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(menuFont, scoreLine[i], 
                    new Vector2(linePosX,
                        (linePosY - menuFont.LineSpacing * (menuButtons.Count /2)) + ((menuFont.LineSpacing + padding) * i)), color);
            }
        }

        private void ReadTextFile()
        {
            string line;

            using (StreamReader file = new StreamReader(@"C:\Users\Imaginote\Desktop\Karl_Sherry_FYP\2.5D_FYP\2.5D_FYP\2.5D_FYPContent\highScores.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    line = file.ReadLine();
                    char[] delimiters = new char[] { '\t' };
                    string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    highScores[i].initials = parts[0];
                    highScores[i].score = Convert.ToInt64(parts[1]);
                    highScores[i].scoreDate = parts[2];
                    highScores[i].scoreTime = parts[3];
                }
                file.Close();
            }
        }
    }
}
