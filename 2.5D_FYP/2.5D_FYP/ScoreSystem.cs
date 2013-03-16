using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class ScoreSystem
    {
        public struct Score
        {
            public string initials;
            public long score;
            public string scoreDate;
            public string scoreTime;
        }

        Score[] highScores = new Score[10];
        long playerScore;

        public ScoreSystem()
        {

        }

        public void Update(GameTime gameTime)
        {
            playerScore = Game1.Instance().Player.playerScore;

            if (playerScore > highScores[9].score)
            {
                highScores[9].score = playerScore;
                SortScores(highScores);
                WriteTextFile();

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(highScores[i].score + " " + highScores[i].initials + " " + highScores[i].scoreDate + " " + highScores[i].scoreTime);
                }
            }
        }

        private void ReadTextFile()
        {
            string line;

            using (StreamReader file = new StreamReader(@"C:\Users\Imaginote\Desktop\Karl_Sherry_FYP\ScoreSystem\ScoreSystem\ScoreSystemContent\highScores.txt"))
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

        private void WriteTextFile()
        {
            string line;

            using (StreamWriter file = new StreamWriter(@"C:\Users\Imaginote\Desktop\Karl_Sherry_FYP\ScoreSystem\ScoreSystem\ScoreSystemContent\highScores.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    char[] delimiters = new char[] { '\t' };

                    line = highScores[i].initials + delimiters[0] +
                    highScores[i].score + delimiters[0] +
                    highScores[i].scoreDate + delimiters[0] +
                    highScores[i].scoreTime;
                    file.WriteLine(line);
                }
                file.Close();
            }
        }

        private void SortScores(Score[] scores)
        {
            int i, min, max;
            Score temp;

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

    } // End of Class ScoreSystem
}
