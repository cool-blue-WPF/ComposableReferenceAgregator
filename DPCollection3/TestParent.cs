using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace CollectionBinding
{
	public class TestParent : Label
	{
		#region refresh
		private static void Refresh (TestParent instance)
		{
			instance.Content = string.Format("parent value:\t{0}", instance.AttachedString);
			foreach (var myItem in instance.MyItems)
			{
				instance.Content += string.Format("\n  child value:\t{0}\t{1}",
								myItem.AttachedString, myItem.UnAttachedString);
			}
			instance.Content += string.Format("\n Attached DP References are{0} equal",
				TestParent.AttachedStringProperty == TestChild.AttachedStringProperty
				? "" : " NOT");

		}

		public static void RefreshAsync(TestParent tp)
		{
			tp.Dispatcher.InvokeAsync(() => Refresh(tp));
		}

		private void OnPropertyChanged (object oldvalue, object newvalue)
		{
			if (oldvalue != null && oldvalue.Equals(newvalue)) return;
			RefreshAsync(this);
		}

		#endregion

		#region Inheritable AP string AttachedString

		public static readonly
			DependencyProperty AttachedStringProperty =
				DependencyProperty.RegisterAttached(
					"AttachedString", typeof(string),
					typeof(TestParent),
					new FrameworkPropertyMetadata("Not Set in Parent",
						FrameworkPropertyMetadataOptions.Inherits, OnStringChanged));

		public static void SetAttachedString(DependencyObject target, string value)
		{
			target.SetValue(AttachedStringProperty, value);
		}

		public static string GetAttachedString(DependencyObject target)
		{
			return (string) target.GetValue(AttachedStringProperty);
		}

		private static void OnStringChanged(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var instance = d as TestParent;
			if (instance == null) return;
			instance.OnPropertyChanged(e.OldValue, e.NewValue);
		}

		public string AttachedString
		{
			get { return GetAttachedString(this); }
			set { SetAttachedString(this, (string) value); }
		}

		#endregion

		#region DP ObservableCollection<TestChild> MyItems

		public static readonly DependencyProperty MyItemsProperty =
			DependencyProperty.Register(
				"MyItems", typeof(ObservableCollection<TestChild>),
				typeof(TestParent),
				new PropertyMetadata(default(ObservableCollection<TestChild>)));

		public ObservableCollection<TestChild> MyItems
		{
			get { return (ObservableCollection<TestChild>) GetValue(MyItemsProperty); }
			set { SetValue(MyItemsProperty, value); }
		}

		private void MyItem_Changed(object s, PropertyChangedEventArgs args )
		{
			var e = args as MyPropertyChangedEventArgs;
			if (e == null)
				OnPropertyChanged(new object(), new object());
			else
				OnPropertyChanged(e.OldValue, e.NewValue);
		}

		void MyItems_Changed(object d, NotifyCollectionChangedEventArgs e)
		{
			var addedItems = e.NewItems as IList;
			var deletedItems = e.OldItems as IList;

			if (addedItems != null)
			{
				foreach (var addedItem in addedItems)
				{
					this.AddLogicalChild((TestChild) addedItem);
					((TestChild)addedItem).PropertyChanged += MyItem_Changed;
				}
			}
			if (deletedItems != null)
			{
				foreach (var deletedItem in deletedItems)
				{
					this.RemoveLogicalChild((TestChild) deletedItem);
					((TestChild)deletedItem).PropertyChanged -= MyItem_Changed;
				}
			}
		}

		#endregion

		// Connect to Logical Tree
		protected override IEnumerator LogicalChildren
		{
			get { return MyItems.GetEnumerator(); }
		}

		public TestParent()
		{
			MyItems = new ObservableCollection<TestChild>();
			MyItems.CollectionChanged += MyItems_Changed;
		}

		static TestParent()
		{
			// Use standard label template
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TestParent),
				new FrameworkPropertyMetadata(typeof(Label)));
		}
	}

	public class TestChild : FrameworkElement, INotifyPropertyChanged
	{
		#region DP string UnAttachedString

		public static readonly DependencyProperty UnAttachedStringProperty = 
			DependencyProperty.Register(
				"UnAttachedString", typeof(string), 
				typeof(TestChild),
				new PropertyMetadata(default(string), OnStringChanged));

		public string UnAttachedString
		{
			get { return (string) GetValue(UnAttachedStringProperty); }
			set { SetValue(UnAttachedStringProperty, value); }
		}

		#endregion

		#region Inherited AP string AttachedString

		public static readonly DependencyProperty AttachedStringProperty =
			TestParent.AttachedStringProperty.AddOwner(typeof(TestChild),
				new FrameworkPropertyMetadata(
					(object) TestParent.AttachedStringProperty.DefaultMetadata.DefaultValue,
						OnStringChanged));

		private static void OnStringChanged(DependencyObject d,
											DependencyPropertyChangedEventArgs e)
		{
			((TestChild) d).OnPropertyChanged(e.OldValue, e.NewValue, e.Property.Name);
		}

		public string AttachedString
		{
			get { return (string) GetValue(AttachedStringProperty); }
			set { SetValue(AttachedStringProperty, (string) value); }
		}

		#endregion

		#region INotifyPropertyChanged implimentation
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(object oldValue, object newValue,
			[CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this,
					new MyPropertyChangedEventArgs(propertyName, oldValue, newValue));
		}

		#endregion
	}

	public class MyPropertyChangedEventArgs : PropertyChangedEventArgs
	{
		public MyPropertyChangedEventArgs(string propertyName, object oldValue, 
			object newValue) : base(propertyName)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public object OldValue;
		public object NewValue;
	}
}