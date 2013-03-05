using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace _2._5D_FYP
{
    public class Bullet : Entity
    {
        float travelTime = 0.0f;
        float maxTime = 1.0f;

        List<Entity> asteroidList = new List<Entity>();

        public Bullet(List<Entity> list) 
        {
            _entityModel = "Models//sphere";
            _entityName = "Bullet";

            _maxSpeed = 100.0f; _scale = 1.0f;

            _alive = true;

            parentList = list;
                parentList.Add(this);

            asteroidList = game.AsteroidList;
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                walk(_maxSpeed * timeDelta);

                if (travelTime >= maxTime)
                {
                    _alive = false;
                }

                travelTime += timeDelta;

                if (!_alive)
                    parentList.Remove(this);
            }
            else parentList.Remove(this);
        }


    }
}
