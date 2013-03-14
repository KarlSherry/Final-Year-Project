using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class ForceField : Entity
    {
        float angle = 0.0f;

        public ForceField(List<Entity> list)
        {
            _entityModel = "Models//sphereBlue";
            _entityName = "Force Field";
            _type = this.GetType();

            _pos = game.Station._pos;
            _look = Vector3.Forward;

            _scale = 100.0f;

            _alive = true;

            parentList = list;

            if(_alive)
                parentList.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                if (!_alive)
                    parentList.Remove(this);

                angle += timeDelta;
            }
        }

        public override void Draw(GameTime gameTime)
        {if (_alive)
            {
                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        if (!(mesh.BoundingSphere.Radius == 0.0f))
                        {
                            _entitySphere = mesh.BoundingSphere.Transform(_worldTransform);
                            _entitySphere.Center = _pos;
                            _entitySphere.Radius = mesh.BoundingSphere.Radius * _scale;
                        }

                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.Alpha = 0.25f;
                            effect.World = _worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                } // End of if(_model != null)
            }
        }
    }
}
