using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
    public class Timeslot
    {
        private string _ID;
        private string _processID;
        private long _startTime;
        private long _endTime;

        public Timeslot(string ID, string processID, long startTime, long endTime)
        {
            _ID = ID;
            _processID = processID;
            _startTime = startTime;
            _endTime = endTime;
        }

        public Timeslot(long startTime, long endTime, string ID, string processID)
        {
            _startTime = startTime;
            _endTime = endTime;
            _ID = ID;
            _processID = processID;
        }

        public long getDuration()
        {
            return _endTime - _startTime;
        }

        public long getStartTime()
        {
            return _startTime;
        }

        public long getEndTime()
        {
            return _endTime;
        }
    }
}