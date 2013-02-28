using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class CollisionDetection
    {
        bool intersect = false;
        Type collidedType;
        public Boolean CheckCollision(Entity entity, List<Entity> children)
        {
            collidedType = null;

            for (int i = 0; i < children.Count; i++)
            {
                if (entity._entitySphere.Intersects(children.ElementAt(i)._entitySphere) || children.ElementAt(i)._entitySphere.Intersects(entity._entitySphere))
                {
                    intersect = true;
                }
                else
                {
                    intersect = false;
                }
            }

            return intersect;
        }
    } // End of class CollisionDetection
}
