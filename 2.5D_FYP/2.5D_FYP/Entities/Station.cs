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

        List<Entity> enemyBulletList = new List<Entity>();

        public Station(List<Entity> list)
        {
            _entityModel = "Station Phases//CompleteStations5a";
            _entityName = "Station";
            _type = this.GetType();

            _pos = new Vector3(0, -40, 0);
            _look = Vector3.Forward;

            _scale = 50.0f;
            _health = 10.0f; _defence = 100.0f;

            _alive = true;

            parentList = list;
            if(_alive)
                parentList.Add(this);

            enemyBulletList = Game1.Instance().EnemyBulletList;
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                hasHitSomething = CheckCollision(enemyBulletList);
                if (hasHitSomething)
                    CollisionHandler(enemyBulletList);

                if (!_alive)
                    parentList.Remove(this);

                angle += timeDelta;
            }
            else
                parentList.Remove(this);
        }

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity e in list)
            {
                if (e is Bullet)
                {
                    _health -= (e._damageOnCollision / _defence);
                    e._alive = false;
                }
            }
        }
   
    } // End of Station Class
}
