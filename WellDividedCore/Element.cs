using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class Element
	{
		public Dictionary<Attribute, Attribute.AttributeInstance> Attributes { get; } = new Dictionary<Attribute, Attribute.AttributeInstance>();
	}
}
