using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class GeneralAttribute : Attribute
	{
		public GeneralAttribute() : base() { }

		// TODO: move to a generic factory
		public override AttributeInstance GetInstance(string value)
		{
			return new GeneralAttributeInstance(value);
		}

		public class GeneralAttributeInstance : AttributeInstance
		{
			public string Value { get; }

			public GeneralAttributeInstance(string value)
			{
				Value = value;
			}
		}
	}
}
