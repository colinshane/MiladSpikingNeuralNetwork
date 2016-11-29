using MSNN.LearningRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	[Serializable]
	public class SynapseCore
	{
		private float weight;
		private int delay;
		public float Weight
		{
			get
			{
				return weight;
			}
			private set
			{
				weight = Math.Max(value, Network.SynapseMinWeight);
				weight = Math.Min(weight, Network.SynapseMaxWeight);
			}
		}
		public int Delay
		{
			get
			{
				return delay;
			}
			private set
			{
				delay = Math.Max(value, Network.SynapseMinDelay);
				delay = Math.Min(delay, Network.SynapseMaxDelay);
			}
		}
		
		private ILearningRule learningRule;

		public SynapseCore(float synapseWeight, int synapseDelay, ILearningRule synapseLearningRule)
		{
			Weight = synapseWeight;
			Delay = synapseDelay;
			learningRule = synapseLearningRule;
		}

		public void ApplyLearningRule(Synapse synapse)
		{
			float dW = learningRule.GetWeightChange(synapse);
			Weight += dW;
		}
		
		public NeuronStatistics GetPreNeuronStatisticsModule(Neuron neuron)
		{
			return learningRule.GetPreNeuronStatistics(neuron);
		}

		public string GetPreNeuronStatisticsKey()
		{
			return learningRule.GetPreNeuronStatisticsKey();
		}

		public NeuronStatistics GetPostNeuronStatisticsModule(Neuron neuron)
		{
			return learningRule.GetPostNeuronStatistics(neuron);
		}

		public string GetPostNeuronStatisticsKey()
		{
			return learningRule.GetPostNeuronStatisticsKey();
		}

		public bool NeedsPreNeuronStatistics()
		{
			return learningRule.HasPreNeuronStatistics;
		}

		public bool NeedsPostNeuronStatistics()
		{
			return learningRule.HasPostNeuronStatistics;
		}
	}
}
