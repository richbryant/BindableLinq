using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Threading;
using Bindable.Linq.Adapters.Incoming;
using Bindable.Linq.Adapters.Outgoing;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Collections;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Dependencies.Defintions;
using Bindable.Linq.Helpers;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Interfaces.Internal;
using Bindable.Linq.Iterators;
using Bindable.Linq.Operators;
using Bindable.Linq.Threading;

namespace Bindable.Linq
{
    public static class BindableEnumerable
{
	private static DependencyAnalysis DefaultDependencyAnalysis = DependencyAnalysis.Automatic;

	public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	{
		return source.OrderBy(keySelector, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
	{
		return source.OrderBy(keySelector, comparer, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.OrderBy(keySelector, null, dependencyAnalysisMode);
	}

	public static IOrderedBindableCollection<TSource> OrderBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		keySelector.ShouldNotBeNull("keySelector");
		var orderByIterator = new OrderByIterator<TSource, TKey>(source, new ItemSorter<TSource, TKey>(null, keySelector.Compile(), comparer, ascending: true), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return orderByIterator.WithDependencyExpression(keySelector.Body, keySelector.Parameters[0]);
		}
		return orderByIterator;
	}

	private static IBindable<TElement> MaxInternal<TElement, TAverage>(IBindableCollection<TElement> source, INumeric<TElement, TAverage> numeric)
	{
		source.ShouldNotBeNull("source");
		return source.Aggregate<TElement, TElement>(sources => numeric.Max(sources));
	}

	public static IBindable<decimal> Max(this IBindableCollection<decimal> source)
	{
		return MaxInternal(source, new DecimalNumeric());
	}

	public static IBindable<double?> Max(this IBindableCollection<double?> source)
	{
		return MaxInternal(source, new DoubleNullNumeric());
	}

	public static IBindable<double> Max(this IBindableCollection<double> source)
	{
		return MaxInternal(source, new DoubleNumeric());
	}

	public static IBindable<int?> Max(this IBindableCollection<int?> source)
	{
		return MaxInternal(source, new Int32NullNumeric());
	}

	public static IBindable<long?> Max(this IBindableCollection<long?> source)
	{
		return MaxInternal(source, new Int64NullNumeric());
	}

	public static IBindable<int> Max(this IBindableCollection<int> source)
	{
		return MaxInternal(source, new Int32Numeric());
	}

	public static IBindable<decimal?> Max(this IBindableCollection<decimal?> source)
	{
		return MaxInternal(source, new DecimalNullNumeric());
	}

	public static IBindable<long> Max(this IBindableCollection<long> source)
	{
		return MaxInternal(source, new Int64Numeric());
	}

	public static IBindable<float?> Max(this IBindableCollection<float?> source)
	{
		return MaxInternal(source, new FloatNullNumeric());
	}

	public static IBindable<float> Max(this IBindableCollection<float> source)
	{
		return MaxInternal(source, new FloatNumeric());
	}

	public static IBindable<decimal> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<double> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<decimal?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<double?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<int> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<int?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<long?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<long> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<float?> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<float> Max<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector).Max();
	}

