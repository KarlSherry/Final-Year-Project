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
        string[] enemyType = { "Alien7Green", "Alien7Blue", "Alien7Red", "AlienMama1" };

        public bool fireWeapon = false;
        private bool hasHitSomething = false;

        List<Entity> playerBulletList = new List<Entity>();
        List<Entity> enemyList = new List<Entity>();
        
        private int currentRound;

        public Enemy(List<Entity> list)
        {
            currentRound = Game1.getCurrentRound();

            if (currentRound % 1 == 0)
            {
                enemyTypeIndex = Entity.randomGenerator.Next(0, 1);
            }
            if (currentRound % 2 == 0)
            {
                enemyTypeIndex = Entity.randomGenerator.Next(0, 2);
            }
            if (currentRound % 3 == 0)
            {
                enemyTypeIndex = Entity.randomGenerator.Next(0, 3);
            }
            if (currentRound % 4 == 0)
            {
                enemyTypeIndex = Entity.randomGenerator.Next(0, 4);
            }

            getEnemyType(enemyTypeIndex);

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
            _look = new Vector3(randomClamped(), 0, randomClamped());

            _alive = true;

            parentList = list;
            if (_alive)
                parentList.Add(this);

            weaponIndex = Entity.randomGenerator.Next(0, 3);

            playerBulletList = game.PlayerBulletList;
            enemyList = game.EnemyList;
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

                _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                getEnemyType(enemyTypeIndex);

                hasHitSomething = CheckCollision(playerBulletList);
                if (hasHitSomething)
                    CollisionHandler(playerBulletList);
                hasHitSomething = CheckCollision(enemyList);
                if (hasHitSomething)
                    CollisionHandler(enemyList);
                hasHitSomething = CheckCollision(Game1.Instance().StageList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().StageList);

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

                if (!_alive)
                {
                    Game1.Instance().Player.playerScore += (100 * (long)_scale);
                    parentList.Remove(this);
                }
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
                /*if (entity._entityCollisionFlag == true && entity is ForceField)
                {
                    if (enemyTypeIndex != 3)
                    {
                        _force = _pos - entity._pos;
                    }
                }
                if (entity._entityCollisionFlag == true && entity is Enemy)
                {
                    if (enemyTypeIndex != 3)
                    {
                        _force = entity._pos - _pos;
                    }
                }*/
            }
        }

        public void getEnemyType(int index)
        {
            switch (index)
            {
                case 0:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    weaponIndex = 0;
                    _maxSpeed = 500.0f; _maxForce = 10.0f; _scale = 3.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    if(game.Player.playerState != Player.State.Safe)
                        _force = pursue(game.Player);
                    if ((game.Player._pos - _pos).Length() < 100)
                        weapon.CheckWeaponFire(weaponIndex, this);
                    break;
                case 1:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    weaponIndex = 1;
                    _maxSpeed = 500.0f; _maxForce = 10.0f; _scale = 4.5f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;

                    if (game.Player.playerState != Player.State.Safe)
                        _force = pursue(game.Player);
                    if ((game.Player._pos - _pos).Length() < 100)
                        weapon.CheckWeaponFire(weaponIndex, this);
                    break;
                case 2:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    weaponIndex = 2;
                    _maxSpeed = 500.0f; _maxForce = 10.0f; _scale = 6.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    
                    if (game.Player.playerState != Player.State.Safe)
                        _force = pursue(game.Player);
                    if ((game.Player._pos - _pos).Length() < 200)
                        weapon.CheckWeaponFire(weaponIndex, this);
                    break;
                case 3:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    weaponIndex = 2;
                    _maxSpeed = 500.0f; _maxForce = 10.0f; _scale = 5.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    _force = pursue(game.Station);
                    if ((game.Station._pos - _pos).Length() < 400)
                    {
                        weapon.CheckWeaponFire(weaponIndex, this);
                        _maxSpeed = 1; _maxForce = 1;
                        _force = _look * _maxForce;
                    }

                    break;
            }
        }
    }
}
