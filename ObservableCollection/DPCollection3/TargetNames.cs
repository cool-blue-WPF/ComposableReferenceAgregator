using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Util;

/* 
 * Composable CustomControl with Binding support  
 */

namespace CollectionBinding
{
	public class TargetNames : TextBox
	{
		private static TargetNames instance;

		// bind to standard TextBox templates and styles
		static TargetNames()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TargetNames),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		#region Data Context Inhertance

		// This is a private DP that’s used to get the inherited DataContext
		// (see it used in the MethodCommand constructor).

		private static readonly DependencyProperty ElementDataContextProperty =
			DependencyProperty.Register("ElementDataContext",
				typeof(object),
				typeof(TargetNames), null);

		#endregion

		#region DP TargetAttribute

		// todo callback to update
		
		public static readonly DependencyProperty TargetAttributeProperty =
			DependencyProperty.RegisterAttached(
				"TargetAttribute", typeof(string),
				typeof(TargetNames), new PropertyMetadata("Name"));

		

		public string TargetAttribute
		{
			get { return (string) GetValue(TargetAttributeProperty); }
			set { SetValue(TargetAttributeProperty, value); }
		}

		#endregion

		#region DP Targets

		public static readonly DependencyProperty TargetsProperty =
			DependencyProperty.Register(
				"Targets", typeof(FreezableCollection<FZ>),
				typeof(TargetNames),
				new PropertyMetadata(default(FreezableCollection<FZ>)));

		public FreezableCollection<FZ> Targets
		{
			get { return (FreezableCollection<FZ>) GetValue(TargetsProperty); }
			set { SetValue(TargetsProperty, value); }
		}

		private static void ColFzReceiverOnChanged(object d, EventArgs e)
		{
			var item = d as FZ;
			if (item == null) return;
			item.TargetAttribute = instance.TargetAttribute;

			FZChangedCallback.NamedChangedCallback(instance,
				new DependencyPropertyChangedEventArgs(TargetsProperty, null, item));
		}

		#endregion

		// Constructor

		public TargetNames()
		{
			instance = this;

			Targets = new FreezableCollection<FZ>();
			Targets.Changed += ColFzReceiverOnChanged;

			// Set a default Binding onto the private ElementDataContextProperty.
			// This defaults to the inherited DataContext.

			BindingOperations.SetBinding(
				this,
				ElementDataContextProperty,
				new Binding());
		}
	}

	public class FZ : Freezable
	{
		public static readonly DependencyProperty ItemProperty = DependencyProperty
			.Register(
				"Item", typeof(FrameworkElement),
				typeof(FZ), new PropertyMetadata(default(FrameworkElement)));

		public FrameworkElement Item
		{
			get { return (FrameworkElement) GetValue(ItemProperty); }
			set { SetValue(ItemProperty, value); }
		}

		public string Type { get; set; }

		// todo change to DP and callback on change

		private string _targetAttribute;
		public string TargetAttribute
		{
			get { return _targetAttribute; }
			set
			{
				if (_targetAttribute == default(string))
					_targetAttribute = value;
			}
		}

		public object Atrribute
		{
			get { return Utility.GetMemberByName(Item, TargetAttribute); }
			set { Utility.SetMemberByName(Item, TargetAttribute, value); }
		}

		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}

		public FZ()
		{
			Type = "Indirect";

			var targetAttribute =
				DependencyPropertyDescriptor.FromProperty(
					TargetNames.TargetAttributeProperty,
					typeof(FZ)
				);
			targetAttribute.AddValueChanged(this, TargetAttributeChanged);
		}

		private void TargetAttributeChanged(object sender, EventArgs eventArgs)
		{
			TargetAttribute = ((TargetNames)sender).TargetAttribute;
		}
	}
}