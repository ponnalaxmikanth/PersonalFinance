using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BaseDataAccess
    {
        static string serverPath = string.Empty;

        public void SetPath(string path)
        {
            serverPath = path;
        }
    }
}
