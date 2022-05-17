namespace ActivityTracker
{
    /// <summary>
    /// Clasa pentru informatiile despre un proces, nume si id
    /// </summary>
    public class StoredProcess
    {
        private string _uniqueProcesID;
        private string _processName;

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

        /// <summary>
        /// Constructorul clasei, dependent de id si nume proces
        /// </summary>
        /// <param name="uniqueProcesID"></param>
        /// <param name="processName"></param>
        public StoredProcess(string uniqueProcesID, string processName)
        {
            _uniqueProcesID = uniqueProcesID;
            _processName = processName;
        }
    }
}
