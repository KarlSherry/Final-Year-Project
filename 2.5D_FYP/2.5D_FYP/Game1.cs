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
using System.Timers;
using System.Diagnostics;


namespace _2._5D_FYP
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Stopwatch timer = new Stopwatch();
        public static int currentRound = 1;
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

        public MenuSystem menuSystem { get; set; }

        public List<Entity> StageList = new List<Entity>();
        public List<Entity> EnemyList = new List<Entity>();
        public List<Entity> AsteroidList = new List<Entity>();
        public List<Entity> MetalList = new List<Entity>();
        public List<Entity> PlayerBulletList = new List<Entity>();
        public List<Entity> EnemyBulletList = new List<Entity>();

        public World World { get; set; }
        public Camera Camera { get; set; }
        public Ground Ground { get; set; }
        public Station Station { get; set; }
        public Player Player { get; set; }
        public ForceField ForceField { get; set; }
        public Enemy[] Enemies { get; set; }
        public Asteroid[] Asteroids { get; set; }
        public Metal[] Metals { get; set; }


        public HUD HeadsUpDisplay { get; set; }

        public GameTime gametime;

        public static string GameState = "Main Menu Screen";

        public int EnemyCount, AsteroidCount, MetalCount;

        public ParticleSystem particleSystem;
        public ScoreSystem scoreSystem;

        public bool isNewGame = true;
        public bool hasMenuSystem = false;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            int EnemyBaseCount = 3, AsteroidBaseCount = 15, MetalBaseCount = 15;

            menuSystem = new MenuSystem();

            EnemyCount = EnemyBaseCount * currentRound;
            if (EnemyCount > 30) EnemyCount = 25;
            AsteroidCount = AsteroidBaseCount * currentRound;
            if (AsteroidCount > 50) AsteroidCount = 50;
            MetalCount = MetalBaseCount * currentRound;
            if (MetalCount > 50) MetalCount = 50;

            if (isNewGame)
            {
                timer = new Stopwatch();
                World = new World();//
                Camera = new Camera();//

                Ground = new Ground(StageList);//
                Station = new Station(StageList);//
                Player = new Player(StageList);
                ForceField = new ForceField(StageList);

                GameState = "Main Menu Screen";
                isNewGame = false;
            }

            Enemies = new Enemy[EnemyCount];
            for (int i = 0; i < EnemyCount; i++)
            {
                Enemies[i] = new Enemy(EnemyList);
            }

            Asteroids = new Asteroid[AsteroidCount];//
            for (int i = 0; i < AsteroidCount; i++) 
            {
                Asteroids[i] = new Asteroid(AsteroidList);
            }

            Metals = new Metal[MetalCount];//
            for (int i = 0; i < MetalCount; i++)
            {
                Metals[i] = new Metal(MetalList);
            }

            HeadsUpDisplay = new HUD();

            particleSystem = new ParticleSystem();
            scoreSystem = new ScoreSystem();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            menuSystem.LoadContent(this.Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < StageList.Count; i++)
                StageList[i].LoadContent();

            for (int i = 0; i < EnemyList.Count; i++)
                EnemyList[i].LoadContent();

            for (int i = 0; i < AsteroidList.Count; i++)
                AsteroidList[i].LoadContent();

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].LoadContent();

            HeadsUpDisplay.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            timer.Start();

            menuSystem.Update(gameTime);
            Camera.Update(gameTime);

            if (!menuSystem.gameScreen.isRunning)
            {
                isNewGame = true;
                StageList.Clear();
                EnemyList.Clear();
                AsteroidList.Clear();
                MetalList.Clear();
                EnemyBulletList.Clear();
                PlayerBulletList.Clear();

                Initialize();
                LoadContent();
                GameState = "Main Menu Screen";
                currentRound = 1;
            }

            previousRound = currentRound;

            if (EnemyList.Count <= 0 || MetalList.Count <= 0)
            {
                currentRound += 1;
            }

            if (currentRound != previousRound)
            {
                previousRound = currentRound;
                Initialize();
                LoadContent();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            menuSystem.Draw(gameTime, spriteBatch);
        }

        public static int getCurrentRound()
        {
            return currentRound;
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
