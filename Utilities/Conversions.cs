using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static decimal? ToDecimal(object param)
        {
            decimal retValue = decimal.MinValue;
            try
            {
                if (param == null) return null;
                if (decimal.TryParse(param.ToString(), out retValue)) return retValue;
                else return null;
            }
            catch (Exception ex)
            {
            }
            return retValue;
        }

        public static decimal ToDecimal(object param, decimal defaultValue)
        {
            decimal retValue = defaultValue;
            try
            {
                if (param == null) return defaultValue;
                decimal.TryParse(param.ToString(), out retValue);
            }
            catch (Exception ex)
            {
            }
            return retValue;
        }

        public static double ToDouble(object param, double defaultValue)
        {
            double retValue = defaultValue;
            try
            {
                if (param == null) return defaultValue;
                double.TryParse(param.ToString(), out retValue);
            }
            catch (Exception ex)
            {
            }
            return retValue;
        }

        static public DateTime ToDateTime(object val, DateTime defaultValue)
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

        public static string DataTableToJSON(DataTable table)
        {
            string JSONString = string.Empty;
            try
            {
                JSONString = JsonConvert.SerializeObject(table);
            }
            catch (Exception ex) {
            }
            return JSONString;
        }

        public static string DataSetToJSON(DataSet dataSet)
        {
            string JSONString = string.Empty;
            try
            {
                JSONString = JsonConvert.SerializeObject(dataSet);
            }
            catch (Exception ex)
            {
            }
            return JSONString;
        }

        public static DataSet JSONToDataSet(string jsonString) {
            DataSet resultDataSet = null;
            try {
                resultDataSet = JsonConvert.DeserializeObject<DataSet>(jsonString);
            }
            catch (Exception ex)
            {
            }
            return resultDataSet;
        }

        public static int ToInt(object param, int defaultValue) {
            int retValue = defaultValue;
            try
            {
                if (param == null) return defaultValue;
                int.TryParse(param.ToString(), out retValue);
            }
            catch (Exception ex)
            {
            }
            return retValue;
        }

        public static string ToString(object param, string defaultValue)
        {
            string retValue = defaultValue;
            try
            {
                if (param == null) return defaultValue;
                //int.TryParse(param.ToString(), out retValue);
                retValue = string.IsNullOrWhiteSpace(param.ToString()) ? defaultValue : param.ToString();
            }
            catch (Exception ex)
            {
            }
            return retValue;
        }

    }
}
