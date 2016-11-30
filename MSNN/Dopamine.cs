using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public static class Dopamine
	{
		private static float td = 1;
		private static float d = 0;
		private static float[] dopamineLevels;
		private static IDopamineInjector injector = null;

		/// <summary>
		/// Gets the dopamine level at a given simulation time.
		/// </summary>
		/// <param name="time">Simulation time within the last learning interval.</param>
		/// <returns>Dopamine level</returns>
		public static float GetDopamineLevel(int time)
		{
			if (time < Network.Time && time >= Network.Time - Network.LearningInterval)
			{
				return dopamineLevels[time % Network.LearningInterval];
			}
			else
				throw new IndexOutOfRangeException("Dopamine levels are ony available for the last learning interval");
		}

		/// <summary>
		/// Sets parameters related to dopamine dynamics in the network
		/// </summary>
		/// <param name="decayFactor">td</param>
		public static void SetParameters(float decayFactor, IDopamineInjector dopamineInjector)
		{
			td = decayFactor;
			d = 0;
			dopamineLevels = new float[Network.LearningInterval];
			injector = dopamineInjector;
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
				//dopamineLevels[currentTime] = 
				//	Math.Max(0, dopamineLevels[preTime] - dopamineLevels[preTime] / td + injected);
				dopamineLevels[currentTime] = dopamineLevels[preTime] - dopamineLevels[preTime] / td + injected;
			}
		}

		/// <summary>
		/// Gets current dopamine level
		/// </summary>
		public static float DopamineLevel
		{
			get { return dopamineLevels[Network.Time % Network.LearningInterval]; }
		}

		public static void Reset()
		{
			for(int i = 0; i < dopamineLevels.Length; i++)
			{
				dopamineLevels[i] = 0;
			}
		}
	}
}
