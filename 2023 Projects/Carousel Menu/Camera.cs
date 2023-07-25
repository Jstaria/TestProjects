using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carousel_Menu
{
    // Camera.cs

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Camera
    {
        // Camera position in 3D space
        public Vector3 Position { get; private set; }

        // Camera's view matrix for rendering
        public Matrix ViewMatrix { get; private set; }

        // Camera's projection matrix for rendering
        public Matrix ProjectionMatrix { get; private set; }

        // Rotation angle around the Y-axis (yaw)
        private float yaw;

        // Rotation angle around the X-axis (pitch)
        private float pitch;

        // Camera rotation speed
        private float rotationSpeed = 0.01f;

        // Camera movement speed
        private float moveSpeed = 5.0f;

        // Create a new camera with the specified viewport
        public Camera(Viewport viewport)
        {
            Position = new Vector3(0f, 0f, 200f);
            yaw = 0f;
            pitch = 0f;

            float aspectRatio = (float)viewport.Width / viewport.Height;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.1f, 1000f);
        }

        // Update the camera
        public void Update()
        {
            HandleInput();
            UpdateViewMatrix();
        }

        // Handle input to control camera movement and rotation
        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            // Rotation control
            yaw -= rotationSpeed * gamePadState.ThumbSticks.Right.X;
            pitch += rotationSpeed * gamePadState.ThumbSticks.Right.Y;

            // Clamping the pitch to prevent flipping
            pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f);

            // Movement control
            Vector3 moveVector = new Vector3(-gamePadState.ThumbSticks.Left.X, 0f, gamePadState.ThumbSticks.Left.Y);
            moveVector += new Vector3(keyboardState.IsKeyDown(Keys.A) ? -1f : 0f, 0f, keyboardState.IsKeyDown(Keys.W) ? -1f : 0f);
            moveVector += new Vector3(keyboardState.IsKeyDown(Keys.D) ? 1f : 0f, 0f, keyboardState.IsKeyDown(Keys.S) ? 1f : 0f);

            if (moveVector != Vector3.Zero)
                moveVector.Normalize();

            // Apply movement and rotation to the camera position
            Position += Vector3.Transform(moveVector * moveSpeed, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f));
        }

        // Update the view matrix based on the current position and orientation
        private void UpdateViewMatrix()
        {
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);
            Vector3 cameraForward = Vector3.Transform(Vector3.Forward, rotationMatrix);
            Vector3 cameraTarget = Position + cameraForward;

            Vector3 cameraUp = Vector3.Transform(Vector3.Up, rotationMatrix);

            ViewMatrix = Matrix.CreateLookAt(Position, cameraTarget, cameraUp);
        }

        // Get the camera transformation matrix (used for SpriteBatch rendering)
        public Matrix TransformMatrix => Matrix.CreateTranslation(-Position);
    }

}
