using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class MiniMap
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation

        public MiniMap()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.0001f) _zoom = 0.0001f; } // Negative zoom will flip image
        }

        public Matrix get_transformation(Vector2 p)
        {
            return Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                Matrix.CreateRotationZ(_rotation) *
                                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                Matrix.CreateTranslation(new Vector3(p.X, p.Y, 0));

        }

    } // end of Camera2D
}