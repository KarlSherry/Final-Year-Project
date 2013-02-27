using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class FloatingObjects : Entity
    {
        float angle = 0.0f;

        public FloatingObjects()
        {
            _worldTransform = Matrix.Identity;           
        }

        public override void Initialize()
        {
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();
            _maxSpeed = randomGenerator.Next(10, 25);
            base.Initialize();
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
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            angle += timeDelta;

            _pos += _look * timeDelta * _maxSpeed;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(_pos);

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
                } // End of foreach(ModelMesh mesh in _model.Meshes)
            }

            base.Draw(gameTime);
        }
        //public void 
    }
}
