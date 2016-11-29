using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public interface ILearningRule
	{
		float GetWeightChange(Synapse synapse);
		ILearningRule GetCopy();
		
		bool HasPreNeuronStatistics { get; }
		bool HasPostNeuronStatistics { get; }
		
		NeuronStatistics GetPreNeuronStatistics(Neuron neuron);
		string GetPreNeuronStatisticsKey();
		NeuronStatistics GetPostNeuronStatistics(Neuron neuron);
		string GetPostNeuronStatisticsKey();
	}
}
