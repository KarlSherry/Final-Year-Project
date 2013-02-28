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

                CollisionHandler(game.Children);

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
                        _entitySphere.Radius = 50;
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
            Asteroid a = new Asteroid();
            for (int i = 0; i < children.Count; i++)
            {
                if (_entitySphere.Intersects(children.ElementAt(i)._entitySphere) && children.ElementAt(i).GetType() == game.Asteroid[24]._type)
                {
                    // This one does not seem to be working correclty........
                    _velocity = -_velocity;
                }
                else if (_entitySphere.Intersects(children.ElementAt(i)._entitySphere) && children.ElementAt(i).GetType() == game.Player._type)
                {
                    Alive = false;
                    children.Remove(this);
                }
                else if (_entitySphere.Intersects(children.ElementAt(i)._entitySphere) && children.ElementAt(i).GetType() == game.Player.weapon.bullet._type)
                {
                    Alive = false;
                    children.Remove(this);
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
