using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Asteroid : Entity
    {
        float angle = 0.0f;
        bool hasHitSomething;

        List<Entity> childrenList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();

        public Asteroid(List<Entity> list)
        {
            _entityModel = "Models//Asteroid";
            _entityName = "Asteroid";
            _type = this.GetType();

            _pos = new Vector3(Entity.randomGenerator.Next(-900, 900), _YAxis, Entity.randomGenerator.Next(-900, 900));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _maxSpeed = randomGenerator.Next(10, 25); _scale = randomGenerator.Next(1, 4);

            hasHitSomething = false;
            _alive = true;

            parentList = list;
            if (_alive)
                list.Add(this);

            childrenList = game.StageList;
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void Update( GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                hasHitSomething = CheckCollision(childrenList);
                if (hasHitSomething)
                    CollisionHandler(childrenList);

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;

                if (!_alive)
                    parentList.Remove(this);
            }
        }

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Player)
                {
                    _alive = false;
                    entity._entityCollisionFlag = false;
                }
                else if (entity._entityCollisionFlag == true && entity is Asteroid)
                {
                    entity._look = -entity._look;
                    entity._entityCollisionFlag = false;
                }
            }            
        }
    }
}
