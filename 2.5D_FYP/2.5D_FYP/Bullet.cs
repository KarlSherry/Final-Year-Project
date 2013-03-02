using System;
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
        float maxTime = 3.0f;
        bool hasHitSomething;

        CollisionDetection c = new CollisionDetection();
        List<Entity> children = Game1.Instance().Children;

        public Bullet() 
        {
            _alive = true;
            hasHitSomething = false;
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>(_entityModelName);
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(.5f) * Matrix.CreateTranslation(_pos);

                hasHitSomething = c.CheckCollision(this, children);
                if (hasHitSomething == true)
                {
                    CollisionHandler(children);
                }

                float speed = 10.0f;
                walk(speed * timeDelta);

                if (travelTime >= maxTime)
                {
                    Alive = false;
                    game.Children.Remove(this);
                }

                travelTime += timeDelta;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Alive)
            {
                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        _entitySphere = mesh.BoundingSphere;
                        _entitySphere.Center = _pos;
                        _entitySphere.Radius = 5;

                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.World = _worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    }
                }
            }
        } // End of Draw(GameTime gameTime)

        public override void CollisionHandler(List<Entity> children)
        {            
            if (hasHitSomething)
            {
                foreach(Entity e in children)
                {
                    /*if (e._entityCollisionFlag == true && e is Asteroid)
                    {
                        Console.WriteLine("Bullet collided");
                        _alive = false;
                        e._entityCollisionFlag = false;
                    }*/

                    /*if (e._entityCollisionFlag == true && e is Enemy)
                    {
                        _alive = false;
                        Console.WriteLine("Bullet collided");
                        e._entityCollisionFlag = false;
                    }*/
                }
            }
        } // End of CollisionHandler(List<Entity children)
    } // End of Bullet Class
}
