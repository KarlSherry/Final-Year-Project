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
        float maxTime = 5.0f;

        public Bullet(List<Entity> list) 
        {
            _entityModel = "Models//sphere";
            _entityName = "Bullet";

            _scale = 1.0f;

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

                float speed = 10.0f;
                walk(speed * timeDelta);

                travelTime += timeDelta;
            }
        }
    }
}
