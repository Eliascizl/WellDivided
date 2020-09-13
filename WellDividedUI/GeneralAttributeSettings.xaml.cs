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
	/// Interaction logic for AttributeSettings.xaml
	/// </summary>
	public partial class GeneralAttributeSettings : UserControl
	{
		internal GeneralAttribute GeneralAttribute { get; private set; }

		public GeneralAttributeSettings(GeneralAttribute generalAttribute)
		{
			InitializeComponent();

			this.GeneralAttribute = generalAttribute;
			attributeNameLabel.Content = generalAttribute.Name;

		}


		private void balancedCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			importanceLabel.IsEnabled = true;
			importanceSlider.IsEnabled = true;
			importanceValue.IsEnabled = true;
		}

		private void balancedCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			importanceLabel.IsEnabled = false;
			importanceSlider.IsEnabled = false;
			importanceValue.IsEnabled = false;
		}
	}
}
