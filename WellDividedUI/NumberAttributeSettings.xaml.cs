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

namespace WellDividedUI
{
	/// <summary>
	/// Interaction logic for NumberAttributeSettings.xaml
	/// </summary>
	public partial class NumberAttributeSettings : UserControl
	{
		internal NumberAttribute NumberAttribute { get; private set; }

		private static readonly string[] evaluetedBySource = new string[] { "Balanced by sum", "Balanced by average"};
		
		internal NumberAttributeSettings(NumberAttribute numberAttribute)
		{
			InitializeComponent();

			NumberAttribute = numberAttribute;

			attributeNameLabel.Content = numberAttribute.Name;

			evaluatedByComboBox.ItemsSource = evaluetedBySource;
		}

		internal EvaluateBy GetEvaluateBy()
		{
			if ((string)evaluatedByComboBox.SelectedItem == evaluetedBySource[0])
			{
				return EvaluateBy.Sum;
			}
			else if ((string)evaluatedByComboBox.SelectedItem == evaluetedBySource[1])
			{
				return EvaluateBy.Average;
			}
			throw new InvalidOperationException("Not chosen what to balance by.");
		}

		private void balancedCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			importanceLabel.IsEnabled = true;
			importanceSlider.IsEnabled = true;
			importanceValue.IsEnabled = true;
			evaluatedByComboBox.IsEnabled = true;
		}

		private void balancedCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			importanceLabel.IsEnabled = false;
			importanceSlider.IsEnabled = false;
			importanceValue.IsEnabled = false;
			evaluatedByComboBox.IsEnabled = false;
		}
	}
}
