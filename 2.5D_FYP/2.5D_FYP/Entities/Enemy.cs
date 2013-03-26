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
        
        public int enemyTypeIndex = 0;
        string[] enemyType = { "Alien7Green", "Alien7Blue", "Alien7Red", "AlienMama1" };

        private bool hasHitSomething = false;
        
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

            _pos = _randomPosition;

            _look = new Vector3(randomClamped(), 0, randomClamped());

            _alive = true;

            _parentList = list;
            _parentList.Add(this);
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

                getEnemyBehaviour(enemyTypeIndex);

                if (Math.Sqrt(Math.Pow(_pos.X - 0, 2) + Math.Pow(_pos.Z - 0, 2)) > 4000)
                {
                    _pos = -_pos;
                }

                hasHitSomething = CheckCollision(this, Game1.Instance().PlayerBulletList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().PlayerBulletList);
                hasHitSomething = CheckCollision(this, Game1.Instance().EnemyList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().EnemyList);
                hasHitSomething = CheckCollision(this,Game1.Instance().StageList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().StageList);

                weapon.Update(gameTime);
                if(_fireWeapon)
                    weapon.CheckWeaponFire(_weaponIndex, this);

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
                    _parentList.Remove(this);
                }
            }
            else _parentList.Remove(this);           
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
                if (entity._entityCollisionFlag == true && entity is ForceField)
                {
                    if (enemyTypeIndex != 3)
                    {
                        _force = _pos - entity._pos * 20;
                    }
                }
                if (entity._entityCollisionFlag == true && entity is Enemy)
                {
                    if (enemyTypeIndex != 3)
                    {
                        _force = (_pos - entity._pos) * 20;
                    }
                }
            }
        }

        public void getEnemyType(int index)
        {
            switch (index)
            {
                case 0:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    _weaponIndex = 0;
                    _maxSpeed = 250.0f; _maxForce = 10.0f; _scale = 3.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    break;
                case 1:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    _weaponIndex = 1;
                    _maxSpeed = 250.0f; _maxForce = 10.0f; _scale = 4.5f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    break;
                case 2:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    _weaponIndex = 2;
                    _maxSpeed = 250.0f; _maxForce = 10.0f; _scale = 6.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    break;
                case 3:
                    _entityModel = "Models//Enemies//" + enemyType[enemyTypeIndex];
                    _weaponIndex = 1;
                    _maxSpeed = 500.0f; _maxForce = 10.0f; _scale = 5.0f; _mass = 1.0f; _rotationSpeed = 5.0f;
                    _attackStrength = 1;
                    break;
            }
        }

        public void getEnemyBehaviour(int index)
        {
            switch (index)
            {
                case 0:
                        if (game.Player.playerState != Player.State.Safe)
                            _force = seek(Game1.Instance().Player._pos);
                        if ((Game1.Instance().Player._pos - _pos).Length() < 300)
                            _fireWeapon = true;
                        else _fireWeapon = false;
                    break;
                case 1:                    
                        if (game.Player.playerState != Player.State.Safe)
                            _force = seek(Game1.Instance().Player._pos);
                        if ((Game1.Instance().Player._pos - _pos).Length() < 300)
                            _fireWeapon = true;
                        else _fireWeapon = false;
                    break;
                case 2: 
                        if (game.Player.playerState != Player.State.Safe)
                            _force = seek(Game1.Instance().Player._pos);
                        if ((Game1.Instance().Player._pos - _pos).Length() < 300)
                            _fireWeapon = true;
                        else _fireWeapon = false;
                    break;
                case 3:
                    _force = pursue(game.Station);
                    if ((Game1.Instance().Station._pos - _pos).Length() < 400)
                    {
                        weapon.CheckWeaponFire(_weaponIndex, this);
                        _maxSpeed = 1; _maxForce = 1;
                        _force = _look * _maxForce;
                    }
                    break;
            }
        }
    }
}
