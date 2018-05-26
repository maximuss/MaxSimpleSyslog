using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        //private readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Test test = new Test();
            //test.TestJson();
            //test.TestAllLogLevel();
            //test.TestException();
            //test.TestNlogLevel();
            //test.TestSeveralServers();
            //test.TestWithNewInstans();
            test.TestWriteSyslogInitFile();
            test.TestInitializeEmptyInit();
        }
    }
}
