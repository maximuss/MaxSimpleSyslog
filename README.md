# MaxSimpleSyslogNetFramework
Basic the same as my MaxSimpleSyslog, but this one run on .NET Framework 4.xx 

### The goal is to provide at small and lightweight framework that support Syslog
I need a small framework that can work with a syslog server and NLog

### Usage
Downlod the zip file or clone the repository.

If there is a NLog.config file present in the assembly library, it wil automatic start using NLog.

#### Log level

The supplied log level is:
- Emergency
- Alert
- Critical
- Error
- Warning
- Notice
- Informational
- Debug


The framework also supports the log level used in Nlog and other similar frameworks 
- Emergency, Alert, Critical is equal to Fatal in NLog
- Error is equal to Error in NLog
- Warn is equal to Warn in NLog
- Info is equal to Info in NLog
- Debug is equal to Debug in NLog
- Trace is equal to Trace in NLog	


#### NLog Wrapper
It's possible to log in the same way as using NLog.

	MaxNLog log = new MaxNLog();
	log.Info("Nlog Wrapper - Info");
	log.Debug("NLog Wrapper - Debug");
	log.Error("NLog Wrapper - Error");

### Initialize
Start with initialize the logging, it's a static class so only do it once, but there is three ways to do it:
* With parameter of hostname, port etc.
  * public static void Initialize(string hostName, int port, string appName = "", Facility facility = Facility.User) 
* A IList of SyslogInits
  * public static void Initialize(IList<SyslogInit> syslogInits)syslogInits);
* Empty constructor, it will only work if there already is a sysloginit.json file.
  * public static void Initialize()

### Json syslog init file
The framework also support use of a sysloginit.json file that contains info for one or more log server.
To make the file it is possible to first use the initialization with the parameter of IList<SyslogInit> and then call Syslog.WriteToFile, that function create a json file in the assembly library.
If the ini file is present then it's not necessary to init the syslog anymore, the syslog framework will try to read the json file and use the info supplied in the file.
Ex:

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

It's also possible to delete the json syslog init file, use this method:

	Syslog.DeleteJsonInitFile();
	
**Facility** is a static class with all the provided options.
 
For logging you just use:

	Logger.Emergency("Message");
	
Notice that if you dont supply it with a **tag** then is use the assembly, class name and method in the **tag** section.
	
	
### Syslog server
I have only tested the framework on Syslog Watcher from https://syslogwatcher.com/

The framework should be working with any Syslog server, but with Syslog Watcher i can guarantee the framework is working.


