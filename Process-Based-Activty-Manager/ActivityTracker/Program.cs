using System;

namespace ActivityTracker
{
	class Program
	{
		static void Main(string[] args)
		{
			Model myModel = new Model();
			myModel.StartThread();


			//myModel.ceva();
			//myModel.GetAllProcesses();

			Console.ReadKey();
		}
	}
}
