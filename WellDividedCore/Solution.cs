using System;
using System.Collections.Generic;
using System.Text;

namespace WellDividedCore
{
	public class Solution
	{
		private Divider divider;
		public List<Group> Groups { get; private set; }

		public Solution(Divider divider)
		{
			this.divider = divider;
		}

		public float Evaluate()
		{
			// 

			// evaluate the balance of each attribute separately and apply the importance
			float score = 0f;
			for (int i = 0; i < divider.BalancedAttributes.Count; i++)
			{
				score += divider.BalancedAttributes[i].Evaluate(Groups) * divider.BalancedAttributes[i].Importance;
			}

			return 0f;
		}
	}
}
