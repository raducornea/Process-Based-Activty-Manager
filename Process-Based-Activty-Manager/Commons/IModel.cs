﻿using System.Collections.Generic;

namespace ActivityTracker
{
	/// <summary>
	/// Interfata pentru Model in cazul in care mai apar si altele noi
	/// </summary>
	public interface IModel
	{
		/// <summary>
		/// Se seteaza prezentatorul - MVP Pattern
		/// </summary>
		/// <param name="presenter"></param>
		void SetPresenter(IPresenter presenter);


		/// <summary>
		/// Proprietate pentru obtinerea proceselor rulante curente
		/// </summary>
		List<StoredProcess> AllProcessesList
		{
			get;
		}

		List<string> AllProcessNames
		{
			get;
		}

		List<ActiveProcess> ActiveProcess
		{
			get;
		}

		List<string> ActiveProcessNames
		{
			get;
		}

		/// <summary>
		/// Se cauta obtinerea timpului total petrecut pe un anumit proces din baza de date, in functie de numele lui
		/// </summary>
		/// <param name="processName"></param>
		/// <returns></returns>
		uint GetProcessTotalTime(string processName);

		/// <summary>
		/// Se cauta toate timestamp-urile unui proces spre a fi afisate pe interfata grafica
		/// </summary>
		/// <param name="processName"></param>
		/// <returns></returns>
		List<Timeslot> GetProcessTimeslots(string processName);

		/// <summary>
		/// Se updateaza timeslot-ul din baza de data - mai precis, se modifica durata de executie in functie de end time
		/// </summary>
		/// <param name="timeslotID"></param>
		/// <param name="duration"></param>
		void UpdateTimeSlots();

		/// <summary>
		/// Se obtin toate procesele rulante curente si se adauga in baza de date
		///
		/// </summary>
		void ScreenProcesses();
	}
}
