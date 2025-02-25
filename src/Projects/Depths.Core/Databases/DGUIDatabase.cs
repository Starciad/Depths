using Depths.Core.GUISystem;
using Depths.Core.GUISystem.Common.GUIs;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DGUIDatabase
    {
        private readonly Dictionary<string, DGUI> guis = [];

        internal void Initialize()
        {
            this.guis.Add("HUD", new DHudGUI());
        }

        internal DGUI GetGUIByIdentifier(string identifier)
        {
            return this.guis[identifier];
        }
    }
}
