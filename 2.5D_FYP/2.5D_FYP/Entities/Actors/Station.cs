using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Station : Entity
    {
        float angle = 0.0f;

        bool hasHitSomething = false;
        bool newModel = false;

        List<Entity> enemyBulletList = new List<Entity>();

        Model[] stationModels = new Model[10];

        public Station(List<Entity> list)
        {
            _entityModel = "Station Phases//station1";
            _entityName = "Station";
            _type = this.GetType();

            _pos = new Vector3(0, -40, 0);
            _look = Vector3.Forward;

            _scale = 50.0f;
            _health = 10.0f; _defence = 100.0f;

            _alive = true;

            _parentList = list;
            if(_alive)
                _parentList.Add(this);

            enemyBulletList = Game1.Instance().EnemyBulletList;
        }

        public override void LoadContent()
        {
            for (int i = 0; i < 10; i++)
            {
                stationModels[i] = Game1.Instance().Content.Load<Model>("Station Phases//station" + (i + 1));
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(-angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                if (_health < 10) { _model = stationModels[0]; }
                if (_health >= 10 && _health < 20) { _model = stationModels[1]; }
                if (_health >= 20 && _health < 30) { _model = stationModels[2]; }
                if (_health >= 30 && _health < 40) { _model = stationModels[3]; }
                if (_health >= 40 && _health < 50) { _model = stationModels[4]; }
                if (_health >= 50 && _health < 60) { _model = stationModels[5]; }
                if (_health >= 60 && _health < 70) { _model = stationModels[6]; }
                if (_health >= 70 && _health < 80) { _model = stationModels[7]; }
                if (_health >= 80 && _health < 90) { _model = stationModels[8]; }
                if (_health >= 90 && _health <= 100) { _model = stationModels[9]; }

                hasHitSomething = CheckCollision(this, enemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(enemyBulletList);
                hasHitSomething = CheckCollision(this, Game1.Instance().StageList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().StageList);

                if (_health <= 0) _alive = false;

                if (!_alive)
                    _parentList.Remove(this);

                angle += timeDelta;
            }
            else
                _parentList.Remove(this);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_alive)
            {
                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        if (!(mesh.BoundingSphere.Radius == 0.0f))
                        {
                            _entitySphere = mesh.BoundingSphere.Transform(_worldTransform);
                            _entitySphere.Center = new Vector3(0, _YAxis, 0);
                            _entitySphere.Radius = mesh.BoundingSphere.Radius * _scale;
                        }

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
            }
        } // End of Draw

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity e in list)
            {
                if (e._entityCollisionFlag == true && e is Bullet)
                {
                    if(_entitySphere.Intersects(e._entitySphere))
                    {
                        this._health -= (e._damageOnCollision / _defence);
                        e._alive = false;
                    }
                    e._entityCollisionFlag = false;
                }
                if (e._entityCollisionFlag == true && e is Player)
                {
                    Player p = Game1.Instance().Player;
                    if (p.capacity > 0 && p.Docking)
                    {
                        p.capacity--;
                        _health += 10;
                        e._health++;
                    }

                    e._entityCollisionFlag = false;
                }

            }
        }
   
    } // End of Station Class
}
