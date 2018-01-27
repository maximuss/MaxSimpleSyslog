using SimpleSyslog;

namespace MaxSimpleSyslog
{
    public class Logger
    {
        readonly string _sender;

        public Logger(string sender)
        {
            _sender = sender;
        }

        internal void SendLog(Severity severity, string message, params object[] args)
        {
            message = string.Format(message, args);
            Syslog.Send(severity, _sender, message);
        }

        public void Emergency(string message, params object[] args)
        {
            SendLog(Severity.Emergency, message, args);
        }

        public void Alert(string message, params object[] args)
        {
            SendLog(Severity.Alert, message, args);
        }

        public void Critical(string message, params object[] args)
        {
            SendLog(Severity.Critical, message, args);
        }

        public void Error(string message, params object[] args)
        {
            SendLog(Severity.Error, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            SendLog(Severity.Warn, message, args);
        }

        public void Notice(string message, params object[] args)
        {
            SendLog(Severity.Notice, message, args);
        }

        public void Info(string message, params object[] args)
        {
            SendLog(Severity.Info, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            SendLog(Severity.Debug, message, args);
        }
    }
}