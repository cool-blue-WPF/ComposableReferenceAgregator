﻿
using System;
using System.Collections.Generic;
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

			var prop = string.IsNullOrEmpty(name) ? null : _getProperty(item, name);
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

	public static class Extensions
	{
		public static string DefaultIfEmpty(this String str, string defaultValue)
		{
			return string.IsNullOrEmpty(str) ? defaultValue : str;
		}
	}

	public class ValueHistory<T>
	{
		private readonly Stack<T> _history = new Stack<T>();

		public T Current
		{
			set { _history.Push(value); }
			get { return _history.Peek(); }
		}

		public T Pop()
		{
			return _history.Pop();	
		}

		public T Restore()
		{
			_history.Pop();
			return _history.Peek();
		}

		public ValueHistory(T value)
		{
			_history.Push(value);
		}
	}
}