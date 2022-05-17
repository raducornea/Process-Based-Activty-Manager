namespace ActivityTracker
{
    /// <summary>
    /// Clasa pentru informatiile despre un proces, nume si id
    /// </summary>
    public class StoredProcess
    {
        private string _uniqueProcesID;
        private string _processName;
        private long _currentTimeslotID = 0;

        /// <summary>
        /// Proprietate pentru id
        /// </summary>
        public string UniqueProcesID
        {
            get => _uniqueProcesID;
        }

        /// <summary>
        /// Proprietate pentru nume
        /// </summary>
        public string ProcessName
        {
            get => _processName;
        }

        public long CurrentTimeslotID
        {
            get => _currentTimeslotID;
            //set { _currentTimeslotID = value; }
        }

        /// <summary>
        /// Constructorul clasei, dependent de id si nume proces
        /// </summary>
        /// <param name="uniqueProcesID"></param>
        /// <param name="processName"></param>
        public StoredProcess(string uniqueProcesID, string processName, long currentTimeslotID )
        {
            _uniqueProcesID = uniqueProcesID;
            _processName = processName;
            _currentTimeslotID = currentTimeslotID;
    }
}
}
