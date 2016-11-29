using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	[Serializable]
	public class Synapse
	{
		public Neuron PreNeuron { get; private set; }
		public Neuron PostNeuron { get; private set; }
		public SynapseCore Core { get; private set; }
		private Queue<int> spikeTimes;

		public float Weight
		{
			get
			{
				return Core.Weight;
			}
		}

		public int Delay
		{
			get
			{
				return Core.Delay;
			}
		}
		
		public Synapse(Neuron pre, Neuron post, SynapseCore synapseCore)
		{
			spikeTimes = new Queue<int>();
			PreNeuron = pre;
			PostNeuron = post;
			Core = synapseCore;

			//has pre neuron statistics?
			if(Core.NeedsPreNeuronStatistics())
				PreNeuron.AddStatistics(Core.GetPreNeuronStatisticsKey(), Core.GetPreNeuronStatisticsModule(PreNeuron));
			//has post neuron statistics?
			if (Core.NeedsPostNeuronStatistics())
				PostNeuron.AddStatistics(Core.GetPostNeuronStatisticsKey(), Core.GetPostNeuronStatisticsModule(PostNeuron));
		}

		public Synapse(Neuron pre, Neuron post,
			float synapseWeight, int synapseDelay, ILearningRule synapseLearningRule) :
			this(pre, post, new SynapseCore(synapseWeight, synapseDelay, synapseLearningRule))
		{ }

		public void ApplyLearning()
		{
			Core.ApplyLearningRule(this);
		}

		public void AddSpikeTime()
		{
			spikeTimes.Enqueue(Network.Time + Delay);
		}

		public float GetPotential()
		{
			if(spikeTimes.Count !=0 && spikeTimes.Peek() == Network.Time)
			{
				spikeTimes.Dequeue();

				return (PreNeuron.Type == NeuronTypes.Inhibitory ? -1 : 1) * Weight;
			}
			return 0.0f;
		}

		public void ClearSpikeTimes()
		{
			spikeTimes.Clear();
		}
	}
}
