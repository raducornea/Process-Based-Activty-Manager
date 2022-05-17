using System;
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
            _ID = ID;
            _processID = processID;
            _startTime = startTime;
            _endTime = endTime;
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