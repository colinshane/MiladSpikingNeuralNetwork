using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.LearningRules
{
	public class NoChangeLearningRule : ILearningRule
	{
		public bool HasPostNeuronStatistics
		{
			get
			{
				return false;
			}
		}

		public bool HasPreNeuronStatistics
		{
			get
			{
				return false;
			}
		}

		public ILearningRule GetCopy()
		{
			return new NoChangeLearningRule();
		}

		public NeuronStatistics GetPostNeuronStatistics(Neuron neuron)
		{
			throw new NotImplementedException();
		}

		public string GetPostNeuronStatisticsKey()
		{
			throw new NotImplementedException();
		}

		public NeuronStatistics GetPreNeuronStatistics(Neuron neuron)
		{
			throw new NotImplementedException();
		}

		public string GetPreNeuronStatisticsKey()
		{
			throw new NotImplementedException();
		}

		public float GetWeightChange(Synapse synapse)
		{
			return 0;
		}
	}
}
