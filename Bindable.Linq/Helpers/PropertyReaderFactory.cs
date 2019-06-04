using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bindable.Linq.Helpers
{
    internal static class PropertyReaderFactory
{
	private class DelegatePropertyReader<TInput, TReturn, TCast> : IPropertyReader<TCast> where TReturn : TCast
	{
		private readonly Func<TInput, TReturn> _caller;

		public DelegatePropertyReader(Func<TInput, TReturn> caller)
		{
			_caller = caller;
		}

		public TCast GetValue(object input)
		{
			return _caller((TInput)input);
		}
	}

	private class FieldReader<TCast> : IPropertyReader<TCast>
	{
		private readonly FieldInfo _field;

		public FieldReader(FieldInfo field)
		{
			_field = field;
		}

		public TCast GetValue(object input)
		{
			try
			{
				return (TCast)_field.GetValue(input);
			}
			catch
			{
				return default(TCast);
			}
		}
	}

	private static readonly Dictionary<string, object> Readers = new Dictionary<string, object>();

	public static IPropertyReader<TCast> Create<TCast>(Type objectType, string propertyName)
	{
		var key = objectType.AssemblyQualifiedName + "-" + propertyName;
		IPropertyReader<TCast> propertyReader = null;
		if (Readers.ContainsKey(key))
		{
			propertyReader = (Readers[key] as IPropertyReader<TCast>);
		}

        if (propertyReader != null) return propertyReader;
        var property = objectType.GetProperty(propertyName);
        if (property != null)
        {
            if (!typeof(TCast).IsAssignableFrom(property.PropertyType)) return null;
            var type = typeof(Func<, >).MakeGenericType(property.DeclaringType, property.PropertyType);
            var type2 = typeof(DelegatePropertyReader<, , >).MakeGenericType(property.DeclaringType, property.PropertyType, typeof(TCast));
            var getMethod = property.GetGetMethod();
            if (getMethod == null)
            {
                throw new ArgumentException($"The property {propertyName} on type '{property.DeclaringType}' does not contain a getter which could be accessed by the SyncLINQ binding infrastructure.");
            }
            var @delegate = Delegate.CreateDelegate(type, getMethod);
            propertyReader = (IPropertyReader<TCast>)Activator.CreateInstance(type2, @delegate);
            Readers[key] = propertyReader;
        }
        else
        {
            var field = objectType.GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null || !typeof(TCast).IsAssignableFrom(field.FieldType)) return null;
            propertyReader = new FieldReader<TCast>(field);
            Readers[key] = propertyReader;
        }
        return propertyReader;
	}
}
}