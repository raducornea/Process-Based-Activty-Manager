﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
    public class Timeslot
    {
        private long _ID;
        private string _processID;
        private long _startTime;
        private long _endTime;

        public Timeslot(long ID, string processID, long startTime, long endTime)
        {
            if (startTime > endTime || startTime < 0 || endTime < 0)
            {
                throw new Exception("Timeslots introduced wrongly!");
            }    

            _ID = ID;
            _processID = processID;
            _startTime = startTime;
            _endTime = endTime;
        }

        public long Duration
        {
            get { return _endTime - _startTime; }
        }

        public long StartTime
        {
            get { return _startTime; }
        }

        public long EndTime
        {
            get { return _endTime; }
        }
    }
}