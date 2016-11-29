using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.GroupLearningStrategies
{
	public class LearningAllGroupStrategy : IGroupLearningStrategy
	{
		public void ApplyLearning(Group group)
		{
			for(int i = 0; i < group.Count; i++)
			{
				group[i].Learn();
			}
		}
	}
}
