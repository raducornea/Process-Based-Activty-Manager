using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ActivityTracker
{
	public class ActiveProcess : StoredProcess
	{
        private long _currentTimeslotID;
        private Process _asociatedProcess;

        public long CurrentTimeslotID
        {
            get
            {
                if (_currentTimeslotID == -1)
                {
                    throw new Exception("There is no asociated timeslot");
                }
                return _currentTimeslotID;
            }
        }

        public Process AsociatedProcess
        {
            get
            {
                if (_asociatedProcess == null)
                {
                    throw new Exception("There is no asociated process");
                }
                return _asociatedProcess;
            }
        }

        public ActiveProcess(StoredProcess dormandProcess, Process asociatedProcess, long currentTimeslotID): 
            base(dormandProcess.UniqueProcesID, dormandProcess.ProcessName)
        {
            _currentTimeslotID = currentTimeslotID;
            _asociatedProcess = asociatedProcess;
        }
    }
}
