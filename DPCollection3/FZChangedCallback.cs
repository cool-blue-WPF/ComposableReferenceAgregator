using System.Windows;
using System.Windows.Controls;
using Util;

namespace CollectionBinding
{
	static class FZChangedCallback
	{
		public static void NamedChangedCallback (
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var host = d as TextBox;
			if (host == null) return;
			//var attrinuteName = ((TargetGroup)d).TargetAttribute;
			var item = e.NewValue as FZ;
			if (item == null) return;
			var name = item.Atrribute;
			if (name == null) return;
			var route = Utility.GetMemberByName(item, "Type") ?? "Direct";
			host.Text += string.Format("{2}{0}\t{1}", route, name.ToString(),
				host.Text == "" ? "" : "\n");
		}
	}
}
