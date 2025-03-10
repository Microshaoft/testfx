﻿using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OutputTestProject
{

    [TestClass]
    public class UnitTest1
    {
        private static readonly Random rng = new Random();

        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void ClassInitialize(TestContext _)
        {
            WriteLines("UnitTest1 - ClassInitialize");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            WriteLines("UnitTest1 - TestInitialize");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            WriteLines("UnitTest1 - TestCleanup");
        }


        [ClassCleanup()]
        public static void ClassCleanup()
        {
            WriteLines($"UnitTest1 - ClassCleanup");
        }

        [TestMethod]
        public void TestMethod1()
        {
            WriteLines("UnitTest1 - TestMethod1");
            // This makes the outputs more likely to run into each other 
            // when running in parallel.
            // It also makes the test longer, because we check in the test
            // that all tests started before any test finished (to make sure
            // they actually run in parallel), and this gives us more leeway
            // on slower machines.
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod1");
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod1");
        }

        [TestMethod]
        public void TestMethod2()
        {
            WriteLines("UnitTest1 - TestMethod2");
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod2");
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod2");
        }

        [TestMethod]
        public void TestMethod3()
        {
            WriteLines("UnitTest1 - TestMethod3");
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod3");
            Thread.Sleep(rng.Next(20, 50));
            WriteLines("UnitTest1 - TestMethod3");
        }

        private static void WriteLines(string message)
        {
            Trace.WriteLine(message);
            Console.WriteLine(message);
            Console.Error.WriteLine(message);
        }
    }
}
