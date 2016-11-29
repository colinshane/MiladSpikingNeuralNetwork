using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.Neurons
{
	public class SpikerNeuron : Neuron
	{
		public override Neuron GetCopy()
		{
			return new SpikerNeuron();
		}

		public override void Reset()
		{
			
		}

		protected override void UpdatePotential(float extraInput)
		{
			PropagateSpike();
		}
	}
}
