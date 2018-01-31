# MaxSimpleSylog framework

### The goal is to provide at small and lightweight framework that support Syslog.

The project is build on .NET core v2

I builded it with Raspberry PI 3 with Windows Iot installed. I needed a small lightweight framework that i could use for logging to a syslog server.

### Usage
Downlod the zip file or clone the repository.

Start with initialize the logging, it's a static class so only do it once.
 
	Syslog.Initialize("192.168.0.2", 514,"tag");
	
The complete signature for initialize is:

	public static void Initialize(string hostName, int port, string appName = "", Facility facility = Facility.User)
	
**Facility** is a static class with all the log level.
 
For logging you just use:

	Logger.Emergency("With Tag");
	
Notice that if you dont supply it with a **tag** then is use the assembly, class name and method in the **tag** section.
	


