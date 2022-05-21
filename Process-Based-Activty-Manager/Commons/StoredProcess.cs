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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                StoredProcess p = (StoredProcess)obj;
                return (_uniqueProcesID == p.UniqueProcesID) && (_processName == p.ProcessName);
            }
        }

    }
}
