using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Divider divider = new Divider();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void loadDataButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				attributesSettings.Children.Clear();
				runButton.IsEnabled = false;
				var path = dialog.FileName;

				char separator;
				if (inputSeparatorTextBox.Text != "")
				{
					separator = inputSeparatorTextBox.Text[0];
				}
				else
				{
					separator = '\t';
				}
				try
				{
					divider.LoadDataFromText(path, separator);
				}
				catch(Exception exception)
				{
					MessageBox.Show(exception.Message, "Error loading data", MessageBoxButton.OK);
					return;
				}

				runButton.IsEnabled = true;

				UpdateAttributesSettings();
			}
		}

		// TODO: merge to one common parent, so there are not two lists
		private List<GeneralAttributeSettings> generalAttributes;
		private List<NumberAttributeSettings> numberAttributes;
		private void UpdateAttributesSettings()
		{
			generalAttributes = new List<GeneralAttributeSettings>();
			numberAttributes = new List<NumberAttributeSettings>();
			for (int i = 0; i < divider.Attributes.Count; i++)
			{
				UIElement uIElement;
				var attribute = divider.Attributes[i];
				if (attribute is GeneralAttribute)
				{
					GeneralAttributeSettings generalAttribute = new GeneralAttributeSettings((GeneralAttribute)attribute);
					generalAttributes.Add(generalAttribute);
					uIElement = generalAttribute;
				}
				else if (attribute is NumberAttribute)
				{
					NumberAttributeSettings numberAttribute = new NumberAttributeSettings((NumberAttribute)attribute);
					numberAttributes.Add(numberAttribute);
					uIElement = numberAttribute;
				}
				else
				{
					throw new InvalidOperationException("The UI does not support the given attribute type.");
				}

				attributesSettings.Children.Add(uIElement);
			}
		}

		private void runButton_Click(object sender, RoutedEventArgs e)
		{
			int groupCount;
			if (groupCountTextBox.Text != "")
			{
				groupCount = int.Parse(groupCountTextBox.Text);
				if (groupCount < 2)
				{
					MessageBox.Show("There have to be at least 2 groups.", "Error starting the algorithm", MessageBoxButton.OK);
					return;
				}
			}
			else
			{
				MessageBox.Show("You did not choose how many groups the elements should be divided into.", "Error starting the algorithm", MessageBoxButton.OK);
				return;
			}

			bool elementsCountsBalanced = balanceElementsCountsCheckBox.IsChecked.HasValue ? balanceElementsCountsCheckBox.IsChecked.Value : false;

			try
			{
				(var balancedAttributes, var importanceValues, var evaluateBies) = GetBalancedAttributes();

				divider.UpdateSettings(groupCount, elementsCountsBalanced, balancedAttributes, importanceValues, evaluateBies);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Error setting the data for the algorithm", MessageBoxButton.OK);
				return;
			}

			SetShownAttributes();
			
			runButton.IsEnabled = false;
			Thread algorithmThread = new Thread(() => 
			{ 
				divider.Divide();
				DisplaySolution(divider.FinalSolution);
				Dispatcher.Invoke(() =>
				{
					runButton.IsEnabled = true;
					saveButton.IsEnabled = true;
				});
			})
			{
				IsBackground = true
			};
			algorithmThread.Start();
		}

		private List<Attribute> shownAttributes;

		private void SetShownAttributes()
		{
			shownAttributes = new List<Attribute>();
			foreach (var attribute in generalAttributes)
			{
				if (attribute.shownCheckBox.IsChecked.HasValue && attribute.shownCheckBox.IsChecked.Value == true)
					shownAttributes.Add(attribute.GeneralAttribute);
			}
			foreach (var attribute in numberAttributes)
			{
				if (attribute.shownCheckBox.IsChecked.HasValue && attribute.shownCheckBox.IsChecked.Value == true)
					shownAttributes.Add(attribute.NumberAttribute);
			}
		}

		private void DisplaySolution(Solution finalSolution)
		{
			// TODO: implement
		}

		private (List<Attribute>, List<int>, List<EvaluateBy>) GetBalancedAttributes()
		{
			// TODO: this is ugly
			var result = (new List<Attribute>(), new List<int>(), new List<EvaluateBy>());
			foreach (var attribute in generalAttributes)
			{
				if(attribute.balancedCheckBox.IsChecked.HasValue && attribute.balancedCheckBox.IsChecked.Value == true)
				{
					result.Item1.Add(attribute.GeneralAttribute);
					result.Item2.Add((int)attribute.importanceSlider.Value);
					result.Item3.Add(0); // just pasting a random value that will not be used
				}
			}

			foreach (var attribute in numberAttributes)
			{
				if (attribute.balancedCheckBox.IsChecked.HasValue && attribute.balancedCheckBox.IsChecked.Value == true)
				{
					result.Item1.Add(attribute.NumberAttribute);
					result.Item2.Add((int)attribute.importanceSlider.Value);
					result.Item3.Add(attribute.GetEvaluateBy());
				}
			}

			return result;
		}

		private static readonly Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
		private void groupsCountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = regex.IsMatch(e.Text);
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
			{
				var path = dialog.FileName;

				char separator;
				if (outputSeparatorTextBox.Text != "")
				{
					separator = outputSeparatorTextBox.Text[0];
				}
				else
				{
					separator = '\t';
				}
				try
				{
					divider.SaveDataToText(path, separator);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.Message, "Error saving data", MessageBoxButton.OK);
				}
			}
		}
	}
}
