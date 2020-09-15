using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WellDividedCore.Utility
{
	static class IEnumerableExtensions
	{
		/// <summary>
		/// Calculates the standard deviation of the given data. The expected value must be known before-hand.
		/// It uses the N variant, not the N - 1. This means that the method expects the data to include all the data, not just a sample.
		/// </summary>
		/// <param name="source">Some numbers.</param>
		/// <param name="expectedValue">The expected value (average) of the numbers.</param>
		/// <returns></returns>
		public static float StandardDeviation(this IEnumerable<float> source, float expectedValue)
		{
			var sum = 0f;
			var count = 0;
			foreach (var number in source)
			{
				sum += (number - expectedValue) * (number - expectedValue);
				count++;
			}

			return (float)Math.Sqrt(sum / count);
		}
	}
}
