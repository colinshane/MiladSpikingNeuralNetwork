using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSNN
{
	[Serializable]
	public class Layer
	{
		public string Name { get; private set; }
		private List<Group> groups;

		public Group this[int idx]
		{
			get
			{
				if (idx < groups.Count)
					return groups[idx];
				throw new IndexOutOfRangeException();
			}
		}

		public int Count
		{
			get
			{
				return groups.Count;
			}
		}

		public Layer(string name)
		{
			Name = name;
			groups = new List<Group>();
		}

		public int AddGroup(Group group)
		{
			groups.Add(group);
			return groups.Count - 1;
		}

		public void Learn()
		{
			foreach (Group group in groups)
				group.Learn();
		}

		public void ClearSpikeTimes()
		{
			foreach (Group group in groups)
				group.ClearSpikeTimes();
		}

		public void Update()
		{
			foreach (Group group in groups)
				group.Update();
		}

		public void PauseLearning()
		{
			foreach(Group g in groups)
				g.SetLearningOff();
		}

		public void ResumeLearning()
		{
			foreach (Group g in groups)
				g.ResumeLearning();
		}
    }
}
