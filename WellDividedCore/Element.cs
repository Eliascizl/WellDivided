using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	/// <summary>
	/// An element - a row of the table. It has instances of all the attributes.
	/// </summary>
	public class Element
	{
		public Dictionary<Attribute, Attribute.AttributeInstance> Attributes { get; } = new Dictionary<Attribute, Attribute.AttributeInstance>();
	}
}
