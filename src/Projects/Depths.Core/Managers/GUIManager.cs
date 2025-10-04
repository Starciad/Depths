using Depths.Core.Databases;
using Depths.Core.GUISystem;

using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace Depths.Core.Managers
{
    internal sealed class GUIManager
    {
        private readonly List<GUI> openGuis = [];
        private readonly GUIDatabase guiDatabase;

        internal GUIManager(GUIDatabase guiDatabase)
        {
            this.guiDatabase = guiDatabase;
        }

        internal void Update()
        {
            for (byte i = 0; i < this.openGuis.Count; i++)
            {
                GUI gui = this.openGuis[i];

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
                GUI gui = this.openGuis[i];

                if (gui == null)
                {
                    continue;
                }

                gui.Draw(spriteBatch);
            }
        }

        internal void Open(string identifier)
        {
            GUI gui = this.guiDatabase.GetGUIByIdentifier(identifier);

            if (this.openGuis.Contains(gui))
            {
                return;
            }

            gui.Load();
            this.openGuis.Add(gui);
        }

        internal void Close(string identifier)
        {
            GUI gui = this.guiDatabase.GetGUIByIdentifier(identifier);

            gui.Unload();
            _ = this.openGuis.Remove(gui);
        }
    }
}
