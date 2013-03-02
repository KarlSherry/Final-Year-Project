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

        public Ground Gound {get; set; }
        public World World { get; set; }
        public Camera Camera { get; set; }
        public Player Player { get; set; }
        public HUD HeadsUpDisplay { get; set; }
        public Station Station { get; set; }
        public Asteroid[] Asteroid { get; set; }

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
            Camera = new Camera();
            children.Add(Camera);

            Gound = new Ground();
            children.Add(Gound);

            World = new World();
            children.Add(World);

            Player = new Player();
            children.Add(Player);

            Station = new Station();
            children.Add(Station);

            Asteroid = new Asteroid[AsteroidCount];
            for (int i = 0; i < AsteroidCount; i++) 
            {
                Asteroid[i] = new Asteroid();
                Asteroid[i]._entityName = Asteroid[i]._entityName + "_"+ i;
                Children.Add(Asteroid[i]);
            }

            HeadsUpDisplay = new HUD();

            for (int i = 0; i < children.Count; i++)
                children[i].Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < children.Count; i++)
                children[i].LoadContent();

            HeadsUpDisplay.LoadContent();
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
            
            HeadsUpDisplay.Update(Player, gameTime);

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

            HeadsUpDisplay.Draw(gameTime);
            spriteBatch.End();
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        { 
            get { return graphics; } 
        }
    }
}
