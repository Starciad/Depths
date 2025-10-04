using System;

namespace Depths.Core.Constants
{
    public static class GameConstants
    {
        public const string TITLE = "Depths";

        internal static readonly Version VERSION = new(2, 0, 0, 0);
        internal const string AUTHOR = "Davi \"Starciad\" Fernandes";

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
