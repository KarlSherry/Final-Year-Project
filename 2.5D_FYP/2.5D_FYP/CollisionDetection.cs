using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class CollisionDetection
    {
        static bool intersect;

        public CollisionDetection()
        {
            intersect = false;
        }

        public Boolean CheckCollision(Entity entity, List<Entity> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                intersect = entity._entitySphere.Intersects(children.ElementAt(i)._entitySphere);                
            }
            if (intersect == true)
            {
                Console.WriteLine("2 Entites are intersecting somewhere");
                return true;
            }
            else
            {
                return false;
            }
        }
    } // End of class CollisionDetection
}
