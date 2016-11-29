using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public abstract class SynapseStatistics
	{
		protected Synapse synapse;
		public abstract ArrayList GetData();
		public abstract void SaveData();

		public SynapseStatistics(Synapse targetSynapse)
		{
			this.synapse = targetSynapse;
		}
	}
}
