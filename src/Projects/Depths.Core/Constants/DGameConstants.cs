using System;

namespace Depths.Core.Constants
{
    public static class DGameConstants
    {
        public static Version VERSION => new(1, 0, 0, 0);

        public const string TITLE = "Depths";
        public const string AUTHOR = "Davi \"Starciad\" Fernandes";

        public static string GetTitleAndVersionString()
        {
            return string.Concat(TITLE, " - v", VERSION);
        }
    }
}
