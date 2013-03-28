using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2._5D_FYP
{
    public class Player : Entity
    {
        public enum State
        {
            Attack,
            Defence,
            Safe,
        };
        public State playerState = State.Attack;

        private Vector3 acceleration;
        public Weapon weapon;

        public int capacity = 0;
        private float timeSinceLastHit = 0;

        public string weaponName = null;
        public string[] weaponArray = {"Single-Fire","Multi-Fire","Rocket Launcher","EMP-Pulse"};

        private KeyboardState keyState;

        private bool keyPressed = false;
        public bool hasHitSomething = false;
        private bool isThrusting = false;
        public bool Docking = false;

        public bool changeWeapon = false;

        List<Entity> stageList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();
        List<Entity> enemyBulletList = new List<Entity>();

        public long playerScore = 0;

        public Player(List<Entity> list)
        {
            _entityModel = "Models//SpaceShip3";
            _entityName = "Player";
            _type = this.GetType();

            _pos = new Vector3(200, _YAxis, 200);
            _look = Vector3.Forward;
            _look.Normalize();

            _right = new Vector3(1, 0, 0);
            _up = new Vector3(0, 1, 0);
            _globalUp = new Vector3(0, 1, 0);

            _maxSpeed = 500.0f; _maxForce = 100.0f; _scale = 5.0f; _mass = 1.0f; _rotationSpeed = 2.5f;
            _health = 100.0f; _shield = 100.0f;

            weapon = new Weapon();
            weaponName = weaponArray[_weaponIndex];
            
            _alive = true;
            
            _parentList = list;

            if (_alive)
                _parentList.Add(this);

            stageList = game.StageList;
            asteroidList = game.AsteroidList;
            metalList = game.MetalList;
            enemyBulletList = game.EnemyBulletList;
        } // End of Player()

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                if (Math.Sqrt(Math.Pow(_pos.X - 0, 2) + Math.Pow(_pos.Z - 0, 2)) > 4000)
                {
                    _pos = -_pos;
                }

                hasHitSomething = CheckCollision(this,asteroidList);
                if (hasHitSomething)
                    CollisionHandler(asteroidList);

                hasHitSomething = CheckCollision(this,metalList);
                if (hasHitSomething)
                    CollisionHandler(metalList);

                hasHitSomething = CheckCollision(this,stageList);
                if (hasHitSomething)
                    CollisionHandler(stageList);

                hasHitSomething = CheckCollision(this,enemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(enemyBulletList);

                weapon.Update(gameTime);

                //if(_fireWeapon)
                weapon.CheckWeaponFire(_weaponIndex, this);

                acceleration = _force / _mass;

                _velocity += acceleration * timeDelta;

                _pos += _velocity * timeDelta;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                if(!isThrusting)
                    _velocity *= 0.99f;

                keyState = Keyboard.GetState();

                if (keyState.IsKeyDown(Keys.Up))
                {
                    if (_velocity == Vector3.Zero)
                        Docking = false;
                    isThrusting = true;
                    addForce(_look * _maxForce);

                    if (_force.Length() > _maxForce)
                    {
                        _force.Normalize();
                        _force *= _maxForce;
                    }
                }
                else isThrusting = false;

                if (keyState.IsKeyDown(Keys.Space))
                    _fireWeapon = true;
                else _fireWeapon = false;

                if (keyState.IsKeyDown(Keys.Left))
                {
                    if (_velocity == Vector3.Zero)
                        Docking = false;
                    yaw(_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.Right))
                {
                    if(_velocity == Vector3.Zero)
                        Docking = false;
                    yaw(-_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.W))
                {
                    if (!keyPressed)
                    {
                        changeWeapon = true;
                        _weaponIndex = ++_weaponIndex % 4;
                        weaponName = weaponArray[_weaponIndex];
                        keyPressed = true;
                    }
                }
                else { changeWeapon = false; keyPressed = false;}

                if (keyState.IsKeyDown(Keys.D))
                {
                    if (!Docking)
                    {
                        Docking = true;
                    }
                }
                if (keyState.IsKeyDown(Keys.A))
                {
                    if (Docking)
                    {
                        Docking = false;
                    }
                }
                if (Docking)
                {
                    Vector3 aboveStation = new Vector3(0, _YAxis, 0);
                    _force = seek(aboveStation);
                    _look = Vector3.Normalize(_velocity);
                    if((Vector3.Zero - _pos).Length() < 200)
                        _force = arrive(aboveStation);
                }

                if (timeSinceLastHit >= 10.0f) _shield++;
                if (_shield <= 0) _shield = 0;
                if (_shield > 100) _shield = 100;

                if (_health == 0) _alive = false;
                if (_health <= 0) _health = 0;
                if (_health > 100) _health = 100;

                if (capacity <= 0) capacity = 0;
                if (capacity > 20) capacity = 20;

                if (!_alive)
                    _parentList.Remove(this);

                timeSinceLastHit += timeDelta;

            }
            else _parentList.Remove(this);
        } // End of Update(GameTime gameTime)

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                {
                    timeSinceLastHit = 0.0f;
                    _shield -= entity._damageOnCollision;
                    if (_shield <= 0)
                        this._health -= entity._damageOnCollision;

                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is ForceField)
                {
                    entity._entityCollisionFlag = false;
                    if (Docking)
                    {
                        playerState = State.Safe;
                    }
                    else playerState = State.Attack;
                }

                if (entity._entityCollisionFlag == true && entity is Metal)
                {
                    playerScore += 75;
                    capacity += 1;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is Bullet)
                {
                    timeSinceLastHit = 0.0f;
                    _shield -= entity._damageOnCollision;
                    if (_shield <= 0)
                        _health -= entity._damageOnCollision;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
            }
        } // End of CollisionHandler(List<Entity> children)

        void addForce(Vector3 force)
        {
            this._force += force;
        }

        Vector3 seek(Vector3 targetPos)
        {
            Vector3 desiredVelocity;

            desiredVelocity = targetPos - _pos;
            desiredVelocity.Normalize();
            desiredVelocity *= _maxSpeed;

            return (desiredVelocity - _velocity);
        }

        public Vector3 arrive(Vector3 targetPos)
        {
            Vector3 distanceToTarget = targetPos - _pos;

            float slowingDistance = 100.0f;
            float distance = distanceToTarget.Length();
            if (distance == 0.0f)
            {
                return Vector3.Zero;
            }
            const float DecelerationTweaker = 100.0f;
            float ramped = _maxSpeed * (distance / (slowingDistance * DecelerationTweaker));

            float clamped = Math.Min(ramped, _maxSpeed);
            Vector3 desiredVelocity = clamped * (distanceToTarget / distance);

            return desiredVelocity - _velocity;
        }

    } // End of Player Class
}
