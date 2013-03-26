﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace _2._5D_FYP
{
    public class Bullet : Entity
    {
        float travelTime = 0.0f;
        float maxTime = 1.0f;

        Weapon w = Game1.Instance().Player.weapon;

        #region Bullet Constructor
        // Parameter passed: The list that this bullet will be added to.
        public Bullet(List<Entity> list) 
        {
            _maxForce = 200.0f;

            _alive = true;

            _parentList = list;
                _parentList.Add(this);
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos,_look, _up);

                Vector3 acceleration = _force / _mass;
                
                _velocity += acceleration;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                _pos += _velocity * timeDelta;

                //walk(_maxSpeed * timeDelta);

                if (travelTime >= maxTime)
                {
                    _alive = false;
                }
                travelTime += timeDelta;

                if (!_alive)
                    _parentList.Remove(this);
            }
            else _parentList.Remove(this);
        }

        #region Method to check for Bullet Type
        // Parameter passed: The Entity in question's weaponIndex(ie. weaponType).
        public void getBulletType(Entity e, int entityWeaponIndex)
        {
            switch (entityWeaponIndex)
            {
                case 0:
                    _entityModel = "Models//ParticleSpheres//sphere";
                    _maxForce = 200.0f;
                    _maxSpeed = 400.0f; _scale = 1.0f; _mass = 1.0f;
                    maxTime = 1.0f;
                    _look = e._look;
                    _force = _look * _maxForce;
                    _damageOnCollision = 5 * e._attackStrength;
                    break;
                case 1:
                    _entityModel = "Models//ParticleSpheres//sphere";
                    _maxForce = 200.0f;
                    _maxSpeed = 1000.0f; _scale = 2.0f; _mass = 1.0f;
                    maxTime = 1.0f;
                    _look = e._look;
                    _force = _look * _maxForce;
                    _damageOnCollision = 2 * e._attackStrength;
                    break;
                case 2:
                    _entityModel = "Models//ParticleSpheres//Rocket";
                    _maxForce = 10.0f;
                    _maxSpeed = 500.0f; _scale = 1.0f; _mass = 1.0f;
                    maxTime = 10.0f;
                    _force = seek(Game1.Instance().Player._pos);
                    _damageOnCollision = 20 * e._attackStrength;
                    break;
                case 3: 
                    break;
            }
        }
        #endregion

        Vector3 seek(Vector3 targetPos)
        {
            Vector3 desiredVelocity;

            desiredVelocity = targetPos - _pos;
            desiredVelocity.Normalize();
            desiredVelocity *= _maxSpeed;

            return (desiredVelocity - _velocity);
        }
    }
}