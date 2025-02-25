using Depths.Core.Entities.Common;
using Depths.Core.GUISystem;
using Depths.Core.GUISystem.Common.GUIs;
using Depths.Core.Managers;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DGUIDatabase
    {
        private readonly Dictionary<string, DGUI> guis = [];

        internal void Initialize(DTextManager textManager, DPlayerEntity playerEntity)
        {
            this.guis.Add("HUD", new DHudGUI(textManager, playerEntity));

            foreach (DGUI gui in this.guis.Values)
            {
                gui.Initialize();
            }
        }

        internal DGUI GetGUIByIdentifier(string identifier)
        {
            return this.guis[identifier];
        }
    }
}
