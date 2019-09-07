using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utilities
{
    public class WriteToFile
    {
        readonly string _application = "Utilities";
        readonly string _component = "WriteToFile";
        static public void Write(string filePath, string text) {
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch (Exception ex) {
                //DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }
    }
}
