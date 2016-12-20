using System.Windows;

namespace APTest.Spec
{
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent();
			this.Loaded += (s, e) =>
			{
				Test.Refresh();
			};
		}
	}
}
