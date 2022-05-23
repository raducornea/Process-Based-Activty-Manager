/*******************************************************************************
 *                                                                             *
 *  File:        StoredProcess.cs                                              *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Data class for processes and their details                    *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

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
            // Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            StoredProcess p = (StoredProcess)obj;
            return (_uniqueProcesID == p.UniqueProcesID) && (_processName == p.ProcessName);
        }

    }
}
