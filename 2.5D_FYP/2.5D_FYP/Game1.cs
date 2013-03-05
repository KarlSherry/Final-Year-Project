using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _2._5D_FYP
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public int currentRound = 1;
        int previousRound = 0;

        private static Game1 instance = null;
        public static Game1 Instance() { return instance; }
        GraphicsDeviceManager graphics;

        public SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        public List<Entity> StageList { get; set; }
        public List<Entity> EnemyList { get; set; }
        public List<Entity> AsteroidList { get; set; }
        public List<Entity> MetalList { get; set; }
        public List<Entity> PlayerBulletList { get; set; }
        public List<Entity> EnemyBulletList { get; set; }

        public World World { get; set; }
        public Camera Camera { get; set; }
        public Ground Ground { get; set; }
        public Station Station { get; set; }
        public Player Player { get; set; }
        public Enemy[] Enemies { get; set; }
        public Asteroid[] Asteroids { get; set; }
        public Metal[] Metals { get; set; }

        public HUD HeadsUpDisplay { get; set; }

        private int EnemyCount;
        private int AsteroidCount;
        private int MetalCount;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            EnemyCount = 5 * currentRound;
            AsteroidCount = 100;
            MetalCount = 0;

            StageList = new List<Entity>();
            EnemyList = new List<Entity>();
            AsteroidList = new List<Entity>();
            MetalList = new List<Entity>();
            PlayerBulletList = new List<Entity>();
            EnemyBulletList = new List<Entity>();

            World = new World(StageList);//
            Camera = new Camera(StageList);//
            Ground = new Ground(StageList);//
            Station = new Station(StageList);//
            Player = new Player(StageList);//

            Enemies = new Enemy[EnemyCount];
            for (int i = 0; i < EnemyCount; i++)
            {
                Enemies[i] = new Enemy(EnemyList);
                Enemies[i]._entityName = Enemies[i]._entityName + "_" + i;
            }

            Asteroids = new Asteroid[AsteroidCount];//
            for (int i = 0; i < AsteroidCount; i++) 
            {
                Asteroids[i] = new Asteroid(AsteroidList);
                Asteroids[i]._entityName = Asteroids[i]._entityName + "_"+ i;
            }

            Metals = new Metal[MetalCount];//
            for (int i = 0; i < MetalCount; i++)
            {
                Metals[i] = new Metal(MetalList);
                Metals[i]._entityName = Metals[i]._entityName + "_" + i;
            }

            HeadsUpDisplay = new HUD();

            for (int i = 0; i < StageList.Count; i++)
                StageList[i].Initialize();

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].Initialize();

            for (int i = 0; i < AsteroidList.Count; i++)
                AsteroidList[i].Initialize();

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].Initialize();

            for (int i = 0; i < PlayerBulletList.Count; i++)
                PlayerBulletList[i].Initialize();

            for (int i = 0; i < EnemyBulletList.Count; i++)
                EnemyBulletList[i].Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < StageList.Count; i++)
                StageList[i].LoadContent();

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].LoadContent();

            for (int i = 0; i < AsteroidList.Count; i++)
                AsteroidList[i].LoadContent();

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].LoadContent();

            for (int i = 0; i < PlayerBulletList.Count; i++)
                PlayerBulletList[i].LoadContent();

            for (int i = 0; i < EnemyBulletList.Count; i++)
                EnemyBulletList[i].LoadContent();

            HeadsUpDisplay.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            previousRound = currentRound;

            if (EnemyList.Count <= 0)
                currentRound += 1;

            /*if (currentRound != previousRound)
            {
                Initialize();
                LoadContent();
                previousRound = currentRound;
            }*/

            for (int i = 0; i < StageList.Count; i++)
                StageList[i].Update(gameTime);

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].Update(gameTime);

            for (int i = 0; i < AsteroidList.Count; i++)
                AsteroidList[i].Update(gameTime);

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].Update(gameTime);

            for (int i = 0; i < PlayerBulletList.Count; i++)
                PlayerBulletList[i].Update(gameTime);

            for (int i = 0; i < EnemyBulletList.Count; i++)
                EnemyBulletList[i].Update(gameTime);
            
            HeadsUpDisplay.Update(Player, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);            

            spriteBatch.Begin();
            for (int i = 0; i < StageList.Count; i++)
            {
                DepthStencilState state = new DepthStencilState();
                state.DepthBufferEnable = true;
                GraphicsDevice.DepthStencilState = state;
                StageList[i].Draw(gameTime);
            }

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].Draw(gameTime);

            for (int i = 0; i < AsteroidList.Count; i++)
                AsteroidList[i].Draw(gameTime);

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].Draw(gameTime);

            for (int i = 0; i < PlayerBulletList.Count; i++)
                PlayerBulletList[i].Draw(gameTime);

            for (int i = 0; i < EnemyBulletList.Count; i++)
                EnemyBulletList[i].Draw(gameTime);

            HeadsUpDisplay.Draw(gameTime);
            spriteBatch.End();
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
