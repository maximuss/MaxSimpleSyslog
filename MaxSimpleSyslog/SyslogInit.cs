using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxSimpleSyslog;

namespace MaxSimpleSysLogNetFramework
{
    public class SyslogInit
    {
        /// <summary>
        /// Default facility is Facility.User
        /// </summary>
        public Facility Facility { get; set; }
        /// <summary>
        /// URL og IP to the syslog server
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// The Port the syslog server listen on
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// The default value og AppName is string.empty
        /// </summary>
        public string AppName { get; set; }
        public readonly int VERSION = 1;

        public SyslogInit()
        {
            Facility = Facility.User;
            AppName = string.Empty;
        }
    }
}
