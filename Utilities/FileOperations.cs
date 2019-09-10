using System;
using System.IO;

namespace Utilities
{
    public class FileOperations
    {
        readonly string _application = "Utilities";
        readonly string _component = "WriteToFile";
        static public void Write(string filePath, string text)
        {
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch (Exception ex)
            {
                //DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        static public string ReadFileContent(string filePath)
        {
            string resultString = string.Empty;
            try
            {
                resultString = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                //DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return resultString;
        }

    }
}
