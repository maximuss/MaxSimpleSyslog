using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MaxSimpleSyslog;

namespace SimpleSyslog
{
    public static class Syslog
    {
        static bool _useLocalTime;
        static readonly UdpClient UdpClient = new UdpClient();
        static Facility _facility;
        static string _fixedSender;
        static string _hostName;
        static int _port;
        static string _messageFormat;

        public static void Initialize(string hostName, int port, Facility facility = Facility.User, string sender = null, bool useLocalTime = false, string messageFormat = null)
        {
            _hostName = hostName;
            _port = port;
            _fixedSender = sender;
            _facility = facility;
            _useLocalTime = useLocalTime;
            _messageFormat = messageFormat;
        }

        public static Logger Logger(this object obj)
        {
            return new Logger(obj.GetType()
                                 .Name);
        }

        static void SendLog(Severity severity, string sender, string message, params object[] args)
        {
            new Logger(sender).SendLog(severity, message, args);
        }

        public static void Emergency(string sender, object arg)
        {
            Emergency(sender, arg.ToString());
        }

        public static void Emergency(string sender, string message, params object[] args)
        {
            SendLog(Severity.Emergency, sender, message, args);
        }

        public static void Alert(string sender, object arg)
        {
            Alert(sender, arg.ToString());
        }

        public static void Alert(string sender, string message, params object[] args)
        {
            SendLog(Severity.Alert, sender, message, args);
        }

        public static void Critical(string sender, object arg)
        {
            Critical(sender, arg.ToString());
        }

        public static void Critical(string sender, string message, params object[] args)
        {
            SendLog(Severity.Critical, sender, message, args);
        }

        public static void Error(string sender, object arg)
        {
            Error(sender, arg.ToString());
        }

        public static void Error(string sender, string message, params object[] args)
        {
            SendLog(Severity.Error, sender, message, args);
        }

        public static void Warn(string sender, object arg)
        {
            Warn(sender, arg.ToString());
        }

        public static void Warn(string sender, string message, params object[] args)
        {
            SendLog(Severity.Warn, sender, message, args);
        }

        public static void Notice(string sender, object arg)
        {
            Notice(sender, arg.ToString());
        }

        public static void Notice(string sender, string message, params object[] args)
        {
            SendLog(Severity.Notice, sender, message, args);
        }

        public static void Info(string sender, object arg)
        {
            Info(sender, arg.ToString());
        }

        public static void Info(string sender, string message, params object[] args)
        {
            SendLog(Severity.Info, sender, message, args);
        }

        public static void Debug(string sender, object arg)
        {
            Debug(sender, arg.ToString());
        }

        public static void Debug(string sender, string message, params object[] args)
        {
            SendLog(Severity.Debug, sender, message, args);
        }

        internal static void Send(Severity severity, string sender, string message)
        {
            if (string.IsNullOrWhiteSpace(_hostName) || _port == 0)
                return;
            //sender = (_fixedSender ?? sender).Replace(' ', '_');
            var messageFormat = _messageFormat ?? "{message}";
            message = messageFormat.Replace("{message}", message);
            var dateTime = _useLocalTime ? DateTime.Now.ToString("s") : DateTime.UtcNow.ToString("s");
            var hostName = Dns.GetHostName();
            message = $"<{(int)_facility*8 + (int) severity}>{dateTime} {hostName} {sender} {message}";
            var bytes = Encoding.UTF8.GetBytes(message);
            UdpClient.SendAsync(bytes, bytes.Length, _hostName, _port);
        }
    }
}