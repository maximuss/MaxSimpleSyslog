using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxSimpleSysLogNetFramework
{
    public class MaxNLog
    {
        public MaxNLog()
        {
            
        }

        public void Trace(string message)
        {
            Logger.Notice(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Error(string message, Exception e = null)
        {
            Logger.Error(message,e);
        }

        public void Fatal(string message, Exception e = null)
        {
            Logger.Emergency(message,e);
        }
    }
}
