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
		private List<Element> elements;
		private List<Attribute> attributes;

		/// <summary>
		/// Get a CSV table and create a list of elements and a list of attributes.
		/// First row of the table should specify the names of the attributes.
		/// Second row of the table should specify the data types of the attributes: "num" for a numeric attribute or empty for a general text attribute.
		/// </summary>
		/// <param name="path">System path to the file</param>
		/// <exception cref="IOException">Thrown when there is a problem with loading.</exception>
		public void LoadDataFromText(string path, char delimiter = '\t')
		{

		}
	}
}
