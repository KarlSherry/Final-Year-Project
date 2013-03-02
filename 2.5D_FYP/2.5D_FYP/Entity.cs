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
        public static Random randomGenerator = new Random(DateTime.Now.Millisecond);
        public Vector3 _pos = Vector3.Zero;

        public Vector3 _look = Vector3.Forward;
        public Vector3 _right = Vector3.Right;
        public Vector3 _up = Vector3.Up;
        public Vector3 _basis = Vector3.Forward;
        public Vector3 _globalUp = Vector3.Backward;

        public Vector3 _velocity = Vector3.Zero;
        public Vector3 _force = Vector3.Zero;
        public float _maxSpeed = 0.0f, _maxForce = 0.0f, _mass = 1.0f, _rotation = 0.0f;
        public float _health = 0.0f, _shield = 0.0f;

        public string _entityModelName;
        public string _entityName;
        public bool _entityCollisionFlag;

        public Matrix _worldTransform = new Matrix();

        public Model _model = null;
        public BoundingSphere _entitySphere;

        public bool keyPressed = false;

        public bool _alive = false;
        public bool Alive 
        {
            get { return _alive; }
            set { _alive = value; } 
        }

        public Type _type;

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
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

        public void pitch(float angle)
        {
            Matrix T = Matrix.CreateFromAxisAngle(_right, angle);
            _look = Vector3.Transform(_look, T);
        }

        public float getPitch()
        {
            if (_look.Y == _basis.Y)
            {
                return 0;
            }
            Vector3 localBasis = new Vector3(_look.X, 0, _look.Z);
            localBasis.Normalize();
            float dot = Vector3.Dot(localBasis, _look);
            float angle = (float)Math.Acos(dot);
            if (float.IsNaN(angle))
            {
                return 0.0f;
            }
            if (_look.Y < 0)
            {
                angle = (MathHelper.Pi * 2.0f) - angle;
            }

            return angle;
        }
        
        public void walk(float timeDelta)
        {
            float speed = 5.0f;
            _pos += _look * timeDelta * speed;
        }
    }
}
