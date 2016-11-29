using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	[Serializable]
	public delegate float ExtraInputPatternDelegate(int neuronID);
	public class Group
	{
		public string Name { get; private set; }
		protected Neuron[] neurons;
		protected IGroupLearningStrategy learningStrategy;
		protected IGroupLearningStrategy learningStrategyHolder;
		protected ExtraInputPatternDelegate getExtraInput;

		public Neuron this[int idx]
		{
			get
			{
				if (idx < neurons.Length)
					return neurons[idx];
				throw new IndexOutOfRangeException();
			}
		}

		public int Count
		{
			get { return neurons.Length; }
		}

		protected Group(){}

		public Group(string name, int numberOfNeuron, Neuron sampleNeuron,
			IGroupLearningStrategy groupLearningStrategy, ExtraInputPatternDelegate extraInputGenerator = null)
		{
			learningStrategy = groupLearningStrategy;
			AddCopyNeurons(numberOfNeuron, sampleNeuron);
			getExtraInput = extraInputGenerator;
		}

		private void AddCopyNeurons(int numberOfCopies, Neuron sampleNeuron)
		{
			neurons = new Neuron[numberOfCopies];
			for(int i = 0; i < numberOfCopies; i++)
			{
				neurons[i] = sampleNeuron.GetCopy();
			}
		}

		protected virtual void UpdateNeurons()
		{
			if (getExtraInput != null)
			{
				for (int i = 0; i < Count; i++)
				{
					neurons[i].Update(getExtraInput(i));
				}
			}
			else
			{
				for (int i = 0; i < Count; i++)
				{
					neurons[i].Update();
				}
			}
		}

		public void Update()
		{
			UpdateNeurons();
			UpdateNeuronStatistics();
		}

		public void Learn()
		{
			learningStrategy.ApplyLearning(this);
			//ClearSpikeTimes();
		}

		public void ClearSpikeTimes()
		{
			for (int i = 0; i < neurons.Length; i++)
			{
				neurons[i].ClearSpikeTimes();
			}
		}

		public void Reset()
		{
			for(int i = 0; i < neurons.Length; i++)
			{
				neurons[i].Reset();
			}
			ClearSpikeTimes();
		}

		public void SetLearningOff()
		{
			learningStrategyHolder = learningStrategy;
			learningStrategy = new GroupLearningStrategies.LearningOffGroupStrategy();
		}

		public void ResumeLearning()
		{
			learningStrategy = learningStrategyHolder;
		}

		public void SetLearningOn(IGroupLearningStrategy groupLearningStrategy)
		{
			learningStrategy = groupLearningStrategy;
		}

		protected void UpdateNeuronStatistics()
		{
			foreach (Neuron neuron in neurons)
				neuron.UpdateStatistics();
		}

		public void SetExtraInputGenerator(ExtraInputPatternDelegate extraGenerator)
		{
			this.getExtraInput = extraGenerator;
		}

		public void RemoveExtraInputGenerator()
		{
			this.getExtraInput = null;
		}

		public List<SynapseCore> GetPreSynapseCores()
		{
			List<SynapseCore> cores = new List<SynapseCore>();
			foreach (Neuron n in neurons)
				for (int i = 0; i < n.PreSynapsesCount; i++)
					cores.Add(n.GetPreSynapse(i).Core);
			return cores;
		}
	}
}
