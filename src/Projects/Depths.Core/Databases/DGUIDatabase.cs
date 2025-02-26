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

        internal void Initialize(DAssetDatabase assetDatabase, DTextManager textManager, DGUIManager guiManager, DGameInformation gameInformation)
        {
            this.guis.Add("HUD", new DHudGUI(textManager, guiManager, gameInformation));
            this.guis.Add("Surface Stats", new DSurfaceStatsGUI(assetDatabase, textManager, gameInformation));

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
