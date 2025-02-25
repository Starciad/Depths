using Depths.Core.Databases;
using Depths.Core.GUISystem;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.Managers
{
    internal sealed class DGUIManager
    {
        private readonly List<DGUI> openGuis = [];
        private readonly DGUIDatabase guiDatabase;

        internal DGUIManager(DGUIDatabase guiDatabase)
        {
            this.guiDatabase = guiDatabase;
        }

        internal void Update()
        {
            foreach (DGUI gui in this.openGuis)
            {
                if (gui == null)
                {
                    continue;
                }

                gui.Update();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (DGUI gui in this.openGuis)
            {
                if (gui == null)
                {
                    continue;
                }

                gui.Draw(spriteBatch);
            }
        }

        internal void Open(string identifier)
        {
            this.openGuis.Add(guiDatabase.GetGUIByIdentifier(identifier));
        }

        internal void Close(string identifier)
        {
            this.openGuis.Remove(guiDatabase.GetGUIByIdentifier(identifier));
        }
    }
}
