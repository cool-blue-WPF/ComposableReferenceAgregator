using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CollectionBinding.Spec
{
	public partial class MainWindow : Window
	{
		public void OnItitialised (object sender, EventArgs args)
		{
			string hostName, targetAtt, itemName;
			var target = args as State;
			if (target != null)
			{
				hostName = target.Target.Host;
				targetAtt = target.Target.TargetAttr ?? "nothing";
				itemName = target.Target.targetName ?? "nothing";
			}
			else
			{
				hostName = targetAtt = itemName = "nothing";
			}
			Debug.WriteLine("\n^{0} from {1} targetting {2} on {3}^\n",
				((FrameworkElement)sender).Name, hostName, targetAtt, itemName);
				
		}
		public MainWindow ()
		{
			InitializeComponent();
			var buttons = RootStack.Children.OfType<Button>();
			foreach (var button in buttons)
			{
				button.Content = new TextBox()
				{
					Text = "hash=" + button.GetHashCode(),
					Margin = new Thickness(6, 0, 0, 0)
				};
				button.HorizontalAlignment = HorizontalAlignment.Stretch;
				button.HorizontalContentAlignment = HorizontalAlignment.Left;
				
			}
		}
	}
}
