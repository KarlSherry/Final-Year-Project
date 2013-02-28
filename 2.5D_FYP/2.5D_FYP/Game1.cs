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
        private static Game1 instance = null;
        public static Game1 Instance() { return instance; }
        GraphicsDeviceManager graphics;

        public SpriteBatch spriteBatch;
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }

        List<Entity> children = new List<Entity>();
        public List<Entity> Children
        {
            get { return children; }
            set { children = value; }
        }

        private Ground ground;
        public Ground Ground
        {
            get { return ground; }
            set { ground = value; }
        }

        public World world;

        private Camera camera;
        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        private Player player;
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        private HUD headsUpDisplay;

        private Station station;
        public Station Station
        {
            get { return station; }
            set { station = value; }
        }

        private Asteroid[] asteroid;
        public Asteroid[] Asteroid
        {
            get { return asteroid; }
            set { asteroid = value; }
        }

        private int AsteroidCount = 100;

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
            camera = new Camera();
            children.Add(camera);

            ground = new Ground();
            children.Add(ground);

            world = new World();
            children.Add(world);

            player = new Player();
            children.Add(player);

            station = new Station();
            children.Add(station);

            asteroid = new Asteroid[AsteroidCount];
            for (int i = 0; i < AsteroidCount; i++) 
            {
                asteroid[i] = new Asteroid();
                asteroid[i]._entityModel = "Models//Asteroid";
                asteroid[i]._entityName = "Asteroid-*" + i;
                Console.WriteLine(asteroid[i]._entityName);
                asteroid[i]._pos = new Vector3(Entity.randomGenerator.Next(-900, 900), 50, Entity.randomGenerator.Next(-900, 900));
                Children.Add(asteroid[i]);
            }

            headsUpDisplay = new HUD();

            for (int i = 0; i < children.Count; i++)
                children[i].Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < children.Count; i++)
                children[i].LoadContent();

            headsUpDisplay.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);
            }

            //collisionDetection.CheckCollision(player, children);
            headsUpDisplay.Update(player, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);            

            spriteBatch.Begin();
            for (int i = 0; i < children.Count; i++)
            {
                DepthStencilState state = new DepthStencilState();
                state.DepthBufferEnable = true;
                GraphicsDevice.DepthStencilState = state;
                children[i].Draw(gameTime);
            }

            headsUpDisplay.Draw(gameTime);
            spriteBatch.End();
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
