using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using DPSF;

namespace _2._5D_FYP
{
    public class ParticleSystem
    {
        public int totalParticles = 100;
        public Vector3 position = Vector3.Zero;
        public Texture2D sprite;
        public Particle[] particle;
        public int currentParticle = 0;
        public int end = 60;
        public bool started = false;

        List<Particle> particleList = new List<Particle>();

        public void Initialize()
        {
            Random r = new Random();
            particle = new Particle[totalParticles];

            for (int i = 0; i < totalParticles; i++)
            {
                particle[i] = new Particle(position
                                          ,new Vector3((float)r.Next(-100, 100)/100, (float)r.Next(-100, 100)/100, (float)r.Next(-100, 100)/100)
                                          ,r.Next(100,200), r.Next(1,6), 3.0f, r.Next(0,3));
                particle[i].LoadContent();
                particleList.Add(particle[i]);
            }
        }

        public void Start(Entity e, Vector3 emitterPos)
        {
            started = true;
            position = emitterPos;
            Initialize();
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
    }

    /*public class ParticleSystem : DefaultTexturedQuadParticleSystem
    {
        public ParticleSystem(Game game) : base(game) { }

        public override void AutoInitialize(GraphicsDevice graphicsDevice, ContentManager contentManager, SpriteBatch spriteBatch)
        {
            InitializeTexturedQuadParticleSystem(graphicsDevice, contentManager, 1000, 5000, UpdateVertexProperties, "Fire");
        }

        public void LoadParticleSystem(Vector3 pos)
        {
            ParticleInitializationFunction = InitializeParticle;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleWidthAndHeightUsingLerp);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleColorUsingLerp);

            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp, 1);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 200);

            ParticleSystemEvents.LifetimeData.Lifetime = 3.0f;
            ParticleSystemEvents.AddTimedEvent(0.0f, UpdateParticleSystemEmitParticlesAutomaticallyOn);
            ParticleSystemEvents.AddTimedEvent(1.0f, UpdateParticleSystemEmitParticlesAutomaticallyOff);

            Emitter.ParticlesPerSecond = 100;
            Emitter.PositionData.Position = pos;
        }

        public void InitializeParticle(DefaultQuadParticle p)
        {
            p.Lifetime = 2.0f;
            p.Position = Emitter.PositionData.Position;

            Vector3 vMin = new Vector3(-50, -50, -50);
            Vector3 vMax = new Vector3(50, 50, 50);

            p.Velocity = DPSFHelper.RandomVectorBetweenTwoVectors(vMin, vMax);

            p.Width = p.StartWidth = p.EndWidth =
                p.Height = p.StartHeight = p.EndHeight = RandomNumber.Next(10, 50);

            p.RotationalVelocity = new Vector3(MathHelper.Pi, MathHelper.Pi, MathHelper.Pi);
        }
    }*/
}
