﻿using System;
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

        public Station()
        {
            _entityModel = "Models//Elite Models//coriolis";
            _entityName = "Station";
            _type = this.GetType();

            _pos = new Vector3(0, _YAxis, 0);
            _look = Vector3.Forward;

            _scale = 0.25f;

            _alive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);

                angle += timeDelta;
            }
        }
    } // End of Station Class
}
