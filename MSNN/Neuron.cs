using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	[Serializable]
	public enum NeuronTypes
	{
		Excitatory,
		Inhibitory
	}

	/// <summary>
	/// An abstract class which defines the overall framework of a neuron in the network.
	/// </summary>
	[Serializable]
	public abstract class Neuron
	{
		/// <summary>
		/// Gets the spike train of neuron in the last learning interval.
		/// </summary>
		public List<int> SpikeTimes { get; private set; }

		/// <summary>
		/// Gets the type of neuron (inhibitory/exitatory).
		/// </summary>
		public NeuronTypes Type { get; protected set; }

		private Dictionary<string, NeuronStatistics> statistics;
		protected List<Synapse> preSynapses;
		protected List<Synapse> postSynapses;
		
		protected abstract void UpdatePotential(float extraInput);

		/// <summary>
		/// Resets neuron's dynamics.
		/// </summary>
		public abstract void Reset();

		/// <summary>
		/// Generates a copy of neuron.
		/// </summary>
		/// <returns></returns>
		public abstract Neuron GetCopy();

		/// <summary>
		/// Gets the number of pre synapses.
		/// </summary>
		public int PreSynapsesCount
		{
			get
			{
				return preSynapses.Count;
			}
		}

		/// <summary>
		/// Gets the number of post synapses.
		/// </summary>
		public int PostSynapsesCount
		{
			get
			{
				return postSynapses.Count;
			}
		}
		
		private void Initialize()
		{
			preSynapses = new List<Synapse>();
			postSynapses = new List<Synapse>();
			SpikeTimes = new List<int>();
			statistics = new Dictionary<string, NeuronStatistics>();
		}
		
		public Neuron() { Initialize(); } //for the sake of inheritance and generics!

		/// <summary>
		/// Creats and initializes a neuron (it must be called by all inherited classes!).
		/// </summary>
		/// <param name="neuronType">Inhibitory or exitatory neuron</param>
		public Neuron(NeuronTypes neuronType)
		{
			Initialize();
			Type = neuronType;
		}

		/// <summary>
		/// Updates the neuron dynamics.
		/// </summary>
		/// /// <param name="extraInput">Extra input current</param>
		public void Update(float extraInput = 0)
		{
			UpdatePotential(extraInput);
		}

		/// <summary>
		/// Calls the learning mechanisms of each pre synapse.
		/// </summary>
		public void Learn()
		{
			foreach (Synapse syn in preSynapses)
			{
				syn.ApplyLearning();
			}
		}

		/// <summary>
		/// Clears spike train.
		/// </summary>
		public void ClearSpikeTimes()
		{
			SpikeTimes.Clear();
		}

		/// <summary>
		/// Adds a new spike-time to the spike train and post synaptic buffers.
		/// </summary>
		protected void PropagateSpike()
		{
			SpikeTimes.Add(Network.Time);
			foreach (Synapse syn in postSynapses)
				syn.AddSpikeTime();
		}

		//public void ForceSpike()
		//{
		//	PropagateSpike();
		//	Reset();
		//}

		/// <summary>
		/// Updates existing neuron statistics.
		/// </summary>
		public void UpdateStatistics()
		{
			foreach (NeuronStatistics nstat in statistics.Values)
				nstat.SaveData();
		}

		/// <summary>
		/// Gets the pre synapse using its id. 
		/// </summary>
		/// <param name="synapseID">Synapse id (zero-based)</param>
		/// <returns></returns>
		public Synapse GetPreSynapse(int synapseID)
		{
			if (synapseID >= preSynapses.Count)
				throw new IndexOutOfRangeException();
			return preSynapses[synapseID];
		}

		/// <summary>
		/// Gets the post synapse using its id. 
		/// </summary>
		/// <param name="synapseID">Synapse id (zero-based)</param>
		/// <returns></returns>
		public Synapse GetPostSynapse(int synapseID)
		{
			if (synapseID >= postSynapses.Count)
				throw new IndexOutOfRangeException();
			return postSynapses[synapseID];
		}

		/// <summary>
		/// Adds a new pre synapse. 
		/// </summary>
		/// <param name="preSynapse">Synapse information</param>
		public void AddPreSynapse(Synapse preSynapse)
		{
			preSynapses.Add(preSynapse);
		}

		/// <summary>
		/// Adds a new post synapse.
		/// </summary>
		/// <param name="postSynapse">Synapse information</param>
		public void AddPostSynapse(Synapse postSynapse)
		{
			postSynapses.Add(postSynapse);
		}

		/// <summary>
		/// Adds a new statistic to the neuron.
		/// </summary>
		/// <param name="key">A key by which the statistic can be found later.</param>
		/// <param name="stat">A neuron statistic</param>
		public void AddStatistics(string key, NeuronStatistics stat)
		{
			if(!statistics.ContainsKey(key))
				statistics.Add(key, stat);
		}

		/// <summary>
		/// Checks if a statistic exists.
		/// </summary>
		/// <param name="key">Statistic's key</param>
		/// <returns>True if the key exists.</returns>
		public bool StatisticExists(string key)
		{
			return statistics.ContainsKey(key);
		}

		/// <summary>
		/// Returns a neuron statistic.
		/// </summary>
		/// <param name="key">Statistic's key</param>
		/// <returns></returns>
		public NeuronStatistics GetStatistics(string key)
		{
			return statistics[key];
		}
	}
}
