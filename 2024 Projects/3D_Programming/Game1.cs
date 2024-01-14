using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3D_Programming
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;

        // Camera
        private Camera mainCamera;

        private BasicEffect basicEffect; // Tells XNA how to draw object

        // Geometric Info
        private VertexPositionColor[] triangleVertices; // For color, VertexPositionTexture for texture mapping
        private VertexBuffer vertexBuffer; // Graphics buffer

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Using default constructor
            mainCamera = new Camera();
            mainCamera.MoveSpeed = 5f;

            // BasicEffect, very powerful shader with open source code: https://learn.microsoft.com/en-us/previous-versions/windows/silverlight/dotnet-windows-silverlight/bb195083(v=xnagamestudio.35)
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            // Create triangle
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Orange);
            triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Yellow);

            // An array of drawables we will give to the GPU
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);
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
            // Pass in camera matrices
            basicEffect.Projection = mainCamera.ProjectionMatrix;
            basicEffect.View = mainCamera.ViewMatrix;
            basicEffect.World = mainCamera.WorldMatrix;

            GraphicsDevice.Clear(Color.MediumPurple);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            // Optional
            // Turn off back face culling so we can see both sides of our created geometry
            // Keeping this on is generally how everyone else would do it, it is quite literally just to be able to use our orbiting camera
            // and see both sides of the triangle

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            // Loop through all effects in shader, apply them, and draw our shader
            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            base.Draw(gameTime);
        }
    }
}