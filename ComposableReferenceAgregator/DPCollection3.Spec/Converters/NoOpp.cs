using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace CollectionBinding.Spec.Converters
{
	public class NoOppConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			Debug.Print("binding on {2} value: {0} type: {1}", value, targetType, parameter);
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
