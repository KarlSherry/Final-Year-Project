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
        public Enemy[] Enemies { get; set; }
        public Asteroid[] Asteroids { get; set; }
        public Metal[] Metals { get; set; }

        public HUD HeadsUpDisplay { get; set; }

        public GameTime gametime;

        private int EnemyBaseCount = 15, AsteroidBaseCount = 100, MetalBaseCount = 100;
        private int EnemyCount, AsteroidCount, MetalCount;

        public ParticleSystem p;

        //public ParticleSystem p;
        public bool pHit = false;

        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            
            World = new World(StageList);//
            Camera = new Camera(StageList);//
            Ground = new Ground(StageList);//
            Station = new Station(StageList);//
            Player = new Player(StageList);//

            for (int i = 0; i < StageList.Count; i++)
                StageList[i].Initialize();
        }

        protected override void Initialize()
        {
            EnemyCount = EnemyBaseCount * currentRound;
            AsteroidCount = AsteroidBaseCount * currentRound;
            MetalCount = MetalBaseCount * currentRound;

            Enemies = new Enemy[EnemyCount];
            for (int i = 0; i < EnemyCount; i++)
            {
                Enemies[i] = new Enemy(EnemyList);
                Enemies[i].getEnemyType(Enemies[i].enemyTypeIndex);
                Enemies[i]._entityName = Enemies[i]._entityName + "_" + i;
                Console.WriteLine(Enemies[i]._entityName);
            }

            Asteroids = new Asteroid[AsteroidCount];//
            for (int i = 0; i < AsteroidCount; i++) 
            {
                Asteroids[i] = new Asteroid(AsteroidList);
                Asteroids[i]._entityName = Asteroids[i]._entityName + "_"+ i;
                Console.WriteLine(Asteroids[i]._entityName);
            }

            Metals = new Metal[MetalCount];//
            for (int i = 0; i < MetalCount; i++)
            {
                Metals[i] = new Metal(MetalList);
                Metals[i]._entityName = Metals[i]._entityName + "_" + i;
                Console.WriteLine(Metals[i]._entityName);
            }

            HeadsUpDisplay = new HUD();

            //for (int i = 0; i < StageList.Count; i++)
              //  StageList[i].Initialize();

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

            p = new ParticleSystem();

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
            //p.Destroy();
        }

        protected override void Update(GameTime gameTime)
        {
            timer.Start();

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            previousRound = currentRound;

            if (EnemyList.Count <= 0)
                currentRound += 1;

            if (currentRound != previousRound)
            {
                Initialize();
                LoadContent();
                previousRound = currentRound;
            }

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

            p.Update(gameTime);

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
            {
                AsteroidList[i].Draw(gameTime);
            }

            for (int i = 0; i < MetalList.Count; i++)
                MetalList[i].Draw(gameTime);

            for (int i = 0; i < PlayerBulletList.Count; i++)
                PlayerBulletList[i].Draw(gameTime);

            for (int i = 0; i < EnemyBulletList.Count; i++)
                EnemyBulletList[i].Draw(gameTime);

            p.Draw(gameTime);

            HeadsUpDisplay.Draw(gameTime);
            spriteBatch.End();
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
