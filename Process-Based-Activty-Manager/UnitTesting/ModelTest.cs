using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ActivityTracker;
using System.Data.SQLite;
using System.IO;

namespace UnitTesting
{
    [TestClass]
    public class ModelTest
    {
        public static DatabaseManager _database;

        [ClassInitialize()]
        public static void InitializeDatabase(TestContext context)
        {
            // default location: Process-Based-Activty-Manager\Process-Based-Activty-Manager\UnitTesting\bin\Debug
            _database = DatabaseManager.Instance("databaseTEST.sqlite3");
        }

        [TestMethod]
        public void TestDatabaseFileExistence()
        {
            string fileName = "./databaseTEST.sqlite3";
            Assert.IsTrue(File.Exists(fileName));
        }

        [TestMethod]
        public void TestDatabaseExistence()
        {
            Assert.AreNotEqual(_database, null);
        }

        [TestMethod]
        public void TestAddNewTimeSlot()
        {
            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            Assert.AreEqual(timestamp, _database.AddNewTimeSlot(""));
        }


        /// <summary>
        /// Pentru baza de date, in cazul in care e goala, nu ar avea ce sa obtina oricum, deci nu are sens sa testez situatia de egalite
        /// </summary>
        [TestMethod]
        public void TestGetProcessFromName()
        {
            try
            {
                _database.GetProcessFromName("");
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetProcessFromName(), but got: " + exception.Message);
            }
        }

        [TestMethod]
        public void TestGetProcesses()
        {
            try
            {
                _database.GetProcesses();
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetProcesses(), but got: " + exception.Message);
            }
        }

        [TestMethod]
        public void TestGetProcessesNames()
        {
            try
            {
                _database.GetProcessesNames();
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetProcessesNames(), but got: " + exception.Message);
            }
        }       
        
        [TestMethod]
        public void TestGetTotalTimeForProcess()
        {
            try
            {
                _database.GetTotalTimeForProcess("");
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetTotalTimeForProcess(), but got: " + exception.Message);
            }
        }        

        [TestMethod]
        public void TestGetTimeSlotsForProcess()
        {
            try
            {
                _database.GetTimeSlotsForProcess("");
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetTimeSlotsForProcess(), but got: " + exception.Message);
            }
        }

    }
}
