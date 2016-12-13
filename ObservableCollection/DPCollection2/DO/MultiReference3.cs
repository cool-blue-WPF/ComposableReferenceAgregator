using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Util;

/* Experiment to investigate data binding on a child element
 * 
 * DO_Receiver 
 * Dependency Property that binds a Dependency Object
 *   Binding on dependency properties on the bound FZ doesn't work
 *   Cannot find governing FrameworkElement or FrameworkContentElement for target element
 *   
 * Direct DO_Receiver
 * Dependency Property that binds a FrameworkElement
 *   Binding
 */
namespace DPCollection2.DO
{
	public class MultiReference3 : TextBox
	{
		private static MultiReference3 instance;

		static MultiReference3 ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiReference3),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		#region DP DO_Receiver

		public static readonly DependencyProperty DO_ReceiverProperty = 
			DependencyProperty.Register(
			"DO_Receiver", typeof(DependencyObject), 
			typeof(MultiReference3),
			new PropertyMetadata(default(DependencyObject), Shared.NamedChangedCallback));

		public DependencyObject DO_Receiver
		{
			get { return (DependencyObject) GetValue(DO_ReceiverProperty); }
			set { SetValue(DO_ReceiverProperty, value); }
		}

		#endregion

		// This is a private DP that’s used to get the inherited DataContext
		// (see it used in the MethodCommand constructor).

		private static readonly DependencyProperty ElementDataContextProperty =
			DependencyProperty.Register("ElementDataContext",
				typeof(object),
				typeof(MultiReference3), null);

		#region DP FZ_Receiver

		public static readonly DependencyProperty FZ_ReceiverProperty =
			DependencyProperty.Register(
			"FZ_Receiver", 
			typeof(Freezable),
			typeof(MultiReference3),
			new PropertyMetadata(default(Freezable), NamedChangedCallback));

		public Freezable FZ_Receiver
		{
			get { return (Freezable)GetValue(FZ_ReceiverProperty); }
			set { SetValue(FZ_ReceiverProperty, value); }
		}

		public static void NamedChangedCallback(
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Does not call back for attribute binding
			Shared.NamedChangedCallback(d, e);
		}

		#endregion

		#region DP colFZ_Receiver

		public static readonly DependencyProperty colFZ_ReceiverProperty = 
			DependencyProperty.Register(
			"colFZ_Receiver", typeof(FreezableCollection<FZ>), 
			typeof(MultiReference3),
			new PropertyMetadata(default(FreezableCollection<FZ>)));

		public FreezableCollection<FZ> colFZ_Receiver
		{
			get { return (FreezableCollection<FZ>) GetValue(colFZ_ReceiverProperty); }
			set { SetValue(colFZ_ReceiverProperty, value); }
		}

		//private static void 

		private static void ColFzReceiverOnChanged(object d, EventArgs e)
		{
			var item = d as FZ;
			if (item == null) return;
			Shared.NamedChangedCallback(instance,
				new DependencyPropertyChangedEventArgs(colFZ_ReceiverProperty, null, item));
		}

		#endregion

		// Constructor

		public MultiReference3()
		{
			instance = this;
			
			colFZ_Receiver = new FreezableCollection<FZ>();
			colFZ_Receiver.Changed += ColFzReceiverOnChanged;
			
			// Set a default Binding onto the private ElementDataContextProperty.
			// A default binding just binds to the inherited DataContext.  This is how
			// MethodCommand typically gets the object on which to invoke the method.

			BindingOperations.SetBinding(
				this,
				ElementDataContextProperty,
				new Binding());

			
		}
	}

	public class DO : DependencyObject
	{
		public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
			"Item", typeof(FrameworkElement), 
			typeof(DO), new PropertyMetadata(default(FrameworkElement)));

		public FrameworkElement Item
		{
			get { return (FrameworkElement) GetValue(ItemProperty); }
			set { SetValue(ItemProperty, value); }
		}


		public string Name
		{
			get { return Item == null ? "null" : Item.Name; }
		}

		public string Type { get; set; }

		public DO()
		{
			Type = "Indirect";
		}
	}

	public class FZ : Freezable
	{
		public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
			"Item", typeof(FrameworkElement),
			typeof(FZ), new PropertyMetadata(default(FrameworkElement)));

		public FrameworkElement Item
		{
			get { return (FrameworkElement)GetValue(ItemProperty); }
			set { SetValue(ItemProperty, value); }
		}

		public string Type { get; set; }

		public string Name
		{
			get { return Item == null ? null : Item.Name; }
		}

		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}

		public FZ()
		{
			Type = "Indirect";
		}
	}

}
