using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    class Metal : Asteroid
    {
        public List<Entity> materialList = new List<Entity>();

        int materialCount = 25;

        Asteroid[] material;

        public Metal() 
        {
            material = new Asteroid[materialCount];
            _entityModel = "Models//StationMaterial";
        } 

        public void CreateMaterialList() 
        {
            for (int i = 0; i < materialCount; i++)
            {
                if (materialList.Count < materialCount)
                {
                    material[i] = new Asteroid();
                    material[i].Initialize();
                    material[i]._entityModel = _entityModel;
                    material[i]._pos = new Vector3(randomGenerator.Next(-900, 900), 50, randomGenerator.Next(-900, 900));
                    materialList.Add(material[i]);
                }

                Game1.Instance().Children.Add(materialList.ElementAt(i));
            }
        }
    }
}
