using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
    public class StoredProcess
    {
        uint _uniqueProcesID;
        string _processName;

        public uint UniqueProcesID
        {
            get => _uniqueProcesID;
        }

        public string ProcessName
        {
            get => _processName;
        }

        public StoredProcess(uint uniqueProcesID, string processName)
        {
            _uniqueProcesID = uniqueProcesID;
            _processName = processName;
        }
    }
}
