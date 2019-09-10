using System;

namespace DataAccess
{
    public class BaseDataAccess
    {
        static public string DataStorePath = string.Empty;
        static public bool UseMockData = false;
        static public bool EnableStoreDataAsJson = false;

        public BaseDataAccess()
        {
            SetDataStorePathFromConfig();
            SetUseMockDataFlag();
        }

        static public void SetDataStorePathFromConfig()
        {
            try
            {
                DataStorePath = Utilities.ConfigManager.GetConfigValue("DataStorePath", string.Empty);
            }
            catch (Exception ex) { }
        }

        static public void SetUseMockDataFlag() {
            try {
                string value = Utilities.ConfigManager.GetConfigValue("UseMockData", string.Empty);
                if (!string.IsNullOrEmpty(value)) {
                    Boolean.TryParse(value, out UseMockData);
                }
            }
            catch (Exception ex) { }
        }

        static public void StoreDataAsJson()
        {
            try
            {
                string value = Utilities.ConfigManager.GetConfigValue("StoreDataAsJson", string.Empty);
                if (!string.IsNullOrEmpty(value))
                {
                    Boolean.TryParse(value, out EnableStoreDataAsJson);
                }
            }
            catch (Exception ex) { }
        }

    }
}