	public static IBindable<int> Subtract(this IBindable<int> source, IBindable<int> toSubtract)
	{
		return source.Project(s => s + toSubtract.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<float> Subtract(this IBindable<float> source, IBindable<float> toSubtract)
	{
		return source.Project(s => s + toSubtract.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<decimal> Subtract(this IBindable<decimal> source, IBindable<decimal> toSubtract)
	{
		return source.Project(s => s + toSubtract.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<double> Subtract(this IBindable<double> source, IBindable<double> toSubtract)
	{
		return source.Project(s => s + toSubtract.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<long> Subtract(this IBindable<long> source, IBindable<long> toSubtract)
	{
		return source.Project(s => s + toSubtract.Current, DependencyAnalysis.Automatic);
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source) where TSource : class
	{
		return source.AsBindableInternal<TSource>(null);
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source) where TSource : class
	{
		return source.AsBindableInternal<TSource>(null);
	}

	public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source) where TSource : class where TResult : TSource
	{
		return source.AsBindable<TSource, TResult>((IDispatcher)null);
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source, IDispatcher dispatcher) where TSource : class
	{
		return source.AsBindableInternal<TSource>(dispatcher);
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source, IDispatcher dispatcher) where TSource : class
	{
		return source.AsBindableInternal<TSource>(dispatcher);
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable source, Dispatcher dispatcher) where TSource : class
	{
		return source.AsBindableInternal<TSource>(DispatcherFactory.Create(dispatcher));
	}

	public static IBindableCollection<TSource> AsBindable<TSource>(this IEnumerable<TSource> source, Dispatcher dispatcher) where TSource : class
	{
		return source.AsBindableInternal<TSource>(DispatcherFactory.Create(dispatcher));
	}

	public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source, IDispatcher dispatcher) where TSource : class where TResult : TSource
	{
		return from e in source.AsBindable(dispatcher)
		where e is TResult
		select (TResult)e;
	}

	public static IBindableCollection<TResult> AsBindable<TSource, TResult>(this IEnumerable<TSource> source, Dispatcher dispatcher) where TSource : class where TResult : TSource
	{
		return source.AsBindable<TSource, TResult>(DispatcherFactory.Create(dispatcher));
	}

	private static IBindableCollection<TSource> AsBindableInternal<TSource>(this IEnumerable source, IDispatcher dispatcher) where TSource : class
	{
		source.ShouldNotBeNull("source");
		if (dispatcher == null)
		{
			dispatcher = DispatcherFactory.Create();
		}

        switch (source)
        {
            case IBindableCollection<TSource> bindableCollection:
                return bindableCollection;
            case IBindingList _ when !(source is INotifyCollectionChanged):
                return new BindingListToBindableCollectionAdapter<TSource>(source, dispatcher);
            default:
                return new ObservableCollectionToBindableCollectionAdapter<TSource>(source, dispatcher);
        }
    }

	private static IBindable<TElement> MinInternal<TElement, TAverage>(IBindableCollection<TElement> source, INumeric<TElement, TAverage> numeric)
	{
		source.ShouldNotBeNull("source");
		return source.Aggregate<TElement, TElement>(sources => numeric.Min(sources));
	}

	public static IBindable<decimal> Min(this IBindableCollection<decimal> source)
	{
		return MinInternal(source, new DecimalNumeric());
	}

	public static IBindable<double> Min(this IBindableCollection<double> source)
	{
		return MinInternal(source, new DoubleNumeric());
	}

	public static IBindable<decimal?> Min(this IBindableCollection<decimal?> source)
	{
		return MinInternal(source, new DecimalNullNumeric());
	}

	public static IBindable<double?> Min(this IBindableCollection<double?> source)
	{
		return MinInternal(source, new DoubleNullNumeric());
	}

	public static IBindable<int?> Min(this IBindableCollection<int?> source)
	{
		return MinInternal(source, new Int32NullNumeric());
	}

	public static IBindable<long?> Min(this IBindableCollection<long?> source)
	{
		return MinInternal(source, new Int64NullNumeric());
	}

	public static IBindable<float?> Min(this IBindableCollection<float?> source)
	{
		return MinInternal(source, new FloatNullNumeric());
	}

	public static IBindable<int> Min(this IBindableCollection<int> source)
	{
		return MinInternal(source, new Int32Numeric());
	}

	public static IBindable<long> Min(this IBindableCollection<long> source)
	{
		return MinInternal(source, new Int64Numeric());
	}

	public static IBindable<float> Min(this IBindableCollection<float> source)
	{
		return MinInternal(source, new FloatNumeric());
	}

	public static IBindable<decimal> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<double> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<long?> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<int> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<double?> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<float?> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<long> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<decimal?> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<int?> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<float> Min<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
	{
		return source.Select(selector).Min();
	}

	public static IBindable<TSource> First<TSource>(this IBindableCollection<TSource> source)
	{
		return source.FirstOrDefault();
	}

	public static IBindable<TSource> First<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.FirstOrDefault(predicate);
	}

	public static IBindable<TSource> FirstOrDefault<TSource>(this IBindableCollection<TSource> source)
	{
		source.ShouldNotBeNull("source");
		return source.ElementAtOrDefault(0);
	}

	public static IBindable<TSource> FirstOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.Where(predicate).FirstOrDefault();
	}

	public static IBindable<bool> All<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		source.ShouldNotBeNull("source");
		predicate.ShouldNotBeNull("predicate");
		return source.Where(predicate).Count().Switch()
			.Case(count => count >= 1, result: true)
			.Default(result: false)
			.EndSwitch();
	}

	public static IBindableCollection<TResult> SelectMany<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, IBindableCollection<TResult>>> selector) where TSource : class where TResult : class
	{
		return source.SelectMany(selector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> SelectMany<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, IBindableCollection<TResult>>> selector, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TResult : class
	{
		source.ShouldNotBeNull("source");
		return source.Select(selector, dependencyAnalysisMode).UnionAll();
	}

	public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	{
		return source.ThenBy(keySelector, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
	{
		return source.ThenBy(keySelector, comparer, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.ThenBy(keySelector, null, dependencyAnalysisMode);
	}

	public static IOrderedBindableCollection<TSource> ThenBy<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		keySelector.ShouldNotBeNull("keySelector");
		var orderedBindableCollection = source.CreateOrderedIterator(keySelector.Compile(), comparer, descending: false);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return orderedBindableCollection.WithDependencyExpression(keySelector.Body, keySelector.Parameters[0]);
		}
		return orderedBindableCollection;
	}

	public static IBindable<TSource> SingleOrDefault<TSource>(this IBindableCollection<TSource> source)
	{
		return source.FirstOrDefault();
	}

	public static IBindable<TSource> SingleOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.FirstOrDefault(predicate);
	}

	public static IBindable<TSource> ElementAtOrDefault<TSource>(this IBindableCollection<TSource> source, int index)
	{
		source.ShouldNotBeNull("source");
		return new ElementAtAggregator<TSource>(source, index, source.Dispatcher);
	}

	public static IBindable<int> Count<TSource>(this IBindableCollection<TSource> source)
	{
		return source.Aggregate(sources => sources.Count);
	}

	public static IBindable<int> Count<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		predicate.ShouldNotBeNull("predicate");
		return source.Where(predicate).Count();
	}

	public static IBindable<TResult> Aggregate<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TResult>> func)
	{
		return source.Aggregate(func, DefaultDependencyAnalysis);
	}

	public static IBindable<TSource> Aggregate<TSource>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TSource>> func)
	{
		return source.Aggregate(func, DefaultDependencyAnalysis);
	}

	public static IBindable<TAccumulate> Aggregate<TSource, TAccumulate>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
	{
		return source.Aggregate(seed, func, DefaultDependencyAnalysis);
	}

	public static IBindable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> resultSelector)
	{
		return source.Aggregate(seed, func, resultSelector, DefaultDependencyAnalysis);
	}

