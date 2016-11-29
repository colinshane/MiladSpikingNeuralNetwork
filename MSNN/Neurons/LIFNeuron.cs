using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.Neurons
{
	[Serializable]
	public class LIFNeuron : Neuron
	{
		//Neuron parameters
		private float equilibriumPotential, tao, afterSpikePotential;
		private float threshold;

		private float potential;

		//public LIFNeuron(NeuronTypes neuronType) : base(neuronType) { }

		public LIFNeuron(NeuronTypes neuronType, float lifEquilibriumPotential, float lifTao, float lifAfterSpikePotential,
			float lifThreshold):base(neuronType)
		{
			SetParameters(lifEquilibriumPotential, lifTao, lifAfterSpikePotential, lifThreshold);
		}

		protected LIFNeuron(LIFNeuron lifNeuron):base(lifNeuron.Type)
		{
			SetParameters(lifNeuron.equilibriumPotential, lifNeuron.tao,
				lifNeuron.afterSpikePotential, lifNeuron.threshold);
		}

		private void SetParameters(float lifEquilibriumPotential, float lifTao, float lifAfterSpikePotential,
			float lifThreshold)
		{
			equilibriumPotential = lifEquilibriumPotential;
			tao = lifTao;
			afterSpikePotential = lifAfterSpikePotential;
			threshold = lifThreshold;
			potential = equilibriumPotential;
		}

		public override void Reset()
		{
			//ClearSpikeTimes();
			potential = equilibriumPotential;
		}

		protected override void UpdatePotential(float extraInput)
		{
			float inputCurrent = extraInput;
			foreach (Synapse syn in preSynapses)
			{
				inputCurrent += syn.GetPotential();
			}

			potential += ((equilibriumPotential - potential) / tao) + inputCurrent;
			if (potential >= threshold)
			{
				PropagateSpike();
				potential = afterSpikePotential;
			}
		}

		protected void GoToAfterSpikeState()
		{
			potential = afterSpikePotential;
		}

		public override Neuron GetCopy()
		{
			LIFNeuron copy = new LIFNeuron(this.Type, this.equilibriumPotential,
				this.tao, this.afterSpikePotential, this.threshold);
			return copy;
		}

		public override string ToString()
		{
			return $"v = {potential}, spikes = {string.Join(",", SpikeTimes)}";
		}
	}
}
