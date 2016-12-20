using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace CollectionBinding.Spec
{
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent();
			var buttons = RootStack.Children.OfType<Button>();
			foreach (var button in buttons)
			{
				var cmd = ((RoutedUICommand) button.Command).Name;
				var param = button.CommandParameter;

				button.Content = new TextBox()
				{
					Text = string.Format("hash={0} cmd: {1} param: {2}", 
					button.GetHashCode(), cmd, param),
					Margin = new Thickness(6, 0, 0, 0)
				};
				button.HorizontalAlignment = HorizontalAlignment.Stretch;
				button.HorizontalContentAlignment = HorizontalAlignment.Left;
			}

			var targets = Group.Targets;
			var defAttr = Group.TargetAttribute;
			var attrFZ = targets[0].TargetAttribute;
			Debug.WriteLine("FZ TargetAttribute {0}", (string)attrFZ);
		}
	}
}
