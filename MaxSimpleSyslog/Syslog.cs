using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using MaxSimpleSyslog;

namespace SimpleSyslog
{
    public static class Syslog
    {
        private static readonly UdpClient UdpClient = new UdpClient();
        private static Facility _facility;
        private static string _hostName;
        private static int _port;
        private static string _appName;
        private static readonly int VERSION = 1;

        public static void Initialize(string hostName, int port, string appName = "", Facility facility = Facility.User)
        {
            _hostName = hostName;
            _port = port;
            _facility = facility;
            _facility = facility;
            _appName = appName;
        }

        internal static void Send(Severity severity, string message, Exception e)
        {
            if (string.IsNullOrWhiteSpace(_hostName) || _port == 0)
                return;
            var constructMessage = ConstructMessage(severity, _facility, message, _appName,e);
            UdpClient.SendAsync(constructMessage, constructMessage.Length, _hostName, _port);
        }

        private static byte[] ConstructMessage(Severity level, Facility facility, string message, string tag, Exception e)
        {
            int prival = ((int)facility) * 8 + ((int)level);
            string pri = string.Format("<{0}>", prival);
            string timestamp =
                new DateTimeOffset(DateTime.Now, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)).ToString("yyyy-MM-ddTHH:mm:ss");

            string hostname = string.Empty;
            //found that this code doesn't work on all machines; enclosed in a try block
            try
            {
                hostname = Dns.GetHostEntry(Environment.UserDomainName).HostName;
            }
            catch
            {
                hostname = "unknown";
            }

            if (string.IsNullOrEmpty(tag))
            {
                tag = ClassNameAndMethod(true);
            }
            else
            {
                message = ClassNameAndMethod(false) + message;
            }



            string header = $"{pri}{VERSION} {timestamp} {hostname} {tag}";

            List<byte> syslogMsg = new List<byte>();
            syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(header));
            if (message != "")
            {
                // Lampe: took out "BOM" prefix
                //syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(" BOM"));
                syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(" "));
                syslogMsg.AddRange(System.Text.Encoding.UTF8.GetBytes(message));
            }

            var array = syslogMsg.ToArray();
            return array;
        }

        private static string ClassNameAndMethod(bool isTag)
        {
            StackTrace st = new StackTrace();
            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);
                var module = sf.GetMethod().Module.Name;
                if(module == null)
                    continue;

                if (module.Contains("MaxSimpleSyslog"))
                    continue;

                var className = sf.GetMethod().DeclaringType.FullName;
                var methodName = sf.GetMethod().Name;
                if (!isTag)
                {
                    return $"[{className}.{methodName}] - ";
                }

                return $"{className}.{methodName} ";
            }

            return String.Empty;
        }

    }
}