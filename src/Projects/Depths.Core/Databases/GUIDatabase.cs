using Depths.Core.GUISystem;
using Depths.Core.GUISystem.Common.GUIs;
using Depths.Core.Interfaces.General;
using Depths.Core.Managers;

using System.Collections.Generic;

namespace Depths.Core.Databases
{
    internal sealed class GUIDatabase : IResettable
    {
        private readonly Dictionary<string, GUI> guis = [];

        internal void Initialize(AssetDatabase assetDatabase, GameInformation gameInformation, GUIManager guiManager, InputManager inputManager, MusicManager musicManager, ShopDatabase shopDatabase, TextManager textManager)
        {
            RegisterGUI(new GameOverGUI("Game Over", assetDatabase, guiManager, gameInformation));
            RegisterGUI(new MainMenuGUI("Main Menu", assetDatabase, gameInformation, guiManager, inputManager, musicManager, textManager));
            RegisterGUI(new HudGUI("HUD", textManager, guiManager, gameInformation));
            RegisterGUI(new SurfaceStatsGUI("Surface Stats", assetDatabase, textManager, guiManager, gameInformation));
            RegisterGUI(new TruckGUI("Truck Store", assetDatabase, gameInformation, guiManager, inputManager, shopDatabase, textManager));
            RegisterGUI(new PlayerInformationGUI("Player Information", assetDatabase, gameInformation, guiManager, inputManager, textManager));
            RegisterGUI(new VictoryGUI("Victory", assetDatabase, gameInformation, guiManager));
            RegisterGUI(new CreditsGUI("Credits", assetDatabase, gameInformation, guiManager, inputManager, musicManager, textManager));
            RegisterGUI(new PauseGUI("Pause", assetDatabase, gameInformation, guiManager, inputManager, textManager));

            foreach (GUI gui in this.guis.Values)
            {
                gui.Initialize();
            }
        }

        private void RegisterGUI(GUI gui)
        {
            this.guis.Add(gui.Identifier, gui);
        }

        internal GUI GetGUIByIdentifier(string identifier)
        {
            return this.guis[identifier];
        }

        public void Reset()
        {
            foreach (GUI gui in this.guis.Values)
            {
                gui.Reset();
            }
        }
    }
}
