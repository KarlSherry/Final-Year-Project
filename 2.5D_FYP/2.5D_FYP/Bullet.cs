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
        float maxTime = 1.0f;

        Weapon w = Game1.Instance().Player.weapon;

        public Bullet(List<Entity> list) 
        {
            _alive = true;

            parentList = list;
                parentList.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                walk(_maxSpeed * timeDelta);

                if (travelTime >= maxTime)
                {
                    _alive = false;
                }

                travelTime += timeDelta;

                if (!_alive)
                    parentList.Remove(this);
            }
            else parentList.Remove(this);
        }

        public void getBulletType(int b)
        {
            switch (b)
            {
                case 0:
                    _entityModel = "Models//sphere";
                    _maxSpeed = 100.0f; _scale = 1.0f;
                    break;
                case 1:
                    _entityModel = "Models//sphere";
                    _maxSpeed = 100.0f; _scale = 2.0f;
                    break;
                case 2:
                    _entityModel = "Models//Elite Models//boa";
                    _maxSpeed = 200.0f; _scale = 0.1f;
                    break;
                case 3: break;
            }
        }
    }
}
