using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2._5D_FYP
{
    public class Ground : Entity
    {
        GraphicsDeviceManager graphics;
        VertexPositionTexture[] vertices;
        BasicEffect basicEffect;
        public float groundWidth = 0.0f, groundHeight = 0.0f;

        public Ground(List<Entity> list)
        {
            _entityName = "Ground";

            parentList = list;
            parentList.Add(this);

            graphics = game.GraphicsDeviceManager;
            groundWidth = game.World.worldWidth; groundHeight = game.World.worldHeight;            
        }

        public override void LoadContent()
        {
            float twidth = groundWidth / 100, theight = groundHeight / 100;

            vertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-groundWidth, -50, groundHeight), new Vector2(0, theight)),
                new VertexPositionTexture(new Vector3(-groundWidth, -50, -groundHeight), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(groundWidth, -50, groundHeight), new Vector2(twidth, theight)),

                new VertexPositionTexture(new Vector3(groundWidth, -50, groundHeight), new Vector2(twidth, theight)),
                new VertexPositionTexture(new Vector3(-groundWidth, -50, -groundHeight), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(groundWidth, -50, -groundHeight), new Vector2(twidth, 0))
            };

            Texture2D portrait = Game1.Instance().Content.Load<Texture2D>("Stars");
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = portrait;
        }

        public override void Update(GameTime gameTime)
        {
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = Game1.Instance().Camera.projection;
            basicEffect.View = Game1.Instance().Camera.view;
        }

        public override void Draw(GameTime gameTime)
        {
            EffectPass effectPass = basicEffect.CurrentTechnique.Passes[0];
            effectPass.Apply();
            SamplerState state = new SamplerState();
            state.AddressU = TextureAddressMode.Wrap;
            state.AddressV = TextureAddressMode.Wrap;
            state.AddressW = TextureAddressMode.Wrap;
            state.Filter = TextureFilter.Anisotropic;
            graphics.GraphicsDevice.SamplerStates[0] = state;
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
        }
    }
}
