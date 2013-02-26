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

            _maxSpeed = 50.0f;
            _maxForce = 15.0f;
            _mass = 1.0f;
            _health = 100.0f;
            _shield = 100.0f;

            weaponName = weaponArray[weaponIndex];

            weapon = new Weapon();
            rotationSpeed = 5.0f;
        } // End of Player()

        public override void Initialize()
        {
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

            base.Update(gameTime);
        } // End of Update(GameTime gameTime)

        /*public static void CheckForCollisions(ref Player player, ref Station Entity)
        {
            //Creates 2 bounding spheres, 1 for the player and one for station
            for (int i = 0; i < player._model.Meshes.Count; i++)
            {
                BoundingSphere playerSphere = player._model.Meshes[i].BoundingSphere;
                playerSphere.Center = player._pos;

                playerSphere.Radius = 5;

                for (int j = 0; j < Entity._model.Meshes.Count; j++) 
                {
                    BoundingSphere objSphere = Entity._model.Meshes[i].BoundingSphere;
                    objSphere.Center = Entity._pos;

                    objSphere.Radius = 5;

                    //Checks to see if the player sphere intersects with the station sphere
                    if (playerSphere.Intersects(objSphere))
                        if(player._health > 0)
                            player._health -= 50;
                }
            }
        }*/

        public override void Draw(GameTime gameTime)
        {
            _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateScale(0.5f) * Matrix.CreateWorld(_pos, _look, _up);

            if (_model != null)
            {
                foreach (ModelMesh mesh in _model.Meshes)
                {
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
