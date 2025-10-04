using System;

using Depths.Core.IO;

#if WINDOWS_DX
using System.Windows.Forms;
#endif

#if !DEBUG
using System.Text;
using Depths.Core.Constants;
#endif

namespace Depths.Game
{
    internal static class Program
    {
        private static Core.DGame game;

        [STAThread]
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

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
            string logFilename = DFile.WriteException(value);

            StringBuilder logString = new();
            _ = logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            _ = logString.AppendLine();
            _ = logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            _ = logString.AppendLine();
            _ = logString.AppendLine($"Exception: {value.Message}");

            _ = MessageBox.Show(logString.ToString(), $"{GameConstants.GetTitleAndVersionString()} - Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
#endif
    }
}
