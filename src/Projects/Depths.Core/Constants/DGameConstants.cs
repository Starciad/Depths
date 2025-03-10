using System;

namespace Depths.Core.Constants
{
    internal static class DGameConstants
    {
        internal static Version VERSION => new(1, 0, 0, 0);

        internal const string TITLE = "Depths";
        internal const string AUTHOR = "Davi \"Starciad\" Fernandes";

        internal static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
