using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Depths.Core.Interfaces.Managers
{
    internal interface IDGraphicsManager
    {
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        GraphicsDevice GraphicsDevice { get; }
        Viewport Viewport { get; }
    }
}
