using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.IO;

namespace WellDividedCore
{
	/// <summary>
	/// Main class of the core library.
	/// An instance should correspond to one usage of the algorithm. There will probably usually only be one instance.
	/// </summary>
	public class Divider
	{
		internal List<Element> Elements { get; private set; }

		public List<Attribute> Attributes { get; private set; }

		private List<Attribute.AttributeFactory> possibleAttributes = new List<Attribute.AttributeFactory>() { new Attribute.AttributeFactoryConcrete<GeneralAttribute>(""), new Attribute.AttributeFactoryConcrete<NumberAttribute>("num") };

		/// <summary>
		/// Get a CSV table and create a list of elements and a list of attributes. Fill the elements with the instances of their attributes.
		/// First row of the table should specify the names of the attributes.
		/// Second row of the table should specify the data types of the attributes: "num" for a numeric attribute or empty for a general text attribute.
		/// </summary>
		/// <param name="path">System path to the file</param>
		/// <exception cref="IOException">Thrown when there is a problem with loading.</exception>
		/// <exception cref="FormatException">Thrown when there is an error with parsing the text.</exception>
		public void LoadDataFromText(string path, char separator = '\t')
		{
			Elements = new List<Element>();
			Attributes = new List<Attribute>();

			StreamReader reader = null;
			try
			{
				reader = new StreamReader(path);

				var names = reader.ReadLine().Split(separator);
				var types = reader.ReadLine().Split(separator);
				for (int i = 0; i < types.Length; i++)
				{
					if (names[i] == "")
					{
						continue;
					}

					Attribute attribute = null;
					foreach (var possibleAttribute in possibleAttributes)
					{
						attribute = possibleAttribute.GetAttribute(types[i]);
						if (attribute != null)
							break;
					}
					
					if (attribute == null)
					{
						throw new IOException("The second row of the file contains incorrect data.");
					}

					attribute.Name = names[i];
					Attributes.Add(attribute);
				}

				string line;
				while ((line = reader.ReadLine()) != null)
				{
					Element element = new Element();

					var data = line.Split(separator);
					for (int i = 0; i < Attributes.Count; i++)
					{
						element.Attributes.Add(Attributes[i], Attributes[i].GetInstance(data[i]));
					}

					Elements.Add(element);
				}
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		}

		internal int GroupCount { get; private set; }
		internal bool ElementCountsBalanced { get; private set; }
		internal List<Attribute> BalancedAttributes { get; private set; } = new List<Attribute>();

		/// <summary>
		/// Sets the new settings for the balancing algorithm - all the attributes that should be balanced, whether to balance the counts of elements in groups etc.
		/// </summary>
		public void UpdateSettings(int groupCount, bool elementsCountsBalanced, List<Attribute> balancedAttributes, List<int> importanceValues, List<EvaluateBy> evaluateBies)
		{
			GroupCount = groupCount;
			ElementCountsBalanced = elementsCountsBalanced;
			BalancedAttributes = balancedAttributes;
			for (int i = 0; i < BalancedAttributes.Count; i++)
			{
				BalancedAttributes[i].Importance = importanceValues[i];
				if(BalancedAttributes[i] is NumberAttribute)
				{
					((NumberAttribute)balancedAttributes[i]).EvaluateBy = evaluateBies[i];
				}

				BalancedAttributes[i].SetExpectations(Elements, GroupCount);
			}
		}

		public Solution FinalSolution { get; private set; }

		public void Divide()
		{
			var solution = Solution.GenerateRandomDistributedSolution(this);

			var score = solution.Evaluate();

			for (int i = 0; i < 1_000; i++)
			{
				var newSolution = solution.RandomImprove();
				var newScore = newSolution.Evaluate();
				if (newScore > score)
				{
					solution = newSolution;
					score = newScore;
				}
			}

			FinalSolution = solution;
		}

		/// <summary>
		/// Saves the last found solution to the given file.
		/// </summary>
		/// <param name="path">Where to save the output data.</param>
		/// <param name="separator">What symbol should separate the columns in the output data.</param>
		/// <exception cref="IOException">Thrown when there is a problem with loading.</exception>
		public void SaveDataToText(string path, char separator = '\t')
		{
			StreamWriter writer = null;

			try
			{
				writer = new StreamWriter(path);

				StringBuilder builder = new StringBuilder();
				builder.Append("Group ID");
				for (int i = 0; i < Attributes.Count; i++)
				{
					builder.Append(separator);
					builder.Append(Attributes[i].Name);
				}
				writer.WriteLine(builder.ToString());

				for (int i = 0; i < FinalSolution.Groups.Length; i++)
				{
					for (int j = 0; j < FinalSolution.Groups[i].Elements.Count; j++)
					{
						builder = new StringBuilder();
						builder.Append(i);
						foreach (var attribute in Attributes)
						{
							builder.Append(separator);
							builder.Append(FinalSolution.Groups[i].Elements[j].Attributes[attribute]);
						}
						writer.WriteLine(builder.ToString());
					}
				}
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}
	}
}
