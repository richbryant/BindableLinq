using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Interfaces.Events;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Interfaces
{
    public interface IBindableCollection : IEnumerable, IRefreshable, INotifyCollectionChanged, INotifyPropertyChanged, IAcceptsDependencies, IDisposable
    {
        IDispatcher Dispatcher
        {
            get;
        }

        int Count
        {
            get;
        }

        bool HasEvaluated
        {
            get;
        }

        void Evaluate();
    }

    public interface IBindableCollection<TElement> : IEnumerable<TElement>, IBindableCollection
    {
        TElement this[int index]
        {
            get;
        }

        event EvaluatingEventHandler<TElement> Evaluating;
    }
}
