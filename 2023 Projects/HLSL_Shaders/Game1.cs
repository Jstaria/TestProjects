using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HLSL_Shaders
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model model;
        Effect effect;

        Matrix viewMatrix;
        Matrix projectionMatrix;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Initialize camera settings
            Vector3 cameraPosition = new Vector3(0, 0, 5);
            Vector3 cameraTarget = Vector3.Zero;
            Vector3 cameraUpVector = Vector3.Up;
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);

            float fieldOfView = MathHelper.PiOver4;
            float aspectRatio = graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            float nearPlane = 0.1f;
            float farPlane = 1000f;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlane, farPlane);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the 3D model and the effect
            model = Content.Load<Model>("sword_basemesh"); // Replace "YourModelName" with the name of your model file
            effect = Content.Load<Effect>("Effect");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Set the rasterizer state for the 3D model (optional)
            RasterizerState rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullCounterClockwiseFace,
                FillMode = FillMode.Solid
            };
            GraphicsDevice.RasterizerState = rasterizerState;

            DrawModelWithEffect(model, Matrix.Identity, viewMatrix, projectionMatrix);

            base.Draw(gameTime);
        }

        private void DrawModelWithEffect(Model model, Matrix world, Matrix view, Matrix projection)
        {
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                }
                mesh.Draw();
            }
        }
    }
}
