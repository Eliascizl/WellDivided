using System;
using System.Collections.Generic;
using System.Text;
using WellDividedCore.Utility;

namespace WellDividedCore
{
	public class NumberAttribute : Attribute
	{
		public NumberAttribute() : base() { }

		public EvaluateBy EvaluateBy { get; internal set; }

		private float expectedSum;
		private float expectedAverage;

		internal override void SetExpectations(List<Element> elements, int groupCount)
		{
			var sum = 0f;
			for (int i = 0; i < elements.Count; i++)
			{
				var value = ((NumberAttributeInstance)elements[i].Attributes[this]).Value;
				sum += value;
			}

			expectedSum = sum / groupCount;
			expectedAverage = sum / elements.Count;
		}

		internal override float Evaluate(Group[] groups)
		{
			float[] values = new float[groups.Length];
			for (int i = 0; i < groups.Length; i++)
			{
				var sum = 0f;
				for (int j = 0; j < groups[i].Elements.Count; j++)
				{
					var value = ((NumberAttributeInstance)groups[i].Elements[j].Attributes[this]).Value;
					sum += value;
				}
				values[i] = sum;
			}

			float expectedValue;
			if (EvaluateBy == EvaluateBy.Average)
			{
				for (int i = 0; i < groups.Length; i++)
				{
					values[i] /= groups[i].Elements.Count;
				}
				expectedValue = expectedAverage;
			}
			else if (EvaluateBy == EvaluateBy.Sum)
			{
				expectedValue = expectedSum;
			}
			else
			{
				throw new InvalidOperationException("The " + nameof(EvaluateBy) + " property is not set.");
			}

			var deviation = values.StandardDeviation(expectedValue);
			// Expected value is either the sum or the sum divided by something. The sum is always positive, so the expected value is always > 0
			return Math.Min(0f, 1f - (deviation / expectedValue));
		}

		// TODO: move to a generic factory
		internal override AttributeInstance GetInstance(string value)
		{
			return new NumberAttributeInstance(value);
		}

		public class NumberAttributeInstance : AttributeInstance
		{
			public float Value { get; }

			public NumberAttributeInstance(string value)
			{
				Value = float.Parse(value);
			}

		}
	}

	public enum EvaluateBy
	{
		Sum, Average
	}
}
