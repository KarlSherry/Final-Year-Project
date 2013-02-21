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
        public Station()
        {
            _pos = new Vector3(0, 50, 0);
            _look = Vector3.Forward;
        }

        public override void LoadContent()
        {
            _model = Game1.Instance().Content.Load<Model>("Station");
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            _worldTransform = Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateScale(10.0f) * Matrix.CreateWorld(_pos, _look, _up);

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
    } // End of Station Class
}
