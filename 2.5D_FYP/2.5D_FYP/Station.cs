using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Station  : Entity
    {
        float angle = 0.0f;

        public Station()
        {
            _pos = new Vector3(0, 50, 0);
            _look = Vector3.Forward;
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>("Models//Elite Models//coriolis");
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _worldTransform = Matrix.CreateRotationY(angle) *  Matrix.CreateScale(0.25f) * Matrix.CreateWorld(_pos, _look, _up);
            angle += timeDelta;
        }

        public override void Draw(GameTime gameTime)
        {
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
            } // End of if(_model != null)
        } // End of Draw(GameTime gameTime)
    } // End of Station Class
}
