using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTroll.Extentions
{
    static class Strings
    {
        public static bool StartsWithLowerCase(this string param)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(param))
            {
                var firstChar = param.ToCharArray().First();
                result = Char.IsLower(firstChar);
            }
            return result;
        }

        public static bool StartsWithUpperCase(this string param)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(param))
            {
                var firstChar = param.ToCharArray().First();
                result = Char.IsUpper(firstChar);
            }
            return result;
        }
    }
}
