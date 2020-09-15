using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WellDividedCore;
using Attribute = WellDividedCore.Attribute;

namespace WellDividedUI
{
	/// <summary>
	/// Interaction logic for GroupResult.xaml
	/// </summary>
	public partial class GroupResult : UserControl
	{
		private Group group;
		private int groupID;

		public GroupResult(Group group, int groupID)
		{
			InitializeComponent();

			this.group = group;
			this.groupID = groupID;
		}

		internal void DisplayGroup(List<Attribute> shownAttributes, Attribute sortedAttribute)
		{
			overviewLabel.Content = $"Group ID: {groupID}\nNumber of elements: {group.Elements.Count}";

			// TODO: wouldn't have to modify the group
			if(sortedAttribute != null)
				group.Elements.Sort((x, y) => x.Attributes[sortedAttribute].CompareTo(y.Attributes[sortedAttribute]));

			string[] elementsText = new string[group.Elements.Count];

			for (int i = 0; i < elementsText.Length; i++)
			{
				StringBuilder builder = new StringBuilder();

				for (int j = 0; j < shownAttributes.Count; j++)
				{
					builder.Append(group.Elements[i].Attributes[shownAttributes[j]].ToString());
					builder.Append(' ');
				}

				elementsText[i] = builder.ToString();
			}

			elementsListBox.ItemsSource = elementsText;
		}
	}
}
