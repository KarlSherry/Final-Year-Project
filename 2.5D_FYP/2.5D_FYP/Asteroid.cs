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

            Alive = true;
            hasHitSomething = false;

            children = game.Children;
        }

        public override void Initialize()
        {
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();
            _maxSpeed = randomGenerator.Next(10, 25);
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>(_entityName);
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                hasHitSomething = c.CheckCollision(this, children);
                CollisionHandler(children);

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Alive)
            {
                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(_pos);

                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        _entitySphere = mesh.BoundingSphere;
                        _entitySphere.Center = _pos;
                        _entitySphere.Radius = 100;
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
                }
            }
        } // End of Draw(GameTime gameTime)

        public override void CollisionHandler(List<Entity> children)
        {
            if (hasHitSomething == true)
                Console.WriteLine("Asteroid TRUE" + _pos);

            if (hasHitSomething)
            {
                foreach (Entity entity in children)
                {
                    if (entity is Player)
                    {
                        Alive = false;
                    }
                    else if (entity is Station)
                    {
                        Alive = false;
                    }
                }
            }
        }

        /******************public override void CollisionHandler()
        {
        CollisionDetection collision = new CollisionDetection();
        Type collidingEntity;
        collidingEntity = collision.CheckCollision(this, Game1.Instance().Children);
        if(collidingEntity == game.Player._type)
        {
        Alive = false;
        }
        }******************/
    }
}
