using Depths.Core.Managers;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DGameStatsGUI : DGUI
    {
        private readonly DGUIManager guiManager;
        private readonly DGameInformation gameInformation;

        internal DGameStatsGUI(string identifier, DGUIManager guiManager, DGameInformation gameInformation) : base(identifier)
        {
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;
        }

        protected override void OnBuild()
        {

        }
    }
}
