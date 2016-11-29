using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.LearningRules
{
	public class STDPNeuronStatistics : NeuronStatistics
	{
		//Temporarly save spike times around the stdp effective time window
		private List<int> lastSpikesOld;
		private List<int> lastSpikesNew;
		private int window;

		public STDPNeuronStatistics(Neuron targetNeuron, int tao)
			:base(targetNeuron)
		{
			window = tao;
			lastSpikesOld = new List<int>();
			lastSpikesNew = new List<int>();
		}

		public override ArrayList GetData()
		{
			ArrayList result = new ArrayList();
			result.Add(lastSpikesOld);
			return result;
		}

		public override void SaveData()
		{
			if((Network.Time + 1) % Network.LearningInterval == 0)
				SaveBoundarySpikeTimes();
		}

		private void SaveBoundarySpikeTimes()
		{
			lastSpikesOld = lastSpikesNew;
			lastSpikesNew = new List<int>();
			for (int i = neuron.SpikeTimes.Count - 1; i >= 0; i--)
			{
				int tmp = neuron.SpikeTimes[i];
				if (tmp + Network.SynapseMaxDelay >= Network.Time - window)
				{
					lastSpikesNew.Add(tmp);
					break;
				}
				else
					break;
			}
		}
	}
}
