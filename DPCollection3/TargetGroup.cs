using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using Util;

/* 
 * Composable CustomControl with Binding support  
 */

namespace CollectionBinding
{
	public class TargetGroup : TextBox
	{
		#region refresh
		public static void _Refresh (
			DependencyObject d, DependencyObject s, object newValue)
		{
			var host = d as TextBox;
			if (host == null) return;
			//var attrinuteName = ((TargetGroup)d).TargetAttribute;
			var item = s as FE;
			if (item == null) return;
			var targetAttribute = item.Atrribute;
			if (targetAttribute == null) return;
			var route = Utility.GetMemberByName(item, "Type") ?? "Direct";
			host.Text += string.Format("{2}{0}\t{1}", route, targetAttribute.ToString(),
				host.Text == "" ? "" : "\n");
		}

		public static void Refresh(DependencyObject d, DependencyObject s,
			object nweValue)
		{
			var host = d as TargetGroup;
			if (host == null) return;
			host.Text = string.Format("parent: TargetAttribute: {0}",
				host.TargetAttribute);
			var targets = host.Targets;
			foreach (var target in targets)
			{
				host.Text += string.Format("\n{0}\t{1}\t{2}", target.Name,
					target.TargetAttribute, target.Atrribute);
			}
		}

		public void RefreshAsync (DependencyObject sender, object newValue)
		{
			Dispatcher.InvokeAsync(() => Refresh(this, sender, newValue));
		}

		private void OnPropertyChanged (DependencyObject sender, object oldvalue, object newvalue)
		{
			if (oldvalue != null && oldvalue.Equals(newvalue)) return;
			RefreshAsync(sender, newvalue);
		}

		#endregion

		#region Events
		public event EventHandler DebugProbe;

		protected virtual void OnDebugProbe(object sender, EventArgs e)
		{
			EventHandler handler = DebugProbe;
			if (handler != null)
			{
				handler(sender, e);
			}
		}

		#endregion

		#region AP TargetAttribute

		public static readonly DependencyProperty TargetAttributeProperty =
			DependencyProperty.RegisterAttached(
				"TargetAttribute", typeof(string),
				typeof(TargetGroup),
				new FrameworkPropertyMetadata("CommandParameter",
						FrameworkPropertyMetadataOptions.Inherits,
						TargetAttributeChangedCallback));

		public static void SetTargetAttribute (DependencyObject target, string value)
		{
			target.SetValue(TargetAttributeProperty, value);
		}

		public static string GetTargetAttribute (DependencyObject target)
		{
			return (string)target.GetValue(TargetAttributeProperty);
		}

		private static void TargetAttributeChangedCallback (DependencyObject d,
			DependencyPropertyChangedEventArgs e)
		{
			var instance = d as TargetGroup;
			if (instance == null) return;
			instance.OnPropertyChanged(d, e.OldValue, e.NewValue);
		}

		public string TargetAttribute
		{
			get { return GetTargetAttribute(this); }
			set { SetTargetAttribute(this, (string)value); }
		}

		#endregion

		#region DP Targets

		public static readonly DependencyProperty TargetsProperty =
			DependencyProperty.Register(
				"Targets", typeof(ObservableCollection<FE>),
				typeof(TargetGroup),
				new PropertyMetadata(default(ObservableCollection<FE>)));

		public ObservableCollection<FE> Targets
		{
			get { return (ObservableCollection<FE>) GetValue(TargetsProperty); }
			set { SetValue(TargetsProperty, value); }
		}

		private void Target_Changed (object s, PropertyChangedEventArgs args)
		{
			var e = args as MyPropertyChangedEventArgs;
			if (e == null)
				OnPropertyChanged((DependencyObject) s, new object(), new object());
			else
				OnPropertyChanged((DependencyObject) s, e.OldValue, e.NewValue);
		}

		void Targets_Changed (object d, NotifyCollectionChangedEventArgs e)
		{
			var addedItems = e.NewItems as IList;
			var deletedItems = e.OldItems as IList;

			if (addedItems != null)
			{
				foreach (var addedItem in addedItems)
				{
					this.AddLogicalChild((FE)addedItem);
					((FE)addedItem).PropertyChanged += Target_Changed;
				}
			}
			if (deletedItems != null)
			{
				foreach (var deletedItem in deletedItems)
				{
					this.RemoveLogicalChild((FE)deletedItem);
					((FE)deletedItem).PropertyChanged -= Target_Changed;
				}
			}
		}

		protected override IEnumerator LogicalChildren
		{
			get { return Targets.GetEnumerator(); }
		}		

		#endregion

		// Constructor

		public TargetGroup()
		{
			Targets = new ObservableCollection<FE>();
			Targets.CollectionChanged += Targets_Changed;
		}

		// bind to standard TextBox templates and styles
		static TargetGroup ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TargetGroup),
				new FrameworkPropertyMetadata(typeof(TargetGroup)));
		}
	}

	public class FE : FrameworkElement, INotifyPropertyChanged
	{
		//public new string Name { get; set; }

		public int Hash
		{
			get
			{
				return GetHashCode();
			}
		}

		#region DP FrameworkElement Target

		public static readonly DependencyProperty TargetProperty = DependencyProperty
			.Register(
				"Target", typeof(FrameworkElement),
				typeof(FE), 
				new PropertyMetadata(default(FrameworkElement), OnPropertyChanged));

		public FrameworkElement Target
		{
			get { return (FrameworkElement) GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		public string ItemName
		{
			get { return Target == null ? null : Target.Name; }
		}

		#endregion

		public string Type { get; set; }

		#region AP string TargetAttribute

		public static readonly DependencyProperty TargetAttributeProperty = 
			TargetGroup.TargetAttributeProperty.AddOwner(typeof(FE),
			new FrameworkPropertyMetadata
			{
				DefaultValue = TargetGroup.TargetAttributeProperty.DefaultMetadata
									.DefaultValue,
				Inherits = true,
				PropertyChangedCallback = OnPropertyChanged
			});

		private static void OnPropertyChanged (DependencyObject d,
											DependencyPropertyChangedEventArgs e)
		{
			((FE)d).OnPropertyChanged(e.OldValue, e.NewValue, e.Property.Name);
		}


		public string TargetAttribute
		{
			get { return (string)GetValue(TargetAttributeProperty); }
			set { SetValue(TargetAttributeProperty, value); }
		}


		public object Atrribute
		{
			get { return Utility.GetMemberByName(Target, TargetAttribute); }
			set { Utility.SetMemberByName(Target, TargetAttribute, value); }
		}

		#endregion

		public FE()
		{
			Type = "Indirect";
		}

		static FE()
		{
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged (object oldValue, object newValue,
			[CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this,
					new MyPropertyChangedEventArgs(propertyName, oldValue, newValue));
		}
	}
}