using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class UserProcess
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

        public UserProcess(uint uniqueProcesID, string processName)
        {
            _uniqueProcesID = uniqueProcesID;
            _processName = processName;
        }
    }
}
