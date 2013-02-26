using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    class Metal : FloatingObjects
    {
        public List<Entity> materialList = new List<Entity>();

        int materialCount = 25;

        FloatingObjects[] material;

        public Metal() 
        {
            material = new FloatingObjects[materialCount];
            _entityName = "Models//StationMaterial";
        } 

        public void CreateMaterialList() 
        {
            for (int i = 0; i < materialCount; i++)
            {
                if (materialList.Count < materialCount)
                {
                    material[i] = new FloatingObjects();
                    material[i].Initialize();
                    material[i]._entityName = _entityName;
                    material[i]._pos = new Vector3(randomGenerator.Next(-900, 900), 50, randomGenerator.Next(-900, 900));
                    materialList.Add(material[i]);
                }

                Game1.Instance().Children.Add(materialList.ElementAt(i));
            }
        }
    }
}
