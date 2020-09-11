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
		public bool ToBalance { get; private set; }

		public int Importance { get; private set; }

		protected Attribute()
		{
			
		}

		public abstract AttributeInstance GetInstance(string value);

		/// <summary>
		/// Each element shoud have instances of all their attributes. This is the base class for such instances.
		/// When inheriting, you should add a distinct "value" field which has the data type as your attribute directs.
		/// </summary>
		public abstract class AttributeInstance
		{

		}

		public abstract class AttributeFactory
		{
			protected string typeName;

			public AttributeFactory(string typeName)
			{
				this.typeName = typeName;
			}

			public abstract Attribute GetAttribute(string typeName);
		}

		public class AttributeFactoryConcrete<TAttribute> : AttributeFactory where TAttribute : Attribute, new()
		{
			public AttributeFactoryConcrete(string typeName) : base(typeName)
			{

			}

			public override Attribute GetAttribute(string typeName)
			{
				if (this.typeName == typeName)
					return new TAttribute();
				return null;
			}
		}
	}
}
