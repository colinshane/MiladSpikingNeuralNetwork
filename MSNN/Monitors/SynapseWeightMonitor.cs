using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MSNN.Monitors
{
	public class SynapseWeightMonitor : IMonitor
	{
		public string Name { get; private set; }
		private List<Synapse> synapses;
		public List<string> SynapseNames { get; private set; }
		public List<List<float>> Weights { get; private set; }
		public int Count
		{
			get
			{
				return synapses.Count;
			}
		}

		public SynapseWeightMonitor(string name)
		{
			Name = name;
			synapses = new List<Synapse>();
			SynapseNames = new List<string>();
			Weights = new List<List<float>>();
		}

		public void AddSynapse(string name, Synapse synapse)
		{
			SynapseNames.Add(name);
			synapses.Add(synapse);
			Weights.Add(new List<float>());
			Weights.Last().Add(synapse.Weight);
		}

		public void SaveToFile()
		{
			StreamWriter writer = new StreamWriter(Name + ".txt");
			for (int i = 0; i < synapses.Count; i++)
			{
				writer.WriteLine(SynapseNames[i] + ":" + string.Join(",", Weights[i]));
			}
			writer.Close();
		}

		public void Update()
		{
			if (Network.Time % Network.LearningInterval == 0)
			{
				for(int i = 0; i < synapses.Count; i++)
				{
					Weights[i].Add(synapses[i].Weight);
				}
			}
		}
	}
}
