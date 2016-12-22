using System.Windows;
using CollectionBinding;

namespace APTest.Spec
{
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent();
			this.Loaded += (s, e) =>
			{
				TestParent.RefreshAsync(Test);
			};
		}
	}
}
