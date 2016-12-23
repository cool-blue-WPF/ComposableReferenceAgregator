using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CollectionBinding
{
	public class ListViewAggregator : ItemsControl
	{
		public static readonly DependencyProperty TargetAttributeProperty = DependencyProperty.RegisterAttached(
			"TargetAttribute", typeof(string), typeof(ListViewAggregator), new PropertyMetadata(default(string)));

		public static void SetTargetAttribute(DependencyObject element, string value)
		{
			element.SetValue(TargetAttributeProperty, value);
		}

		public static string GetTargetAttribute(DependencyObject element)
		{
			return (string) element.GetValue(TargetAttributeProperty);
		}

		static ListViewAggregator ()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ListViewAggregator), 
				new FrameworkPropertyMetadata(typeof(ItemsControl)));
		}
	}

	public static class Attached
	{
		public static readonly DependencyProperty TargetAttributeProperty = 
			DependencyProperty.RegisterAttached(
			"TargetAttribute", typeof(string), typeof(Attached), 
			new FrameworkPropertyMetadata(default(string), 
				FrameworkPropertyMetadataOptions.Inherits));

		public static void SetTargetAttribute (DependencyObject element, string value)
		{
			element.SetValue(TargetAttributeProperty, value);
		}

		public static string GetTargetAttribute (DependencyObject element)
		{
			return (string)element.GetValue(TargetAttributeProperty);
		}
	}

	public class ActiveLabel : Label
	{
		#region Inherited AP string TargetAttribute

		public static readonly DependencyProperty TargetAttributeProperty =
			TestParent.TargetAttributeProperty.AddOwner(typeof(TestChild),
				new FrameworkPropertyMetadata(
					(object)TestParent.TargetAttributeProperty.DefaultMetadata.DefaultValue));

		public string TargetAttribute
		{
			get { return (string)GetValue(TargetAttributeProperty); }
			set { SetValue(TargetAttributeProperty, (string)value); }
		}

		#endregion
	}
}
