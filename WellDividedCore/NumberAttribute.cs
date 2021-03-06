﻿using System;
using System.Collections.Generic;
using System.Text;
using WellDividedCore.Utility;

namespace WellDividedCore
{
	/// <summary>
	/// A number attribute (any real number) that is balanced either by the sum of the instances or their average.
	/// </summary>
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
			return Math.Max(0f, 1f - (deviation / expectedValue));
		}

		// TODO: move to a generic factory
		internal override AttributeInstance GetInstance(string value)
		{
			return new NumberAttributeInstance(value);
		}

		/// <summary>
		/// An instance of a <see cref="NumberAttribute"/>. Basically a float.
		/// </summary>
		public class NumberAttributeInstance : AttributeInstance
		{
			internal float Value { get; }

			public NumberAttributeInstance(string value)
			{
				Value = float.Parse(value);
			}

			public override int CompareTo(AttributeInstance other)
			{
				return Value.CompareTo(((NumberAttributeInstance)other).Value);
			}

			public override string ToString()
			{
				return Value.ToString();
			}
		}
	}

	/// <summary>
	/// All the options of evaluation of this attribute.
	/// </summary>
	public enum EvaluateBy
	{
		Sum, Average
	}
}
