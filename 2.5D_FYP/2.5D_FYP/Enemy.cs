using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2._5D_FYP
{
    public class Enemy : Entity
    {
        int enemyTypeIndex = 0;
        string[] enemyType = { "anaconda", "python", "viper", "gecko" };

        bool hasHitSomething;

        public Enemy(List<Entity> list)
        {
            enemyTypeIndex = Entity.randomGenerator.Next(0, 4);
            _entityModel = "Models//Elite Models//" + enemyType[enemyTypeIndex];
            _entityName = "Enemy";

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth, game.World.worldWidth));
            _look = new Vector3(randomClamped(), 0, randomClamped());

            _maxSpeed = 500.0f; _maxForce = 150.0f; _scale = 0.2f; _mass = 10.0f; _rotationSpeed = 5.0f;

            hasHitSomething = false;
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
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _worldTransform = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateScale(_scale) * Matrix.CreateWorld(_pos, _look, _up);
        }
    }
}
