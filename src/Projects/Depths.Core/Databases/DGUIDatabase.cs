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
            RegisterGUI(new DHudGUI("HUD", textManager, guiManager, gameInformation));
            RegisterGUI(new DSurfaceStatsGUI("Surface Stats", assetDatabase, textManager, guiManager, gameInformation));
            RegisterGUI(new DGameOverGUI("Game Over", assetDatabase, guiManager, gameInformation));
            RegisterGUI(new DGameStatsGUI("Game Stats", guiManager, gameInformation));

            foreach (DGUI gui in this.guis.Values)
            {
                gui.Initialize();
            }
        }

        private void RegisterGUI(DGUI gui)
        {
            this.guis.Add(gui.Identifier, gui);
        }

        internal DGUI GetGUIByIdentifier(string identifier)
        {
            return this.guis[identifier];
        }
    }
}
