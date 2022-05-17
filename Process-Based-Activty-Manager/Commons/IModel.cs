using System.Collections.Generic;

namespace ActivityTracker
{
	/// <summary>
	/// Interfata pentru Model in cazul in care mai apar si altele noi
	/// </summary>
	public interface IModel
	{

		/// <summary>
		/// Proprietate pentru obtinerea proceselor rulante curente
		/// </summary>
		List<StoredProcess> GeneralProcessList
		{
			get;
		}

		/// <summary>
		/// Proprietate pentru a primi numele proceselor curente
		/// </summary>
		List<string> ProcessNameList
		{
			get;
		}

		/// <summary>
		/// Se cauta obtinerea timpului total petrecut pe un anumit proces din baza de date, in functie de numele lui
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		uint GetProcessTotalTime(string processID);

		/// <summary>
		/// Se cauta toate timestamp-urile unui proces spre a fi afisate pe interfata grafica
		/// </summary>
		/// <param name="processID"></param>
		/// <returns></returns>
		List<Timeslot> GetProcessTimeslots(string processID);

		/// <summary>
		/// Se adauga un timeslot nou - posibil atunci cand apare un nou proces pe fundal
		/// </summary>
		/// <param name="processID"></param>
		void AddNewTimeSlot(string processID);

		/// <summary>
		/// Se updateaza timeslot-ul din baza de data - mai precis, se modifica durata de executie in functie de end time
		/// </summary>
		/// <param name="timeslotID"></param>
		/// <param name="duration"></param>
		void UpdateTimeSlots();

		/// <summary>
		/// Se seteaza prezentatorul - MVP Pattern
		/// </summary>
		/// <param name="presenter"></param>
		void SetPresenter(IPresenter presenter);

		/// <summary>
		/// Se obtin toate procesele rulante curente si se adauga in baza de date
		/// TODO: nu ar trebui sa le transmita mai departe si pe interfata cu user-ul????
		/// </summary>
		void ScreenWindowsProcesses();
	}
}
