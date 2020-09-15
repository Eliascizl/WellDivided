using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace WellDividedCore
{
	/// <summary>
	/// The columns of the table are attributes.
	/// By these the elements are divided into groups.
	/// </summary>
	public abstract class Attribute
	{
		public string Name { get; set; }

		public int Importance { get; internal set; }

		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Preparation needed for the Evaluation.
		/// </summary>
		/// <param name="elements"></param>
		/// <param name="groupCount"></param>
		internal abstract void SetExpectations(List<Element> elements, int groupCount);

		/// <summary>
		/// Evaluates the given group for balancing.
		/// Be sure to have run <see cref="SetExpectations(List{Element}, int)"/> before.
		/// </summary>
		/// <param name="groups">The groups to be evaluated.</param>
		/// <returns>A float between 0 and 1.</returns>
		internal abstract float Evaluate(Group[] groups);

		/// <summary>
		/// Returns the <see cref="Attribute"/> of the given type if the two given strings match.
		/// </summary>
		/// <typeparam name="TAttribute">Attribute type to return.</typeparam>
		/// <param name="setting">First string - considered the initial "setting".</param>
		/// <param name="toCheck">Second string - the one to be checked.</param>
		/// <returns>A new empty <see cref="Attribute"> of the given type."/></returns>
		internal static TAttribute GetAttribute<TAttribute>(string setting, string toCheck) where TAttribute : Attribute, new()
		{
			if (setting == toCheck)
				return new TAttribute();
			return null;
		}

		internal abstract AttributeInstance GetInstance(string value);

		/// <summary>
		/// Each element shoud have instances of all their attributes. This is the base class for such instances.
		/// When inheriting, you should add a distinct "value" field which has the data type as your attribute directs.
		/// </summary>
		public abstract class AttributeInstance : IComparable<AttributeInstance>
		{
			public abstract int CompareTo(AttributeInstance other);
		}
	}
}
