using System;
using System.Diagnostics;
using System.Windows;

namespace CollectionBinding.Spec
{
	public partial class MainWindow : Window
	{
		public void OnItitialised (object sender, EventArgs args)
		{
			Debug.WriteLine(String.Format("\n{0} Initialised\n", 
				((FrameworkElement)sender).Name));
				
		}
		public MainWindow ()
		{
			InitializeComponent();
		}

	}
}
