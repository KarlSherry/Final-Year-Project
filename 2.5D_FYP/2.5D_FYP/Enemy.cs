using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class Enemy : Entity
    {
        private Vector3 acceleration;
        private Weapon weapon = new Weapon();

        private int weaponIndex = 0;

        public int enemyTypeIndex = 0;
        string[] enemyType = { "gecko", "ferdelance", "viper", "asp" };

        public bool fireWeapon = false;
        private bool hasHitSomething = false;

        List<Entity> playerBulletList = new List<Entity>();

        public Enemy(List<Entity> list)
        {
            enemyTypeIndex = Entity.randomGenerator.Next(0, 4);

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
            _look = new Vector3(randomClamped(), 0, randomClamped());

            _alive = true;

            parentList = list;
            if (_alive)
                parentList.Add(this);

            weaponIndex = Entity.randomGenerator.Next(0, 3);

            playerBulletList = game.PlayerBulletList;
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

                _worldTransform = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                getEnemyType(enemyTypeIndex);

                hasHitSomething = CheckCollision(playerBulletList);
                if (hasHitSomething)
                    CollisionHandler(playerBulletList);

                weapon.Update(gameTime);

                _look = Vector3.Normalize(_velocity);

                acceleration = _force / _mass;

                _velocity += acceleration * timeDelta;
                
                _velocity *= 0.99f;

                _pos += _velocity * timeDelta;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                if ((game.Player._pos - _pos).Length() < 100)
                    weapon.CheckWeaponFire(weaponIndex, this);
                    

                if (!_alive)
                    parentList.Remove(this);
            }
            else parentList.Remove(this);           
        }

        Vector3 seek(Vector3 targetPos)
        {
            Vector3 desiredVelocity;

            desiredVelocity = targetPos - _pos;
            desiredVelocity.Normalize();
            desiredVelocity *= _maxSpeed;

            return (desiredVelocity - _velocity);
        }

        Vector3 pursue(Entity entity)
        {
            float dist = (entity._pos - _pos).Length();

            float lookAhead = (dist / _maxSpeed);

            Vector3 target = entity._pos + (lookAhead * entity._velocity);

            if (entity is Station)
                target.Y = entity._YAxis;

            return seek(target);
        }

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Bullet)
                {
                    _alive = false;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
            }
        }

        public void getEnemyType(int index)
        {
            switch (index)
            {
                case 0:
                    _entityModel = "Models//Elite Models//" + enemyType[enemyTypeIndex];
                    weaponIndex = 0;
                    _maxSpeed = 1000.0f; _maxForce = 150.0f; _scale = 0.1f; _mass = 10.0f; _rotationSpeed = 5.0f;
                    _force = pursue(game.Player);
                    break;
                case 1:
                    _entityModel = "Models//Elite Models//" + enemyType[enemyTypeIndex];
                    weaponIndex = 1;
                    _maxSpeed = 500.0f; _maxForce = 150.0f; _scale = 1.0f; _mass = 15.0f; _rotationSpeed = 5.0f;
                    _force = pursue(game.Player);
                    break;
                case 2:
                    _entityModel = "Models//Elite Models//" + enemyType[enemyTypeIndex];
                    weaponIndex = 2;
                    _maxSpeed = 300.0f; _maxForce = 150.0f; _scale = 0.25f; _mass = 25.0f; _rotationSpeed = 5.0f;
                    _force = pursue(game.Player);
                    break;
                case 3:
                    _entityModel = "Models//Elite Models//" + enemyType[enemyTypeIndex];
                    weaponIndex = 2;
                    _maxSpeed = 250.0f; _maxForce = 150.0f; _scale = 0.5f; _mass = 50.0f; _rotationSpeed = 5.0f;
                    _force = pursue(game.Station);
                    break;
            }
        }
    }
}
