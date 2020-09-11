using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WellDividedCore.Utility
{
	static class IEnumerableExtensions
	{
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
