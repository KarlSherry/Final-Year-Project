using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class Asteroid : FloatingObjects
    {
        public Asteroid()
        {
            _entityName = "Models//Asteroid";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void CreateAsteroidList()
        {
            for (int i = 0; i < 25; i++)
            {
                FloatingObjects asteroid = new FloatingObjects();
                asteroid._entityName = _entityName;
                asteroid.LoadContent();
                asteroid._pos = new Vector3(randomGenerator.Next(-900, 900), 50, randomGenerator.Next(-900, 900));
                Game1.Instance().Children.Add(asteroid);

                //System.Threading.Thread.Sleep(3000); // Delay implemented to ensure the position and look vectors are truely random
            }
        }

    }
}
