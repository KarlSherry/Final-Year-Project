using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace _2._5D_FYP
{
    public class Asteroid : Entity
    {
        float angle = 0.0f;

        bool hasHitSomething;

        List<Entity> stageList = new List<Entity>();
        List<Entity> asteroidList = new List<Entity>();
        List<Entity> metalList = new List<Entity>();
        List<Entity> playerBulletList = new List<Entity>();

        SoundEffect asteroidExplosion = Game1.Instance().Content.Load<SoundEffect>("SoundEffects//explosion");

        public Asteroid(List<Entity> list)
        {
            _entityModel = "Models//FloatingObjects//Asteroid";
            _entityName = "Asteroid";
            _type = this.GetType();

            _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth +1, game.World.worldWidth-1)
                , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth+1, game.World.worldWidth-1));
            _look = new Vector3(randomClamped(), 0, randomClamped());
            _look.Normalize();

            _maxSpeed = randomGenerator.Next(10, 25); _maxForce = 5.0f;  _scale = (randomGenerator.Next(0, 4)) * 2; _mass = _scale;
            _damageOnCollision = 5 * _scale;

            hasHitSomething = false;
            _alive = true;

            parentList = list;
            if (_alive)
                list.Add(this);

            stageList = game.StageList;
            asteroidList = game.AsteroidList;
            metalList = game.MetalList;
            playerBulletList = game.PlayerBulletList;
        }

        float randomClamped()
        {
            return 1.0f - ((float)randomGenerator.NextDouble() * 2.0f);
        }

        public override void Update( GameTime gameTime)
        {
            if (_alive)
            {
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                _worldTransform = Matrix.CreateRotationY(angle) * Matrix.CreateRotationZ(angle) * Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_pos);

                hasHitSomething = CheckCollision(stageList);
                if (hasHitSomething)
                    CollisionHandler(stageList);

                hasHitSomething = CheckCollision(asteroidList);
                if(hasHitSomething)
                    CollisionHandler(asteroidList);

                hasHitSomething = CheckCollision(metalList);
                if (hasHitSomething)
                    CollisionHandler(metalList);

                hasHitSomething = CheckCollision(playerBulletList);
                if(hasHitSomething)
                    CollisionHandler(playerBulletList);

                if (_pos.Length() > game.World.worldWidth)
                {
                    _pos = new Vector3(Entity.randomGenerator.Next(-game.World.worldWidth + 1, game.World.worldWidth -1)
                           , _YAxis, Entity.randomGenerator.Next(-game.World.worldWidth +1, game.World.worldWidth -1));
                }

                _pos += _look * timeDelta * _maxSpeed;

                angle += timeDelta;

                if (!_alive)
                {
                    game.particleSystem.Start(this, _pos);
                    asteroidExplosion.Play(0.10f,0.5f,0.0f);
                    parentList.Remove(this);
                }
            }
            else parentList.Remove(this);
        }

        public override void CollisionHandler(List<Entity> list)
        {
            foreach (Entity entity in list)
            {
                if (entity._entityCollisionFlag == true && entity is Player)
                {
                    _alive = false;
                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is Asteroid)
                {
                    _look = -_look;
                    entity._entityCollisionFlag = false;
                }
                if (entity._entityCollisionFlag == true && entity is Metal)
                {
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
                if(entity._entityCollisionFlag == true && entity is Bullet)
                {
                    Game1.Instance().Player.playerScore += (50 * (long)_scale);
                    _alive = false;
                    entity._alive = false;
                    entity._entityCollisionFlag = false;
                }
            }            
        }
    }
}
