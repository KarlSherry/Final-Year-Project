using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class Metal : Entity
    {
        int metalTypeIndex;
        string[] metalType = { "frag11", "frag22", "frag33", "frag44" };

        float angle = 0.0f;

        public Metal(List<Entity> list)
        {
            metalTypeIndex = Entity.randomGenerator.Next(0, 4);
            getMetalType(metalTypeIndex);

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

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

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                if (_pos.Length() > game.World.worldWidth)
                {
                    _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                           , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
                }

                angle += timeDelta;

                _pos += _look * timeDelta * _maxSpeed;
            }
            else parentList.Remove(this);
        }

        public void getMetalType(int index)
        {
            switch (index)
            {
                case 0:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 5.0f;
                    break;
                case 1:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 5.0f;
                    break;
                case 2:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 5.0f;
                    break;
                case 3:
                    _entityModel = "Models//FloatingObjects//" + metalType[metalTypeIndex];
                    _maxSpeed = randomGenerator.Next(10, 25); _scale = 5.0f;
                    break;

            }
        }
    } // End of Class Metal
}
