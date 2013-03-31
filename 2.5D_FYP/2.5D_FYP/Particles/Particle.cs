using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Particle
    {
        private Model model;
        private Matrix worldTransform;
        public Vector3 Position { get; set; }
        private Vector3 Velocity { get; set; }
        private float Speed { get; set; }
        private float Scale { get; set; }
        private float LifeTime { get; set; }
        private float StartTime { get; set; }
        private float Angle { get; set; }
        private float Transparency { get; set; }
        public bool Active { get; set; }

        private int Index { get; set; }    

        public Particle(Vector3 pos, Vector3 vel, float speed, float scale, float lifeTime, int index)
        {
            Position = pos;
            Velocity = vel;
            Speed = speed;
            Scale = scale;
            LifeTime = lifeTime;
            StartTime = 0.0f;
            Angle = 0.0f;
            Transparency = 1.0f;
            Active = true;

            Index = index;
        }

        public void LoadContent()
        {
            if (Index == 1) model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelFire");                  
        }

        public void LoadAsteroidParticle()
        {
            if(Index == 0) model = Game1.Instance().Content.Load<Model>("Models//FloatingObjects//Asteroid");
            Scale = 0.5f;
        }
        public void LoadMiniParticles()
        {
            if (Index == 0) model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelGreen");          
        }
        public void LoadMidiParticles()
        {
            if (Index == 0) model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelBlue");
        }
        public void LoadMaxiParticles()
        {
            if (Index == 0) model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelRed");
        }
        public void LoadMegaParticles()
        {
            switch (Index)
            {
                case 0:
                    model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelGreen");
                    break;
                case 1:
                    model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelBlue");
                    break; 
                case 2:
                    model = Game1.Instance().Content.Load<Model>("Models//ParticleSpheres//AlienShrapnelRed");
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            worldTransform = Matrix.CreateRotationY(Angle) * Matrix.CreateRotationZ(Angle) * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);

            Position += Velocity * Speed * timeDelta;
            StartTime += timeDelta;

            if (StartTime > LifeTime)
                Active = false;

            Angle += timeDelta;
            Transparency -= (timeDelta * 2);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Active)
            {
                if (model != null)
                {
                    foreach (ModelMesh mesh in model.Meshes)
                    {

                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;

                            if(Index == 1 || Index == 2)
                                effect.Alpha = Transparency;

                            effect.World = worldTransform;
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                } // End of if(_model != null)
            }
        }
    }
}
