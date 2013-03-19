using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2._5D_FYP
{
    public class GameScreen : BaseScreen
    {
        public bool isRunning = false;
        public bool isPaused = false;
        public bool isNewGame = false;

        public GameScreen()
        {
            isRunning = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                isPaused = true;
                Game1.GameState = "Pause Menu Screen";
            }

            if (!isPaused) 
            {
                for (int i = 0; i < Game1.Instance().StageList.Count; i++)
                    Game1.Instance().StageList[i].Update(gameTime);

                for (int i = 0; i < Game1.Instance().EnemyList.Count; i++)
                    Game1.Instance().EnemyList[i].Update(gameTime);

                for (int i = 0; i < Game1.Instance().AsteroidList.Count; i++)
                    Game1.Instance().AsteroidList[i].Update(gameTime);

                for (int i = 0; i < Game1.Instance().MetalList.Count; i++)
                    Game1.Instance().MetalList[i].Update(gameTime);

                for (int i = 0; i < Game1.Instance().PlayerBulletList.Count; i++)
                    Game1.Instance().PlayerBulletList[i].Update(gameTime);

                for (int i = 0; i < Game1.Instance().EnemyBulletList.Count; i++)
                    Game1.Instance().EnemyBulletList[i].Update(gameTime);

                Game1.Instance().HeadsUpDisplay.Update(Game1.Instance().Player, gameTime);

                Game1.Instance().particleSystem.Update(gameTime);
            }

            if (!Game1.Instance().Player._alive)
            {
                if (Game1.Instance().Player.playerScore > MenuSystem.menus.highscoreScreen.highScores[9].score)
                {
                    MenuSystem.menus.newScoreScreen = new NewScoreScreen(Game1.Instance().Player.playerScore);
                    MenuSystem.menus.newScoreScreen.LoadContent(Game1.Instance().Content);
                    Game1.GameState = "New Score Screen";
                }
                else Game1.GameState = "Game Over Screen";
            }
        }
        #region Draw GameScreen
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < Game1.Instance().StageList.Count; i++)
            {
                DepthStencilState state = new DepthStencilState();
                state.DepthBufferEnable = true;
                Game1.Instance().GraphicsDevice.DepthStencilState = state;
                Game1.Instance().StageList[i].Draw(gameTime);
            }

            for (int i = 0; i < Game1.Instance().EnemyList.Count; i++)
                Game1.Instance().EnemyList[i].Draw(gameTime);

            for (int i = 0; i < Game1.Instance().AsteroidList.Count; i++)
            {
                Game1.Instance().AsteroidList[i].Draw(gameTime);
            }

            for (int i = 0; i < Game1.Instance().MetalList.Count; i++)
                Game1.Instance().MetalList[i].Draw(gameTime);

            for (int i = 0; i < Game1.Instance().PlayerBulletList.Count; i++)
                Game1.Instance().PlayerBulletList[i].Draw(gameTime);

            for (int i = 0; i < Game1.Instance().EnemyBulletList.Count; i++)
                Game1.Instance().EnemyBulletList[i].Draw(gameTime);

            Game1.Instance().particleSystem.Draw(gameTime);

            Game1.Instance().HeadsUpDisplay.Draw(spriteBatch);
        }
        #endregion
    }
}
