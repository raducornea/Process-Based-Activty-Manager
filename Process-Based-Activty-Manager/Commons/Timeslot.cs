using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
    public class Timeslot
	{
		private long _startTime;
		private long _endTime;
		private String _ID;
		private String _processID;


		public Timeslot(long startTime, long endTime, String ID, String processID)
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
