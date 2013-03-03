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
        public Vector3 acceleration;
        public Weapon weapon;
        BoundingSphere playerSphere;
        public BoundingSphere PlayerSphere
        {
            get { return playerSphere; }
            set { playerSphere = value; }
        }

        public float capacity = 0.0f;
        public int weaponIndex = 0;
        public string weaponName = null;
        public string[] weaponArray = {"Single-Fire","Multi-Fire","Rocket Launcher","EMP-Pulse"};

        public KeyboardState keyState;

        public bool keyPressed = false;
        bool hasHitSomething;
        
        List<Entity> childrenList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();

        public Player()
        {
            _entityModel = "Models//Elite Models//cobramk3";
            _entityName = "Player";
            _type = this.GetType();

            _pos = new Vector3(50, _YAxis, 50);
            _look = new Vector3(0, 0, -1);

            _right = new Vector3(1, 0, 0);
            _up = new Vector3(0, 1, 0);
            _globalUp = new Vector3(0, 1, 0);

            _maxSpeed = 500.0f; _maxForce = 150.0f; _mass = 10.0f; _rotationSpeed = 5.0f; _scale = 0.5f;
            _health = 100.0f; _shield = 100.0f;

            weaponName = weaponArray[weaponIndex];
            weapon = new Weapon();

            hasHitSomething = false;
            _alive = true;

            childrenList = game.Children;
            asteroidList = game.AsteroidList;
            metalList = game.MetalList;
        } // End of Player()

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(asteroidList);
                if (hasHitSomething)
                {
                    //CollisionHandler(asteroidList);
                    CheckPlayerAsteroidCollision(asteroidList);
                }

                hasHitSomething = CheckCollision(metalList);
                if (hasHitSomething)
                {
                    //CollisionHandler(childrenList);
                    CheckPlayerMetalCollision(metalList);
                }

                hasHitSomething = CheckCollision(childrenList);
                if (hasHitSomething)
                {
                    //CollisionHandler(childrenList);
                    CheckPlayerMetalCollision(childrenList);
                }

                weapon.Update(gameTime);
                weapon.CheckWeaponFire(weaponIndex, this);

                acceleration = _force / _mass;

                _velocity += acceleration * timeDelta;

                _pos += _velocity * timeDelta;

                _force = Vector3.Zero;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                if (_velocity.Length() > 0.0001f)
                {
                    _right = Vector3.Cross(_look, _up);
                }

                _velocity *= 0.999f;

                keyState = Keyboard.GetState();

                if (keyState.IsKeyDown(Keys.Up))
                {
                    addForce(_look * _maxForce);

                    if (_force.Length() > _maxForce)
                    {
                        _force.Normalize();
                        _force *= _maxForce;
                    }
                }
                if (keyState.IsKeyDown(Keys.Left))
                {
                    yaw(_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.Right))
                {
                    yaw(-_rotationSpeed * timeDelta);
                }
                if (keyState.IsKeyDown(Keys.W))
                {
                    if (!keyPressed)
                    {
                        weaponIndex = ++weaponIndex % 4;
                        weaponName = weaponArray[weaponIndex];
                        keyPressed = true;
                    }
                }
                else keyPressed = false;

                if (_health == 0) 
                {
                    _alive = false;
                    game.Children.Remove(this);
                }
                if (_health <= 0)
                {
                    _health = 0;
                }
                if (_health > 100)
                {
                    _health = 100;
                }
            }

            hasHitSomething = false;
            base.Update(gameTime);
        } // End of Update(GameTime gameTime)

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                //if (entity is Asteroid)
                {
                    _health -= 5;
                    entity._entityCollisionFlag = false;
                    Console.WriteLine("hit ast");
                }
                else if (entity._entityCollisionFlag == true && entity is Station)
                {
                    _health++;
                    entity._entityCollisionFlag = false;
                }
            }
        } // End of CollisionHandler(List<Entity> children)

        public void CheckPlayerAsteroidCollision(List<Entity> list) 
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                //if (entity is Asteroid)
                {
                    _health -= 5;
                    entity._entityCollisionFlag = false;
                    Console.WriteLine("hit ast");
                }
            }
        }

        public void CheckPlayerMetalCollision(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Metal)
                //if (entity is Asteroid)
                {
                    entity._entityCollisionFlag = false;
                    Console.WriteLine("hit metal");
                }
            }
        }

        public void CheckPlayerStationCollision(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Station)
                //if (entity is Asteroid)
                {
                    _health++;
                    entity._entityCollisionFlag = false;
                    Console.WriteLine("hit station");
                }
            }
        }

        void addForce(Vector3 force)
        {
            this._force += force;
        }
    } // End of Player Class
}
