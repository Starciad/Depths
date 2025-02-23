using Depths.Core.Interfaces.Managers;
using Depths.Core.Mathematics.Primitives;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.Managers
{
    internal sealed class DCameraManager
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Zoom
        {
            get => this.zoom;
            set
            {
                if (value < this.MinimumZoom || value > this.MaximumZoom)
                {
                    throw new ArgumentException("Zoom must be between MinimumZoom and MaximumZoom");
                }

                this.zoom = value;
            }
        }
        public float MinimumZoom
        {
            get => this.minimumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MinimumZoom must be greater than zero");
                }

                if (this.Zoom < value)
                {
                    this.Zoom = this.MinimumZoom;
                }

                this.minimumZoom = value;
            }
        }
        public float MaximumZoom
        {
            get => this.maximumZoom;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MaximumZoom must be greater than zero");
                }

                if (this.Zoom > value)
                {
                    this.Zoom = value;
                }

                this.maximumZoom = value;
            }
        }

        private float maximumZoom = float.MaxValue;
        private float minimumZoom;
        private float zoom;

        private readonly IDGraphicsManager graphicsManager;

        public DCameraManager(IDGraphicsManager graphicsManager)
        {
            this.graphicsManager = graphicsManager;
            Reset();
        }

        public void Move(Vector2 direction)
        {
            this.Position += Vector2.Transform(direction, Matrix.CreateRotationZ(-this.Rotation));
        }

        public void Rotate(float deltaRadians)
        {
            this.Rotation += deltaRadians;
        }

        public void ZoomIn(float deltaZoom)
        {
            ClampZoom(this.Zoom + deltaZoom);
        }

        public void ZoomOut(float deltaZoom)
        {
            ClampZoom(this.Zoom - deltaZoom);
        }

        public void ClampZoom(float value)
        {
            this.Zoom = value < this.MinimumZoom ? this.MinimumZoom : value > this.MaximumZoom ? this.MaximumZoom : value;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = this.graphicsManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = this.graphicsManager.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        public Matrix GetViewMatrix()
        {
            return GetVirtualViewMatrix() * Matrix.Identity;
        }

        private Matrix GetVirtualViewMatrix()
        {
            return Matrix.CreateTranslation(new(-this.Position.X, this.Position.Y, 0.0f)) *
                   Matrix.CreateTranslation(new(-this.Origin, 0.0f)) *
                   Matrix.CreateRotationZ(this.Rotation) *
                   Matrix.CreateScale(this.Zoom, this.Zoom, 1) *
                   Matrix.CreateTranslation(new(this.Origin, 0.0f));
        }

        public bool InsideCameraBounds(Vector2 targetPosition, DSize2 targetSize, bool inWorldPosition, float toleranceFactor = 0f)
        {
            Vector2 topLeft = targetPosition;
            Vector2 bottomRight = targetPosition + new Vector2(targetSize.Width, targetSize.Height);

            topLeft -= new Vector2(toleranceFactor);
            bottomRight += new Vector2(toleranceFactor);

            Vector2 screenTopLeft = topLeft;
            Vector2 screenBottomRight = bottomRight;

            if (inWorldPosition)
            {
                screenTopLeft = WorldToScreen(topLeft);
                screenBottomRight = WorldToScreen(bottomRight);
            }

            // =========================== //

            // IN-GAME | FINAL
            Viewport viewport = this.graphicsManager.Viewport;

            // =========================== //

            return screenBottomRight.X >= 0 && screenTopLeft.X < viewport.Width &&
                   screenBottomRight.Y >= 0 && screenTopLeft.Y < viewport.Height;
        }

        public void Reset()
        {
            this.Rotation = 0;
            this.Zoom = 1;
            this.Origin = new Vector2(this.graphicsManager.Viewport.Width, this.graphicsManager.Viewport.Height) / 2f;
            this.Position = Vector2.Zero;
        }
    }
}
