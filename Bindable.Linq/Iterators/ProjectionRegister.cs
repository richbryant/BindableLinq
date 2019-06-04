using System;
using System.Collections.Generic;
using System.Linq;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    internal sealed class ProjectionRegister<TSource, TResult> : IDisposable
    {
        private readonly Func<TSource, TResult> _projector;

	    private object ProjectionLock { get; } = new object();

        private IDictionary<TSource, TResult> Projections { get; } = new Dictionary<TSource, TResult>();

        public ProjectionRegister(Func<TSource, TResult> projector)
	    {
		    _projector = projector;
	    }

	    public void Store(TSource source, TResult result)
	    {
		    lock (ProjectionLock)
		    {
			    if (Projections.ContainsKey(source))
			    {
				    Projections[source] = result;
			    }
			    else
			    {
				    Projections.Add(source, result);
			    }
		    }
	    }

	    public bool HasExistingProjection(TSource source)
	    {
		    return InnerGetExistingProjection(source) != null;
	    }

	    private object InnerGetExistingProjection(TSource source)
	    {
		    source.ShouldNotBeNull("source");
		    object result = null;
            if (source == null) return null;
            lock (ProjectionLock)
            {
                if (Projections.ContainsKey(source))
                {
                    result = Projections[source];
                }
            }
            return result;
	    }

	    public TResult ReProject(TSource source)
	    {
		    var result = _projector(source);
		    Store(source, result);
		    return result;
	    }

	    public TResult Project(TSource source)
	    {
		    var obj = InnerGetExistingProjection(source);
		    if (obj != null)
		    {
			    return (TResult)obj;
		    }
		    var result = _projector(source);
		    Store(source, result);
		    return result;
	    }

	    public IEnumerable<TResult> CreateOrGetProjections(IEnumerable<TSource> range)
        {
            return range.Where(source => source != null).Select(Project);
        }

	    public IEnumerable<TResult> GetProjections(IEnumerable<TSource> range)
        {
            return range.Where(item => item != null).Select(InnerGetExistingProjection).Where(obj => obj != null).Cast<TResult>();
        }

	    public void Remove(TSource source)
	    {
		    lock (ProjectionLock)
		    {
			    if (Projections.ContainsKey(source))
			    {
				    Projections.Remove(source);
			    }
		    }
	    }

	    public void RemoveRange(IEnumerable<TSource> sourceItems)
	    {
		    foreach (var sourceItem in sourceItems)
		    {
			    Remove(sourceItem);
		    }
	    }

	    public void Clear()
	    {
		    lock (ProjectionLock)
		    {
			    Projections.Clear();
		    }
	    }

	    public object GetExistingProjection(TSource item)
	    {
		    return InnerGetExistingProjection(item);
	    }

	    public void Dispose()
	    {
		    Clear();
	    }
    }
}