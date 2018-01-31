using System;
using System.Diagnostics;
using MaxSimpleSyslog;
using SimpleSyslog;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void WithTagTest()
        {
            Syslog.Initialize("192.168.0.2", 514,"tag");
            Logger.Emergency("With Tag");
            
        }

        [Fact]
        public void WithoutTagTest()
        {
            Syslog.Initialize("192.168.0.2", 514);
            Logger.Emergency("Without Tag");
        }
    }
}
