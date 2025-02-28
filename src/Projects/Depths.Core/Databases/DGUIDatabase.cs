using Depths.Core.GUISystem;
using Depths.Core.GUISystem.Common.GUIs;
using Depths.Core.Managers;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class DGUIDatabase
    {
        private readonly Dictionary<string, DGUI> guis = [];

        internal void Initialize(DAssetDatabase assetDatabase, DGameInformation gameInformation, DGUIManager guiManager, DInputManager inputManager, DMusicManager musicManager, DTextManager textManager)
        {
            RegisterGUI(new DGameOverGUI("Game Over", assetDatabase, guiManager, gameInformation));
            RegisterGUI(new DMainMenuGUI("Main Menu", assetDatabase, gameInformation, guiManager, inputManager, musicManager, textManager));
            RegisterGUI(new DHudGUI("HUD", textManager, guiManager, gameInformation));
            RegisterGUI(new DSurfaceStatsGUI("Surface Stats", assetDatabase, textManager, guiManager, gameInformation));
            RegisterGUI(new DTruckGUI("Truck Store", assetDatabase, gameInformation, guiManager, inputManager, textManager));
            RegisterGUI(new DPlayerInformationGUI("Player Information", assetDatabase, gameInformation, guiManager, inputManager, textManager));

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
