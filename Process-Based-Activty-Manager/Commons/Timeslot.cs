using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracker
{
	public class Timeslot
	{
		DateTime _createdTime;
		uint _duration;    //in seconds amigo

		public Timeslot(DateTime createdTime, uint duration) 
		{
			_createdTime = createdTime;
			_duration = duration;
		}
	}
}
