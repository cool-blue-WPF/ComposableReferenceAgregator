using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DPCollection
{
	//public class MultiReference : TextBox
	//{
	//	private string _title;

	//	static MultiReference()
	//	{
	//		DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiReference), 
	//			new FrameworkPropertyMetadata(typeof(TextBox)));
	//	}

	//	public string Title
	//	{
	//		get { return _title; }
	//		set
	//		{
	//			_title = value;
				
	//		}
	//	}

	//	public ObservableCollection<string> ElementNames { get; set; }

		

	//	public MultiReference()
	//	{
	//		ElementNames = new ObservableCollection<string>();
	//		ElementNames.CollectionChanged += ElementNames_CollectionChanged;
	//	}

	//	void ElementNames_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
	//	{
	//		if (e.NewItems != null)
	//		{
	//			foreach (var newItem in e.NewItems)
	//			{
	//				this.Text += String.Format("{0}\n", ((ButtonReference)newItem).ToString());
	//			}
	//		}
	//		if (e.OldItems == null) return;
	//		foreach (var oldItem in e.OldItems)
	//		{
				
	//		}
	//	}
	//}

	//public class ButtonReference : Button
	//{
	//	public ButtonReference()
	//	{
	//		Property = "Name";
	//	}
	//	public string Property { get; set; }
	//	public Button Target { get; set; }

	//	public new string ToString ()
	//	{
	//		return Target.Name;
	//	}
	//}
	//public class ElementReference<T>
	//{
	//	public string Property { get; set; }
	//	public T Target;

	//	public new object ToString()
	//	{
	//		return typeof(T).GetProperty(Property).GetValue(Target);
	//	}
	//}
	public class CustomControl : TextBox
	{
		static CustomControl ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		private static CustomControl instance;
		public TextBox Tb;
		
		public static readonly DependencyProperty ItemsProperty = 
			DependencyProperty.Register("Items", typeof(IList), 
			typeof(CustomControl), new PropertyMetadata(new List<string>(), 
				PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject d, 
			DependencyPropertyChangedEventArgs e)
		{
			var list = d.GetValue(ItemsProperty) as List<string>;
			if (list == null || list.Count == 0) return;
			var tb = instance as TextBox;
			tb.Text = "";
			foreach (var item in list)
			{
				tb.Text += String.Format("{0}\n", item);
			}
		}

		public IList Items
		{
			get { return (IList)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}

		public CustomControl ()
		{
			Items = new List<string>();
			Tb = this as  TextBox;
			instance = this;
		}
	}
	public class ObservableControl : TextBox
	{
		static ObservableControl ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ObservableControl),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}


		public ObservableCollection<string> Items { get; set; }

		void Items_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var newItem in e.NewItems)
				{
					this.Text += String.Format("{0}\n", ((string)newItem));
				}
			}
			if (e.OldItems == null) return;
			foreach (var oldItem in e.OldItems)
			{

			}
		}

		public ObservableControl ()
		{
			Items = new ObservableCollection<string>();
			Items.CollectionChanged += Items_CollectionChanged;
		}
	}

	public class ObservableObjectsControl : TextBox
	{
		static ObservableObjectsControl ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ObservableObjectsControl),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}


		public ObservableCollection<Item> Items { get; set; }

		void Items_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var newItem in e.NewItems)
				{
					this.Text += String.Format("{0}\n", ((Item)newItem).Text);
				}
			}
			if (e.OldItems == null) return;
			foreach (var oldItem in e.OldItems)
			{

			}
		}

		public ObservableObjectsControl ()
		{
			Items = new ObservableCollection<Item>();
			Items.CollectionChanged += Items_CollectionChanged;
		}
	}

	public class Items : DependencyObject
	{
		
	}
	public class Item
	{
		public string Text { get; set; }
	}

	public class Item1 : FrameworkElement
	{
		
	}

	public class BindableObjectsControl : TextBox
	{
		static BindableObjectsControl ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BindableObjectsControl),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
			"Item",
			typeof(FrameworkElement), 
			typeof(BindableObjectsControl),
			new PropertyMetadata(default(FrameworkElement),
				OnItemChanged));

		private static void OnItemChanged(DependencyObject d, 
			DependencyPropertyChangedEventArgs e)
		{
			var item = e.NewValue as DPItem;
			if (item == null) return;
			var host = d as BindableObjectsControl;
			if (host == null) return;
			host.Text += String.Format("{0}\n", (item.Text));
		}

		public FrameworkElement Item
		{
			get { return (FrameworkElement)GetValue(ItemProperty); }
			set { SetValue(ItemProperty, value); }
		}
		
		public FreezableCollection<Freezable> BindableItems
		{
			get { return (FreezableCollection<Freezable>) GetValue(BindableItemsProperty); }
			set { SetValue(BindableItemsProperty, value); }
		}

		public static readonly DependencyPropertyKey BindableItemsPropertyKey = 
			DependencyProperty.RegisterReadOnly(
			"BindableItems", 
			typeof(FreezableCollection<Freezable>), 
			typeof(BindableObjectsControl), 
			new PropertyMetadata(new FreezableCollection<Freezable>(),
				PropertyChangedCallback));

		public static readonly DependencyProperty BindableItemsProperty =
			BindableItemsPropertyKey.DependencyProperty;

		private static void PropertyChangedCallback(DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var items = e.NewValue as FreezableCollection<Freezable>;
			if (items == null || items.Count == 0) return;
			var item = items.Count == 0 ? null : items[items.Count - 1] as FreezableItem;
			if (item == null) return;
			var host = d as BindableObjectsControl;
			if (host == null) return;
			host.Text += String.Format("{0}\n", (item.Text));
		}

		void Items_CollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var newItem in e.NewItems)
				{
					this.Text += String.Format("{0}\n", ((string)newItem));
					this.AddLogicalChild(newItem);
				}
			}
			if (e.OldItems == null) return;
			foreach (var oldItem in e.OldItems)
			{
				this.RemoveLogicalChild(oldItem);
			}
		}
		//private void BindableItems_Changed (object sender, EventArgs e)
		//{
		//	var list = sender as FreezableCollection<Freezable>;
		//	if (list == null) return;
		//	foreach (var item in list)
		//	{
		//		if (item != null)
		//			this.Text += String.Format("{0}\n", ((Freezable)item).Text);
		//	}

		//}

		//protected override IEnumerator LogicalChildren
		//{
		//	get { return BindableItems.GetEnumerator(); }
		//}

		public BindableObjectsControl ()
		{
			SetValue(BindableItemsPropertyKey, new FreezableCollection<Freezable>());
			//BindableItems.Changed += BindableItems_Changed;
		}
	}

	public class FreezableItem : Freezable
	{
		public Button Target
		{
			get { return (Button) GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		public static readonly DependencyProperty TargetProperty = 
			DependencyProperty.Register(
			"Target", 
			typeof(Button), typeof(FreezableItem), 
			new PropertyMetadata(default(Button)));

		private string _text;
		public string Text
		{
			get
			{
				return Target == null ? "null" : Target.Name;
			}
		}

		protected override Freezable CreateInstanceCore ()
		{
			return new FreezableItem();
		}
	}

	public class DPItem : FrameworkElement
	{
		public Button Target
		{
			get { return (Button)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register(
			"Target",
			typeof(Button), typeof(DPItem),
			new PropertyMetadata(default(Button)));

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
