using System;
using System.Diagnostics;
using MaxSimpleSyslog;
using NUnit.Framework;
using SimpleSyslog;
using Xunit;
using Xunit.Sdk;

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

        [Fact]
        public void ExceptionTest()
        {
            Syslog.Initialize("192.168.0.2", 514);
            try
            {
                throw new TestClassException("My total wrong exception");
            }
            catch (Exception e)
            {
                Logger.Error("Exception test",e);
            }
            
        }
    }
}
