using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionBinding
{
	public class MyPropertyChangedEventArgs : PropertyChangedEventArgs
	{
		public MyPropertyChangedEventArgs (string propertyName, object oldValue,
			object newValue)
			: base(propertyName)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public object OldValue;
		public object NewValue;
	}

}
