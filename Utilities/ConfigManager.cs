using System;
using System.Configuration;

namespace Utilities
{
    static public class ConfigManager
    {
        static public string GetConfigValue(string key, string defaultValue) {
            string returnValue = defaultValue;
            try
            {
                returnValue = ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex) {
            }
            return returnValue;
        }
    }
}
