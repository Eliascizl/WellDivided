using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	/// <summary>
	/// A possible division of the elements into groups.
	/// </summary>
	public class Solution
	{
		private Divider divider;

		public Group[] Groups { get; private set; }

		public Solution(Divider divider)
		{
			this.divider = divider;
		}

		/// <summary>
		/// Creates a new solution with the groups initially set to the copies of the given groups.
		/// </summary>
		/// <param name="divider">The divider that is using this solution.</param>
		/// <param name="groups">The initial groups.</param>
		public Solution(Divider divider, Group[] groups) : this(divider)
		{
			Groups = new Group[groups.Length];
			for (int i = 0; i < Groups.Length; i++)
			{
				Groups[i] = new Group(groups[i]);
			}
		}

		private static readonly Random random = new Random();

		/// <summary>
		/// Takes the divider's elements and randomly distributes them among the groups while keeping them in their initial place.
		/// </summary>
		/// <param name="divider">The divider that is using this solution.</param>
		/// <returns>A new solution with the generated groups.</returns>
		public static Solution GenerateRandomDistributedSolution(Divider divider)
		{
			var solution = new Solution(divider);

			solution.Groups = new Group[divider.GroupCount];
			for (int i = 0; i < solution.Groups.Length; i++)
			{
				solution.Groups[i] = new Group();
			}

			var elements = new List<Element>(divider.Elements);

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

		/// <summary>
		/// Evaluates this division into groups. It uses the average of the evaluation of all the attributes (gotten from the divider) multiplied by their importance.
		/// </summary>
		/// <returns>A float between 0 and 1. NaN if there are no balanced attributes.</returns>
		public float Evaluate()
		{
			float score = 0f;
			int totalImportance = 0;
			for (int i = 0; i < divider.BalancedAttributes.Count; i++)
			{
				score += divider.BalancedAttributes[i].Evaluate(Groups) * divider.BalancedAttributes[i].Importance;
				totalImportance += divider.BalancedAttributes[i].Importance; // TODO this should stay the same throughout the algorithm so there shouldn't be a need to calculate it every time
			}

			return score / totalImportance;
		}

		/// <summary>
		/// Swaps two elements from different groups or just moves one element from one group to another - chosen by the setting whether to balance counts of elements in groups.
		/// </summary>
		/// <returns>A new solution with new changed groups.</returns>
		internal Solution RandomChange()
		{
			var solution = new Solution(divider, Groups);

			var firstGroupIndex = random.Next(Groups.Length);
			var secondGroupIndex = random.Next(Groups.Length - 1);
			if (secondGroupIndex >= firstGroupIndex)
				secondGroupIndex++;

			var firstList = solution.Groups[firstGroupIndex].Elements;
			var secondList = solution.Groups[secondGroupIndex].Elements;

			var firstElementIndex = random.Next(firstList.Count);
			
			var swappedElement = firstList[firstElementIndex];
			
			if (divider.ElementCountsBalanced)
			{
				var secondElementIndex = random.Next(secondList.Count);

				firstList[firstElementIndex] = secondList[secondElementIndex];
				secondList[secondElementIndex] = swappedElement;
			}
			else
			{
				firstList.RemoveAt(firstElementIndex);
				secondList.Add(swappedElement);
			}

			return solution;
		}
	}
}
