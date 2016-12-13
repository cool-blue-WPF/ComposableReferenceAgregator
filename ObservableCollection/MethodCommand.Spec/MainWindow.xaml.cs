using System.ComponentModel;
using System.Windows;
using Util;

namespace MethodCommand.Spec
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new File() {Name = "TestName", Target = Target};
		}
	}

	public class File : INotifyPropertyChanged

	{
		string _name;

		public string Name

		{
			get { return _name; }

			set

			{
				_name = value;

				if (PropertyChanged != null)

					PropertyChanged(this,
						new PropertyChangedEventArgs("Name"));
			}
		}

		public object Target { get; set; }

		public void Write(string newName)

		{
			Utility.SetMemberByName(Target, "Text", newName);
		}


		public event PropertyChangedEventHandler PropertyChanged;
	}
}