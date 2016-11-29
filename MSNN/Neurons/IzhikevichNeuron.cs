using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.Neurons
{
	/// <summary>
	/// Implements Izhikevich neuron model.
	/// </summary>
	[Serializable]
	public class IzhikevichNeuron : Neuron
	{
		private float v, u, a, b, c, d;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="neuronType">Inhibitory or exitatory neuron</param>
		/// <param name="paramA">Parameter a of Izhikevhich neuron</param>
		/// <param name="paramB">Parameter b of Izhikevhich neuron</param>
		/// <param name="paramC">Parameter c of Izhikevhich neuron</param>
		/// <param name="paramD">Parameter d of Izhikevhich neuron</param>
		public IzhikevichNeuron(NeuronTypes neuronType, float paramA, float paramB, float paramC, float paramD)
			:base(neuronType)
		{
			a = paramA;
			b = paramB;
			c = paramC;
			d = paramD;
			v = c;
			u = b * v;
		}

		/// <summary>
		/// Generates a copy of the nuron with the same parameters.
		/// </summary>
		/// <returns></returns>
		public override Neuron GetCopy()
		{
			return new IzhikevichNeuron(this.Type, this.a, this.b, this.c, this.d);
		}

		/// <summary>
		/// Resets neuron (potential and recovery).
		/// </summary>
		public override void Reset()
		{
			v = c;
			u = b * v;
		}

		/// <summary>
		/// Updates neuron dynamics.
		/// </summary>
		protected override void UpdatePotential(float extraInput)
		{
			float inputCurrent = extraInput;
			foreach (Synapse syn in preSynapses)
			{
				inputCurrent += syn.GetPotential();
			}

			//float preV = v;
			v = v + 0.5f * (0.04f * v * v + 5 * v + 140 - u + inputCurrent);
			v = v + 0.5f * (0.04f * v * v + 5 * v + 140 - u + inputCurrent);
			u = u + a * (b * v - u);
			if (v >= 30)
			{
				PropagateSpike();
				v = c;
				u = u + d;
			}
		}

		public override string ToString()
		{
			return $"v = {v}, u = {u}";
		}
	}
}
