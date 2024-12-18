using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenDMS.Domain
{
    public static class StringExtensions
    {
        public static string Quoted(this string text)
        {
            return text is null ? "NULL" :  "'" + text.Replace("'", "''") + "'";
        }
    }
}
