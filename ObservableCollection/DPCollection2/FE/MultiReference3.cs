using System.Windows;
using System.Windows.Controls;
using Util;

/* Experiment to investigate data binding on a child element
 * 
 * DO_Receiver 
 * Dependency Property that binds a Dependency Object
 *   Binding on dependency properties on the bound DO doesn't work
 *   Cannot find governing FrameworkElement or FrameworkContentElement for target element
 *   
 * Direct DO_Receiver
 * Dependency Property that binds a FrameworkElement
 *   Binding
 */

namespace DPCollection2.FE
{
	public class MultiReference3 : TextBox
	{
		static MultiReference3()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiReference3),
				new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		#region FE_Receiver

		public static readonly DependencyProperty FE_ReceiverProperty =
			DependencyProperty.Register(
				"FE_Receiver",
				typeof(FrameworkElement),
				typeof(MultiReference3),
				new PropertyMetadata(default(FrameworkElement), Shared.NamedChangedCallback));

		public FrameworkElement FE_Receiver
		{
			get { return (FrameworkElement) GetValue(FE_ReceiverProperty); }
			set { SetValue(FE_ReceiverProperty, value); }
		}

		#endregion
	}

	public class FE : FrameworkElement
	{
		public static readonly DependencyProperty ItemProperty = DependencyProperty
			.Register(
				"Item", typeof(FrameworkElement),
				typeof(FE), new PropertyMetadata(default(FrameworkElement)));

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

		public FE()
		{
			Type = "Indirect";
		}
	}
}