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
        Weapon weapon;
        Metal metal;
        Asteroid asteroid;
        BoundingSphere playerSphere;
        public BoundingSphere PlayerSphere
        {
            get { return playerSphere; }
            set { playerSphere = value; }
        }

        float rotationSpeed = 5.0f;

        public float capacity = 0.0f;
        public int weaponIndex = 0;
        public string weaponName = null;
        public string[] weaponArray = {"Single-Fire","Multi-Fire","Rocket Launcher","EMP-Pulse"};

        KeyboardState keyState;

        bool keyPressed = false;

        public Player()
        {
            _worldTransform = Matrix.Identity;
            _pos = new Vector3(50, 50, 50);
            _look = new Vector3(0, 0, -1);
            _right = new Vector3(1, 0, 0);
            _up = new Vector3(0, 1, 0);
            _globalUp = new Vector3(0, 1, 0);

            _maxSpeed = 500.0f;
            _maxForce = 150.0f;
            _mass = 10.0f;
            _health = 100.0f;
            _shield = 100.0f;

            weaponName = weaponArray[weaponIndex];
            asteroid = new Asteroid();

            weapon = new Weapon();
            metal = new Metal();
            rotationSpeed = 5.0f;
        } // End of Player()

        public override void Initialize()
        {
            asteroid = Game1.Instance().asteroid;
            base.Initialize();
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>("Models//Elite Models//cobramk3");
        } // End of LoadContent()

        void addForce(Vector3 force)
        {
            this._force += force;
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            weapon.Update(weaponIndex, gameTime);
            weapon.CheckWeaponFire();

            //CollisionDetection.CheckPlayerAsteroidCollision(this, asteroid);

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

            if(keyState.IsKeyDown(Keys.Up))
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
                yaw(rotationSpeed * timeDelta);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                yaw(-rotationSpeed * timeDelta);
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

            if (IsHIt == true)
            {
                _health++;
                IsHIt = false;
            }

            base.Update(gameTime);
        } // End of Update(GameTime gameTime)

        public override void Draw(GameTime gameTime)
        {
            _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateScale(0.5f) * Matrix.CreateWorld(_pos, _look, _up);

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
        } // End of Draw(GameTime gameTime)
    } // End of Player Class
}
