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
    class Bullet : Entity
    {
        public Bullet() { }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>(_entityName);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _worldTransform = Matrix.CreateScale(.5f) * Matrix.CreateTranslation(_pos);

            float speed = 10.0f;
            walk(speed * timeDelta);
        }

        public override void Draw(GameTime gameTime)
        {           
            // Draw the mesh
            if (_model != null)
            {
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    _entitySphere = mesh.BoundingSphere;
                    _entitySphere.Center = _pos;

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
