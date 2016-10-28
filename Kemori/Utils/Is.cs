using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemori.Utils
{
    static class Is
    {
        public static Boolean Int ( String s )
        {
            Int32 result;
            return Int32.TryParse ( s, out result );
        }
    }
}
