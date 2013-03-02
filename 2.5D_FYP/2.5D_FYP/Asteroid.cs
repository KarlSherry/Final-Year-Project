using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Asteroid : Entity
    {
        float angle = 0.0f;
        public float scale = 0.0f;
        int asteroidCount = 25;
        bool hasHitSomething;

        List<Entity> children = new List<Entity>();
        CollisionDetection c = new CollisionDetection();

        public int AsteroidCount
        {
            get { return asteroidCount; }
            set { asteroidCount = value; }
        }

        public Asteroid()
        {
            _worldTransform = Matrix.Identity;
            _type = this.GetType();
            _entityName = "Asteroid";

            Alive = true;
            hasHitSomething = false;

            children = game.Children;
        }

        public override void Initialize()
        {
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();
            _maxSpeed = randomGenerator.Next(10, 25);
            scale = randomGenerator.Next(1, 3);
            _mass = scale;
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>(_entityModelName);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                hasHitSomething = c.CheckCollision(this, children);
                CollisionHandler(children);

                /*Vector3 acceleration = _force / _mass;

                _velocity += acceleration * timeDelta;

                _pos += _velocity * timeDelta;

                _force = _look * 5;

                if (_velocity.Length() > _maxSpeed)
                {
                    _velocity.Normalize();
                    _velocity *= _maxSpeed;
                }

                if (_velocity.Length() > 0.0001f)
                {
                    _right = Vector3.Cross(_look, _up);
                } */

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;

                if (_alive == false)
                    game.Children.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Alive)
            {
                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(_pos);

                if (_model != null)
                {
                    foreach (ModelMesh mesh1 in _model.Meshes)
                    {
                        _entitySphere = mesh1.BoundingSphere;
                        _entitySphere.Center = _pos;
                        _entitySphere.Radius = mesh1.BoundingSphere.Radius * scale;
                        foreach (BasicEffect effect in mesh1.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.World = _worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh1.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                }
            }
        } // End of Draw(GameTime gameTime)

        public override void CollisionHandler(List<Entity> children)
        {
            if (hasHitSomething)
            {
                foreach (Entity entity in children)
                {
                    if (entity._entityCollisionFlag == true && entity is Player)
                    {
                        _alive = false;
                        entity._entityCollisionFlag = false;
                    }
                    else if (entity._entityCollisionFlag == true && entity is Asteroid)
                    {
                        entity._look = -entity._look;
                        entity._entityCollisionFlag = false;
                    }
                    else if (entity._entityCollisionFlag == true && entity is Bullet)
                    {
                        _alive = false;
                        entity._entityCollisionFlag = false;
                    }
                }
            }
        }
    }
}
