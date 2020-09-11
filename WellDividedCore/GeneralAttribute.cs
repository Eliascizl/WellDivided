using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class GeneralAttribute : Attribute
	{
		public GeneralAttribute() : base() { }

		public override float Evaluate(List<Group> groups)
		{
			Dictionary<string, int>[] instanceCounts = new Dictionary<string, int>[groups.Count];
			for (int i = 0; i < groups.Count; i++)
			{
				for (int j = 0; j < groups[i].Elements.Count; j++)
				{
					if(instanceCounts[i].TryGetValue(groups[i].Elements[j].Attributes[this], out int value))
					{

					} 
					else
					{

					}
				}
			}

			return 0f;
		}

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
