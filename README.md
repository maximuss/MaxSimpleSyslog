# MaxSimpleSylog framework

### The goal is to provide at small and lightweight framework that support Syslog.

The project is build on .NET core v2

I builded it with Raspberry PI 3 with Windows Iot installed. I needed a small lightweight framework that i could use for logging to a syslog server.

### Usage
Downlod the zip file or clone the repository.

Start with initialize the logging, it's a static class so only do it once.
     Syslog.Initialize("192.168.0.2", 514,"tag");
 



