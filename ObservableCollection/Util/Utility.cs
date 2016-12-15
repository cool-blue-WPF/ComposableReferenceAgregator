
using System;
using System.Data;
using System.Reflection;

namespace Util
{
	public static class Utility
	{
		static PropertyInfo _getProperty(object item, string name)
		{
			var t = item.GetType();
			return t.GetProperty(name);
		}
		
		public static object GetMemberByName(object item, string name)
		{
			//if(item == null) throw new NoNullAllowedException(
			//	String.Format("******Cannot reflect member {0} on null object******", name));

			if (item == null) return null;

			var prop = _getProperty(item, name);
			return prop == null ? null : prop.GetValue(item, null);
		}
		public static void SetMemberByName(object item, string name,
			object value)
		{
			var prop = _getProperty(item, name);
			if (prop == null)
			{
				throw new InvalidOperationException(
					String.Format(
					"Property or Field {0} does not exist on {1}",
					name, item.ToString()));
			}
			prop.SetValue(item, value);
		}
	}
}