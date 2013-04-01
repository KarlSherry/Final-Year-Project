using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Metal : Entity
    {
        int metalTypeIndex;
        string[] metalType = { "Metal1", "Metal2", "Metal3", "Metal4" };

        float angle = 0.0f;
        float emmisiveColorDelta = 0.01f;

        public Metal(List<Entity> list)
        {
            metalTypeIndex = Entity.randomGenerator.Next(0, 4);
            getMetalType(metalTypeIndex);

            _pos = _randomPosition;

            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _alive = true;

            _parentList = list;
            if (_alive)
                _parentList.Add(this);
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

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                if (_pos.Length() > 4250)
                    _pos = -_pos;

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;                     

                if (_emmisiveR >= 1.0f || _emmisiveG >= 1.0f)
                {
                    emmisiveColorDelta = -0.01f;
                }
                if (_emmisiveR <= 0.0f || _emmisiveG <= 0.0f)
                {
                    emmisiveColorDelta = 0.01f;
                }

                //_emmisiveR += emmisiveColorDelta;
                //_emmisiveG += emmisiveColorDelta;
            }
            else _parentList.Remove(this);
        }

        public void getMetalType(int index)
        {
            switch (index)
            {
                case 0:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 10.0f;
                    break;
                case 1:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 10.0f;
                    break;
                case 2:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 10.0f;
                    break;
                case 3:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 10.0f;
                    break;

            }
        }
    } // End of Class Metal
}
