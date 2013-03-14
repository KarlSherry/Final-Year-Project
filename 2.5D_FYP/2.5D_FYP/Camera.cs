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
    public class Camera : Entity
    {        
        public Matrix projection;
        public Matrix view;
        private MouseState mouseState;

        private bool firstPerson = false;
        private bool topDown = true;

        public Camera(List<Entity> list)
        {
            _entityName = "Camera";

            _pos = new Vector3(0, 300, 100);
            _look = Vector3.Forward;

            parentList = list;
            parentList.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            view = Matrix.CreateLookAt(_pos, _pos + _look, _up);

            mouseState = Mouse.GetState();

            int mouseX = mouseState.X;
            int mouseY = mouseState.Y;

            int midX = GraphicsDeviceManager.DefaultBackBufferHeight / 2;
            int midY = GraphicsDeviceManager.DefaultBackBufferWidth / 2;

            int deltaX = mouseX - midX;
            int deltaY = mouseY - midY;

            yaw(-(float)deltaX / 100.0f);
            pitch(-(float)deltaY / 100.0f);
            Mouse.SetPosition(midX, midY);

            Vector3 playerPosition = Game1.Instance().Player._pos;

            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.F2))
            {
                firstPerson = true;
                topDown = false;
            }
            if (k.IsKeyDown(Keys.F1))
            {
                firstPerson = false;
                topDown = true;
            }

            if (topDown)
            {
                _look = Game1.Instance().Player._look;
                _pos = new Vector3(playerPosition.X, playerPosition.Y + 500.0f, playerPosition.Z + 250.0f);
                //_look = Game1.Instance().Player._look;
                view = Matrix.CreateLookAt(_pos, playerPosition + _look, _up);
            }
            if(firstPerson)
            {
                _pos = new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z);
                _look = Game1.Instance().Player._look;
                view = Matrix.CreateLookAt(_pos, _pos + _look, _up);
            }

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), Game1.Instance().GraphicsDeviceManager.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
        }

        public Matrix getProjection() { return projection; }
        public Matrix getView() { return view; }
        
    }
}
