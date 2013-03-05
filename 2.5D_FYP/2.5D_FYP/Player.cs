﻿using System;
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

        public int capacity = 0;
        public int weaponIndex = 0;

        public string weaponName = null;
        public string[] weaponArray = {"Single-Fire","Multi-Fire","Rocket Launcher","EMP-Pulse"};

        public KeyboardState keyState;

        public bool keyPressed = false;
        bool hasHitSomething;

        List<Entity> stageList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();
        List<Entity> enemyBulletList = new List<Entity>();

        public Player(List<Entity> list)
        {
            _entityModel = "Models//Elite Models//cobramk3";
            _entityName = "Player";
            _type = this.GetType();

            _pos = new Vector3(50, _YAxis, 50);
            _look = new Vector3(0, 0, -1);

            _right = new Vector3(1, 0, 0);
            _up = new Vector3(0, 1, 0);
            _globalUp = new Vector3(0, 1, 0);

            _maxSpeed = 500.0f; _maxForce = 150.0f; _scale = 0.5f; _mass = 10.0f; _rotationSpeed = 5.0f;
            _health = 100.0f; _shield = 100.0f;

            weaponName = weaponArray[weaponIndex];
            weapon = new Weapon();

            hasHitSomething = false;
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

                _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(asteroidList);
                if (hasHitSomething)
                    CollisionHandler(asteroidList);

                hasHitSomething = CheckCollision(metalList);
                if (hasHitSomething)
                    CollisionHandler(metalList);

                hasHitSomething = CheckCollision(stageList);
                if (hasHitSomething)
                    CollisionHandler(stageList);

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

                if (_shield <= 0)
                    _shield = 0;
                if (_shield > 100)
                    _shield = 100;

                if (_health == 0) 
                    _alive = false;
                if (_health <= 0)
                    _health = 0;
                if (_health > 100)
                    _health = 100;

                if (capacity <= 0)
                    capacity = 0;
                if (capacity > 15)
                    capacity = 15;

                if (!_alive)
                    parentList.Remove(this);
            }
            else parentList.Remove(this);
        } // End of Update(GameTime gameTime)

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                {
                    _shield -= 5 * entity._scale;
                    if (_shield <= 0)
                        _health -= 5 * entity._scale;

                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is Station)
                {
                    _health++;
                    entity._entityCollisionFlag = false;
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
                    _health -= 5;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
            }
        } // End of CollisionHandler(List<Entity> children)

        void addForce(Vector3 force)
        {
            this._force += force;
        }
    } // End of Player Class
}
