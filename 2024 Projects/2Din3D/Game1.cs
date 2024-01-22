using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2Din3D
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        private Triangle tri;

        private Mesh triangleMesh = new Mesh();
        private List<Mesh> meshes = new List<Mesh>();

        private Camera camera;
        private BasicEffect basicEffect; // Tells XNA how to draw object

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            meshes.Add(new Mesh());
            meshes[0].CreateSquare(Vector3.Zero, 3);

            camera = new Camera(Vector3.Zero, new Vector3(0,0,-100), 64,36, 0.1f, 1000f, true, false);
            camera.MoveSpeed = 1;

            // BasicEffect, very powerful shader with open source code: https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb195083(v=xnagamestudio.35)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();

            base.Initialize();
        }

        protected override void LoadContent()
        {

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            meshes[0].MoveMesh(new Vector3(.01f, 0, 0));


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Pass in camera matrices
            basicEffect.Projection = camera.ProjectionMatrix;
            basicEffect.View = camera.ViewMatrix;
            basicEffect.World = camera.WorldMatrix;

            GraphicsDevice.Clear(Color.MediumPurple);

            foreach (Mesh m in meshes)
            {
                GraphicsDevice.SetVertexBuffer(m.VertexBuffer);

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, m.VertexBuffer.VertexCount);
                }
            }

            base.Draw(gameTime);
        }
    }
}