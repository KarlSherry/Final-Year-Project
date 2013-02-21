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
        SpriteFont spriteFont;

        float rotationSpeed = 5.0f;
        float lastFired = 0.0f;

        public float capacity = 0.0f;


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

            rotationSpeed = 5.0f;
            spriteFont = Game1.Instance().Content.Load<SpriteFont>("Verdana");
        } // End of Player()

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>("fighter");
        } // End of LoadContent()

        void addForce(Vector3 force)
        {
            this._force += force;
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            KeyboardState keyState = Keyboard.GetState();

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

            if (keyState.IsKeyDown(Keys.Space))
            {
                if (lastFired >= 0.5f)
                {
                    lastFired = 0.0f;

                    Bullet bullet = new Bullet();
                    bullet.LoadContent();
                    bullet._pos = _pos;
                    bullet._look = _look;
                    Game1.Instance().Children.Add(bullet);
                }
            }

            lastFired += timeDelta;

            base.Update(gameTime);
        } // End of Update(GameTime gameTime)

        public override void Draw(GameTime gameTime)
        {
            _worldTransform = Matrix.CreateWorld(_pos, _look, _up);

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
