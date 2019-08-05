using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    static class DBLogging
    {
        public static void LogException(string application, string component, string exceptionMessage, string stackTrace)
        {
            LoggingDataAccess.LogException(application, component, exceptionMessage, stackTrace);
        }


        public static void LogMetric(string message, string application, string component, double metric)
        {
            LoggingDataAccess.LogMetric(message, application, component, metric);
        }
    }
}
