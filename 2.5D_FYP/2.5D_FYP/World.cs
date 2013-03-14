using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2._5D_FYP
{
    public class World : Entity
    {
        public int worldWidth = 0, worldHeight = 0;

        public World(List<Entity> list) 
        {
            _entityName = "World";

            worldWidth = 3000;
            worldHeight = 3000;

            parentList = list;
            parentList.Add(this);
        }
    }
}
