using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public delegate bool ConnectionPatternDelegate
		(int sourceNeuronIdx, int destinationNeuronIdx);
	[Serializable]
	public class Network
	{
		public string Name { get; private set; }
		public static int Time { get; private set; }
		public static int LearningInterval { get; private set; }
		public static Random rand { get; private set; }
		//public static int seed { get; private set; }

		public static float SynapseMinWeight { get; private set; }
		public static float SynapseMaxWeight { get; private set; }
		public static int SynapseMinDelay { get; private set; }
		public static int SynapseMaxDelay { get; private set; }

		private List<Layer> layers;
		private List<IMonitor> monitors;

		public Layer this[int idx]
		{
			get
			{
				if (idx < layers.Count)
					return layers[idx];
				throw new IndexOutOfRangeException();
			}
		}

		public Network(string networkName, int learningInterval = 100,
			float synapseMinWeight = 0.0f, float synapseMaxWeight = 1.0f,
			int synapseMinDelay = 1, int synapseMaxDelay = 20, int seed = -1)
		{
			layers = new List<Layer>();
			monitors = new List<IMonitor>();
			if (seed != -1)
				rand = new Random(seed);
			else
				rand = new Random();
			Name = networkName;
			Time = 0;
			LearningInterval = learningInterval;
			SynapseMinWeight = synapseMinWeight;
			SynapseMaxWeight = synapseMaxWeight;
			SynapseMinDelay = synapseMinDelay;
			SynapseMaxDelay = synapseMaxDelay;
		}

		public void SetDopamineModule(float decayFactor, IDopamineInjector dopamineInjector)
		{
			Dopamine.SetParameters(decayFactor, dopamineInjector);
		}

		public int AddMonitor(IMonitor monitor)
		{
			this.monitors.Add(monitor);
			return monitors.Count - 1;
		}

		public int AddLayer(Layer layer)
		{
			layers.Add(layer);
			return layers.Count - 1;
		}

		public int AddGroup(Group group, int layerIdx)
		{
			if (layerIdx > layers.Count)
				throw new IndexOutOfRangeException();
			return layers[layerIdx].AddGroup(group);
		}

		public int AddGroup(string groupName, int numberOfNeurons, Neuron sampleNeuron,
			IGroupLearningStrategy groupLearningStrategy, int layerIdx)
		{
			Group g = new Group(groupName, numberOfNeurons, sampleNeuron, groupLearningStrategy);
			return AddGroup(g, layerIdx);
		}

		public int AddGroup(string groupName, int numberOfNeurons, Neuron sampleNeuron,
			IGroupLearningStrategy groupLearningStrategy, ExtraInputPatternDelegate extraInputGenerator, int layerIdx)
		{
			Group g = new Group(groupName, numberOfNeurons, sampleNeuron, groupLearningStrategy, extraInputGenerator);
			return AddGroup(g, layerIdx);
		}

		public void RunNetwork(int numberOfSteps)
		{
			for (int i = 0; i < numberOfSteps; i++, Time++)
			{
				if (Time % LearningInterval == 0)
				{
					//Console.WriteLine(Time);
					
					for (int lay = 0; lay < layers.Count; lay++)
						layers[lay].Learn();

					for (int lay = 0; lay < layers.Count; lay++)
						layers[lay].ClearSpikeTimes();
				}
				for (int lay = 0; lay < layers.Count; lay++)
					layers[lay].Update();

				Dopamine.Update();

				foreach (IMonitor m in monitors)
					m.Update();
			}
		}
		
		#region Static Methods
		public static void MakeConnection(Group sourceGroup, Group destinationGroup, float weight, int delay,
			ConnectionPatternDelegate connectionPattern, 
			ILearningRule learningRule)
		{
			for (int i = 0; i < sourceGroup.Count; i++)
			{
				for (int j = 0; j < destinationGroup.Count; j++)
				{
					if(connectionPattern(i, j))
					{
						Synapse syn = new Synapse(sourceGroup[i], destinationGroup[j],
							weight, delay, learningRule.GetCopy());
						sourceGroup[i].AddPostSynapse(syn);
						destinationGroup[j].AddPreSynapse(syn);
					}
				}
			}
		}

		public static void MakeSharedConnection(Group sourceGroup, Group destinationGroup,
			ConnectionPatternDelegate connectionPattern, List<SynapseCore> synapseCores)
		{
			int coreIdx = 0;
			for (int i = 0; i < destinationGroup.Count; i++)
			{
				for (int j = 0; j < sourceGroup.Count; j++)
				{
					if (connectionPattern(j, i))
					{
						Synapse syn = new Synapse(sourceGroup[j], destinationGroup[i], synapseCores[coreIdx]);
						sourceGroup[j].AddPostSynapse(syn);
						destinationGroup[i].AddPreSynapse(syn);
						coreIdx++;
					}
				}
			}
		}

		public static void MakeRandomValueConnection(Group sourceGroup, Group destinationGroup,
			float minWeight, float maxWeight, int minDelay, int maxDelay,
			ConnectionPatternDelegate connectionPattern, ILearningRule learningRule)
		{
			float range = maxWeight - minWeight;
			for (int i = 0; i < sourceGroup.Count; i++)
			{
				for (int j = 0; j < destinationGroup.Count; j++)
				{
					if (connectionPattern(i, j))
					{
						float randWeight = (float)rand.NextDouble() * range + minWeight;
						int randDelay = rand.Next(minDelay, maxDelay + 1);
						Synapse syn = new Synapse(sourceGroup[i], destinationGroup[j],
							randWeight, randDelay, learningRule.GetCopy());
						sourceGroup[i].AddPostSynapse(syn);
						destinationGroup[j].AddPreSynapse(syn);
					}
				}
			}
		}

		public static Synapse Connect(Neuron n1, Neuron n2, float weight, int delay, ILearningRule learningRule)
		{
			Synapse syn = new Synapse(n1, n2, weight, delay, learningRule.GetCopy());
			n1.AddPostSynapse(syn);
			n2.AddPreSynapse(syn);
			return syn;
		}
		#endregion
	}
}
