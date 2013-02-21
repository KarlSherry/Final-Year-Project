using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    class Material : FloatingObjects
    {
        Random randomGenerator = new Random(DateTime.Now.Millisecond);

        public Material()
        {
            objectName = "StationMaterial";
        }

        public void CreateMaterialList()
        {
            for (int i = 0; i < 25; i++)
            {
                FloatingObjects asteroid = new FloatingObjects();
                asteroid.objectName = objectName;
                asteroid._pos = new Vector3(randomGenerator.Next(-900, 900), 50, randomGenerator.Next(-900, 900));
                Game1.Instance().Children.Add(asteroid);
            }
        }
    }
}
