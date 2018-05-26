using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using MaxSimpleSyslog;
using Newtonsoft.Json;

namespace MaxSimpleSysLogNetFramework
{
    public class Syslog
    {
        private static readonly UdpClient UdpClient = new UdpClient();
        private static Facility _facility;
        private static string _hostName;
        private static int _port;
        private static string _appName;
        private static readonly int VERSION = 1;

        private static IList<SyslogInit> _syslogInits = null;

        /// <summary>
        /// Initialize for one server
        /// </summary>
        /// <param name="hostName">Hostname of the syslog server</param>
        /// <param name="port">The port the server is listening too</param>
        /// <param name="appName">Name of the app. If blank then a empty string will be used</param>
        /// <param name="facility">The default Facility, if blank then will Facility.User be used</param>
        public static void Initialize(string hostName, int port, string appName = "", Facility facility = Facility.User)
        {
            _hostName = hostName;
            _port = port;
            _facility = facility;
            _appName = appName;
        }

        /// <summary>
        /// Initialize with one or more servers
        /// </summary>
        /// <param name="syslogInits">IList of class SyslogInit</param>
        public static void Initialize(IList<SyslogInit> syslogInits)
        {
            _syslogInits = syslogInits;
        }

        /// <summary>
        /// Try to initialize out from sysloginit.json
        /// </summary>
        public static void Initialize()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText($"{currentDirectory}//sysloginit.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                _syslogInits = (IList<SyslogInit>)serializer.Deserialize(file, typeof(List<SyslogInit>));
                if (_syslogInits == null)
                {
                    throw new FileLoadException("There is no file named sysloginit.json or the file is empty");
                }
            }
        }

        public static void WriteToFile()
        {
            if (_syslogInits != null)
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                try
                {
                    using (StreamWriter file = File.CreateText($"{currentDirectory}//sysloginit.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, _syslogInits);
                    }

                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            else
            {
                throw new Exception("There is no entries in SysLogInits. Not possible to write syslog init file.");
            }
            
        }


        internal void Send(Severity severity, string message, Exception e)
        {
            //If _syslogInits is null, first try to initialize it wit a empty constructor. It is possible that there is a init json file created, but the user
            //have decided not to init it everytime.
            if(_syslogInits == null)
                Initialize();

            //If the _syslogInits not is empty use that info
            if (_syslogInits != null)
            {
                foreach (var syslogInit in _syslogInits)
                {
                    if(string.IsNullOrWhiteSpace(syslogInit.HostName) || syslogInit.Port == 0)
                        throw new Exception("Hostname and port can not be empty or null");

                    SendMessage(severity,syslogInit.Facility,message,syslogInit.AppName,e,syslogInit.HostName,syslogInit.Port);
                }
            }
            else
            {
                //Otherwise send it with data supplied throught the initialize with "manually" data
                if (string.IsNullOrWhiteSpace(_hostName) || _port == 0)
                    throw new Exception("Hostname and port can not be empty or null");

                SendMessage(severity,_facility,message,_appName,e,_hostName,_port);
            }
        }

        private void SendMessage(Severity severity, Facility facility, string message, string appName, Exception e, string hostName, int port)
        {
            var constructMessage = ConstructMessage(severity, facility, message, appName, e);
            UdpClient.SendAsync(constructMessage, constructMessage.Length, hostName, port);
            NLogLogger.Log(severity, message, ClassNameAndMethod(false), e);

        }

        private byte[] ConstructMessage(Severity level, Facility facility, string message, string tag, Exception e)
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

            if (e != null)
            {
                message = $"[{message}]{Environment.NewLine} {Environment.NewLine} {e}";
            }

            string header = $"{pri}{VERSION} {timestamp} {hostname} {tag}";

            List<byte> syslogMsg = new List<byte>();
            syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(header));
            if (message != "")
            {
                message = message.Replace($"\r\n", Environment.NewLine);
                syslogMsg.AddRange(Encoding.ASCII.GetBytes(" "));
                syslogMsg.AddRange(Encoding.UTF8.GetBytes(message));
            }

            var array = syslogMsg.ToArray();
            return array;
        }

        private string ClassNameAndMethod(bool isTag)
        {
            var assemblyName =Assembly.GetExecutingAssembly().FullName;

            StackTrace st = new StackTrace();
            for (int i = 0; i < st.FrameCount; i++)
            {
                // Note that high up the call stack, there is only
                // one stack frame.
                StackFrame sf = st.GetFrame(i);
                var module = sf.GetMethod().Module.Name;
                if(module == null)
                    continue;

                module = module.Replace(".dll", string.Empty);

                if (assemblyName.Contains(module))
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