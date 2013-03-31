using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _2._5D_FYP
{
    public class ParticleSystem
    {
        public int totalParticles = 50;
        public Vector3 position = Vector3.Zero;
        public Texture2D sprite;
        public Particle[] particle;
        public int currentParticle = 0;
        public int end = 60;
        public bool started = false;

        List<Particle> particleList = new List<Particle>();

        public void Start(Entity e, Vector3 emitterPos)
        {
            started = true;
            position = emitterPos;
            if (e is Asteroid)
            {
                InitializeAsteroidExplosion();
            }
            if (e is Enemy)
            {
                InitializeEnemyExplosion(e._entityName);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (started)
            {
                currentParticle++;

                if (currentParticle < totalParticles)
                {
                    particle[currentParticle].Active = true;
                    particle[currentParticle].Position = position;
                }

                for (int i = 0; i < totalParticles; i++)
                {
                    if (particle[i].Active)
                        particle[i].Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (started)
            {
                for (int i = 0; i < totalParticles; i++)
                {
                    if (particle[i].Active)
                        particle[i].Draw(gameTime);
                }
            }
        }

        public void InitializeAsteroidExplosion()
        {
            Random r = new Random();
            particle = new Particle[totalParticles];

            for (int i = 0; i < totalParticles; i++)
            {
                particle[i] = new Particle(position
                                          , new Vector3((float)r.Next(-100, 100) / 100, (float)r.Next(-100, 100) / 100, (float)r.Next(-100, 100) / 100)
                                          , r.Next(200, 500), r.Next(1, 6), 1.5f, r.Next(0,1));

                particle[i].LoadAsteroidParticle(); 
                particleList.Add(particle[i]);
            }
        }

        public void InitializeEnemyExplosion(string enemyName)
        {
            Random r = new Random();
            particle = new Particle[totalParticles];

            for (int i = 0; i < totalParticles; i++)
            {
                particle[i] = new Particle(position
                                          , new Vector3((float)r.Next(-100, 100) / 100, (float)r.Next(-100, 100) / 100, (float)r.Next(-100, 100) / 100)
                                          , r.Next(200, 500), r.Next(1, 6), 1.5f, r.Next(0,2));

                if (enemyName == "Mini-Z") particle[i].LoadMiniParticles();
                if (enemyName == "Midi-Z") particle[i].LoadMidiParticles();
                if (enemyName == "Maxi-Z") particle[i].LoadMaxiParticles();
                if (enemyName == "Mega-Z") particle[i].LoadMegaParticles(); 
                particle[i].LoadContent();
                particleList.Add(particle[i]);
            }
        }
    }
}
