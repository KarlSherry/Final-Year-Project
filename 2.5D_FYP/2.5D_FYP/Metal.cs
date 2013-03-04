using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class Metal : Entity
    {
        float angle = 0.0f;

        public Metal(List<Entity> list)
        {
            _entityModel = "Models//StationMaterial";
            _entityName = "Metal";
            _type = this.GetType();

            _pos = new Vector3(Entity.randomGenerator.Next(-900, 900), _YAxis, Entity.randomGenerator.Next(-900, 900));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _maxSpeed = randomGenerator.Next(10, 25); _scale = 5.0f;

            _alive = true;

            parentList = list;
            if (_alive)
                parentList.Add(this);
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;
            }
            else
                parentList.Remove(this);
        }
    }
}
