using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public static class DbTypeConvertor
    {

        private const char LTR_EMBED = '\u202B';
        private const char POP_DIRECTIONAL = '\u202C';

        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        public static bool? ToNullableBool(this string s)
        {
            bool i;
            if (bool.TryParse(s, out i)) return i;
            return null;
        }

        public static string StringRTL(string inputStr)
        {
            return LTR_EMBED + inputStr + POP_DIRECTIONAL;
        }
    }
}
