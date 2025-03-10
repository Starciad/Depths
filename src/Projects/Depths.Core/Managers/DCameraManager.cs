using Depths.Core.Constants;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace Depths.Core.Managers
{
    internal sealed class DCameraManager
    {
        internal Vector2 Position { get; set; }
        internal float Rotation { get; set; }
        internal Vector2 Origin { get; set; }
        internal float Zoom
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
        internal float MinimumZoom
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
        internal float MaximumZoom
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

        private readonly DGraphicsManager graphicsManager;

        internal DCameraManager(DGraphicsManager graphicsManager)
        {
            this.graphicsManager = graphicsManager;

            this.Rotation = 0;
            this.Zoom = 1f;
            this.Origin = new Vector2(DScreenConstants.SCREEN_WIDTH, DScreenConstants.SCREEN_HEIGHT) / 2f;
            this.Position = Vector2.Zero;
        }

        internal Vector2 WorldToScreen(Vector2 worldPosition)
        {
            Viewport viewport = this.graphicsManager.Viewport;
            return Vector2.Transform(worldPosition + new Vector2(viewport.X, viewport.Y), GetViewMatrix());
        }

        internal Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            Viewport viewport = this.graphicsManager.Viewport;
            return Vector2.Transform(screenPosition - new Vector2(viewport.X, viewport.Y), Matrix.Invert(GetViewMatrix()));
        }

        internal Matrix GetViewMatrix()
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
    }
}
