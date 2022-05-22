using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ActivityTracker;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace UnitTesting
{
    /// <summary>
    /// Clasa pentru testarea unitatilor in Database
    /// </summary>
    [TestClass]
    public class ModelTest
    {
        public static DatabaseManager _database;

        /// <summary>
        /// Este necesar sa existe baza de date pentru a rula testele
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize()]
        public static void InitializeDatabase(TestContext context)
        {
            // default location: Process-Based-Activty-Manager\Process-Based-Activty-Manager\UnitTesting\bin\Debug
            _database = DatabaseManager.Instance("databaseTEST.sqlite3");
        }

        /// <summary>
        /// Se verifica existenta fisierului cu baza de date
        /// </summary>
        [TestMethod]
        public void TestDatabaseFileExistence()
        {
            string fileName = "./databaseTEST.sqlite3";
            Assert.IsTrue(File.Exists(fileName));
        }

        /// <summary>
        /// Se verifica existenta propriu-zisa a bazei de date ca instanta
        /// </summary>
        [TestMethod]
        public void TestDatabaseExistence()
        {
            Assert.AreNotEqual(_database, null);
        }

        /// <summary>
        /// Se testeaza crearea unui proces - verificand, asadar, 
        /// daca se returneaza corect din baza de data sub un format
        /// </summary>
        [TestMethod]
        public void TestAddProcess()
        {
            try
            {
                string processName = "ChildActivity";
                _database.AddProcess(processName);

                // verificam daca e null procesul - nu ar trebui, din moment ce abia l-am creat
                StoredProcess currentProcess = _database.GetProcessFromName(processName);

                Assert.AreNotEqual(null, currentProcess);
                Assert.AreEqual("id:" + processName, currentProcess.UniqueProcesID);
                Assert.AreEqual(processName, currentProcess.ProcessName);
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestAddProcess(), but got: " + exception.Message);
            }
        }

        /// <summary>
        /// Trebuie considerata situatia in care sunt introduse noi procese, daca stergerile se fac cum trebuie
        /// </summary>
        [TestMethod]
        public void TestGetProcesses()
        {
            try
            {
                // curatam tabela inainte de a testa pe o lista de procese
                _database.DeleteProcessTable();
                List<StoredProcess> userProcesses = _database.GetProcesses();

                // testam daca avem lista cu 0 elemente
                Assert.AreEqual(0, userProcesses.Count);

                // numim cateva procese
                string processName1 = "ChildActivity1";
                string processName2 = "ChildActivity2";
                string processName3 = "ChildActivity3";

                // cream o mica lista cu procese
                List<string> processNames = new List<string>();
                processNames.Add(processName1);
                processNames.Add(processName2);
                processNames.Add(processName3);
                
                // adaugam in baza de date procesele 
                foreach(string element in processNames)
                {
                    _database.AddProcess(element);
                }

                // dupa ce am adaugat procesele, updatam lista de obiecte de tip StoredProcess
                userProcesses = _database.GetProcesses();

                // vrem sa avem o lista cu cateva elemente, nu nimic
                int count = userProcesses.Count;

                // testam daca sunt mai multe elemente in lista
                Assert.AreNotEqual(0, count);

                // trebuie testat si daca s-a obtinut altceva decat count-ul corect
                Assert.AreEqual(processNames.Count, count);

                // testam apoi fiecare proces daca respecta numele original cu modificarile aferente
                for(int index = 0; index < count; ++index)
                {
                    var currentProcess = userProcesses[index];
                    var originalProcessName = processNames[index];

                    Assert.AreNotEqual(null, currentProcess);
                    Assert.AreEqual("id:" + originalProcessName, currentProcess.UniqueProcesID);
                    Assert.AreEqual(originalProcessName, currentProcess.ProcessName);
                }
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetProcesses(), but got: " + exception.Message);
            }
        }

        /// <summary>
        /// Se verifica daca procesele pe care le luam din baza de date verifica daca au caractere
        /// </summary>
        [TestMethod]
        public void TestGetProcessesNames()
        {
            try
            {
                List<string> userProcesses = _database.GetProcessesNames();

                foreach(string processName in userProcesses)
                {
                    Assert.AreNotEqual("", processName);
                }
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestGetProcessesNames(), but got: " + exception.Message);
            }
        }

        /// <summary>
        /// Testam generarea unui timestamp si verificam daca exista cu adevarat in tabela
        /// </summary>
        [TestMethod]
        public void TestAddNewTimeSlot()
        {
            try 
            {
                string processName = "HackerActivity";

                // se asuma ca nu este in tabela
                _database.AddProcess(processName);

                long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                long newTimeStamp = _database.AddNewTimeSlot(processName);

                Assert.AreEqual(timestamp, newTimeStamp);

                List<Timeslot> timeSlots = _database.GetTimeSlotsForProcess(processName);

                // lista trebuie sa aiba elemente, tinand seama ca de abia am adaugat in baza de date
                Assert.AreNotEqual(0, timeSlots.Count);

                bool isFound = false;
                foreach(Timeslot timeslot in timeSlots)
                {
                    if(timeslot.getStartTime() == timestamp)
                    {
                        isFound = true;
                        break;
                    }
                }

                Assert.IsTrue(isFound);
            }
            catch (Exception exception)
            {
                Assert.Fail("Expected no exception in TestAddNewTimeSlot(), but got: " + exception.Message);
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
