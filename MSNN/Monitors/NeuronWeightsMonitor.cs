using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSNN.Monitors
{
	public delegate bool SynapseSelectorDelegate(int synapseId);
	public class NeuronWeightsMonitor : IMonitor
	{
		private Neuron neuron;
		private SynapseSelectorDelegate isSelected;
		public List<List<float>> Weights { get; private set; }
		public string Name { get; private set; }
		
		public NeuronWeightsMonitor(Neuron neuron, SynapseSelectorDelegate synapseSelector, string name)
		{
			this.neuron = neuron;
			isSelected = synapseSelector;
			Weights = new List<List<float>>();
			Name = name;
		}

		public void SaveToFile()
		{
			StreamWriter writer = new StreamWriter(Name + ".txt");
			for(int i = 0; i < Weights.Count; i++)
			{
				writer.WriteLine(string.Join(",", Weights[i]));
			}
			writer.Close();
		}

		public void Update()
		{
			if (Network.Time % Network.LearningInterval == 0)
			{
				Weights.Add(new List<float>());
				for (int i = 0; i < neuron.PreSynapsesCount; i++)
				{
					if (isSelected(i))
					{
						Weights.Last().Add(neuron.GetPreSynapse(i).Weight);
					}
				}
			}
		}
	}
}
