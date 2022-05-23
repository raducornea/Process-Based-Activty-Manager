/*******************************************************************************
 *                                                                             *
 *  File:        ActiveProcess.cs                                              *
 *  Copyright:   (c) 2022, Apetrei Bogdan-Gabriel                              *
 *  E-mail:      bogdan-gabriel.apetrei@student.tuiasi.ro                      *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Data class for processes running in background                *
 *                                                                             *
 *  This code and information is provided "as is" without warranty of          *
 *  any kind, either expressed or implied, including but not limited           *
 *  to the implied warranties of merchantability or fitness for a              *
 *  particular purpose. You are free to use this source code in your           *
 *  applications as long as the original copyright notice is included.         *
 *                                                                             *
 *******************************************************************************/

using System;
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
