using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.DopaminInjectors
{
	public class ConstantDopaminInjector : IDopaminInjector
	{
		/// <summary>
		/// Gets the amount of dopamin the module injects
		/// </summary>
		public float Amount { get; private set; }

		/// <summary>
		/// Gets the time interval between each dopamin injection
		/// </summary>
		public int InjectionInterval { get; private set; }

		/// <summary>
		/// </summary>
		/// <param name="amount">The amount of dopamin for each injection</param>
		/// <param name="injectionInterval">The time interval between each injection</param>
		public ConstantDopaminInjector(float amount, int injectionInterval)
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
