using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using CollectionBinding.Annotations;

namespace CollectionBinding
{
	public class TestParent : Label
	{
		public void Refresh()
		{
			this.Content = string.Format("parent value:\t{0}", this.AttachedString);
			foreach (var myItem in this.MyItems)
			{
				this.Content += string.Format("\n  child value:\t{0}", myItem.AttachedString);
			}
			this.Content += string.Format("\n Attached DP References are{0} equal",
				TestParent.AttachedStringProperty == TestChild.AttachedStringProperty
				? "" : " NOT");
		}

		#region Inheritable AP string AttachedString

		public static readonly
			DependencyProperty AttachedStringProperty =
				DependencyProperty.RegisterAttached(
					"AttachedString", typeof(string),
					typeof(TestParent),
					new FrameworkPropertyMetadata(default(String),
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
			instance.OnStringChanged(e.OldValue, e.NewValue);
		}

		public string AttachedString
		{
			get { return GetAttachedString(this); }
			set { SetAttachedString(this, (string) value); }
		}

		private void OnStringChanged (object oldvalue, object newvalue)
		{
			this.Refresh();
		}

		#endregion

		#region DP FreezableCollection<TestChild> MyItems

		public static readonly DependencyProperty MyItemsProperty =
			DependencyProperty.Register(
				"MyItems", typeof(FreezableCollection<TestChild>),
				typeof(TestParent),
				new PropertyMetadata(default(FreezableCollection<TestChild>)));

		public FreezableCollection<TestChild> MyItems
		{
			get { return (FreezableCollection<TestChild>) GetValue(MyItemsProperty); }
			set { SetValue(MyItemsProperty, value); }
		}

		private void MyItem_Changed(object s, PropertyChangedEventArgs args )
		{
			var item = s as TestChild;
			if (item == null) return;
			// Ignore Inherited changes
			if (DependencyPropertyHelper
					.GetValueSource(item, TestChild.AttachedStringProperty)
					.BaseValueSource == BaseValueSource.Inherited)
				return;
			this.OnStringChanged(this, args);
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
			MyItems = new FreezableCollection<TestChild>();
			((INotifyCollectionChanged) MyItems).CollectionChanged += MyItems_Changed;
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
		#region Inherited AP string AttachedString

		public static readonly DependencyProperty AttachedStringProperty =
			TestParent.AttachedStringProperty.AddOwner(typeof(TestChild),
				new FrameworkPropertyMetadata(
					(object) TestParent.AttachedStringProperty.DefaultMetadata.DefaultValue,
					FrameworkPropertyMetadataOptions.Inherits, OnStringChanged));

		private static void OnStringChanged(DependencyObject d,
											DependencyPropertyChangedEventArgs e)
		{
			((TestChild) d).OnPropertyChanged(e.Property.Name);
		}

		public string AttachedString
		{
			get { return (string) GetValue(AttachedStringProperty); }
			set { SetValue(AttachedStringProperty, (string) value); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}