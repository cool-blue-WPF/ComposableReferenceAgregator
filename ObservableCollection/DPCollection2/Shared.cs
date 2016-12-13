using System.Windows;
using System.Windows.Controls;
using Util;

namespace DPCollection2
{
	class Shared
	{
		public static void NamedChangedCallback (
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var host = d as TextBox;
			if (host == null) return;
			var item = e.NewValue;
			if (item == null) return;
			var name = Utility.GetMemberByName(item, "Name");
			if (name == null) return;
			var route = Utility.GetMemberByName(item, "Type") ?? "Direct";
			host.Text += string.Format("{2}{0}\t{1}", route, name ?? "null",
				host.Text == "" ? "" : "\n");
		}
	}
}
