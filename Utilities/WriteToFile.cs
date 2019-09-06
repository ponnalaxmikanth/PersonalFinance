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
        static public void Write(string filePath, string text) {
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch (Exception ex) {
            }
        }
    }
}
