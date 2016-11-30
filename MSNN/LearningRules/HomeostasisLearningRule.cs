using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.LearningRules
{
	public class HomeostasisLearningRule : ILearningRule
	{
		private STDPLearningRule stdp;
		private float alpha;
		private int targetRate;
		private int averagingWindow;
		string preNeuronStatisticsKey;
		string postNeuronStatisticsKey;

		public bool HasPostNeuronStatistics
		{
			get
			{
				return true;
			}
		}

		public bool HasPreNeuronStatistics
		{
			get
			{
				return true;
			}
		}

		public HomeostasisLearningRule(float stdpAmpPositive, float stdpAmpNegative,
			int stdpTaoPositive, int stdpTaoNegative,
			float homeostasisAlpha, int homeostasisTargetRate, int homeostasisAveragingWindow,
			int neighborSpikeCount = 0, float dopamineConstant = -1)
			:this(new STDPLearningRule(stdpAmpPositive, stdpAmpNegative, stdpTaoPositive, stdpTaoNegative,
				dopamineConstant), homeostasisAlpha, homeostasisTargetRate, homeostasisAveragingWindow)
		{}

		private HomeostasisLearningRule(STDPLearningRule stdpRule, float homeostasisAlpha,
			int homeostasisTargetRate, int homeostasisAveragingWindow)
		{
			stdp = stdpRule;
			alpha = homeostasisAlpha;
			targetRate = homeostasisTargetRate;
			averagingWindow = homeostasisAveragingWindow;

			preNeuronStatisticsKey = string.Format("home_{0}_{1}", targetRate, averagingWindow);
			postNeuronStatisticsKey = string.Format("home_{0}_{1}", targetRate, averagingWindow);
		}

		public ILearningRule GetCopy()
		{
			return new HomeostasisLearningRule((STDPLearningRule)stdp.GetCopy(),
				this.alpha, this.targetRate, this.averagingWindow);
		}

		public NeuronStatistics GetPostNeuronStatistics(Neuron neuron)
		{
			return new HomeostasisNeuronStatistics(neuron, 
				(STDPNeuronStatistics)stdp.GetPostNeuronStatistics(neuron), targetRate, averagingWindow);
		}

		public string GetPostNeuronStatisticsKey()
		{
			return postNeuronStatisticsKey;
		}

		public NeuronStatistics GetPreNeuronStatistics(Neuron neuron)
		{
			return new HomeostasisNeuronStatistics(neuron,
				(STDPNeuronStatistics)stdp.GetPreNeuronStatistics(neuron), targetRate, averagingWindow);
		}

		public string GetPreNeuronStatisticsKey()
		{
			return preNeuronStatisticsKey;
		}

		public float GetWeightChange(Synapse synapse)
		{
			ArrayList homePreStat = synapse.PreNeuron.GetStatistics(preNeuronStatisticsKey).GetData();
			ArrayList homePostStat = synapse.PostNeuron.GetStatistics(postNeuronStatisticsKey).GetData();

			float div = (float)homePostStat[0];
			float k = (float)homePostStat[1];
			float dw = stdp.GetWeightChange(synapse, (List<int>)homePreStat[2], (List<int>)homePostStat[2]);
			float hm = (alpha * synapse.Weight * div + dw) * k;
			//Dynamic reduction
			alpha *= 0.99f;
			//Console.WriteLine();
			//Console.WriteLine("Alpha = " + alpha);
			//Console.WriteLine();
			return hm;
		}
	}
}
