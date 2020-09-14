using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class Group
	{
		public List<Element> Elements { get; }

		public Group()
		{
			Elements = new List<Element>();
		}

		public Group(Group group) : this()
		{
			Elements = new List<Element>(group.Elements);
		}
	}
}
