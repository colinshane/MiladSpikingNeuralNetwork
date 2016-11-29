﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.LearningRules
{
	public class HomeostasisNeuronStatistics : NeuronStatistics
	{
		STDPNeuronStatistics stdpStat;
		LinkedList<int> averages;
		int chuncksCount;
		int window;
		public float TargetRate { get; private set; }
		public int AverageRate { get; private set; }

		public HomeostasisNeuronStatistics(Neuron targetNeuron, int tao, int rate, int averagingWindow)
			: this(targetNeuron, new STDPNeuronStatistics(targetNeuron, tao),
				rate, averagingWindow) { }

		public HomeostasisNeuronStatistics(Neuron targetNeuron, STDPNeuronStatistics stdpStatistics, int rate,
			int averagingWindow) : base(targetNeuron)
		{
			averages = new LinkedList<int>();
			TargetRate = (rate * averagingWindow) / 1000f;
			chuncksCount = (int)Math.Ceiling((float)averagingWindow / Network.LearningInterval);
			window = averagingWindow;
			AverageRate = 0;

			int firstFill = (int)(TargetRate / chuncksCount);
			for(int i = 0; i < chuncksCount; i++)
			{
				averages.AddLast(firstFill);
				AverageRate += firstFill;
			}

			stdpStat = stdpStatistics;
		}

		public override ArrayList GetData()
		{
			ArrayList stdpData = stdpStat.GetData();
			//returns:
			//[0]: div
			//[1]: K
			//[2]: stdp spike reservations
			float div = 1 - AverageRate / TargetRate;
			float k = 1000 * AverageRate / (window * (1 + Math.Abs(div))); //window should be in seconds

			ArrayList result = new ArrayList();
			result.Add(div);
			result.Add(k);
			result.Add(stdpData[0]);
			return result;
		}

		public override void SaveData()
		{
			stdpStat.SaveData();
			if ((Network.Time + 1) % Network.LearningInterval == 0)
			{
				//avgRate = neuron.SpikeTimes.Count;
				AverageRate -= averages.First.Value;
				AverageRate += neuron.SpikeTimes.Count;
				averages.RemoveFirst();
				averages.AddLast(neuron.SpikeTimes.Count);
			}
		}
		
	}
}
