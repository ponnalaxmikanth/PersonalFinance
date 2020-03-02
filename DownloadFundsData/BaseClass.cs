using OpenQA.Selenium;
using Supremes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class BaseClass
    {
        public void LogMessage(string msg)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "] " + msg);
        }

        public Supremes.Nodes.Document GetDocument(string url)
        {
            Supremes.Nodes.Document doc = null;
            int i = 0;
            while (doc == null)
            {
                try
                {
                    doc = Dcsoup.Parse(new Uri(url), 60 * 1000);
                }
                catch (Exception ex)
                { }
                if (i++ > 10)
                    break;
            }
            return doc;
        }

        //public decimal GetDecimalValue(string value)
        //{
        //    decimal retVal = 0;
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(value))
        //        {
        //            decimal.TryParse(value, out retVal);
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    return retVal;
        //}

        //public ulong? GetUInt64(string val)
        //{
        //    ulong retValue = 0;
        //    try
        //    {
        //        if (!ulong.TryParse(val, out retValue))
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
        //    }
        //    return retValue;
        //}

        public ReadOnlyCollection<IWebElement> GetElementsByXPath(IWebDriver webDriver, string path)
        {
            int count = 0;
            ReadOnlyCollection<IWebElement> collection = null;
            do
            {
                try
                {
                    collection = webDriver.FindElements(By.XPath(path));
                    count++;
                    if (count > 1)
                        System.Threading.Thread.Sleep(1 * 1000 * 30);
                }
                catch (Exception ex) {
                    count++;
                    System.Threading.Thread.Sleep(1 * 1000 * 30);
                }
            }
            while (collection == null && count < 20);
            
            return collection;
        }

        public ReadOnlyCollection<IWebElement> GetElementsById(IWebDriver webDriver, string id)
        {
            int count = 0;
            ReadOnlyCollection<IWebElement> collection = null;
            do
            {
                try
                {
                    collection = webDriver.FindElements(By.Id(id));
                    if (count > 1)
                        System.Threading.Thread.Sleep(1 * 1000 * 30);
                    count++;
                }
                catch (Exception ex)
                {
                    count++;
                    System.Threading.Thread.Sleep(1 * 1000 * 30);
                }
            }
            while (collection == null && count < 20);

            return collection;
        }

    }
}
