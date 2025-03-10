using System;

namespace Depths.Core.Constants
{
    public static class DGameConstants
    {
        internal static Version VERSION => new(2, 0, 0, 0);

        public const string TITLE = "Depths";
        internal const string AUTHOR = "Davi \"Starciad\" Fernandes";

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
