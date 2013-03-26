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
    public class Camera
    {        
        public Matrix projection;
        public Matrix view;

        private Vector3 _pos;
        private Vector3 _look;
        private Vector3 _up;

        public Camera()
        {
            _pos = new Vector3(0, 300, 100);
            _look = Vector3.Forward;
            _up = Vector3.Up;
        }

        public void Update(GameTime gameTime)
        {
            float timeDelta = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            Vector3 playerPosition = Game1.Instance().Player._pos;

            _pos = new Vector3(playerPosition.X, playerPosition.Y + 500.0f, playerPosition.Z + 250.0f);

            view = Matrix.CreateLookAt(_pos, playerPosition, _up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game1.Instance().GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
        }

        public Matrix getProjection() { return projection; }
        public Matrix getView() { return view; }
        
    }
}
