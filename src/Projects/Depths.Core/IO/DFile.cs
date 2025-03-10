using Depths.Core.Constants;

using System;
using System.IO;

namespace Depths.Core.IO
{
    internal static class DFile
    {
        internal static string WriteException(Exception exception)
        {
            string logFileName = string.Concat(DGameConstants.TITLE, "log", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), ".txt").ToLower();
            string logFilePath = Path.Combine(DDirectory.Logs, logFileName);

            using StringWriter stringWriter = new();

            stringWriter.WriteLine(exception.ToString());
            File.WriteAllText(logFilePath, stringWriter.ToString());

            return logFilePath;
        }
    }
}
