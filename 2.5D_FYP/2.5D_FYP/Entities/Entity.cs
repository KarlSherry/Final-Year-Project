using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public abstract class Entity
    {
        public Game1 game = Game1.Instance();

        public Model _model = null;
        public string _entityModel = "Models//ParticleSpheres//sphere";
        public string _entityName;
        public Type _type;
        public Matrix _worldTransform = Matrix.Identity;

        public Vector3 _pos = Vector3.Zero;
        public Vector3 _look = Vector3.Forward;
        public Vector3 _velocity = Vector3.Zero;
        public Vector3 _force = Vector3.Zero;

        public Vector3 _right = Vector3.Right;
        public Vector3 _up = Vector3.Up;
        public Vector3 _basis = Vector3.Forward;
        public Vector3 _globalUp = Vector3.Backward;

        public float _maxSpeed = 0.0f, _maxForce = 0.0f, _scale = 1.0f, _mass = 0.0f, _rotationSpeed = 0.0f;
        public float _health = 0.0f, _shield = 0.0f, _attackStrength = 0.0f, _defence = 0.0f;
        public float _damageOnCollision;
        public float _alpha = 1.0f;
        public float _emmisiveR = 0.0f, _emmisiveG = 0.0f, _emmisiveB = 0.0f;
        public int _weaponIndex = 0;
        public int _YAxis = 0;

        public Vector3 _randomPosition = new Vector3(Entity.randomGenerator.Next(-Game1.Instance().World.worldWidth, Game1.Instance().World.worldHeight)
                , 0, Entity.randomGenerator.Next(-Game1.Instance().World.worldWidth, Game1.Instance().World.worldHeight));

        public bool _entityCollisionFlag;
        public bool _fireWeapon = false;

        public BoundingSphere _entitySphere;

        public bool _alive { get; set; }

        public List<Entity> _parentList;

        public static Random randomGenerator = new Random(DateTime.Now.Millisecond);

        /*public Entity(List<Entity> list) 
        {
            _parentList = list;
            _parentList.Add(this);
        }
        public Entity(Vector3 pos, List<Entity> list) { }*/

        public virtual void LoadContent() 
        {
            _model = Game1.Instance().Content.Load<Model>(_entityModel);
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) 
        {
            if (_alive)
            {
                if (_model != null)
                {
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        if (!(mesh.BoundingSphere.Radius == 0.0f))
                        {
                            _entitySphere = mesh.BoundingSphere.Transform(_worldTransform);
                            _entitySphere.Center = _pos;
                            _entitySphere.Radius = mesh.BoundingSphere.Radius * _scale;
                        }

                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.PreferPerPixelLighting = true;
                            effect.World = _worldTransform;
                            effect.Alpha = _alpha;
                            effect.EmissiveColor = new Vector3(_emmisiveR, _emmisiveG, _emmisiveB);
                            effect.Projection = Game1.Instance().Camera.getProjection();
                            effect.View = Game1.Instance().Camera.getView();
                        }
                        mesh.Draw();
                    } // End of foreach(ModelMesh mesh in _model.Meshes)
                } // End of if(_model != null)
            }
        }
        public virtual void CollisionHandler(List<Entity> list) { }
        //public virtual void CollisionHandler() { }

        public void yaw(float angle)
        {
            Matrix M = Matrix.CreateRotationY(angle);
            _right = Vector3.Transform(_right, M);
            _look = Vector3.Transform(_look, M);
        }

        public float getYaw()
        {
            Vector3 localLook = _look;
            localLook.Y = _basis.Y;
            localLook.Normalize();
            float angle = (float)Math.Acos(Vector3.Dot(_basis, localLook));

            if(_look.X > 0)
                angle = (MathHelper.Pi * 2.0f) - angle;

            return angle;
        }
        
        public void walk(float timeDelta)
        {
            _pos += _look * timeDelta;
        }

        public void strafe(float timeDelta)
        {
            _pos += _right * timeDelta;
        }

        public Boolean CheckCollision(Entity entity, List<Entity> children)
        {
            bool intersect = false;
            for (int i = 0; i < children.Count; i++)
            {
                if (!children.ElementAt(i).Equals(entity))
                {
                    intersect = entity._entitySphere.Intersects(children.ElementAt(i)._entitySphere);

                    if (intersect)
                    {
                        entity._entityCollisionFlag = true;
                        ///////////////
                        if (!(entity.GetType() == children.ElementAt(i).GetType()))
                        {
                            children.ElementAt(i)._entityCollisionFlag = true;
                        }

                        return true;
                    }
                }
            }
            return intersect;
        }
    }
}
