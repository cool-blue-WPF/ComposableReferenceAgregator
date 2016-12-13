using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DPCollection
{

	public class BindableObjectsControlObservable : TextBox
	{
		static BindableObjectsControlObservable ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BindableObjectsControlObservable),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		public ObservableCollection<FreezableItem> BindableItems
		{
			get { return (ObservableCollection<FreezableItem>)GetValue(BindableItemsProperty); }
			set { SetValue(BindableItemsProperty, value); }
		}

		public static readonly DependencyPropertyKey BindableItemsPropertyKey =
			DependencyProperty.Register(
			"BindableItems", 
			typeof(ObservableCollection<FreezableItem>),
			typeof(BindableObjectsControlObservable), new PropertyMetadata(
				default(ObservableCollection<FreezableItem>), PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject dependencyObject, 
			DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
		}

		void Items_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var newItem in e.NewItems)
				{
					this.Text += String.Format("{0}\n", ((FreezableItem)newItem).Text);
					this.AddLogicalChild(newItem);
				}
			}
			if (e.OldItems == null) return;
			foreach (var oldItem in e.OldItems)
			{
				this.RemoveLogicalChild(oldItem);
			}
		}

		protected override IEnumerator LogicalChildren
		{
			get { return BindableItems.GetEnumerator(); }
		}

		public BindableObjectsControlObservable ()
		{
			BindableItems = new ObservableCollection<FreezableItem>();
			BindableItems.CollectionChanged += Items_CollectionChanged;
		}
	}

	public class BindableItem : DependencyObject
	{
		public Button Target
		{
			get { return (Button)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(
			"Target", typeof(Button), typeof(BindableItem), new
				PropertyMetadata(default(Button)));

		private string _text;
		public string Text
		{
			get
			{
				return Target == null ? "null" : Target.Name;
			}
		}
	}
}
