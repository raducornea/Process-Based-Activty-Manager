using System;

namespace ActivityTracker
{

    public class StoredProcess
    {
        protected string _uniqueProcesID;
        protected string _processName;


        public string UniqueProcesID
        {
            get => _uniqueProcesID;
        }


        public string ProcessName
        {
            get => _processName;
        }

        public StoredProcess(string uniqueProcesID, string processName)
        {
            _uniqueProcesID = uniqueProcesID;
            _processName = processName;
        }
     }
}
