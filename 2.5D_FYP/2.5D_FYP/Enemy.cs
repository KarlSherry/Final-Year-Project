using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Enemy : Entity
    {
        Vector3 acceleration;
        Weapon weapon;
        int weaponIndex = 0;
        float scale = 0.2f;
        public int enemyTypeIndex;
        public string[] enemyTypes = {"python","anaconda","viper","gecko"};

        public Enemy()
        {
            _pos = new Vector3(100, 50, 100);
            enemyTypeIndex = randomGenerator.Next(2,4);
            _maxForce = 5;
            _maxSpeed = 25;
            _alive = true;

            weapon = new Weapon();
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>("Models//Elite Models//" + enemyTypes[enemyTypeIndex]);
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _look = Vector3.Normalize(_velocity);

                acceleration = _force / _mass;

                _velocity += acceleration * timeDelta;

                _pos += _velocity * timeDelta;

                _force = seek(Game1.Instance().Player._pos);

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                if (_velocity.Length() > 0.0001f)
                {
                    _right = Vector3.Cross(_look, _up);
                }

                if ((_pos - Game1.Instance().Player._pos).Length() < 50)
                {
                    weapon.Update(gameTime);
                    weapon.CheckWeaponFire(weaponIndex, this);
                }
            }
        }

        Vector3 seek(Vector3 targetPos)
        {
            Vector3 desiredVelocity = new Vector3();

            desiredVelocity = targetPos - _pos;
            desiredVelocity.Normalize();
            desiredVelocity *= _maxSpeed;

            return (desiredVelocity - _velocity);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Alive)
            {
                _worldTransform =  Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(scale) * Matrix.CreateWorld(_pos, _look, _up);

                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        _entitySphere = mesh.BoundingSphere;
                        _entitySphere.Center = _pos;
                        _entitySphere.Radius = 10;
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.World = _worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                } // End of if(_model != null)
            }
        }

        public override void CollisionHandler(List<Entity> list)
        {
        }
    }
}
