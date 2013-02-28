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
        CollisionDetection collision = new CollisionDetection();
        Type collidingEntity = null;
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
            _type = this.GetType();

            weaponName = weaponArray[weaponIndex];
            weapon = new Weapon();
            rotationSpeed = 5.0f;

            _alive = true;
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
            if (Alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                CollisionHandler(game.Children);

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
                if (_health == 0) 
                {
                    _alive = false;
                    game.Children.Remove(this);
                }
                if (_health <= 0)
                {
                    _health = 0;
                }
                if (_health >= 100)
                {
                    _health = 100;
                }
            }

            base.Update(gameTime);
        } // End of Update(GameTime gameTime)

        public override void CollisionHandler(List<Entity> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                if (_entitySphere.Intersects(children.ElementAt(i)._entitySphere) && children.ElementAt(i).GetType() == game.Asteroid[24]._type)
                {
                    if (_health >= 5)
                    {
                        _health -= 5;
                    }
                    else
                    {
                        _health = 0;
                    }
                }
                else if(_entitySphere.Intersects(children.ElementAt(i)._entitySphere) && children.ElementAt(i).GetType() == game.Station._type)
                {
                    if (_health < 100)
                    {
                        _health++;
                    }    
                }
            }
        }


        /****************public override void CollisionHandler(List<Entity> children)
        {
            //CollisionDetection collision = new CollisionDetection();
            //Type collidingEntity = null;

            if (collidingEntity == game.Station._type)
            {
                _health++;
            }
            if (collidingEntity == game.asteroid._type)
            {
                if (_health >= 5)
                {
                    _health -= 5;
                }
                else
                {
                    _health = 0;
                }
            }
        }*/////////////////////

        public override void Draw(GameTime gameTime)
        {
            if (Alive)
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
        }
    } // End of Player Class
}
