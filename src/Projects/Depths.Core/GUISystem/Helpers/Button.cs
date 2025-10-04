using System;

namespace Depths.Core.GUISystem.Helpers
{
    internal sealed class Button(string name, Action onClickCallback)
    {
        internal string Name => name;
        internal Action OnClickCallback => onClickCallback;
    }
}
