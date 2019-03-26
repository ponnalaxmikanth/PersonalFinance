using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Conversions
    {
        static public decimal? GetDecimalValue(object val, decimal? defaultValue = null)
        {
            decimal value = 0;
            try
            {
                if (val != null && !string.IsNullOrEmpty(val.ToString()) && !decimal.TryParse(val.ToString(), out value))
                    return defaultValue;
            }
            catch (Exception ex)
            { }
            return value;
        }

        static public ulong? Getulong(object val, ulong? defaultValue = null)
        {
            ulong retValue = 0;
            try
            {
                if (val != null && !string.IsNullOrEmpty(val.ToString()) && !ulong.TryParse(val.ToString(), out retValue))
                    return defaultValue;
            }
            catch (Exception ex)
            { }
            return retValue;
        }

        static public DateTime? GetDate(object val, DateTime? defaultValue = null)
        {
            DateTime retValue = DateTime.Now;
            try
            {
                if (val != null && !string.IsNullOrEmpty(val.ToString()) && !DateTime.TryParse(val.ToString(), out retValue))
                    return defaultValue;
            }
            catch (Exception ex)
            { }
            return retValue;
        }
    }
}
