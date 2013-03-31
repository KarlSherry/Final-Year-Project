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

        public ForceField(List<Entity> list)
        {
            _entityModel = "Models//ParticleSpheres//sphereBlue1";
            _type = this.GetType();

            _pos = game.Station._pos;
            _look = Vector3.Forward;

            _scale = 200.0f;
            _health = 100.0f;
            _alpha = 0.15f;

            _alive = true;

            _parentList = list;
            _parentList.Add(this);

            _defence = 100;
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(this, Game1.Instance().EnemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(Game1.Instance().EnemyBulletList);

                if (_health <= 0)
                    _alive = false;

                if (!_alive)
                    _parentList.Remove(this);

                angle += timeDelta;
            }
            else
                _parentList.Remove(this);
        }

        public override void CollisionHandler(List<Entity> list)
        {
            foreach(Entity e in list)
            {
                if (_entityCollisionFlag == true && e is Bullet)
                {
                    if (_entitySphere.Intersects(e._entitySphere))
                    {
                        this._health -= (e._damageOnCollision / _defence);
                        e._alive = false;
                    }
                    _entityCollisionFlag = false;
                }
            }
        }
    }
}
