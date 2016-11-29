using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public static class Dopamin
	{
		private static float td = 1;
		private static float d = 0;
		private static float[] dopaminLevels;
		private static IDopaminInjector injector = null;

		/// <summary>
		/// Gets the dopamin level at a given simulation time.
		/// </summary>
		/// <param name="time">Simulation time within the last learning interval.</param>
		/// <returns>Dopamin level</returns>
		public static float GetDopaminLevel(int time)
		{
			if (time < Network.Time && time >= Network.Time - Network.LearningInterval)
			{
				return dopaminLevels[time % Network.LearningInterval];
			}
			else
				throw new IndexOutOfRangeException("Dopamin levels are ony available for the last learning interval");
		}

		/// <summary>
		/// Sets parameters related to dopamin dynamics in the network
		/// </summary>
		/// <param name="decayFactor">td</param>
		public static void SetParameters(float decayFactor, IDopaminInjector dopaminInjector)
		{
			td = decayFactor;
			d = 0;
			dopaminLevels = new float[Network.LearningInterval];
			injector = dopaminInjector;
		}

		/// <summary>
		/// Updates dopamine dynamics
		/// </summary>
		internal static void Update()
		{
			if (injector != null)
			{
				float injected = injector.Inject();

				int currentTime = Network.Time % Network.LearningInterval;
				int preTime = (currentTime == 0 ? Network.LearningInterval - 1 : currentTime - 1);
				//dopaminLevels[currentTime] = 
				//	Math.Max(0, dopaminLevels[preTime] - dopaminLevels[preTime] / td + injected);
				dopaminLevels[currentTime] = dopaminLevels[preTime] - dopaminLevels[preTime] / td + injected;
			}
		}

		/// <summary>
		/// Gets current dopamin level
		/// </summary>
		public static float DopaminLevel
		{
			get { return dopaminLevels[Network.Time % Network.LearningInterval]; }
		}

		public static void Reset()
		{
			for(int i = 0; i < dopaminLevels.Length; i++)
			{
				dopaminLevels[i] = 0;
			}
		}
	}
}
