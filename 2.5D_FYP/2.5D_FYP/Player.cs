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
        private int weaponIndex = 0;
        private float timeSinceLastHit = 0;

        public string weaponName = null;
        public string[] weaponArray = {"Single-Fire","Multi-Fire","Rocket Launcher","EMP-Pulse"};

        private KeyboardState keyState;

        public bool fireWeapon = false;
        private bool keyPressed = false;
        public bool hasHitSomething = false;
        private bool isThrusting = false;
        public bool Docked = false;

        public bool changeWeapon = false;

        List<Entity> stageList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();
        List<Entity> enemyBulletList = new List<Entity>();

        public Player(List<Entity> list)
        {
            _entityModel = "Models//SpaceShip3";
            _entityName = "Player";
            _type = this.GetType();

            _pos = new Vector3(100, _YAxis, 100);
            _look = Vector3.Forward;

            _right = new Vector3(1, 0, 0);
            _up = new Vector3(0, 1, 0);
            _globalUp = new Vector3(0, 1, 0);

            _maxSpeed = 100.0f; _maxForce = 500.0f; _scale = 5.0f; _mass = 10.0f; _rotationSpeed = 2.5f;
            _health = 100.0f; _shield = 100.0f;

            weapon = new Weapon();
            weaponName = weaponArray[weaponIndex];
            
            _alive = true;
            
            parentList = list;

            if (_alive)
                parentList.Add(this);

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

                _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(asteroidList);
                if (hasHitSomething)
                    CollisionHandler(asteroidList);

                hasHitSomething = CheckCollision(metalList);
                if (hasHitSomething)
                    CollisionHandler(metalList);

                hasHitSomething = CheckCollision(stageList);
                if (hasHitSomething)
                    CollisionHandler(stageList);

                hasHitSomething = CheckCollision(enemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(enemyBulletList);

                weapon.Update(gameTime);
                weapon.CheckWeaponFire(weaponIndex, this);

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
                    Docked = false;
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
                    fireWeapon = true;
                else fireWeapon = false;

                if (keyState.IsKeyDown(Keys.Left))
                {
                    Docked = false;
                    yaw(_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.Right))
                {
                    Docked = false;
                    yaw(-_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.W))
                {
                    if (!keyPressed)
                    {
                        changeWeapon = true;
                        weaponIndex = ++weaponIndex % 4;
                        weaponName = weaponArray[weaponIndex];
                        keyPressed = true;
                    }
                }
                else { changeWeapon = false; keyPressed = false;}

                if (keyState.IsKeyDown(Keys.D))
                {
                    if (!Docked)
                    {
                        Docked = true;
                    }
                }

                if (timeSinceLastHit >= 10.0f) _shield++;
                if (_shield <= 0) _shield = 0;
                if (_shield > 100) _shield = 100;

                if (_health == 0) _alive = false;
                if (_health <= 0) _health = 0;
                if (_health > 100) _health = 100;

                if (capacity <= 0) capacity = 0;
                if (capacity > 15) capacity = 15;

                if (!_alive)
                    parentList.Remove(this);

                timeSinceLastHit += timeDelta;

            }
            else parentList.Remove(this);
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
                        _health -= entity._damageOnCollision;

                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is ForceField && Docked)
                {
                    Vector3 aboveStation = new Vector3(entity._pos.X, entity._YAxis, entity._pos.Z);
                    playerState = State.Safe;
                    _health++;
                    _force = arrive(aboveStation);
                }

                if (entity._entityCollisionFlag == true && entity is Metal)
                {
                    capacity += 1;
                    entity._alive = false;
                    Console.WriteLine("metal");
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

        public Vector3 arrive(Vector3 targetPos)
        {
            Vector3 distanceToTarget = targetPos - _pos;

            float slowingDistance = 20.0f;
            float distance = distanceToTarget.Length();
            if (distance == 0.0f)
            {
                return Vector3.Zero;
            }
            const float DecelerationTweaker = 20.0f;
            float ramped = _maxSpeed * (distance / (slowingDistance * DecelerationTweaker));

            float clamped = Math.Min(ramped, _maxSpeed);
            Vector3 desiredVelocity = clamped * (distanceToTarget / distance);

            return desiredVelocity - _velocity;
        }
    } // End of Player Class
}
