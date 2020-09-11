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
		private List<Element> elements = new List<Element>();
		private List<Attribute> attributes = new List<Attribute>();

		private List<Attribute.AttributeFactory> possibleAttributes = new List<Attribute.AttributeFactory>() { new Attribute.AttributeFactoryConcrete<GeneralAttribute>(""), new Attribute.AttributeFactoryConcrete<NumberAttribute>("num") };

		/// <summary>
		/// Get a CSV table and create a list of elements and a list of attributes. Fill the elements with the instances of their attributes.
		/// First row of the table should specify the names of the attributes.
		/// Second row of the table should specify the data types of the attributes: "num" for a numeric attribute or empty for a general text attribute.
		/// </summary>
		/// <param name="path">System path to the file</param>
		/// <exception cref="IOException">Thrown when there is a problem with loading.</exception>
		public void LoadDataFromText(string path, char separator = '\t')
		{
			StreamReader reader = new StreamReader(path);

			var names = reader.ReadLine().Split(separator);
			var types = reader.ReadLine().Split(separator);
			for (int i = 0; i < types.Length; i++)
			{
				if(names[i] == "")
				{
					continue;
				}

				Attribute attribute = null;
				var enumerator = possibleAttributes.GetEnumerator();
				while (attribute == null)
				{
					enumerator.MoveNext();
					attribute = enumerator.Current.GetAttribute(types[i]);
				}
				attribute.Name = names[i];
				attributes.Add(attribute);
			}

			string line;
			while((line = reader.ReadLine()) != null)
			{
				Element element = new Element();

				var data = line.Split(separator);
				for (int i = 0; i < attributes.Count; i++)
				{
					element.Attributes.Add(attributes[i], attributes[i].GetInstance(data[i]));
				}

				elements.Add(element);
			}

			reader.Close();
		}

		private int groupCount;
		internal List<Attribute> BalancedAttributes { get; private set; } = new List<Attribute>();

		public void UpdateSettings()
		{

		}

		private Solution finalSolution;

		public void Divide()
		{

		}

		/// <summary>
		/// Saves the last found solution to the given file.
		/// </summary>
		/// <param name="path">Where to save the output data.</param>
		/// <param name="separator">What symbol should separate the columns in the output data.</param>
		/// <exception cref="IOException">Thrown when there is a problem with loading.</exception>
		public void SaveDataToText(string path, char separator = '\t')
		{
			StreamWriter writer = new StreamWriter(path);

			StringBuilder builder = new StringBuilder();
			builder.Append("Group ID");
			for (int i = 0; i < attributes.Count; i++)
			{
				builder.Append(separator);
				builder.Append(attributes[i].Name);
			}
			writer.WriteLine(builder.ToString());

			for (int i = 0; i < finalSolution.Groups.Count; i++)
			{
				for (int j = 0; j < finalSolution.Groups[i].Elements.Count; j++)
				{
					builder = new StringBuilder();
					builder.Append(i);
					foreach (var attribute in attributes)
					{
						builder.Append(separator);
						builder.Append(finalSolution.Groups[i].Elements[j].Attributes[attribute]);
					}
					writer.WriteLine(builder.ToString());
				}
			}

			writer.Close();
		}
	}
}
