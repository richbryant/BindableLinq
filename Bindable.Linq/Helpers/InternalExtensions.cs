using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bindable.Linq.Helpers
{
    internal static class InternalExtensions
{
	public static void ShouldNotBeNull(this object item, string argumentName)
	{
		if (item == null)
		{
			throw new ArgumentNullException(argumentName);
		}
	}

	public static void ShouldBe<TItem>(this TItem item, Func<TItem, bool> condition, string message)
	{
		if (!condition(item))
		{
			throw new ArgumentException(message);
		}
	}

	public static string FormatWith(this string format, params object[] arguments)
	{
		return string.Format(format, arguments);
	}

	public static List<TElement> EnumerateSafely<TElement>(this IEnumerable<TElement> elements)
	{
		var type = elements.GetType();
		if (type == typeof(List<TElement>))
		{
			return (List<TElement>)elements;
		}

        var list = !(elements is ICollection) ? new List<TElement>() : new List<TElement>(((ICollection)elements).Count);
		list.AddRange(elements);
		return list;
	}

	public static IEnumerable<TElement> UnionAll<TElement>(this IEnumerable<IEnumerable<TElement>> enumerables)
	{
		if (enumerables != null)
		{
			foreach (var enumerable in enumerables)
			{
				if (enumerable != null)
				{
					foreach (var item in enumerable)
					{
						yield return item;
					}
				}
			}
		}
	}

	public static string ConcatStrings(this IEnumerable<string> strings, string separator)
	{
		var stringBuilder = new StringBuilder();
		var enumerator = strings.GetEnumerator();
		var flag = enumerator.MoveNext();
		while (flag)
		{
			stringBuilder.Append(enumerator.Current);
			flag = enumerator.MoveNext();
			if (flag)
			{
				stringBuilder.Append(separator);
			}
		}
        enumerator.Dispose();
		return stringBuilder.ToString();
	}

	public static void ForEach<TElement>(this IEnumerable<TElement> collection, Action<TElement> action)
	{
		if (collection != null)
		{
			foreach (var item in collection)
			{
				if (item != null)
				{
					action(item);
				}
			}
		}
	}

	public static void Evaluate<TElement>(this IEnumerable<TElement> collection)
	{
		using (var enumerator = collection.GetEnumerator())
		{
			enumerator.MoveNext();
		}
	}

	public static TElement Item<TElement>(this IEnumerable<TElement> collection, int index)
	{
		ShouldNotBeNull(collection, "collection");
        if (collection is IList<TElement> list)
		{
			return list[index];
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
		var num = index;
		foreach (var item in collection)
		{
			if (0 == num)
			{
				return item;
			}
			num--;
		}
		throw new ArgumentOutOfRangeException(nameof(index));
	}
}
}