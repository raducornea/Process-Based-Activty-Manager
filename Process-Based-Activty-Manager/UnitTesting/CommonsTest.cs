/*******************************************************************************
 *                                                                             *
 *  File:        CommonsTest.cs                                                *
 *  Copyright:   (c) 2022, Cornea Radu-Valentin                                *
 *  E-mail:      radu-valentin.cornea@student.tuiasi.ro                        *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Unit Tests for Commons File                                   *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

using ActivityTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class CommonsTest
    {
        [TestMethod]
        public void TestTimestampDuration_30_80()
        {
            Assert.AreEqual(50, (new Timeslot(0, "", 30, 80).Duration));
        }
                        
        [TestMethod]
        public void TestTimestampDuration_0_99999()
        {
            Assert.AreEqual(99999, (new Timeslot(0, "", 0, 99999).Duration));
        }


        [TestMethod]
        public void TestTimestamp_StartTime_521_998()
        {
            Assert.AreEqual(521, new Timeslot(0, "", 521, 998).StartTime);
        }
        
        [TestMethod]
        public void TestTimestamp_EndTime_521_998()
        {
            Assert.AreEqual(998, new Timeslot(0, "", 521, 998).EndTime);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTimestamp_521_80()
        {
            new Timeslot(0, "", 521, 80);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTimestamp_MINUS521_MINUS521()
        {
            new Timeslot(0, "", -521, -521);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTimestamp_MINUS521_521()
        {
            new Timeslot(0, "", -521, 521);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTimestamp_521_MINUS521()
        {
            new Timeslot(0, "", 521, -521);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTimestampDuration_MINUS521_MINUS521()
        {
            new Timeslot(0, "", -521, -521);
        }


        [TestMethod]
        public void TestTimestampDurations_30_80()
        {
            var value = new Timeslot(0, "", 30, 80);
            Assert.IsTrue(value.StartTime < value.EndTime);
        }

        [TestMethod]
        public void TestTimestampDurations_80_521()
        {
            var value = new Timeslot(0, "", 80, 521);
            Assert.AreEqual(441, value.Duration);
        }

        [TestMethod]
        public void TestTimestampDurations_0_99999()
        {
            var value = new Timeslot(0, "", 0, 99999);
            Assert.IsTrue(value.StartTime < value.EndTime);
        }




        [TestMethod]
        public void TestUniqueProcesID()
        {
            var value = new StoredProcess("ceva", "altceva");
            Assert.AreEqual("ceva", value.UniqueProcesID);
        }

        [TestMethod]
        public void TestProcessName()
        {
            var value = new StoredProcess("ceva", "altceva");
            Assert.AreEqual("altceva", value.ProcessName);
        }
    }
}
