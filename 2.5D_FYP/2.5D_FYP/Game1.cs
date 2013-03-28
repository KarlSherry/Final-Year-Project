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

        int EnemyBaseCount = 2, AsteroidBaseCount = 6, MetalBaseCount = 6;
        public int EnemyCount, AsteroidCount, MetalCount;

        public ParticleSystem particleSystem;
        public ScoreSystem scoreSystem;

        public bool isNewGame = true;
        public bool isRunning = false;
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
            isRunning = true;

            menuSystem = new MenuSystem();
            World = new World();//
            Camera = new Camera();//

            CreateNewGame();

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
                //CreateNewGame();
                StageList.Clear();
                EnemyList.Clear();
                AsteroidList.Clear();
                MetalList.Clear();
                EnemyBulletList.Clear();
                PlayerBulletList.Clear();
                
                currentRound = 1;
                Initialize();
                LoadContent();
                GameState = "Main Menu Screen";
            }

            previousRound = currentRound;

            if (EnemyList.Count <= 0 || MetalList.Count <= 0)
            {
                currentRound += 1;
            }

            if (currentRound != previousRound)
            {
                previousRound = currentRound;
                CreateNewRound();
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

        public void CreateNewGame()
        {
            GameState = "Main Menu Screen";

            StageList.Clear();
            EnemyList.Clear();
            AsteroidList.Clear();
            MetalList.Clear();
            EnemyBulletList.Clear();
            PlayerBulletList.Clear();

            EnemyCount = EnemyBaseCount * currentRound;
            if (EnemyCount > 25) EnemyCount = 20;
            AsteroidCount = AsteroidBaseCount * currentRound;
            if (AsteroidCount > 50) AsteroidCount = 60;
            MetalCount = MetalBaseCount * currentRound;
            if (MetalCount > 50) MetalCount = 60;

            currentRound = 1;
            timer = new Stopwatch();

            Ground = new Ground(StageList);//
            Station = new Station(StageList);//
            Player = new Player(StageList);
            ForceField = new ForceField(StageList);

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
        }

        public void CreateNewRound()
        {
            EnemyList.Clear(); // Clear Enemy List
            AsteroidList.Clear(); // Clear Asteroid List
            MetalList.Clear(); // Clear Metal List

            // Update Enemy Count
            EnemyCount = EnemyBaseCount * currentRound;
            if (EnemyCount > 20) EnemyCount = 20;
            // Update Asteroid Count
            AsteroidCount = AsteroidBaseCount * currentRound;
            if (AsteroidCount > 60) AsteroidCount = 60;
            // Update Metal Count
            MetalCount = MetalBaseCount * currentRound; 
            if (MetalCount > 60) MetalCount = 60;

            // Create Enemies
            Enemies = new Enemy[EnemyCount]; 
            for (int i = 0; i < EnemyCount; i++)
                Enemies[i] = new Enemy(EnemyList);
            // Create Asteroids
            Asteroids = new Asteroid[AsteroidCount];
            for (int i = 0; i < AsteroidCount; i++)
                Asteroids[i] = new Asteroid(AsteroidList);
            // Create Metals
            Metals = new Metal[MetalCount]; 
            for (int i = 0; i < MetalCount; i++)
                Metals[i] = new Metal(MetalList);

            LoadContent();
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
