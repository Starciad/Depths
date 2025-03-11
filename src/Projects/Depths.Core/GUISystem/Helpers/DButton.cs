using System;

namespace Depths.Core.GUISystem.Helpers
{
    internal sealed class DButton(string name, Action onClickCallback)
    {
        internal string Name => name;
        internal Action OnClickCallback => onClickCallback;
    }
}
