using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WellDividedCore.Utility;

namespace WellDividedCore
{
	/// <summary>
	/// A general attribute that is balanced by the number of appearances of each of the instances.
	/// </summary>
	public class GeneralAttribute : Attribute
	{
		public GeneralAttribute() : base() { }

		private Dictionary<string, float> expectedValues;

		internal override void SetExpectations(List<Element> elements, int groupCount)
		{
			expectedValues = new Dictionary<string, float>();
			for (int i = 0; i < elements.Count; i++)
			{
				var value = ((GeneralAttributeInstance)elements[i].Attributes[this]).Value;
				if (expectedValues.ContainsKey(value))
				{
					expectedValues[value] += 1f;
				}
				else
				{
					expectedValues[value] = 1f;
				}
			}

			var expectedValuesKeys = new List<string>(expectedValues.Keys);
			for (int i = 0; i < expectedValuesKeys.Count; i++)
			{
				expectedValues[expectedValuesKeys[i]] /= groupCount;
			}
		}

		internal override float Evaluate(Group[] groups)
		{
			Dictionary<string, int>[] instanceCounts = new Dictionary<string, int>[groups.Length];
			for (int i = 0; i < instanceCounts.Length; i++)
			{
				instanceCounts[i] = new Dictionary<string, int>();
			}

			for (int i = 0; i < groups.Length; i++)
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
			foreach (var key in expectedValues.Keys)
			{
				evaluatedInstances[key] = EvaluateInstance(key, instanceCounts);
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
			float[] values = new float[instanceCounts.Length];
			for (int i = 0; i < instanceCounts.Length; i++)
			{
				if(instanceCounts[i].TryGetValue(instanceValue, out int value))
				{
					values[i] = value;
				}
				else
				{
					values[i] = 0f;
				}	
			}

			var expectedValue = expectedValues[instanceValue];
			var deviation = values.StandardDeviation(expectedValue);

			// Expected values exist only for instances present, so they are always > 0
			return Math.Max(0f, 1f - (deviation / expectedValue));
		}

		// TODO: move to a generic factory (maybe)
		internal override AttributeInstance GetInstance(string value)
		{
			return new GeneralAttributeInstance(value);
		}

		/// <summary>
		/// Instance of a <see cref="GeneralAttribute"/>. Basically a string.
		/// </summary>
		public class GeneralAttributeInstance : AttributeInstance
		{
			internal string Value { get; }

			public GeneralAttributeInstance(string value)
			{
				Value = value;
			}

			public override int CompareTo(AttributeInstance other)
			{
				return Value.CompareTo(((GeneralAttributeInstance)other).Value);
			}

			public override string ToString()
			{
				return Value;
			}

		}
	}
}
