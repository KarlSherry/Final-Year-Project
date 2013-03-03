using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2._5D_FYP
{
    public class World : Entity
    {
        public float worldWidth = 0, worldHeight = 0;

        public World() 
        {
            _entityName = "World";

            worldWidth = 5000;
            worldHeight = 5000;
        }
    }
}
