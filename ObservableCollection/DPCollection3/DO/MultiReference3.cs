using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DPCollection2;

/* 
 * Composable CustomControl with Binding support  
 */
namespace DPCollection3.DO
{
	public class MultiReference3 : TextBox
	{
		private static MultiReference3 instance;

		static MultiReference3 ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiReference3),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		// This is a private DP that’s used to get the inherited DataContext
		// (see it used in the MethodCommand constructor).

		private static readonly DependencyProperty ElementDataContextProperty =
			DependencyProperty.Register("ElementDataContext",
				typeof(object),
				typeof(MultiReference3), null);

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
			// This defaults to the inherited DataContext.

			BindingOperations.SetBinding(
				this,
				ElementDataContextProperty,
				new Binding());

			
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
