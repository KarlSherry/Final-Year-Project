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
        MiniMap miniMap;

        Texture2D mapUnderLay;
        Texture2D mapPlayer;
        Texture2D mapStation;
        Texture2D mapAsteroids;
        Texture2D mapMetals;
        Texture2D mapEnemies;

        public bool isRunning = false;
        public bool isPaused = false;
        public bool isNewGame = false;

        public GameScreen()
        {
            isRunning = true;

            miniMap = new MiniMap();
            miniMap.Zoom = .03f;
            miniMap._pos = Vector2.Zero;

            mapUnderLay = Game1.Instance().Content.Load<Texture2D>("mapunderlay");
            mapPlayer = Game1.Instance().Content.Load<Texture2D>("mapShip");
            mapStation = Game1.Instance().Content.Load<Texture2D>("mapStation");
            mapAsteroids = Game1.Instance().Content.Load<Texture2D>("mapAsteroids");
            mapMetals = Game1.Instance().Content.Load<Texture2D>("mapAsteroids");
            mapEnemies = Game1.Instance().Content.Load<Texture2D>("mapAsteroids");
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

                Game1.Instance().HeadsUpDisplay.Update(gameTime);

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
            spriteBatch.Begin();

            spriteBatch.Draw(mapUnderLay, new Vector2(1200 - mapUnderLay.Width, 800 - mapUnderLay.Height), Color.White);

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
                Game1.Instance().AsteroidList[i].Draw(gameTime);
            
            for (int i = 0; i < Game1.Instance().MetalList.Count; i++)
                Game1.Instance().MetalList[i].Draw(gameTime);

            for (int i = 0; i < Game1.Instance().PlayerBulletList.Count; i++)
                Game1.Instance().PlayerBulletList[i].Draw(gameTime);

            for (int i = 0; i < Game1.Instance().EnemyBulletList.Count; i++)
                Game1.Instance().EnemyBulletList[i].Draw(gameTime);

            Game1.Instance().particleSystem.Draw(gameTime);

            Game1.Instance().HeadsUpDisplay.Draw(spriteBatch);

            var transformation2 = miniMap.get_transformation(Game1.Instance().GraphicsDevice);

            Vector2 mapPlayerPos = new Vector2(Game1.Instance().Player._pos.X, Game1.Instance().Player._pos.Z);

            spriteBatch.End();

            // Draw the mini map
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        miniMap.get_transformation(new Vector2(1200 - mapUnderLay.Width/2, 800 - mapUnderLay.Height/2)));
            spriteBatch.Draw(mapPlayer, mapPlayerPos, null, Color.White, -(Game1.Instance().Player.getYaw() + MathHelper.PiOver2), new Vector2(mapPlayer.Width / 2, mapPlayer.Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(mapStation, new Vector2(Game1.Instance().Station._pos.X - mapStation.Height / 2, Game1.Instance().Station._pos.Y - mapStation.Width / 2), Color.White);
            
            for (int i = 0; i < (Game1.Instance().AsteroidCount); i++)
            {
                if (Game1.Instance().Asteroids[i]._alive)
                {
                    Vector2 mapAsteroidPos = new Vector2(Game1.Instance().Asteroids[i]._pos.X, Game1.Instance().Asteroids[i]._pos.Z);
                    spriteBatch.Draw(mapAsteroids, mapAsteroidPos, Color.Blue);
                }
            }
            for (int i = 0; i < (Game1.Instance().MetalCount); i++)
            {
                if (Game1.Instance().Metals[i]._alive)
                {
                    Vector2 mapMetalPos = new Vector2(Game1.Instance().Metals[i]._pos.X, Game1.Instance().Metals[i]._pos.Z);
                    spriteBatch.Draw(mapAsteroids, mapMetalPos, Color.Orange);
                }
            }
            for (int i = 0; i < (Game1.Instance().EnemyCount); i++)
            {
                if (Game1.Instance().Enemies[i]._alive)
                {
                    Vector2 mapEnemyPos = new Vector2(Game1.Instance().Enemies[i]._pos.X, Game1.Instance().Enemies[i]._pos.Z);
                    spriteBatch.Draw(mapAsteroids, mapEnemyPos, Color.Red);
                }
            }
            spriteBatch.End();
        }
        #endregion
    }
}
