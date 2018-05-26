using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxSimpleSysLogNetFramework;

namespace Test
{
    public class Test
    {
        private void Ini()
        {
            string server = "fs2fs.maximuss.dk";
            int port = 514;
            Syslog.Initialize(server, port, "SysLogTest");
        }

        public void TestAllLogLevel()
        {
            Ini();
            Logger.Info("Test");
            Logger.Warn("Warning");
            Logger.Emergency("Emergency");
            Logger.Info("Info");
            Logger.Debug("Debug");
            //Console.Read();
        }

        public void TestWithNewInstans()
        {
            Logger.Warn("Warning - New Instans");
        }

        public void TestException()
        {
            Ini();
            try
            {
                throw new Exception("Some random exception");
            }
            catch (Exception e)
            {
                Logger.Error("Test with exception", e);
            }
        }

        public void TestNlogLevel()
        {
            Ini();
            MaxNLog log = new MaxNLog();
            log.Info("Nlog Wrapper - Info");
            log.Debug("NLog Wrapper - Debug");
            log.Error("NLog Wrapper - Error");
        }

        public void TestJson()
        {
            Ini();
            MaxNLog log = new MaxNLog();
            log.Info("iPad: Ordre ID: 343094 | Request Method: Put | MPX Url:  | MPX Json: {\r\n  \"recipients\": [],\r\n  \"data\": {\r\n    \"order_id\": 343094,\r\n    \"route_id\": \"8093\",\r\n    \"vessel_name\": \"PETROATLANTIC\",\r\n    \"vessel_mmsi\": null,\r\n    \"vessel_imo\": 9233818,\r\n    \"vessel_length\": 233.57,\r\n    \"vessel_width\": 42.03,\r\n    \"vessel_draught\": 0.0,\r\n    \"vessel_bridge_offset\": 0.0,\r\n    \"vessel_gps_length_offset\": 0.0,\r\n    \"vessel_gps_width_offset\": 0.0,\r\n    \"vessel_gps_heading_offset\": 0.0,\r\n    \"vessel_port_registry\": \"Nassau\",\r\n    \"vessel_call_sign\": \"C6SW7\",\r\n    \"vessel_last_port\": null,\r\n    \"vessel_bound\": null,\r\n    \"route_name\": null,\r\n    \"vessel_email\": \"master.petroatlantic@Teekay.com\",\r\n    \"owner\": null,\r\n    \"invoice\": null,\r\n    \"pilot_name\": \"\",\r\n    \"assistant_pilot_1_name\": \"\",\r\n    \"assistant_pilot_2_name\": \"\",\r\n    \"assistant_pilot_3_name\": null,\r\n    \"captain_name\": \"Morris Binuhe\",\r\n    \"notes\": \"(SCH) 4 onsignere: Pascual Conde, Terence Obordo, Romeo Noveda, Dan Murillo\\r\\n2 offsignere: Leyson Loquias, Francis Tablin\\r\\nGodkendelse ok\",\r\n    \"pilot_email\": null,\r\n    \"vessel_gross_tonnage\": 54865,\r\n    \"piloting_start\": 1523451600,\r\n    \"piloting_end\": 1523457000,\r\n    \"piloting_mandatory\": false,\r\n    \"tug_fixed_price\": false,\r\n    \"start_time_confirmed\": false,\r\n    \"vessel_cargo_danish\": false,\r\n    \"vessel_extreme_width\": 42.03\r\n  }\r\n}");
        }

        public void TestSeveralServers()
        {
            string server = "fs2fs.maximuss.dk";
            int port = 514;

            IList<SyslogInit> syslogInits = new List<SyslogInit>();
            SyslogInit syslogInit = new SyslogInit();
            syslogInit.HostName = server;
            syslogInit.Port = port;
            syslogInit.AppName = "Server_1";
            syslogInits.Add(syslogInit);

            syslogInit = new SyslogInit();
            syslogInit.HostName = server;
            syslogInit.Port = port;
            syslogInit.AppName = "Server_2";
            syslogInits.Add(syslogInit);

            Syslog.Initialize(syslogInits);

            Logger.Info("Test with several instans (in this case 2)");
        }

        public void TestWriteSyslogInitFile()
        {
            string server = "fs2fs.maximuss.dk";
            int port = 514;

            IList<SyslogInit> syslogInits = new List<SyslogInit>();
            SyslogInit syslogInit = new SyslogInit();
            syslogInit.HostName = server;
            syslogInit.Port = port;
            syslogInit.AppName = "Server_1";
            syslogInits.Add(syslogInit);

            syslogInit = new SyslogInit();
            syslogInit.HostName = server;
            syslogInit.Port = port;
            syslogInit.AppName = "Server_2";
            syslogInits.Add(syslogInit);

            Syslog.Initialize(syslogInits);
            Syslog.WriteToFile();
        }

        public void TestInitializeEmptyInit()
        {
            Syslog.Initialize();
            Logger.Info("Initialize from a json file.");
        }

        public void TestInfoWithoutJsonInitFile()
        {
            Syslog.DeleteJsonInitFile();
            Ini();
            Logger.Info("Logging test without the json file");
        }


    }
}
