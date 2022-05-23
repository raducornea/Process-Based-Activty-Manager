/*******************************************************************************
 *                                                                             *
 *  File:        Timeslot.cs                                                   *
 *  Copyright:   (c) 2022, Cornea Radu-Valentin                                *
 *  E-mail:      radu-valentin.cornea@student.tuiasi.ro                        *
 *  Website:     https://github.com/raducornea/Process-Based-Activty-Manager   *
 *  Description: Data Class for Timestamps including startTime and endTime     *
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
    public class Timeslot
    {
        private long _ID;
        private string _processID;
        private long _startTime;
        private long _endTime;

        public Timeslot(long ID, string processID, long startTime, long endTime)
        {
            if (startTime > endTime || startTime < 0 || endTime < 0)
            {
                throw new Exception("Timeslots introduced wrongly!");
            }    

            _ID = ID;
            _processID = processID;
            _startTime = startTime;
            _endTime = endTime;
        }

        public long Duration
        {
            get => _endTime - _startTime;
        }

        public long StartTime
        {
            get => _startTime;
        }

        public long EndTime
        {
            get => _endTime;
        }
    }
}