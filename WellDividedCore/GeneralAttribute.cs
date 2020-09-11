using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
					var attributeInstance = (GeneralAttributeInstance)groups[i].Elements[j].Attributes[this];
					var attributeValue = attributeInstance.Value;
					if (instanceCounts[i].TryGetValue(attributeValue, out int value))
					{
						instanceCounts[i][attributeValue] = value + 1;
					} 
					else
					{
						instanceCounts[i][attributeValue] = 1;
					}
				}
			}

			Dictionary<string, float> evaluatedInstances = new Dictionary<string, float>();
			for (int i = 0; i < instanceCounts.Length; i++)
			{
				foreach(var key in instanceCounts[i].Keys)
				{
					if(!evaluatedInstances.ContainsKey(key))
					{
						evaluatedInstances[key] = EvaluateInstance(key, instanceCounts);
					}
				}
			}

			float totalScore = 0;
			foreach (var instanceScore in evaluatedInstances.Values)
			{
				totalScore += instanceScore;
			}
			totalScore /= evaluatedInstances.Count;

			return totalScore;
		}

		/// <summary>
		/// Evaluates how much a given string is balanced - how much it appears throughout all groups, not just one.
		/// </summary>
		/// <param name="instanceValue">The current key to count appearances of.</param>
		/// <param name="instanceCounts">The counts of the appearances in the particular groups.</param>
		/// <returns>The balance score for the given key (instance). In the range of 0 to 1.</returns>
		private float EvaluateInstance(string instanceValue, Dictionary<string, int>[] instanceCounts)
		{

			return 0f;
		}

		// TODO: move to a generic factory
		public override AttributeInstance GetInstance(string value)
		{
			return new GeneralAttributeInstance(value);
		}

		public class GeneralAttributeInstance : AttributeInstance
		{
			public string Value { get; internal set; }

			public GeneralAttributeInstance(string value)
			{
				Value = value;
			}
		}
	}
}
