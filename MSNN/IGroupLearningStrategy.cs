using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	public interface IGroupLearningStrategy
	{
		void ApplyLearning(Group group);
	}
}
