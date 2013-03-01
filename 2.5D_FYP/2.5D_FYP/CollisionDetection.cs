using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class CollisionDetection
    {
        bool intersect;

        public CollisionDetection()
        {
        }

        public Boolean CheckCollision(Entity entity, List<Entity> children)
        {
            intersect = false;

            for (int i = 0; i < children.Count; i++)
            {
                if (!children.ElementAt(i).Equals(entity))
                {
                    intersect = entity._entitySphere.Intersects(children.ElementAt(i)._entitySphere);

                    if (intersect == true)
                    {
                        entity._entityCollisionFlag = true;
                        if (!(entity.GetType() == children.ElementAt(i).GetType()))
                        {
                            children.ElementAt(i)._entityCollisionFlag = true;
                        }

                        return true;
                    }
                    else { intersect = false; }
                }              
            }
            return intersect;
        }
    } // End of class CollisionDetection
}
