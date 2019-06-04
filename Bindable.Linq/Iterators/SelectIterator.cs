using System;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Iterators
{
    internal sealed class SelectIterator<TSource, TResult> : Iterator<TSource, TResult> where TSource : class
    {
        public ProjectionRegister<TSource, TResult> ProjectionRegister { get; }

        public SelectIterator(IBindableCollection<TSource> sourceCollection, Func<TSource, TResult> projector, IDispatcher dispatcher)
		    : base(sourceCollection, dispatcher)
	    {
		    ProjectionRegister = new ProjectionRegister<TSource, TResult>(projector);
	    }

	    protected override void EvaluateSourceCollection()
	    {
		    foreach (var item in SourceCollection)
		    {
			    ReactToAdd(-1, item);
		    }
	    }

	    protected override void ReactToAdd(int insertionIndex, TSource addedItem)
	    {
		    var item = ProjectionRegister.Project(addedItem);
		    ResultCollection.Insert(insertionIndex, item);
	    }

	    protected override void ReactToMove(int originalIndex, int newIndex, TSource movedItem)
	    {
		    ResultCollection.Move(newIndex, ProjectionRegister.Project(movedItem));
	    }

	    protected override void ReactToRemove(int index, TSource removedItem)
	    {
		    var existingProjection = ProjectionRegister.GetExistingProjection(removedItem);
            if (existingProjection == null) return;
            ResultCollection.Remove((TResult)existingProjection);
            ProjectionRegister.Remove(removedItem);
        }

	    protected override void ReactToReplace(int index, TSource oldItem, TSource newItem)
	    {
		    ResultCollection.Replace((TResult)ProjectionRegister.GetExistingProjection(oldItem), ProjectionRegister.Project(newItem));
		    ProjectionRegister.Remove(oldItem);
	    }

	    protected override void ReactToItemPropertyChanged(TSource item, string propertyName)
	    {
		    var existingProjection = ProjectionRegister.GetExistingProjection(item);
		    if (existingProjection is TResult result)
		    {
			    ResultCollection.Replace(result, ProjectionRegister.ReProject(item));
		    }
	    }

	    protected override void BeforeResetOverride()
	    {
		    ProjectionRegister.Clear();
		    base.BeforeResetOverride();
	    }

	    protected override void BeforeDisposeOverride()
	    {
		    ProjectionRegister.Dispose();
		    base.BeforeDisposeOverride();
	    }
}
}