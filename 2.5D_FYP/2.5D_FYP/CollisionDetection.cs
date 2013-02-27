using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class CollisionDetection
    {
        public static Boolean CheckPlayerAsteroidCollision(Player entity1, Asteroid entity2)
        {
            List<Entity> children = Game1.Instance().Children;
            foreach (Entity entity in children)
            {
                if (entity is Asteroid)
                {
                    //Asteroid ast = (Asteroid)entity;
                    entity2 = (Asteroid)entity;
                    if (entity2._entitySphere.Intersects(entity1._entitySphere))
                    {
                        entity1.IsHIt = true;
                        Console.WriteLine("true");
                    }
                }
            }
            return entity1.IsHIt;
        }

        public static void CheckPlayerCollision(Entity entity1)
        {
            List<Entity> children = Game1.Instance().Children;
            foreach (Entity entity in children)
            {
                entity.GetType();

                if (entity is Station)
                {
                    if (entity1._entitySphere.Intersects(entity._entitySphere))
                    {
                        entity1.IsHIt = true;
                        Console.WriteLine("true");
                    }
                }
                if (entity is Asteroid)
                {
                    if (entity1._entitySphere.Intersects(entity._entitySphere))
                        Console.WriteLine("Asteroid hit");
                }
            }
            //return entity1.IsHIt;
        }

    } // End of class CollisionDetection
}


/*for (int i = 0; i < entity1._model.Meshes.Count; i++)
{
    ////////////// Haviong issues with the spheres, as in I dont no how large to set them because i can't see them....
    for (int j = 0; j < entity2._model.Meshes.Count; j++)
    {
        //Checks to see if the player sphere intersects with the station sphere
        if (entity1._entitySphere.Intersects(entity2._entitySphere))
        {
            entity1.IsHIt = true;
            Console.WriteLine("true");
        }
        else
        {
            entity1.IsHIt = false;
            Console.WriteLine("false:");
        }
    }
}
return entity1.IsHIt;*/