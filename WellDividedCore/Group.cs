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
	}
}
