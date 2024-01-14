using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Programming
{
    internal class Camera
    {
        // Camera Components
        // ==================================================================================================
        private Vector3 camTarget;
        private Vector3 camPosition;

        // Camera Matrices
        private Matrix projectionMatrix; // 3D => 2D // Basically the lens, maps 3D to 2D 
        private Matrix viewMatrix; // The camera's physical position
        private Matrix worldMatrix; // Projects object into world

        // Orbit
        private bool orbit;

        // Camera speed
        private float moveSpeed;
        // ==================================================================================================

        // Previous input
        private KeyboardState prevKBState;

        // Public Get Variables
        public Matrix ProjectionMatrix { get { return projectionMatrix; } }
        public Matrix ViewMatrix { get { return viewMatrix; } }
        public Matrix WorldMatrix { get { return worldMatrix; } }

        // Public Get/Set Variables
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        public Camera() : this(Vector3.Zero, new Vector3(0f, 0f, -100f), 60f, Game1._graphics.GraphicsDevice.DisplayMode.AspectRatio, .1f, 1000f) { }

        public Camera(Vector3 target, Vector3 position, float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
        {
            this.camTarget = target;
            this.camPosition = position;

            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, nearPlane, farPlane);
            this.viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up); // Vector3.Up = Vector3(0,1,0). Up and down can be any axis you want, this is just the norm. If a model is rotated, the Up vector needs to be rotated
            this.worldMatrix = Matrix.CreateWorld(target, Vector3.Forward, Vector3.Up); // Origin is at Vector3.Zero

            this.orbit = false;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            // Directionals

            // Getting the vector distance bewteen the camera and the target, we can get normalized forward and backward directions
            Vector3 forward = Vector3.Normalize(camTarget - camPosition);
            Vector3 backward = Vector3.Normalize(camPosition - camTarget);

            // Using cross products, we can find the left and right of the camera position
            Vector3 left = Vector3.Normalize(Vector3.Cross(viewMatrix.Up, camTarget - camPosition));
            Vector3 right = Vector3.Normalize(Vector3.Cross(camTarget - camPosition, viewMatrix.Up));

            // ================================
            // Movement
            if (kbState.IsKeyDown(Keys.A))
            {
                camPosition += left * moveSpeed;
                camTarget += left * moveSpeed;
            }
            if (kbState.IsKeyDown(Keys.D))
            {
                camPosition += right * moveSpeed;
                camTarget += right * moveSpeed;
            }
            if (kbState.IsKeyDown(Keys.W))
            {
                camPosition += forward * moveSpeed;
                camTarget += forward * moveSpeed;
            }
            if (kbState.IsKeyDown(Keys.S))
            {
                camPosition += backward * moveSpeed;
                camTarget += backward * moveSpeed;
            }
            if (kbState.IsKeyDown(Keys.Space))
            {
                camPosition += Vector3.Up * moveSpeed;
                camTarget += Vector3.Up * moveSpeed;
            }
            if (kbState.IsKeyDown(Keys.LeftShift))
            {
                camPosition += Vector3.Down * moveSpeed;
                camTarget += Vector3.Down * moveSpeed;
            }

            // Rotation
            float multiplier = .5f;

            if (kbState.IsKeyDown(Keys.Left))
            {
                camTarget = RotateAround(camPosition, camTarget, new Vector3(0, 1, 0), moveSpeed * multiplier);
            }
            if (kbState.IsKeyDown(Keys.Right))
            {
                camTarget = RotateAround(camPosition, camTarget, new Vector3(0, -1, 0), moveSpeed * multiplier);
            }
            if (kbState.IsKeyDown(Keys.Up))
            {
                camTarget = RotateAround(camPosition, camTarget, new Vector3(-1, 0, 0), moveSpeed * multiplier);
            }
            if (kbState.IsKeyDown(Keys.Down))
            {
                camTarget = RotateAround(camPosition, camTarget, new Vector3(1, 0, 0), moveSpeed * multiplier);
            }

            // ================================

            UpdateAll();
            prevKBState = kbState;
        }

        private void UpdateAll()
        {
            this.viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // Vector3.Up = Vector3(0,1,0). Up and down can be any axis you want, this is just the norm. If a model is rotated, the Up vector needs to be rotated
        }

        public static Vector3 RotateAround(Vector3 pivot, Vector3 point, Vector3 direction, float angle)
        {
            // Translate point to be relative to the pivot
            Vector3 translatedPoint = point - pivot;

            // Perform the rotation around the specified direction and angle
            Matrix rotationMatrix = Matrix.CreateFromAxisAngle(direction, MathHelper.ToRadians(angle));
            Vector3 rotatedPoint = Vector3.Transform(translatedPoint, rotationMatrix);

            // Translate the point back to its original position
            rotatedPoint += pivot;

            return rotatedPoint;
        }
    }
}
