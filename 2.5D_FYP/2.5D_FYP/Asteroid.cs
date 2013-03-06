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
        Vector3 acceleration;

        float angle = 0.0f;

        bool hasHitSomething;

        List<Entity> stageList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();
        List<Entity> playerBulletList = new List<Entity>();

        public Asteroid(List<Entity> list)
        {
            _entityModel = "Models//Asteroid";
            _entityName = "Asteroid";
            _type = this.GetType();

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _maxSpeed = randomGenerator.Next(10, 25); _maxForce = 5.0f;  _scale = randomGenerator.Next(1, 4); _mass = _scale;
            _damageOnCollision = 5 * _scale;

            hasHitSomething = false;
            _alive = true;

            parentList = list;
            if (_alive)
                list.Add(this);

            stageList = game.StageList;
            asteroidList = game.AsteroidList;
            metalList = game.MetalList;
            playerBulletList = game.PlayerBulletList;
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

                hasHitSomething = CheckCollision(stageList);
                if (hasHitSomething)
                    CollisionHandler(stageList);

                hasHitSomething = CheckCollision(asteroidList);
                if(hasHitSomething)
                    CollisionHandler(asteroidList);

                hasHitSomething = CheckCollision(metalList);
                if (hasHitSomething)
                    CollisionHandler(metalList);

                hasHitSomething = CheckCollision(playerBulletList);
                if(hasHitSomething)
                    CollisionHandler(playerBulletList);
                
                /*_velocity += _look * _maxSpeed * timeDelta;

                _pos += _velocity * timeDelta;

                _force += _look * _maxForce;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }*/

                _pos += _look * timeDelta * _maxSpeed;

                angle += timeDelta;

                if (!_alive)
                    parentList.Remove(this);
            }
            else parentList.Remove(this);
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
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                {
                    _look = -_look;
                    //entity._look.Normalize();
                    /*//entity._look = _look + entity._look;
                    //entity._look.Normalize();

                    entity._force = entity._look * entity._maxForce;
                    if (entity._force.Length() > _maxForce)
                    {
                        entity._force.Normalize();
                        entity._force *= _maxForce;
                    }*/

                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is Metal)
                {
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
                if(entity._entityCollisionFlag == true && entity is Bullet)
                {
                    _alive = false;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
            }            
        }
    }
}
