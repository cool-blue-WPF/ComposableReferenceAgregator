using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Binding.Annotations;

namespace Binding
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ObservableCollection<User> users = new ObservableCollection<User>();

		public MainWindow ()
		{
			InitializeComponent();

			users.Add(new User() {Name = "John Doe"});
			users.Add(new User() {Name = "Jane Doe"});

			lbUsers.ItemsSource = users;
		}

		private void btnAddUser_Click(object sender, RoutedEventArgs e)
		{
			users.Add(new User() {Name = "New user"});
		}

		private void btnChangeUser_Click(object sender, RoutedEventArgs e)
		{
			if (lbUsers.SelectedItem != null)
				(lbUsers.SelectedItem as User).Name = "Random Name";
		}

		private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
		{
			if (lbUsers.SelectedItem != null)
				users.Remove(lbUsers.SelectedItem as User);
		}
	}
		
	//this works for changes that affect the collection but not
	//for changes to the collection elements themselves
	//public class User
	//{
	//		public string Name { get; set; }
	//}


	//implementing INotifyPropertyChanged informs the UI to subscribe
	//to the public event in this class and update the UI on changes
	public class User : INotifyPropertyChanged
	{
		private string _name;
		public string Name
		{
			get { return this._name; }
			set
			{
				if (this._name != value)
				{
					this._name = value;
					this.NotifyPropertyChanged();
				}
			}
		}
		
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		public void NotifyPropertyChanged (
			[CallerMemberName] string propName = null)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}
