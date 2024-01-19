using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleRayCollision
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

        // Camera speed
        private float moveSpeed;

        // Camera Rotation
        private float rotationY = 0;
        private float rotationX = 0;
        private Vector2 mouseSens = new Vector2(.01f, .01f);

        // Can Move
        private bool canMove;
        // ==================================================================================================

        // Previous input
        private KeyboardState prevKBState;
        private MouseState mouseNow;
        private Vector2 mouseDefault = new Vector2(1280/2, 720/2);

        // Public Get Variables
        public Matrix ProjectionMatrix { get { return projectionMatrix; } }
        public Matrix ViewMatrix { get { return viewMatrix; } }
        public Matrix WorldMatrix { get { return worldMatrix; } }

        // Public Get/Set Variables
        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

        public Camera() : this(Vector3.Zero, new Vector3(0f, 0f, -100f), 60f, Game1._graphics.GraphicsDevice.DisplayMode.AspectRatio, .1f, 10000f, true) { }

        public Camera(Vector3 target, Vector3 position, float fieldOfView, float aspectRatio, float nearPlane, float farPlane, bool canMove)
        {
            this.camTarget = target;
            this.camPosition = position;

            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, nearPlane, farPlane);
            this.viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up); // Vector3.Up = Vector3(0,1,0). Up and down can be any axis you want, this is just the norm. If a model is rotated, the Up vector needs to be rotated
            this.worldMatrix = Matrix.CreateWorld(target, Vector3.Forward, Vector3.Up); // Origin is at Vector3.Zero

            this.canMove = canMove;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            // Directionals

            // Getting the vector distance bewteen the camera and the target, we can get normalized forward and backward directions
            Vector3 forward = Vector3.Normalize(camTarget - camPosition);
            Vector3 backward = Vector3.Normalize(camPosition - camTarget);

            // Using cross products, we can find the left and right of the camera position
            Vector3 left = Vector3.Normalize(Vector3.Cross(Vector3.Up, camTarget - camPosition));
            Vector3 right = Vector3.Normalize(Vector3.Cross(camTarget - camPosition, Vector3.Up));

            // ================================
            // Movement
            if (canMove)
            {
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
                    camTarget = RotateAround(camPosition, camTarget, right, moveSpeed * multiplier);
                }
                if (kbState.IsKeyDown(Keys.Down))
                {
                    camTarget = RotateAround(camPosition, camTarget, left, moveSpeed * multiplier);
                }
            }
            


            UpdateAll();

            // ================================

            prevKBState = kbState;
        }

        private void UpdateAll()
        {
            this.viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up); // Vector3.Up = Vector3(0,1,0). Up and down can be any axis you want, this is just the norm. If a model is rotated, the Up vector needs to be rotated
        }

        #region NotDone
        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(rotationX) * Matrix.CreateRotationY(rotationY);

            Vector3 cameraRotatedTarget = Vector3.Transform(camTarget, cameraRotation);
            //Vector3 rotatedUpVector = Vector3.Transform(cameraUp, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(camPosition, cameraRotatedTarget, Vector3.Up);
        }

        private void MouseMovement()
        {
            Vector2 mouseOffset;
            mouseNow = Mouse.GetState();

            Vector2 MAXDELTA = new Vector2(6, 6);
            
            if (mouseNow.X != mouseDefault.X || mouseNow.Y != mouseDefault.Y)
            {
                mouseOffset = Vector2.Min(MAXDELTA, mouseDefault - new Vector2(mouseNow.X, mouseNow.Y));

                rotationX += mouseOffset.Y * mouseSens.X;
                rotationY += mouseOffset.X * mouseSens.Y;

                Mouse.SetPosition((int)mouseDefault.X, (int)mouseDefault.Y);
            }
        }

        #endregion

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
