using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.DataAccess.Interfaces
{
    public static class DataReaderExtensions
    {
        public static string GetString(this IDataReader reader, string fieldName)
        {
            return  reader[fieldName].ToString();

        }
        public static int GetInteger(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName].ToString();
            if (int.TryParse(value, out int intValue))
                return intValue;
            else
                return 0;
        }
        public static decimal GetDecimal(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName].ToString();
            if (decimal.TryParse(value, out decimal intValue))
                return intValue;
            else
                return 0;
        }

        public static DateTime GetDate(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName].ToString();
            if (DateTime.TryParse(value, out DateTime intValue))
                return intValue;
            else
                return DateTime.MinValue;
        }
        public static DateTime GetNumberDate(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName].ToString();
            if (DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dt))
                return dt;
            else
                return DateTime.MinValue;
        }
    }
}
