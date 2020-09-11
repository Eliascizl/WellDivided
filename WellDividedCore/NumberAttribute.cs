using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class NumberAttribute : Attribute
	{
		public NumberAttribute() : base() { }

		// TODO: move to a generic factory
		public override AttributeInstance GetInstance(string value)
		{
			return new NumberAttributeInstance(value);
		}

		public class NumberAttributeInstance : AttributeInstance
		{
			public int Value { get; }

			public NumberAttributeInstance(string value)
			{
				Value = int.Parse(value);
			}

		}
	}
}
