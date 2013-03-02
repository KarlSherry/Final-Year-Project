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

        public Bullet() 
        {
            _alive = true;
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>(_entityModel);
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(.5f) * Matrix.CreateTranslation(_pos);

                float speed = 10.0f;
                walk(speed * timeDelta);

                if (travelTime >= maxTime)
                {
                    _alive = false;
                    game.Children.Remove(this);
                }

                travelTime += timeDelta;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (_alive)
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
        }
    }
}
