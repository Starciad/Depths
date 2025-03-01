using Depths.Core.Databases;
using Depths.Core.GUISystem;
using Depths.Core.Interfaces.General;

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
            for (byte i = 0; i < this.openGuis.Count; i++)
            {
                DGUI gui = this.openGuis[i];

                if (gui == null)
                {
                    continue;
                }

                gui.Update();
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            for (byte i = 0; i < this.openGuis.Count; i++)
            {
                DGUI gui = this.openGuis[i];

                if (gui == null)
                {
                    continue;
                }

                gui.Draw(spriteBatch);
            }
        }

        internal void Open(string identifier)
        {
            DGUI gui = this.guiDatabase.GetGUIByIdentifier(identifier);

            if (this.openGuis.Contains(gui))
            {
                return;
            }

            gui.Load();
            this.openGuis.Add(gui);
        }

        internal void Close(string identifier)
        {
            DGUI gui = this.guiDatabase.GetGUIByIdentifier(identifier);

            gui.Unload();
            _ = this.openGuis.Remove(gui);
        }
    }
}
