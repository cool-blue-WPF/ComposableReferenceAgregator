using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
		}
	}
}
