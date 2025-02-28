using Depths.Core.Managers;

namespace Depths.Core.GUISystem.Common.GUIs
{
    internal sealed class DGameFinalStatsGUI : DGUI
    {
        // private readonly DGUITextElement titleTextElement;

        private readonly DGUIManager guiManager;
        private readonly DGameInformation gameInformation;

        internal DGameFinalStatsGUI(string identifier, DGUIManager guiManager, DGameInformation gameInformation) : base(identifier)
        {
            this.guiManager = guiManager;
            this.gameInformation = gameInformation;
        }

        protected override void OnBuild()
        {

        }
    }
}
