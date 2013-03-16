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

        public Station(List<Entity> list)
        {
            _entityModel = "Station Phases//stationComplete1";
            _entityName = "Station";
            _type = this.GetType();

            _pos = new Vector3(0, -40, 0);
            _look = Vector3.Forward;

            _scale = 20.0f;

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

                _worldTransform = Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationY(angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                if (!_alive)
                    parentList.Remove(this);

                angle += timeDelta;
            }
        }                   
    } // End of Station Class
}
