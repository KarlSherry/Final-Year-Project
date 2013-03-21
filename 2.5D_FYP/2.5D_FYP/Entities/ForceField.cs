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

        bool hasHitSomething = false;

        List<Entity> enemyBulletList = new List<Entity>();

        float Transparency;

        public ForceField(List<Entity> list)
        {
            _entityModel = "Models//ParticleSpheres//sphereBlue";
            _entityName = "Force Field";
            _type = this.GetType();

            _pos = game.Station._pos;
            _look = Vector3.Forward;

            _scale = 200.0f;
            _health = 100.0f;
            Transparency = 0.02f;

            _alive = true;

            parentList = list;
            if(_alive)
                parentList.Add(this);

            _defence = 100;

            enemyBulletList = Game1.Instance().EnemyBulletList;
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(enemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(enemyBulletList);

                if (_health < 0)
                {
                    _alive = false;
                }

                if (!_alive)
                    parentList.Remove(this);

                angle += timeDelta;
            }
            else
                parentList.Remove(this);
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
                            effect.Alpha = Transparency;
                            effect.World = _worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                } // End of if(_model != null)
            }
        } // End of Draw

        public override void CollisionHandler(List<Entity> list)
        {
            foreach(Entity e in list)
            {
                if (e is Bullet)
                {
                    this._health -= (e._damageOnCollision / _defence);
                    e._alive = false;
                }
            }
        }
    }
}
