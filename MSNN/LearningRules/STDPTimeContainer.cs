using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN.LearningRules
{
	[Serializable]
	class STDPTimeContainer
	{
		private List<int> head;
		private List<int> tail;
		private int delay;

		public STDPTimeContainer(List<int> headValues, List<int> tailValues, int delay = 0)
		{
			head = headValues;
			tail = tailValues;
			this.delay = delay;
		}

		public STDPTimeContainer(List<int> tailValues)
		{
			tail = tailValues;
		}

		public int this[int idx]
		{
			get
			{
				if (idx < 0 || idx >= Count)
					throw new IndexOutOfRangeException();
				if (head != null)
				{
					if (idx < head.Count)
						return head[head.Count - idx - 1] /*they are in reverse*/ + delay;
					return tail[idx - head.Count] + delay;
				}
				return tail[idx] + delay;
			}
		}

		public int Count
		{
			get
			{
				int cnt = 0;
				if (head != null)
					cnt += head.Count;
				if (tail != null)
					cnt += tail.Count;
				return cnt;
			}
		}
	}
}
