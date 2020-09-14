using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class Solution
	{
		private Divider divider;
		public Group[] Groups { get; private set; }

		public Solution(Divider divider)
		{
			this.divider = divider;
		}

		/// <summary>
		/// Takes the divider's elements and randomly distributes them among the groups
		/// </summary>
		/// <param name="divider"></param>
		/// <returns></returns>
		public static Solution GenerateRandomDistributedSolution(Divider divider)
		{
			var solution = new Solution(divider);

			solution.Groups = new Group[divider.GroupCount];
			for (int i = 0; i < solution.Groups.Length; i++)
			{
				solution.Groups[i] = new Group();
			}

			var elements = new List<Element>(divider.Elements);

			Random random = new Random();
			var groupIndex = 0;
			while (elements.Count > 0)
			{
				var elementIndex = random.Next(elements.Count);
				solution.Groups[groupIndex].Elements.Add(elements[elementIndex]);
				elements.RemoveAt(elementIndex);

				if (groupIndex == solution.Groups.Length - 1)
					groupIndex = 0;
				else
					groupIndex++;
			}

			return solution;
		}

		public float Evaluate()
		{
			float score = 0f;
			for (int i = 0; i < divider.BalancedAttributes.Count; i++)
			{
				score += divider.BalancedAttributes[i].Evaluate(Groups) * divider.BalancedAttributes[i].Importance;
			}

			return 0f;
		}
	}
}
