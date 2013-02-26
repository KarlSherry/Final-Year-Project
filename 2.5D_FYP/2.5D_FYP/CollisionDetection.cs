using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class CollisionDetection
    {
        Player player;
        Asteroid floatingObject;
        public CollisionDetection()
        {
        }

        public void CheckPlayerAsteroidCollision()
        {
            player = Game1.Instance().Player;
            floatingObject = Game1.Instance().asteroids;

            for (int i = 0; i < player._model.Meshes.Count; i++)
            {
                BoundingSphere playerSphere = player._model.Meshes[i].BoundingSphere;
                playerSphere.Center = player._pos;

                playerSphere.Radius = 5;

                ////////////// This is the part that's causing me error and has me doubting my floatingobjects class
                for (int j = 0; j < floatingObject._model.Meshes.Count; j++)
                {
                    BoundingSphere objSphere = floatingObject._model.Meshes[i].BoundingSphere;
                    objSphere.Center = floatingObject._pos;

                    objSphere.Radius = 5;

                    //Checks to see if the player sphere intersects with the station sphere
                    if (playerSphere.Intersects(objSphere))
                        if (player._health > 0)
                            player._health -= 50;
                }
            }
        }

    }
}
