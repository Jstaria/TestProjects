using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Carousel
{
    private List<Texture2D> textures; // List of textures to display on the carousel
    private List<CarouselTexture> carouselTextures; // List of CarouselTextures for each texture
    private int currentIndex; // Current index of the texture being displayed
    private float rotationAngle; // Current angle of rotation for the carousel
    private float rotationSpeed = 0.005f; // Initial rotation speed
    private float rotationSpeedIncrement = 0.002f; // Speed increment when left mouse button is held
    private float rotationSpeedDecay = 0.985f; // Speed decay rate when left mouse button is released
    private float cylinderRadius = 240f; // Radius of the cylinder the carousel is placed on
    private float separationAngle; // Separation angle between textures
    private ContentManager contentManager; // ContentManager to load assets
    private bool isMouseLeftButtonDown = false; // Flag to track left mouse button state

    public Carousel(ContentManager contentManager, List<Texture2D> textures)
    {
        this.contentManager = contentManager;
        this.textures = textures;
        carouselTextures = new List<CarouselTexture>();

        // Calculate the separation angle to prevent textures from clipping
        separationAngle = MathHelper.TwoPi / textures.Count;

        // Initialize the carousel with the textures and their positions on the cylinder
        for (int i = 0; i < textures.Count; i++)
        {
            // Calculate the angle and position for each texture on the cylinder
            float angle = i * separationAngle;
            Vector3 position = new Vector3(cylinderRadius * (float)System.Math.Cos(angle), 0f, cylinderRadius * (float)System.Math.Sin(angle));
            Vector3 forward = Vector3.Normalize(-position);
            Vector3 up = Vector3.Up;

            // Create the world matrix to orient the texture as a billboard
            Matrix world = Matrix.CreateWorld(position, forward, up);
            carouselTextures.Add(new CarouselTexture(textures[i], world));
        }

        currentIndex = 0; // Initialize the current index to 0
        rotationAngle = 0f; // Initialize the rotation angle to 0
    }

    public void Update(GameTime gameTime)
    {
        HandleInput(); // Handle player interaction for carousel rotation

        // Apply rotation to the carousel based on the elapsed time and rotation speed
        rotationAngle += rotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        // Wrap the rotation angle around to keep it within 0 to 2π range
        if (rotationAngle >= MathHelper.TwoPi)
            rotationAngle -= MathHelper.TwoPi;
    }

    private void HandleInput()
    {
        // Get the current mouse state
        MouseState mouseState = Mouse.GetState();

        // Check if the left mouse button is down
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            // Set the flag to indicate the left mouse button is held down
            isMouseLeftButtonDown = true;

            // Increase rotation speed when the left mouse button is held down
            rotationSpeed += rotationSpeedIncrement;
        }
        else
        {
            // Left mouse button is released, slow down rotation speed
            isMouseLeftButtonDown = false;
            rotationSpeed *= rotationSpeedDecay;
        }
    }

    public void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix, Vector3 cameraPosition)
    {
        // Update the orientation of each texture on the carousel based on the rotation angle
        foreach (var carouselTexture in carouselTextures)
        {
            // Calculate the angle and position of the texture based on rotation
            float angle = rotationAngle + carouselTextures.IndexOf(carouselTexture) * separationAngle;
            Vector3 position = new Vector3(cylinderRadius * (float)System.Math.Cos(angle), 0f, cylinderRadius * (float)System.Math.Sin(angle));
            Matrix world = Matrix.CreateBillboard(position, cameraPosition, Vector3.Up, null);

            carouselTexture.World = world; // Update the texture's world matrix
        }

        // Set the rendering states for 3D rendering
        graphicsDevice.BlendState = BlendState.AlphaBlend;
        graphicsDevice.DepthStencilState = DepthStencilState.Default;

        // Draw each texture on the carousel using billboard orientation
        foreach (var carouselTexture in carouselTextures)
        {
            DrawCarouselTexture(graphicsDevice, viewMatrix, projectionMatrix, carouselTexture);
        }
    }

    private void DrawCarouselTexture(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix, CarouselTexture carouselTexture)
    {
        // Get the quad vertices for the texture
        VertexPositionTexture[] vertices = GetQuadVertices(carouselTexture.Texture);

        // Load the TexturedEffect shader
        Effect effect = LoadTexturedEffect();

        // Set the texture and transformation matrices in the shader
        effect.CurrentTechnique = effect.Techniques["Textured"];
        effect.Parameters["World"].SetValue(carouselTexture.World);
        effect.Parameters["View"].SetValue(viewMatrix);
        effect.Parameters["Projection"].SetValue(projectionMatrix);
        effect.Parameters["Texture"].SetValue(carouselTexture.Texture);

        // Apply the shader effect and draw the textured quad
        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
        }
    }

    private VertexPositionTexture[] GetQuadVertices(Texture2D texture)
    {
        // Calculate the quad vertices based on the texture size and aspect ratio
        float aspectRatio = (float)texture.Width / texture.Height;
        float halfWidth = 50f; // Adjust the width of the quad
        float halfHeight = halfWidth / aspectRatio;

        // Define the quad vertices (counter-clockwise order for front-facing)
        return new VertexPositionTexture[]
        {
            new VertexPositionTexture(new Vector3(-halfWidth, -halfHeight, 0f), new Vector2(0f, 1f)),
            new VertexPositionTexture(new Vector3(halfWidth, -halfHeight, 0f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(-halfWidth, halfHeight, 0f), new Vector2(0f, 0f)),
            new VertexPositionTexture(new Vector3(halfWidth, -halfHeight, 0f), new Vector2(1f, 1f)),
            new VertexPositionTexture(new Vector3(halfWidth, halfHeight, 0f), new Vector2(1f, 0f)),
            new VertexPositionTexture(new Vector3(-halfWidth, halfHeight, 0f), new Vector2(0f, 0f))
        };
    }

    private Effect LoadTexturedEffect()
    {
        // Load the TexturedEffect.fx shader using the ContentManager
        return contentManager.Load<Effect>("TexturedEffect");
    }
}

public class CarouselTexture
{
    public Texture2D Texture { get; }
    public Matrix World { get; set; }

    public CarouselTexture(Texture2D texture, Matrix world)
    {
        Texture = texture;
        World = world;
    }
}
