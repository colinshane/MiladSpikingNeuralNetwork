using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.DopamineInjectors
{
	public class ConstantDopamineInjector : IDopamineInjector
	{
		/// <summary>
		/// Gets the amount of dopamine the module injects
		/// </summary>
		public float Amount { get; private set; }

		/// <summary>
		/// Gets the time interval between each dopamine injection
		/// </summary>
		public int InjectionInterval { get; private set; }

		/// <summary>
		/// </summary>
		/// <param name="amount">The amount of dopamine for each injection</param>
		/// <param name="injectionInterval">The time interval between each injection</param>
		public ConstantDopamineInjector(float amount, int injectionInterval)
		{
			Amount = amount;
			InjectionInterval = injectionInterval;
		}
		
		public float Inject()
		{
			if (Network.Time % InjectionInterval == 0)
				return Amount;
			return 0;
		}
	}
}