	public static IBindable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> resultSelector, DependencyAnalysis dependencyAnalysisMode)
	{
		return source.Aggregate(seed, func, dependencyAnalysisMode).Project(resultSelector, dependencyAnalysisMode);
	}

	public static IBindable<TResult> Aggregate<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<IBindableCollection<TSource>, TResult>> func, DependencyAnalysis dependencyAnalysisMode)
	{
		source.ShouldNotBeNull("source");
		func.ShouldNotBeNull("func");
		var customAggregator = new CustomAggregator<TSource, TResult>(source, func.Compile(), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return customAggregator.WithDependencyExpression(func, func.Parameters[0]);
		}
		return customAggregator;
	}

	public static IBindable<TAccumulate> Aggregate<TSource, TAccumulate>(this IBindableCollection<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, DependencyAnalysis dependencyAnalysisMode)
	{
		source.ShouldNotBeNull("source");
		func.ShouldNotBeNull("func");
		seed.ShouldNotBeNull("seed");
		var compiledAccumulator = func.Compile();

        TAccumulate Aggregator(IBindableCollection<TSource> sourceElements)
        {
            return Enumerable.Aggregate(sourceElements, seed, (current, sourceElement) => compiledAccumulator(current, sourceElement));
        }

        var customAggregator = new CustomAggregator<TSource, TAccumulate>(source, Aggregator, source.Dispatcher);
		return dependencyAnalysisMode == DependencyAnalysis.Automatic ? customAggregator.WithDependencyExpression(func, func.Parameters[1]) : customAggregator;
    }

	private static IBindable<TAverageResult> AverageInternal<TResult, TAverageResult>(IBindableCollection<TResult> source, INumeric<TResult, TAverageResult> numeric)
	{
		source.ShouldNotBeNull("source");
		return source.Aggregate(sources => numeric.Average(sources));
	}

	public static IBindable<decimal> Average(this IBindableCollection<decimal> source)
	{
		return AverageInternal(source, new DecimalNumeric());
	}

	public static IBindable<decimal?> Average(this IBindableCollection<decimal?> source)
	{
		return AverageInternal(source, new DecimalNullNumeric());
	}

	public static IBindable<double?> Average(this IBindableCollection<double?> source)
	{
		return AverageInternal(source, new DoubleNullNumeric());
	}

	public static IBindable<double?> Average(this IBindableCollection<int?> source)
	{
		return AverageInternal(source, new Int32NullNumeric());
	}

	public static IBindable<double?> Average(this IBindableCollection<long?> source)
	{
		return AverageInternal(source, new Int64NullNumeric());
	}

	public static IBindable<float?> Average(this IBindableCollection<float?> source)
	{
		return AverageInternal(source, new FloatNullNumeric());
	}

	public static IBindable<double> Average(this IBindableCollection<double> source)
	{
		return AverageInternal(source, new DoubleNumeric());
	}

	public static IBindable<double> Average(this IBindableCollection<int> source)
	{
		return AverageInternal(source, new Int32Numeric());
	}

	public static IBindable<double> Average(this IBindableCollection<long> source)
	{
		return AverageInternal(source, new Int64Numeric());
	}

	public static IBindable<float> Average(this IBindableCollection<float> source)
	{
		return AverageInternal(source, new FloatNumeric());
	}

	public static IBindable<decimal> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<decimal?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<double?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<float?> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<float> Average<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
	{
		return source.Select(selector).Average();
	}

	public static IBindable<TResult> Project<TSource, TResult>(this IBindable<TSource> source, Expression<Func<TSource, TResult>> projector)
	{
		return source.Project(projector, DefaultDependencyAnalysis);
	}

	public static IBindable<TResult> Project<TSource, TResult>(this IBindable<TSource> source, Expression<Func<TSource, TResult>> projector, DependencyAnalysis dependencyAnalysisMode)
	{
		source.ShouldNotBeNull("source");
		projector.ShouldNotBeNull("projector");
		var projectOperator = new ProjectOperator<TSource, TResult>(source, projector.Compile(), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return projectOperator.WithDependencyExpression(projector.Body, projector.Parameters[0]);
		}
		return projectOperator;
	}

	public static IOrderedBindableCollection<TSource> ThenByDescending<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	{
		return source.ThenByDescending(keySelector, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> ThenByDescending<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
	{
		return source.ThenByDescending(keySelector, comparer, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> ThenByDescending<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.ThenByDescending(keySelector, null, dependencyAnalysisMode);
	}

	public static IOrderedBindableCollection<TSource> ThenByDescending<TSource, TKey>(this IOrderedBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		keySelector.ShouldNotBeNull("keySelector");
		var orderedBindableCollection = source.CreateOrderedIterator(keySelector.Compile(), comparer, descending: true);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return orderedBindableCollection.WithDependencyExpression(keySelector.Body, keySelector.Parameters[0]);
		}
		return orderedBindableCollection;
	}

	public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	{
		return source.GroupBy(keySelector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.GroupBy(keySelector, s => s, new DefaultComparer<TKey>(), dependencyAnalysisMode);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer) where TSource : class
	{
		return source.GroupBy(keySelector, comparer, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.GroupBy(keySelector, s => s, comparer, dependencyAnalysisMode);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) where TSource : class where TElement : class
	{
		return source.GroupBy(keySelector, elementSelector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TElement : class
	{
		return source.GroupBy(keySelector, elementSelector, null, dependencyAnalysisMode);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector) where TSource : class where TResult : class
	{
		return source.GroupBy(keySelector, resultSelector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TResult : class
	{
		return source.GroupBy(keySelector, s => s, new DefaultComparer<TKey>(), dependencyAnalysisMode).Into(resultSelector);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer) where TSource : class where TElement : class
	{
		return source.GroupBy(keySelector, elementSelector, comparer, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<IBindableGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TElement : class
	{
		source.ShouldNotBeNull("source");
		keySelector.ShouldNotBeNull("keySelector");
		elementSelector.ShouldNotBeNull("elementSelector");
		var groupByIterator = new GroupByIterator<TKey, TSource, TElement>(source, keySelector, elementSelector, comparer, source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return groupByIterator.WithDependencyExpression(keySelector.Body, keySelector.Parameters[0]);
		}
		return groupByIterator;
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector) where TSource : class where TElement : class where TResult : class
	{
		return source.GroupBy(keySelector, elementSelector, resultSelector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TElement : class where TResult : class
	{
		return source.GroupBy(keySelector, elementSelector, null, dependencyAnalysisMode).Into(resultSelector);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) where TSource : class where TResult : class
	{
		return source.GroupBy(keySelector, resultSelector, comparer, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IBindableCollection<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TResult : class
	{
		return source.GroupBy(keySelector, s => s, comparer, dependencyAnalysisMode).Into(resultSelector);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer) where TSource : class where TElement : class where TResult : class
	{
		return source.GroupBy(keySelector, elementSelector, resultSelector, comparer, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class where TElement : class where TResult : class
	{
		return source.GroupBy(keySelector, elementSelector, comparer, dependencyAnalysisMode).Into(resultSelector);
	}

    // ReSharper disable once RedundantNameQualifier
    public static TResult WithDependencyExpression<TResult>(this TResult query, System.Linq.Expressions.Expression expression, ParameterExpression itemParameter) where TResult : IAcceptsDependencies
	{
		var expressionAnalyzer = BindingConfigurations.Default.CreateExpressionAnalyzer();
		var definitions = expressionAnalyzer.DiscoverDependencies(expression, itemParameter);
		return query.WithDependencies(definitions);
	}

	public static TResult WithDependency<TResult>(this TResult query, object externalObject, string propertyPath) where TResult : IAcceptsDependencies
	{
		return query.WithDependency(new ExternalDependencyDefinition(propertyPath, externalObject));
	}

	public static TResult WithDependency<TResult>(this TResult query, DependencyObject dependencyObject, DependencyProperty dependencyProperty) where TResult : IAcceptsDependencies
	{
		return query.WithDependency(new ExternalDependencyDefinition(dependencyProperty.Name, dependencyObject));
	}

	public static TResult WithDependency<TResult>(this TResult query, string propertyPath) where TResult : IAcceptsDependencies
	{
		return query.WithDependency(new ItemDependencyDefinition(propertyPath));
	}

	public static TResult WithDependency<TResult>(this TResult query, IDependencyDefinition definition) where TResult : IAcceptsDependencies
	{
		if (query != null && definition != null)
		{
			query.AcceptDependency(definition);
		}
		return query;
	}

	public static TResult WithDependencies<TResult>(this TResult query, IEnumerable<IDependencyDefinition> definitions) where TResult : IAcceptsDependencies
	{
		if (query != null)
		{
			foreach (var definition in definitions)
			{
				if (definition != null)
				{
					query.AcceptDependency(definition);
				}
			}
		}
		return query;
	}

	public static IBindableCollection<TSource> Union<TSource>(this IBindableCollection<TSource> first, IBindableCollection<TSource> second) where TSource : class
	{
		first.ShouldNotBeNull("first");
		second.ShouldNotBeNull("second");
        var bindableCollection = new BindableCollection<IBindableCollection<TSource>>(first.Dispatcher) {first, second};
        var elements = bindableCollection;
		return new UnionIterator<TSource>(elements, first.Dispatcher);
	}

	public static IBindableCollection<TSource> Union<TSource>(this IBindableCollection<TSource> first, IBindableCollection<TSource> second, IEqualityComparer<TSource> comparer) where TSource : class
	{
		return first.Union(second).Distinct(comparer);
	}

	public static IBindableCollection<TResult> Into<TKey, TElement, TResult>(this IBindableCollection<IBindableGrouping<TKey, TElement>> source, Expression<Func<TKey, IBindableCollection<TElement>, TResult>> resultSelector) where TElement : class where TResult : class
	{
		var func = resultSelector.Compile();
		return source.Select(g => func(g.Key, g), DependencyAnalysis.Disabled);
	}

	public static IBindableCollection<TElement> Concat<TElement>(this IBindableCollection<TElement> first, IBindableCollection<TElement> second) where TElement : class
	{
		return first.Union(second);
	}

	public static IBindableCollection<TResult> OfType<TResult>(this IBindableCollection source) where TResult : class
	{
		return source.AsBindable<object>(source.Dispatcher).Where(s => s is TResult, DependencyAnalysis.Disabled).Select(s => (TResult)s, DependencyAnalysis.Disabled);
	}

	public static IBindable<bool> Any<TSource>(this IBindableCollection<TSource> source)
	{
		source.ShouldNotBeNull("source");
		return source.Count().Switch().Case(count => count >= 1, result: true)
			.Default(result: false)
			.EndSwitch();
	}

	public static IBindable<bool> Any<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		source.ShouldNotBeNull("source");
		predicate.ShouldNotBeNull("predicate");
		return source.Where(predicate).Any();
	}

	public static IBindableCollection<TSource> Select<TSource>(this IBindableCollection<TSource> source) where TSource : class
	{
		return source.Select(DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TResult> Select<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TResult>> selector) where TSource : class
	{
		return source.Select(selector, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TSource> Select<TSource>(this IBindableCollection<TSource> source, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.Select(s => s, dependencyAnalysisMode);
	}

	public static IBindableCollection<TResult> Select<TSource, TResult>(this IBindableCollection<TSource> source, Expression<Func<TSource, TResult>> selector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		selector.ShouldNotBeNull("selector");
		var selectIterator = new SelectIterator<TSource, TResult>(source, selector.Compile(), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			selectIterator = selectIterator.WithDependencyExpression(selector.Body, selector.Parameters[0]);
		}
		return selectIterator;
	}

	public static IOrderedBindableCollection<TSource> OrderByDescending<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TSource : class
	{
		return source.OrderByDescending(keySelector, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> OrderByDescending<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer) where TSource : class
	{
		return source.OrderByDescending(keySelector, comparer, DefaultDependencyAnalysis);
	}

	public static IOrderedBindableCollection<TSource> OrderByDescending<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		return source.OrderByDescending(keySelector, null, dependencyAnalysisMode);
	}

	public static IOrderedBindableCollection<TSource> OrderByDescending<TSource, TKey>(this IBindableCollection<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		keySelector.ShouldNotBeNull("keySelector");
		var orderByIterator = new OrderByIterator<TSource, TKey>(source, new ItemSorter<TSource, TKey>(null, keySelector.Compile(), comparer, ascending: false), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return orderByIterator.WithDependencyExpression(keySelector.Body, keySelector.Parameters[0]);
		}
		return orderByIterator;
	}

	private static IBindable<TElement> SumInternal<TElement, TAverage>(IBindableCollection<TElement> source, INumeric<TElement, TAverage> numeric)
	{
		source.ShouldNotBeNull("source");
		return source.Aggregate<TElement>(sources => numeric.Sum(sources));
	}

	public static IBindable<double> Sum(this IBindableCollection<double> source)
	{
		return SumInternal(source, new DoubleNumeric());
	}

	public static IBindable<decimal?> Sum(this IBindableCollection<decimal?> source)
	{
		return SumInternal(source, new DecimalNullNumeric());
	}

	public static IBindable<decimal> Sum(this IBindableCollection<decimal> source)
	{
		return SumInternal(source, new DecimalNumeric());
	}

	public static IBindable<int> Sum(this IBindableCollection<int> source)
	{
		return SumInternal(source, new Int32Numeric());
	}

	public static IBindable<long> Sum(this IBindableCollection<long> source)
	{
		return SumInternal(source, new Int64Numeric());
	}

	public static IBindable<double?> Sum(this IBindableCollection<double?> source)
	{
		return SumInternal(source, new DoubleNullNumeric());
	}

	public static IBindable<int?> Sum(this IBindableCollection<int?> source)
	{
		return SumInternal(source, new Int32NullNumeric());
	}

	public static IBindable<long?> Sum(this IBindableCollection<long?> source)
	{
		return SumInternal(source, new Int64NullNumeric());
	}

	public static IBindable<float?> Sum(this IBindableCollection<float?> source)
	{
		return SumInternal(source, new FloatNullNumeric());
	}

	public static IBindable<float> Sum(this IBindableCollection<float> source)
	{
		return SumInternal(source, new FloatNumeric());
	}

	public static IBindable<decimal?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal?>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<double?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double?>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<decimal> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, decimal>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<int?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int?>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<double> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, double>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<int> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, int>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<long> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<long?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, long?>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<float?> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float?>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<float> Sum<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, float>> selector) where TSource : class
	{
		return source.Select(selector).Sum();
	}

	public static IBindable<int> Add(this IBindable<int> source, IBindable<int> toAdd)
	{
		return source.Project(s => s + toAdd.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<float> Add(this IBindable<float> source, IBindable<float> toAdd)
	{
		return source.Project(s => s + toAdd.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<decimal> Add(this IBindable<decimal> source, IBindable<decimal> toAdd)
	{
		return source.Project(s => s + toAdd.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<double> Add(this IBindable<double> source, IBindable<double> toAdd)
	{
		return source.Project(s => s + toAdd.Current, DependencyAnalysis.Automatic);
	}

	public static IBindable<long> Add(this IBindable<long> source, IBindable<long> toAdd)
	{
		return source.Project(s => s + toAdd.Current, DependencyAnalysis.Automatic);
	}

	public static IBindableCollection<TResult> Cast<TResult>(this IBindableCollection source) where TResult : class
	{
		return source.AsBindable<TResult>(source.Dispatcher);
	}

	public static IBindable<TSource> Single<TSource>(this IBindableCollection<TSource> source)
	{
		return source.FirstOrDefault();
	}

	public static IBindable<TSource> Single<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.FirstOrDefault(predicate);
	}

	public static IBindableCollection<TSource> Distinct<TSource>(this IBindableCollection<TSource> source) where TSource : class
	{
		return source.Distinct(null);
	}

	public static IBindableCollection<TSource> Distinct<TSource>(this IBindableCollection<TSource> source, IEqualityComparer<TSource> comparer) where TSource : class
	{
		if (comparer == null)
		{
			comparer = new DefaultComparer<TSource>();
		}
		return source.GroupBy(c => comparer.GetHashCode(c), DependencyAnalysis.Disabled).Select(group => group.First().Current, DependencyAnalysis.Disabled);
	}

	public static IBindable<bool> Contains<TSource>(this IBindableCollection<TSource> source, TSource value) where TSource : class
	{
		return source.Contains(value, null);
	}

	public static IBindable<bool> Contains<TSource>(this IBindableCollection<TSource> source, TSource value, IEqualityComparer<TSource> comparer) where TSource : class
	{
		comparer = (comparer ?? new DefaultComparer<TSource>());
		value.ShouldNotBeNull("value");
		return (from s in source
		where comparer.Equals(s, value)
		select s).Any();
	}

	public static IBindingList ToBindingList<TElement>(this IBindableCollection<TElement> bindableCollection) where TElement : class
	{
		return new BindingListAdapter<TElement>(bindableCollection, bindableCollection.Dispatcher);
	}

	public static ISwitchDeclaration<TSource> Switch<TSource>(this IBindable<TSource> source)
	{
		return new EmptySwitchDefinition<TSource>(source, source.Dispatcher);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TSource condition, TResult result)
	{
		return switchDeclaration.Case(c => AreEqual(c, condition), result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TSource condition, Expression<Func<TSource, TResult>> result)
	{
		return switchDeclaration.Case(c => AreEqual(c, condition), result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, bool>> condition, TResult result)
	{
		return switchDeclaration.Case(condition, i => result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, bool>> condition, Expression<Func<TSource, TResult>> result)
	{
		return switchDeclaration.CreateForResultType<TResult>().Case(condition, result);
	}

	public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, TResult result)
	{
		return switchDeclaration.Default(r => result);
	}

	public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitchDeclaration<TSource> switchDeclaration, Expression<Func<TSource, TResult>> result)
	{
		return switchDeclaration.CreateForResultType<TResult>().Default(result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TSource condition, TResult result)
	{
		return currentSwitch.Case(c => AreEqual(c, condition), result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TSource condition, Expression<Func<TSource, TResult>> result)
	{
		return currentSwitch.Case(c => AreEqual(c, condition), result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, bool>> condition, TResult result)
	{
		return currentSwitch.Case(condition, i => result);
	}

	public static ISwitch<TSource, TResult> Case<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, bool>> condition, Expression<Func<TSource, TResult>> result)
	{
		return currentSwitch.AddCase(new SwitchCase<TSource, TResult>(condition, result));
	}

	public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, TResult result)
	{
		return currentSwitch.Default(i => result);
	}

	public static ISwitch<TSource, TResult> Default<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch, Expression<Func<TSource, TResult>> result)
	{
		return currentSwitch.AddCase(new SwitchDefault<TSource, TResult>(result));
	}

	public static IBindable<TResult> EndSwitch<TSource, TResult>(this ISwitch<TSource, TResult> currentSwitch)
	{
		return currentSwitch as IBindable<TResult>;
	}

	private static bool AreEqual(object lhs, object rhs)
	{
		return (lhs == null) ? (rhs == null) : (rhs != null && lhs.Equals(rhs));
	}

	public static IBindableCollection<TSource> Where<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.Where(predicate, DefaultDependencyAnalysis);
	}

	public static IBindableCollection<TSource> Where<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate, DependencyAnalysis dependencyAnalysisMode) where TSource : class
	{
		source.ShouldNotBeNull("source");
		predicate.ShouldNotBeNull("predicate");
		var whereIterator = new WhereIterator<TSource>(source, predicate.Compile(), source.Dispatcher);
		if (dependencyAnalysisMode == DependencyAnalysis.Automatic)
		{
			return whereIterator.WithDependencyExpression(predicate.Body, predicate.Parameters[0]);
		}
		return whereIterator;
	}

	public static IBindableCollection<TSource> UnionAll<TSource>(this IBindableCollection<IBindableCollection<TSource>> sources) where TSource : class
	{
		sources.ShouldNotBeNull("sources");
		return new UnionIterator<TSource>(sources, sources.Dispatcher);
	}

	public static IBindable<TSource> LastOrDefault<TSource>(this IBindableCollection<TSource> source)
	{
		return source.ElementAtOrDefault(-1);
	}

	public static IBindable<TSource> LastOrDefault<TSource>(this IBindableCollection<TSource> source, Expression<Func<TSource, bool>> predicate) where TSource : class
	{
		return source.Where(predicate).LastOrDefault();
	}
}
}
