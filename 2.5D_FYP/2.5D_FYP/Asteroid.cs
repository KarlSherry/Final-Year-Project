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
        int asteroidCount = 25;
        bool hasHitSomething;

        List<Entity> children = new List<Entity>();

        public int AsteroidCount
        {
            get { return asteroidCount; }
            set { asteroidCount = value; }
        }

        public Asteroid()
        {
            _entityModel = "Models//Asteroid";
            _entityName = "Asteroid";
            _type = this.GetType();

            _pos = new Vector3(Entity.randomGenerator.Next(-900, 900), 50, Entity.randomGenerator.Next(-900, 900));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _maxSpeed = randomGenerator.Next(10, 25);

            _alive = true;
            hasHitSomething = false;

            children = game.Children;
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(_pos);

                hasHitSomething = CheckCollision(children);
                if (hasHitSomething)
                    CollisionHandler(children);

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;

                if (_alive == false)
                    game.Children.Remove(this);
            }
        }

        public override void CollisionHandler(List<Entity> children)
        {
            foreach (Entity entity in children)
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
