using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public abstract class NeuronStatistics
	{
		protected Neuron neuron;
		public abstract ArrayList GetData();
		public abstract void SaveData();

		public NeuronStatistics(Neuron targetNeuron)
		{
			neuron = targetNeuron;
		}
	}
}
