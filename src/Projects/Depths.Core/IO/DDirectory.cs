using Depths.Core.Constants;

using System;
using System.IO;

namespace Depths.Core.IO
{
    public static class DDirectory
    {
        internal static string Local => AppDomain.CurrentDomain.BaseDirectory;
        internal static string Logs => Path.Combine(Local, DDirectoryConstants.LOGS_LOCAL);

        public static void Initialize()
        {
            // Local
            _ = Directory.CreateDirectory(Logs);
        }
    }
}
