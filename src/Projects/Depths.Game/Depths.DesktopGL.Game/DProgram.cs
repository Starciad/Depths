using Depths.Core;
using Depths.Core.IO;

using System;

namespace Depths.Game
{
    internal static class DProgram
    {
        private static DGame game;

        [STAThread]
        private static void Main()
        {
#if DEBUG
            InitializeEnvironment();
            InitializeGame();
#else
            try
            {
                InitializeEnvironment();
                InitializeGame();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            finally
            {
                if (game != null)
                {
                    game.Exit();
                    game.Dispose();
                }
            }
#endif
        }

        private static void InitializeEnvironment()
        {
            DDirectory.Initialize();
        }

        private static void InitializeGame()
        {
            game = new();
            game.Run();
        }

#if !DEBUG
        private static void HandleException(Exception value)
        {
            _ = DFile.WriteException(value);
        }
#endif
    }
}
