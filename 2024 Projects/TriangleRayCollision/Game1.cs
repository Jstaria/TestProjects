using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TriangleRayCollision
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;

        private Triangle tri;

        private Mesh triangleMesh = new Mesh();

        private Camera mainCamera;
        private BasicEffect basicEffect; // Tells XNA how to draw object

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            tri = new Triangle();
            tri.vertices = new Vector3[3];
            tri.vertices[0] = new Vector3(0, 20, 0);
            tri.vertices[1] = new Vector3(-20, -20, 0);
            tri.vertices[2] = new Vector3(20, -20, 0);

            Vector3 edge1 = tri.vertices[1] - tri.vertices[0];
            Vector3 edge2 = tri.vertices[2] - tri.vertices[0];

            Vector3 Normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

            tri.normal = Normal;

            tri.faceCount = 1;

            List<Triangle> triangles = new List<Triangle>();

            triangles.Add(tri);

            triangleMesh.AddMeshColor(triangles, Color.Yellow);

            mainCamera = new Camera(Vector3.Zero, new Vector3(0, 0, -100), 60, _graphics.GraphicsDevice.DisplayMode.AspectRatio, 0.1f, 1000f, true);
            mainCamera.MoveSpeed = 5f;

            // BasicEffect, very powerful shader with open source code: https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb195083(v=xnagamestudio.35)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.EnableDefaultLighting();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mainCamera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Pass in camera matrices
            basicEffect.Projection = mainCamera.ProjectionMatrix;
            basicEffect.View = mainCamera.ViewMatrix;
            basicEffect.World = mainCamera.WorldMatrix;

            GraphicsDevice.Clear(Color.MediumPurple);

            foreach (VertexBuffer v in triangleMesh.VertexBuffers)
            {
                GraphicsDevice.SetVertexBuffer(v);

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, v.VertexCount);
                }
            }

            base.Draw(gameTime);
        }
    }
}